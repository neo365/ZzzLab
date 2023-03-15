using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Net;
using ZzzLab.Json;
using ZzzLab.Models.Auth;
using ZzzLab.Net.Http;
using ZzzLab.Web.Models.Auth;
using JsonConvert = ZzzLab.Json.JsonConvert;
using ZzzLab.ExceptionEx;

namespace ZzzLab.Web.Models
{
    public class RestResult
    {
        private static readonly JsonSerializerSettings JSON_SETTING = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore
        };

        private static IActionResult GetResult<T>(int satusCode, T res) where T : class
        {
            return new ContentResult()
            {
                StatusCode = satusCode,
                Content = JsonConvert.ToJson<T>(res, JSON_SETTING),
            };
        }

        private static IActionResult GetResult(RestResponse res)
        {
            return new ContentResult()
            {
                StatusCode = (int)res.StatusCode,
                Content = res.ToJson(JSON_SETTING),
            };
        }

        private static IActionResult GetResult(RestErrorResult res)
        {
            return new ContentResult()
            {
                StatusCode = (int)res.StatusCode,
                Content = res.ToJson(JSON_SETTING),
            };
        }

        public static IActionResult Ok()
        {
            RestResponse res = new RestResponse()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
            };

            return GetResult(res);
        }

        public static IActionResult OkWithMessage(string message)
        {
            RestResponse res = new()
            {
                Success = true,
                Message = message,
                StatusCode = (int)HttpStatusCode.OK
            };

            return GetResult(res);
        }

        public static IActionResult Ok<T>(T item)
        {
            RestItemResponse<T> res = new RestItemResponse<T>()
            {
                Success = true,
                Message = null,
                Item = item,
                StatusCode = (int)HttpStatusCode.OK
            };

            return GetResult(res);
        }

        //public static IActionResult Ok(DataTable table)
        //    => Grid(table);

        public static IActionResult Ok(DataRow row)
        {
            RestItemResponse<dynamic> res = new RestItemResponse<dynamic>()
            {
                Success = true,
                Message = null,
                Item = row.ToModeling(),
                StatusCode = (int)HttpStatusCode.OK
            };

            return GetResult(res);
        }

        public static IActionResult Auth<T>(T auth) where T : class
            => GetResult<T>(200, auth);

        public static IActionResult Fail()
        {
            RestResponse res = new RestResponse()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.OK
            };

            return GetResult(res);
        }

        public static IActionResult Fail(string message)
            => FailWithMessage(message);

        public static IActionResult FailWithMessage(string message)
        {
            RestResponse res = new RestResponse()
            {
                Success = false,
                Message = message,
                StatusCode = (int)HttpStatusCode.OK
            };

            return GetResult(res);
        }

        public static IActionResult Fail(Exception ex)
        {
            RestServerErrorResult res = new RestServerErrorResult()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = ex.GetAllMessages(),
                Error = ex.GetAllExceptionInfo(),
                ErrorDescription = HttpStatusCode.InternalServerError.ToStatusMessage(),
            };

            //Logger.Fatal(ex);

            return GetResult(res);
        }

        public static IActionResult Fail(IEnumerable<ExceptionInfo> collection)
        {
            RestServerErrorResult res = new RestServerErrorResult()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = collection.GetAllMessages(),
                Error = collection,
                ErrorDescription = HttpStatusCode.InternalServerError.ToStatusMessage(),
            };

            //Logger.Fatal(ex);

            return GetResult(res);
        }

        public static IActionResult OkOrFail(bool success)
            => success ? Ok() : Fail();

        public static IActionResult OkOrFail(int result)
            => result > 0 ? Ok() : Fail();

        public static IActionResult OkOrFail(object result)
            => result != null ? Ok(result) : Fail();

        /// <summary>
        /// 성공실패 리턴. 실패시에는 메세지입력가능
        /// </summary>
        /// <param name="success">성공여부</param>
        /// <param name="message">메세지</param>
        /// <returns></returns>
        public static IActionResult OkOrFail(bool success, string message)
            => success ? OkWithMessage(message) : Fail(message);

        public static IActionResult OkOrFail(string? value)
            => string.IsNullOrWhiteSpace(value) ? Fail() : Ok(value);


        public static IActionResult Custom<T>(T item) where T : ResponseBase
        {
            return new ContentResult()
            {
                StatusCode = item.StatusCode,
                Content = item.ToJson(JSON_SETTING),
            };
        }

        public static IActionResult OK<T>(OAuthSuccess<T> item) where T : LoginEntity
        {
            return new ContentResult()
            {
                StatusCode = 200,
                Content = item.ToJson(JSON_SETTING),
            };
        }

        public static IActionResult Fail(OAuthError item)
        {
            return new ContentResult()
            {
                StatusCode = 500,
                Content = item.ToJson(JSON_SETTING),
            };
        }

        /// <summary>
        /// 페이지 리다이렉션 <br />
        /// (see : https://docs.microsoft.com/ko-kr/dotnet/api/microsoft.aspnetcore.mvc.redirectresult?view=aspnetcore-6.0)
        /// </summary>
        /// <param name="url">이동할 url.  The URL to redirect to.</param>
        /// <param name="isPermanent">리디렉션이 영구적 이어야 함을 지정. Specifies whether the redirect should be permanent (301) or temporary (302). </param>
        /// <returns></returns>
        public static IActionResult Redirect(string url, bool isPermanent = false)
            => new RedirectResult(url, isPermanent);

        public static IActionResult BadRequest()
            => Problem(HttpStatusCode.BadRequest, null);

        public static IActionResult BadRequest(string message)
            => Problem(HttpStatusCode.BadRequest, message);

        public static IActionResult Unauthorized()
            => Problem(HttpStatusCode.Unauthorized, null);

        public static IActionResult Unauthorized(string message)
            => Problem(HttpStatusCode.Unauthorized, message);

        public static IActionResult NotFound()
             => Problem(HttpStatusCode.NotFound, null);

        public static IActionResult NotFound(string message)
            => Problem(HttpStatusCode.NotFound, message);

        public static IActionResult Problem(HttpStatusCode statusCode, string? message)
        {
            RestServerErrorResult res = new RestServerErrorResult()
            {
                StatusCode = (int)statusCode,
                ErrorMessage = message,
                ErrorDescription = statusCode.ToStatusMessage()
            };

            return GetResult(res);
        }

        public static IActionResult Problem(string message)
            => Problem(HttpStatusCode.InternalServerError, message);

        public static IActionResult Problem(Exception ex)
            => Fail(ex);

        public static IActionResult Grid<T>(IEnumerable<T> data, int recordsTotal = -1, int recordsFiltered = -1)
            => Grid(Enumerable.Empty<object>(), data, recordsTotal, recordsFiltered);

        public static IActionResult Grid<T>(IEnumerable<object>? headers, IEnumerable<T> data, int recordsTotal = -1, int recordsFiltered = -1)
        {
            if (data == null || data.Any() == false)
            {
                data = Enumerable.Empty<T>();
                recordsFiltered = 0;
            }

            RestGridResponse<T> res = new RestGridResponse<T>()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Headers = headers,
                Items = data,
                RecordsTotal = (recordsTotal < 0 ? data.Count() : recordsTotal),
                RecordsFiltered = (recordsFiltered < 0 ? data.Count() : recordsFiltered),
            };

            return GetResult(res);
        }

        public static IActionResult Grid(DataTable table, int recordsTotal = -1, int recordsFiltered = -1)
        {
            if (table == null || table.Rows.Count == 0) return Empty();

            List<string> list = new List<string>();
            foreach (DataColumn c in table.Columns)
            {
                list.Add(c.ColumnName);
            }

            RestGridResponse<dynamic> res = new RestGridResponse<dynamic>()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Headers = list,
                Items = table.ToModeling(),
                RecordsTotal = (recordsTotal < 0 ? table.Rows.Count : recordsTotal),
                RecordsFiltered = (recordsFiltered < 0 ? table.Rows.Count : recordsFiltered)
            };

            return GetResult(res);
        }

        public static IActionResult Empty()
        {
            RestGridResponse<dynamic> res = new RestGridResponse<dynamic>()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Items = Enumerable.Empty<dynamic>(),
                RecordsTotal = 0,
                RecordsFiltered = 0,
            };

            return GetResult(res);
        }

        public static IActionResult Empty<T>()
        {
            RestGridResponse<T> res = new RestGridResponse<T>()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Items = Enumerable.Empty<T>(),
                RecordsTotal = 0,
                RecordsFiltered = 0,
            };

            return GetResult(res);
        }
    }
}