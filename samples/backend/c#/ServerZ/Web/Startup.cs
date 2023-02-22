using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ZzzLab.Web.Builder;
using ZzzLab.Web.Configuration;
using ZzzLab.Web.Hubs;

namespace ZzzLab.Web
{
    /// <summary>
    /// Web App 환경설정
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Webapp start configuration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
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

            services.AddAuth();
            services.AddSwashbuckle(); // Swagger

            // Enable SignalR CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("BootstrapPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:8082")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });

                opt.AddPolicy("VuetifyPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:8081")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            // SignalR을 전역으로 사용하기 위한 설정
            MainHub.Current = app.ApplicationServices.GetService<IHubContext<MainHub>>();
            NotifyHub.Current = app.ApplicationServices.GetService<IHubContext<NotifyHub>>();

            // CROS 접근 권한 문제
            app.UseCors("VuetifyPolicy");
            app.UseCors("BootstrapPolicy");
            app.UseAllowCors();

            // 개발 / 발행 구분동작
#if true
#if DEBUG
            app.UseDeveloperExceptionPage();
            app.UseSwashbuckle();
#else
            //app.UseExceptionHandler("/Error");
            app.UseHsts();
            //에러가 났을때 Http 상태코드를 전달하기위한 설정
            app.UseStatusCodePages();
            //app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");
            //app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");
#endif
#else
            if (env.IsDevelopment())
            {
                // 개발자 에러페이지는 외부 유출되면 안되므로 개발자한테만 보여주자.
                app.UseDeveloperExceptionPage();
                app.UseSwashbuckle();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseHsts();
                //에러가 났을때 Http 상태코드를 전달하기위한 설정
                //app.UseStatusCodePages();
                //app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");
                //app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");
            }
#endif

            // lifeTime :  Global.asx 와 같은 역할을 함.
            lifetime.UseLifetime<LifetimeJob>();
            WebBuilder.HostLifetime = lifetime;
#if false
            // Http Logger
            app.UseHttpLogging<HttpLoggerCommand>();
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
                endpoints.MapHub<NotifyHub>("/MainHub");
                endpoints.MapHub<NotifyHub>("/NotifyHub");
            });

            // app.UseHttpsRedirection(); //http요청을 https로 리디렉션합니다.
        }
    }
}