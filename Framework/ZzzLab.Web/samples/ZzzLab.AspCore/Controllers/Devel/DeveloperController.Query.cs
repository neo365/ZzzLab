using Microsoft.AspNetCore.Mvc;
using ZzzLab.Data;
using ZzzLab.Web.Models;

namespace ZzzLab.AspCore.Controllers
{
    public partial class DeveloperController
    {
        /// <summary>
        /// 쿼리의 리턴값을 가져온다.
        /// </summary>
        /// <param name="req">QueryRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query/Select")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult QuerySelectFromForm([FromForm] QueryRequest req)
        {
            return QuerySelectFromBody(req);
        }

        /// <summary>
        /// 쿼리의 리턴값을 가져온다.
        /// </summary>
        /// <param name="req">QueryRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query/Select")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult QuerySelectFromBody([FromBody]QueryRequest req)
        {
            try
            {
                if (req == null) return RestResult.BadRequest();
                if (string.IsNullOrEmpty(req.Command)) return RestResult.BadRequest();

                QueryParameterCollection parameters = new QueryParameterCollection();

                if (req.Parameters != null && req.Parameters.Any())
                {
                    foreach (var parameter in req.Parameters)
                    {
                        parameters.Add(parameter.Name, parameter.Value);
                    }
                }

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.Grid(DB.Select(req.Command, parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 쿼리를 실행한다.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Query/Execute")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult QueryExecute(QueryRequest req)
        {
            try
            {
                if (req == null) return RestResult.BadRequest();
                if (string.IsNullOrEmpty(req.Command)) return RestResult.BadRequest();

                QueryParameterCollection parameters = new QueryParameterCollection();

                if (req.Parameters != null && req.Parameters.Any())
                {
                    foreach (var parameter in req.Parameters)
                    {
                        parameters.Add(parameter.Name, parameter.Value);
                    }
                }

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.Ok(DB.Excute(req.Command, parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        public class QueryRequest
        {
            public string? Command { set; get; }
            public IEnumerable<CommandParameter>? Parameters { set; get; } = Enumerable.Empty<CommandParameter>();

            public override string ToString()
                => $"{Command}";
        }

        public class CommandParameter
        {
            public string? Name { set; get; }
            public object? Value { set; get; }

            public override string ToString()
                => $"{Name}: {Value}";
        }
    }
}