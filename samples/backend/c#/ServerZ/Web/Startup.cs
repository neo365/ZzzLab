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
    /// Web App ȯ�漳��
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
            // SignalR�� �������� ����ϱ� ���� ����
            MainHub.Current = app.ApplicationServices.GetService<IHubContext<MainHub>>();
            NotifyHub.Current = app.ApplicationServices.GetService<IHubContext<NotifyHub>>();

            // CROS ���� ���� ����
            app.UseCors("VuetifyPolicy");
            app.UseCors("BootstrapPolicy");
            app.UseAllowCors();

            // ���� / ���� ���е���
#if true
#if DEBUG
            app.UseDeveloperExceptionPage();
            app.UseSwashbuckle();
#else
            //app.UseExceptionHandler("/Error");
            app.UseHsts();
            //������ ������ Http �����ڵ带 �����ϱ����� ����
            app.UseStatusCodePages();
            //app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");
            //app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");
#endif
#else
            if (env.IsDevelopment())
            {
                // ������ ������������ �ܺ� ����Ǹ� �ȵǹǷ� ���������׸� ��������.
                app.UseDeveloperExceptionPage();
                app.UseSwashbuckle();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseHsts();
                //������ ������ Http �����ڵ带 �����ϱ����� ����
                //app.UseStatusCodePages();
                //app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");
                //app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");
            }
#endif

            // lifeTime :  Global.asx �� ���� ������ ��.
            lifetime.UseLifetime<LifetimeJob>();
            WebBuilder.HostLifetime = lifetime;
#if false
            // Http Logger
            app.UseHttpLogging<HttpLoggerCommand>();
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
                endpoints.MapHub<NotifyHub>("/MainHub");
                endpoints.MapHub<NotifyHub>("/NotifyHub");
            });

            // app.UseHttpsRedirection(); //http��û�� https�� ���𷺼��մϴ�.
        }
    }
}