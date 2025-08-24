namespace ReactComments.Services.Utility.ApiResult.Abstraction
{
    public interface IApiOkResult : IApiResult
    {
        object Data { get; set; }
        void SetOkResult(ApiResultStatus apiResultStatus = ApiResultStatus.Ok, string message = null, object data = null);
    }
}
