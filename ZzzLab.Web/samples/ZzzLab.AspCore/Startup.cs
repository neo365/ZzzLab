using Microsoft.AspNetCore.SignalR;
using ZzzLab.AspCore.Configuration;
using ZzzLab.AspCore.Hubs;
using ZzzLab.AspCore.Logging;
using ZzzLab.Web.Builder;

namespace ZzzLab.AspCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            // Init SignalR
            services.AddSignalR();

            services.AddControllers()
                    // Newtonjson을 사용하기 위한 설정
                    .AddNewtonsoftJson(options =>
                    {
                        // Use the default property (Pascal) casing
                        options.SerializerSettings.ContractResolver = new SiteJsonResolver();
                    });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddAuth(true);
            services.AddSwashbuckle(); // Swagger

            // Enable SignalR CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("DevPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:8081", "http://localhost:8082")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                          
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime lifetime)
        {
            // SignalR을 전역으로 사용하기 위한 설정
            NotifyHub.Current = app.ApplicationServices.GetService<IHubContext<NotifyHub>>();

            // CROS 접근 권한 문제
            app.UseCors("DevPolicy");
            app.UseAllowCors();

            app.UseDeveloperExceptionPage();

            // 개발 / 발행 구분동작
            if (env.IsDevelopment())
            {
                // 개발자 에러페이지는 외부 유출되면 안되므로 개발자한테만 보여주자.
                app.UseDeveloperExceptionPage();
                app.UseSwashbuckle();
            }
            else
            {
                //app.UseHsts();
            }

            // lifeTime :  Global.asx 와 같은 역할을 함.
            //lifetime.UseLifetime<LifetimeJob>();
#if true
            // Http Logger
            app.UseHttpLogging<HttpLoggerCommand>();
#else
            // 발생하는 Exception에 대한 처리
            app.UseException<ExceptionMiddleware>();
            //app.UseException();
            //에러가 났을때 Http 상태코드를 전달하기위한 설정
            //app.UseStatusCodePages();
#endif

            app.UseCookiePolicy();

            //웹사이트 기본파일 읽기 설정
            app.UseDefaultFiles();
            //wwwroot 파일읽기 : Backencd와 Frontend를 한서버에 올리기 위한 설정
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); // 반드시 UseRouting ~ UseEndpoints 사이에 들어 가야됨
            app.UseAuthorization();// 반드시 UseRouting ~ UseEndpoints 사이에 들어 가야됨
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<NotifyHub>("/NotifyHub");
            });

            // app.UseHttpsRedirection(); //http요청을 https로 리디렉션합니다.
        }
    }
}