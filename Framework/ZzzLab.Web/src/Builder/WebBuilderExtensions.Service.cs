using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Runtime.CompilerServices;
using ZzzLab.Web.Authentication;

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

        public static void AddAuth(this IServiceCollection services, bool isJWT = false, IEnumerable<string>? userRoles = null)
        {
            AuthenticationBuilder builder = services.AddAuthentication(options =>
             {
                 //options.DefaultAuthenticateScheme = AuthSchemeOptions.Scheme;
                 //options.DefaultChallengeScheme = AuthSchemeOptions.Scheme;
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             });

            if (isJWT)
            {
                builder.AddJwtBearer(options =>
                {
                    //options.RequireHttpsMetadata = false;
                    //options.SaveToken = true;
                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateIssuer = true,
                    //    ValidateAudience = true,
                    //    ValidateLifetime = true,
                    //    ValidateIssuerSigningKey = true,
                    //    ValidIssuer = Configurator.Get("JWT_ISSUER"),
                    //    ValidAudience = Configurator.Get("JWT_AUDIENCE"),
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configurator.Get("JWT_SECURITY_KEY"))),
                    //    ClockSkew = TimeSpan.Zero
                    //};

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configurator.Get("JWT_SECURITY_KEY"))),
                        ValidateIssuer = true,
                        ValidIssuer = Configurator.Get("JWT_ISSUER"),
                        ValidateAudience = true,
                        ValidAudience = Configurator.Get("JWT_AUDIENCE"),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(1),
                    };
                });
            }
            else
            {
                builder.AddScheme<AuthSchemeOptions, ZzzLabAuthHandler>(AuthSchemeOptions.Scheme, options =>
                {
                    options.AuthKey = AuthSchemeOptions.Scheme;
                });
            }

            if (userRoles != null && userRoles.Any())
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