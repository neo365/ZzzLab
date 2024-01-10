using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZzzLab.Web.Builder
{
    public static partial class WebBuilderExtensions
    {
        /// <summary>
        /// Configure the Swagger generator with XML comments, bearer authentication, etc.
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddSwashbuckle(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            // Configures ApiExplorer (needed from ASP.NET Core 6.0).
            services.AddEndpointsApiExplorer();

            // Register the Swagger generator, defining one or more Swagger documents.
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                if (File.Exists(xmlFilePath))
                {
                    options.IncludeXmlComments(xmlFilePath);
                }

                options.OperationFilter<SecurityRequirementsOperationFilter>(true, JwtBearerDefaults.AuthenticationScheme);
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (JWT). Example: \"bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    //BearerFormat = "JWT",
                });

                //options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                //{
                //    {
                //            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                //            {
                //                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                //                {
                //                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                //                    Id = "Bearer"
                //                }
                //            },
                //            new string[] {}
                //    }
                //});

                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });
        }

        public static void AddJWT(this IServiceCollection services, params string[] userRoles)
        {
            AuthenticationBuilder builder = services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             });

            if (Configurator.Setting?.JWTConfig == null) throw new InitializeException("JWT settings not found.");

            builder.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = Configurator.Setting.JWTConfig.GetParameters();
            });

            if (userRoles != null && userRoles.Length != 0)
            {
                // Policies
                services.AddAuthorization(config =>
                {
                    // inline policies
                    foreach (var roleName in userRoles)
                    {
                        config.AddPolicy(roleName, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(roleName).Build());
                    }
                });
            }
        }
    }
}