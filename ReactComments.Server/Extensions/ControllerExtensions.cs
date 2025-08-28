using Microsoft.AspNetCore.Mvc;
using ReactComments.Services.Utility.ApiResult.Abstraction;

namespace ReactComments.Server.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ActionResultByApiResult(this Controller controller, IApiResult apiResult, ILogger logger)
        {
            var apiErrorResult = default(IApiErrorResult);
            var apiOkResult = default(IApiOkResult);

            if (apiResult is IApiErrorResult)
                apiErrorResult = apiResult as IApiErrorResult;

            if (apiResult is IApiOkResult)
                apiOkResult = apiResult as IApiOkResult;

                switch (apiResult.ApiResultStatus)
                {
                    case ApiResultStatus.Ok:
                        return controller.Ok(apiOkResult?.Data);
                    case ApiResultStatus.NotFound:
                        logger.LogWarning(message: apiErrorResult?.LoggerMessage);
                        return controller.NotFound(apiErrorResult?.Errors);
                    case ApiResultStatus.Conflict:
                        logger.LogWarning(message: apiErrorResult?.LoggerMessage);
                        return controller.Conflict(apiErrorResult?.Errors);
                    case ApiResultStatus.NoContent:
                        logger.LogInformation(message: apiResult.Message);
                        return controller.NoContent();
                    case ApiResultStatus.BadRequest:
                    case ApiResultStatus.ValidationFailed:
                    default:
                        logger.LogError(message: apiErrorResult?.LoggerMessage);
                        return controller.BadRequest(apiErrorResult?.Errors);
                }
        }
    }
}
