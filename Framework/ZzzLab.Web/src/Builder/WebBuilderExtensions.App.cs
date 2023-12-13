using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ZzzLab.ExceptionEx;
using ZzzLab.Net.Http;
using ZzzLab.Web.Logging;
using ZzzLab.Web.Middelware;
using ZzzLab.Web.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ZzzLab.Web.Builder
{
    public static partial class WebBuilderExtensions
    {
        /// <summary>
        /// Swagger에 Api 버젼을 적용하기 위한 미들웨어
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder</returns>
        /// <exception cref="ArgumentNullException">IApplicationBuilder 값이 설정되지 않았을 경우</exception>
        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.UseSwagger();

            app.UseSwaggerUI();

            return app;
        }

        /// <summary>
        /// CROS 접근 권한 문제
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder</returns>
        /// <exception cref="ArgumentNullException">IApplicationBuilder 값이 설정되지 않았을 경우</exception>
        public static IApplicationBuilder UseAllowCors(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            // CROS 접근 권한 문제
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            return app;
        }

        private static bool IsRunUseHttpLogging = false;
        private static bool IsRunExceptionHandler = false;

        /// <summary>
        /// 사용자 Exception 처리
        /// </summary>
        /// <typeparam name="T">IExceptionMiddleware</typeparam>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder</returns>
        /// <exception cref="ArgumentNullException">IApplicationBuilder가 주어지지 않았을 경우</exception>
        /// <exception cref="InvalidTypeException">IExceptionMiddleware 가 아닐경우</exception>
        public static IApplicationBuilder UseException<T>(this IApplicationBuilder app) where T : IExceptionMiddleware
        {
            if (IsRunUseHttpLogging) throw new InvalidOperationException("UseExceptionHandler and UseHttpLogging cannot be used at the same time.");

            ArgumentNullException.ThrowIfNull(app);

            if (Activator.CreateInstance(typeof(T)) is IExceptionMiddleware job)
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = job.Invoke });
            }
            else throw new InvalidTypeException(typeof(T));

            IsRunExceptionHandler = true;

            return app;
        }

        /// <summary>
        /// Adds a middleware that can log HTTP requests and responses.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseException(this IApplicationBuilder app)
        {
            if (IsRunUseHttpLogging) throw new InvalidOperationException("UseExceptionHandler and UseHttpLogging cannot be used at the same time.");

            ArgumentNullException.ThrowIfNull(app);

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = Text.Plain;

                    string message = "서버에러가 발생하였습니다.";

                    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (error != null)
                    {
                        context.Response.ContentType = Application.Json;
                        var exception = error.InnerException ?? error;

                        RestServerErrorResult res = new RestServerErrorResult()
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            ErrorMessage = exception.GetAllMessages(),
                            Error = exception.GetAllExceptionInfo(),
                            ErrorDescription = StatusCodes.Status500InternalServerError.ToStatusMessage(),
                        };

                        message = res.ToString();
                    }

                    try
                    {
                        await context.Response.WriteAsync(message);
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(ex.Message);
                    }
                });
            });
            return app;
        }

        /// <summary>
        /// Adds a middleware that can log HTTP requests and responses.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseHttpLogging<T>(this IApplicationBuilder app) where T : IHttpLoggerCommand
        {
            if (IsRunExceptionHandler) throw new InvalidOperationException("UseExceptionHandler and UseHttpLogging cannot be used at the same time.");
            IsRunUseHttpLogging = true;
            ArgumentNullException.ThrowIfNull(app);
            app.UseMiddleware<HttpLoggingMiddleware<T>>();

            // Dummy. 절대 UseMiddleware랑 순서 바꾸지 말것
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    await Task.Run(async () =>
                    {
                        await RunAsync();

                        static Task RunAsync()
                        {
                            return Task.CompletedTask;
                        }
                    });
                });
            });

            IsRunUseHttpLogging = true;
            return app;
        }
    }
}