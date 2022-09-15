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
                    // Newtonjson�� ����ϱ� ���� ����
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
            // SignalR�� �������� ����ϱ� ���� ����
            NotifyHub.Current = app.ApplicationServices.GetService<IHubContext<NotifyHub>>();

            // CROS ���� ���� ����
            app.UseCors("DevPolicy");
            app.UseAllowCors();

            app.UseDeveloperExceptionPage();

            // ���� / ���� ���е���
            if (env.IsDevelopment())
            {
                // ������ ������������ �ܺ� ����Ǹ� �ȵǹǷ� ���������׸� ��������.
                app.UseDeveloperExceptionPage();
                app.UseSwashbuckle();
            }
            else
            {
                //app.UseHsts();
            }

            // lifeTime :  Global.asx �� ���� ������ ��.
            //lifetime.UseLifetime<LifetimeJob>();
#if true
            // Http Logger
            app.UseHttpLogging<HttpLoggerCommand>();
#else
            // �߻��ϴ� Exception�� ���� ó��
            app.UseException<ExceptionMiddleware>();
            //app.UseException();
            //������ ������ Http �����ڵ带 �����ϱ����� ����
            //app.UseStatusCodePages();
#endif

            app.UseCookiePolicy();

            //������Ʈ �⺻���� �б� ����
            app.UseDefaultFiles();
            //wwwroot �����б� : Backencd�� Frontend�� �Ѽ����� �ø��� ���� ����
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); // �ݵ�� UseRouting ~ UseEndpoints ���̿� ��� ���ߵ�
            app.UseAuthorization();// �ݵ�� UseRouting ~ UseEndpoints ���̿� ��� ���ߵ�
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<NotifyHub>("/NotifyHub");
            });

            // app.UseHttpsRedirection(); //http��û�� https�� ���𷺼��մϴ�.
        }
    }
}