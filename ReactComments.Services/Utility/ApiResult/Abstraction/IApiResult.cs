using Newtonsoft.Json;

namespace ReactComments.Services.Utility.ApiResult.Abstraction
{
    public enum ApiResultStatus
    {
        Ok,
        NotFound,
        BadRequest,
        Conflict,
        NoContent,
        ValidationFailed
    }

    public interface IApiResult
    {
        [JsonIgnore]
        ApiResultStatus ApiResultStatus { get; set; }
        string Message { get; set; }
        void SetResult(ApiResultStatus apiResultStatus, string message = null);
    }
}
