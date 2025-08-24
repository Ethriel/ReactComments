using ReactComments.Services.Utility.ApiResult.Abstraction;

namespace ReactComments.Services.Utility.ApiResult.Implementation
{
    public abstract class ApiResult : IApiResult
    {
        public ApiResultStatus ApiResultStatus { get; set; }
        public string Message { get; set; }
        public ApiResult() { }
        public ApiResult(ApiResultStatus apiResultStatus, string message = null) 
        {
            SetResult(ApiResultStatus, message);
        }

        public void SetResult(ApiResultStatus apiResultStatus, string message = null)
        {
            ApiResultStatus = apiResultStatus;
            Message = message;
        }
    }
}
