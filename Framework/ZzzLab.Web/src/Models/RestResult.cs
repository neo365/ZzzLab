using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Net;
using ZzzLab.ExceptionEx;
using ZzzLab.Models.Auth;
using ZzzLab.Net.Http;
using ZzzLab.Web.Models.Auth;

namespace ZzzLab.Web.Models
{
    public static class RestResult
    {
        public const int BASE_OK_CODE = 2000;
        public const int BASE_FAIL_CODE = 2000;
        public const int BASE_NOAUTH_CODE = 4010;
        public const int BASE_PROBLEM_CODE = 5000;
        public const int BASE_EXCEPTION_CODE = 5000;

        private static readonly JsonSerializerSettings JSON_SETTING = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore
        };

        #region IActionResult

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

        #endregion IActionResult

        private static IActionResult ResultWithItem<T>(HttpStatusCode statusCode, bool success, T? item, string? message = null, int code = BASE_OK_CODE, string? trackingId = null)
        {
            if (item is null)
            {
                RestResponse res = new RestResponse()
                {
                    TrackingId = trackingId ?? Guid.NewGuid().ToString(),
                    Success = success,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Code = code
                };
                return GetResult(res);
            }
            if (item is DataRow row)
            {
                RestItemResponse<dynamic> res = new RestItemResponse<dynamic>()
                {
                    TrackingId = trackingId ?? Guid.NewGuid().ToString(),
                    Success = success,
                    Message = message,
                    Item = row.ToModeling(),
                    StatusCode = (int)statusCode,
                    Code = code
                };

                return GetResult(res);
            }
            else if (item is DataTable table) return Grid(table);
            else
            {
                RestItemResponse<T> res = new RestItemResponse<T>()
                {
                    TrackingId = trackingId ?? Guid.NewGuid().ToString(),
                    Success = success,
                    Message = message,
                    Item = item,
                    StatusCode = (int)statusCode,
                    Code = code
                };

                return GetResult(res);
            }
        }

        public static IActionResult Ok(int code = BASE_OK_CODE, string? trackingId = null)
            => ResultWithItem<string>(HttpStatusCode.OK, true, null, null, code, trackingId);

        public static IActionResult OkWithMessage(string? message, int code = BASE_OK_CODE, string? trackingId = null)
            => ResultWithItem<string>(HttpStatusCode.OK, true, null, message, code, trackingId);

        public static IActionResult OkWithItem<T>(T item, string? message = null, int code = BASE_OK_CODE, string? trackingId = null)
            => ResultWithItem<T>(HttpStatusCode.OK, true, item, message, code, trackingId);

        public static IActionResult Fail(int code = BASE_FAIL_CODE, string? trackingId = null)
            => ResultWithItem<string>(HttpStatusCode.OK, false, null, null, code, trackingId);

        public static IActionResult FailWithMessage(string message, int code = BASE_FAIL_CODE, string? trackingId = null)
            => ResultWithItem<string>(HttpStatusCode.OK, false, null, message, code, trackingId);

        public static IActionResult FailWithItem<T>(T item, string? message = null, int code = BASE_FAIL_CODE, string? trackingId = null)
            => ResultWithItem<T>(HttpStatusCode.OK, false, item, message, code, trackingId);

        public static IActionResult Fail(string message, int code = BASE_FAIL_CODE, string? trackingId = null)
            => FailWithMessage(message, code, trackingId);

        public static IActionResult Fail(Exception ex, int code = BASE_EXCEPTION_CODE, string? trackingId = null)
            => Problem(ex, code, trackingId);

        public static IActionResult Fail(IEnumerable<ExceptionInfo> collection, int code = BASE_EXCEPTION_CODE, string? trackingId = null)
            => Problem(collection, code, trackingId);

        public static IActionResult OkOrFail(bool success, string? trackingId = null)
            => success ? Ok(trackingId: trackingId) : Fail(trackingId: trackingId);

        public static IActionResult OkOrFail(object result, string? trackingId = null)
            => result != null ? OkWithItem(result, trackingId: trackingId) : Fail(trackingId: trackingId);

        public static IActionResult OkOrFail<T>(bool success, T item, string? trackingId = null)
            => success ? OkWithItem(item, trackingId: trackingId) : FailWithItem(item, trackingId: trackingId);

        /// <summary>
        /// 성공실패 리턴. 실패시에는 메세지입력가능
        /// </summary>
        /// <param name="success">성공여부</param>
        /// <param name="message">메세지</param>
        /// <returns></returns>
        public static IActionResult OkOrFail(bool success, string message, int code = BASE_EXCEPTION_CODE, string? trackingId = null)
            => success ? Ok(trackingId: trackingId) : Fail(message, code, trackingId: trackingId);

        public static IActionResult AuthOK<T>(OAuthSuccess<T> item, string? trackingId = null) where T : LoginEntity
         => ResultWithItem<OAuthSuccess<T>>(HttpStatusCode.OK, true, item, null, BASE_OK_CODE, trackingId);

        public static IActionResult AuthFail(OAuthError item, string? trackingId = null)
            => ResultWithItem<OAuthError>(HttpStatusCode.Unauthorized, true, item, null, BASE_OK_CODE, trackingId);

        /// <summary>
        /// 페이지 리다이렉션 <br />
        /// (see : https://docs.microsoft.com/ko-kr/dotnet/api/microsoft.aspnetcore.mvc.redirectresult?view=aspnetcore-6.0)
        /// </summary>
        /// <param name="url">이동할 url.  The URL to redirect to.</param>
        /// <param name="isPermanent">리디렉션이 영구적 이어야 함을 지정. Specifies whether the redirect should be permanent (301) or temporary (302). </param>
        /// <returns></returns>
        public static IActionResult Redirect(string url, bool isPermanent = false)
            => new RedirectResult(url, isPermanent);

        public static IActionResult BadRequest(string? trackingId = null)
            => BadRequest(null, trackingId);

        public static IActionResult BadRequest(string? message, string? trackingId = null)
            => Problem(HttpStatusCode.BadRequest, message, trackingId: trackingId);

        public static IActionResult Unauthorized(string? trackingId = null)
            => Unauthorized(null, trackingId);

        public static IActionResult Unauthorized(string? message, string? trackingId = null)
            => Problem(HttpStatusCode.Unauthorized, message, trackingId: trackingId);

        public static IActionResult NotFound(string? trackingId = null)
             => NotFound(null, trackingId);

        public static IActionResult NotFound(string? message, string? trackingId = null)
            => Problem(HttpStatusCode.NotFound, message, trackingId: trackingId);

        public static IActionResult Problem(HttpStatusCode statusCode, string? message, string? description = null, int code = BASE_PROBLEM_CODE, string? trackingId = null)
        {
            RestServerErrorResult res = new RestServerErrorResult()
            {
                TrackingId = trackingId ?? Guid.NewGuid().ToString(),
                StatusCode = (int)statusCode,
                ErrorMessage = message,
                ErrorDescription = description ?? statusCode.ToStatusMessage(),
                Code = code
            };

            return GetResult(res);
        }

        public static IActionResult Problem(string message, string? description = null, int code = BASE_PROBLEM_CODE, string? trackingId = null)
            => Problem(HttpStatusCode.InternalServerError, message, description, code, trackingId);

        public static IActionResult Problem(Exception ex, int code = BASE_EXCEPTION_CODE, string? trackingId = null)
        {
            RestServerErrorResult res = new RestServerErrorResult()
            {
                TrackingId = trackingId ?? Guid.NewGuid().ToString(),
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = ex.GetAllMessages(),
                Error = ex.GetAllExceptionInfo(),
                ErrorDescription = HttpStatusCode.InternalServerError.ToStatusMessage(),
                Code = code
            };

            return GetResult(res);
        }

        public static IActionResult Problem(IEnumerable<ExceptionInfo> collection, int code = BASE_EXCEPTION_CODE, string? trackingId = null)
        {
            RestServerErrorResult res = new RestServerErrorResult()
            {
                TrackingId = trackingId ?? Guid.NewGuid().ToString(),
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = collection.GetAllMessages(),
                Error = collection,
                ErrorDescription = HttpStatusCode.InternalServerError.ToStatusMessage(),
                Code = code
            };

            return GetResult(res);
        }

        public static IActionResult Grid<T>(IEnumerable<T> data, int recordsTotal = -1, int recordsFiltered = -1, int code = BASE_OK_CODE, string? trackingId = null)
            => Grid(Enumerable.Empty<object>(), data, recordsTotal, recordsFiltered, code, trackingId);

        public static IActionResult Grid<T>(IEnumerable<object>? headers, IEnumerable<T> data, int recordsTotal = -1, int recordsFiltered = -1, int code = BASE_OK_CODE, string? trackingId = null)
        {
            if (data == null || data.Any() == false)
            {
                data = Enumerable.Empty<T>();
                recordsFiltered = 0;
            }

            RestGridResponse<T> res = new RestGridResponse<T>()
            {
                TrackingId = trackingId ?? Guid.NewGuid().ToString(),
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Headers = (headers == null || headers.Any() == false ? null : headers),
                Items = data,
                RecordsTotal = (recordsTotal < 0 ? data.Count() : recordsTotal),
                RecordsFiltered = (recordsFiltered < 0 ? data.Count() : recordsFiltered),
                Code = code
            };

            return GetResult(res);
        }

        public static IActionResult Grid(DataTable table, int recordsTotal = -1, int recordsFiltered = -1, int code = BASE_OK_CODE, string? trackingId = null)
        {
            if (table == null || table.Rows.Count == 0) return Empty();

            List<string> list = new List<string>();
            foreach (DataColumn c in table.Columns)
            {
                list.Add(c.ColumnName);
            }

            return Grid<dynamic>(
                list,
                table.ToModeling(),
                (recordsTotal < 0 ? table.Rows.Count : recordsTotal),
                (recordsFiltered < 0 ? table.Rows.Count : recordsFiltered),
                code,
                trackingId);
        }

        public static IActionResult Empty(string? trackingId = null)
            => Grid<dynamic>(null, Enumerable.Empty<dynamic>(), 0, 0, BASE_OK_CODE, trackingId);
    }
}