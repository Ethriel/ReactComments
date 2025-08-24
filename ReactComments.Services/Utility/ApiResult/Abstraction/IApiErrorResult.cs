using Newtonsoft.Json;

namespace ReactComments.Services.Utility.ApiResult.Abstraction
{
    public interface IApiErrorResult : IApiResult
    {
        IEnumerable<string> Errors { get; set; }
        [JsonIgnore]
        string LoggerMessage { get; set; }

        void SetErrorResult(ApiResultStatus apiResultStatus = ApiResultStatus.BadRequest,
            string loggerMessage = "API request failed", string message = null, IEnumerable<string> errors = null);
    }
}
