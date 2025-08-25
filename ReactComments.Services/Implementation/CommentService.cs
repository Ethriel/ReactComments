using Microsoft.Extensions.Logging;
using ReactComments.DAL.Model;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Model;
using ReactComments.Services.Utility.ApiResult.Abstraction;
using ReactComments.Services.Utility.ApiResult.Implementation;
using System.Linq.Expressions;

namespace ReactComments.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IEntityExtendedService<Comment> commentService;
        private readonly IMapperService<Comment, CommentDTO> mapperService;
        private readonly ILogger<ICommentService> logger;
        private readonly IFileService fileService;

        public CommentService(IEntityExtendedService<Comment> commentService,
                              IMapperService<Comment, CommentDTO> mapperService,
                              ILogger<ICommentService> logger, IFileService fileService)
        {
            this.commentService = commentService;
            this.mapperService = mapperService;
            this.logger = logger;
            this.fileService = fileService;
        }
        public IApiResult AddComment(CommentDTO commentDTO)
        {
            var apiResult = default(IApiResult);

            var existingComment = FindCommentById(commentDTO.Id);
            if (existingComment is null)
            {
                var commentFromDto = mapperService.MapEntity(commentDTO);
                if (commentFromDto is null)
                {
                    var mappingErrorMsg = "Invalid mapping result";
                    var loggerErrorMsg = $"{mappingErrorMsg}. Attempted for comment |id: {commentDTO.Id}|";
                    logger.LogError(loggerErrorMsg);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [mappingErrorMsg], loggerErrorMessage: loggerErrorMsg);
                }
                else
                {
                    var addResult = commentService.Create(commentFromDto);
                    if (addResult)
                    {
                        var comment = FindCommentById(commentDTO.Id);
                        var commentToReturnDto = mapperService.MapDto(comment);
                        apiResult = new ApiOkResult(ApiResultStatus.Ok, data: commentToReturnDto);
                    }
                    else
                    {
                        var errorMessage = "Failed to add new comment";
                        var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {commentDTO.Id}|";
                        logger.LogError(loggerErrorMsg);
                        apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
                    }
                }
            }
            else
            {
                var errorMessage = $"Comment |{commentDTO.Id}| already exists!";
                logger.LogError(errorMessage);
                apiResult = new ApiErrorResult(ApiResultStatus.Conflict, errors: [errorMessage]);
            }

            return apiResult;
        }

        public async Task<IApiResult> AddCommentAsync(CommentDTO commentDTO)
        {
            return await Task.FromResult(AddComment(commentDTO));
        }

        public IApiResult CommentDetails(object id)
        {
            var apiResult = default(IApiResult);

            var existingComment = FindCommentById(id);
            if (existingComment is null)
            {
                var errorMessage = "Failed to retreive a comment. Comment does not exist!";
                var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {id}|";
                logger.LogError(loggerErrorMsg);
                apiResult = new ApiErrorResult(ApiResultStatus.NotFound, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }
            else
            {
                var commentDto = mapperService.MapDto(existingComment);
                apiResult = new ApiOkResult(ApiResultStatus.Ok, data: commentDto);
            }

            return apiResult;
        }

        public async Task<IApiResult> CommentDetailsAsync(object id)
        {
            return await Task.FromResult(CommentDetails(id));
        }

        public IApiResult DeleteComment(object id)
        {
            var apiResult = default(IApiResult);

            var existingComment = FindCommentById(id);
            if (existingComment is null)
            {
                var errorMessage = "Failed to delete a comment. Comment does not exist!";
                var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {id}|";
                logger.LogError(loggerErrorMsg);
                apiResult = new ApiErrorResult(ApiResultStatus.NotFound, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }
            else
            {
                var deleteResult = commentService.Delete(id);
                if (deleteResult)
                {
                    apiResult = new ApiOkResult(ApiResultStatus.NoContent);
                }
                else
                {
                    var errorMessage = "Failed to delete a comment";
                    var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {id}|";
                    logger.LogError(loggerErrorMsg);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> DeleteCommentAsync(object id)
        {
            return await Task.FromResult(DeleteComment(id));
        }

        public IApiResult GetComments()
        {
            var apiResult = default(IApiResult);

            var comments = commentService.Read().ToArray();
            if (comments is null || comments.Length == 0)
            {
                var noCommentsMessage = "No comments to show";
                logger.LogWarning(noCommentsMessage);
                apiResult = new ApiOkResult(ApiResultStatus.NoContent, noCommentsMessage);
            }
            else
            {
                var commentsDtos = mapperService.MapDtos(comments);
                apiResult = new ApiOkResult(ApiResultStatus.Ok, data: commentsDtos);
            }

            return apiResult;
        }

        public async Task<IApiResult> GetCommentsAsync()
        {
            return await Task.FromResult(GetComments());
        }

        public IApiResult GetCommentsByCondition(Expression<Func<Comment, bool>> condition)
        {
            var apiResult = default(IApiResult);

            var comments = commentService.ReadEntitiesByCondition(condition).ToArray();
            if (comments is null || comments.Length == 0)
            {
                var noCommentsMessage = "No comments to show";
                logger.LogWarning(noCommentsMessage);
                apiResult = new ApiOkResult(ApiResultStatus.NoContent, noCommentsMessage);
            }
            else
            {
                var commentsDtos = mapperService.MapDtos(comments);
                apiResult = new ApiOkResult(ApiResultStatus.Ok, data: commentsDtos);
            }

            return apiResult;
        }

        public async Task<IApiResult> GetCommentsByConditionAsync(Expression<Func<Comment, bool>> condition)
        {
            return await Task.FromResult(GetCommentsByCondition(condition));
        }

        public IApiResult UpdateComment(CommentDTO commentDTO)
        {
            var apiResult = default(IApiResult);

            var existingComment = FindCommentById(commentDTO.Id);
            if (existingComment is null)
            {
                var errorMessage = "Failed to update a comment. Comment does not exist!";
                var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {commentDTO.Id}|";
                logger.LogError(loggerErrorMsg);
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }
            else
            {
                var toUpdateComment = mapperService.MapEntity(commentDTO);
                var updateResultComment = commentService.Update(existingComment, toUpdateComment);
                if (updateResultComment is not null)
                {
                    var commentToReturnDto = mapperService.MapDto(updateResultComment);
                    apiResult = new ApiOkResult(ApiResultStatus.Ok, data: commentToReturnDto);
                }
                else
                {
                    var errorMessage = "Failed to update a comment";
                    var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {commentDTO.Id}|";
                    logger.LogError(loggerErrorMsg);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> UpdateCommentAsync(CommentDTO commentDTO)
        {
            return await Task.FromResult(UpdateComment(commentDTO));
        }

        public IApiResult UploadFile(CommentUploadFile uploadFile)
        {
            var fileTypeStr = string.Empty;
            var errorMessage = "Upload failed";

            var commentWithFileDto = fileService.UploadFile(uploadFile);
            if (commentWithFileDto is null)
            {
                return new ApiErrorResult(ApiResultStatus.BadRequest, $"{errorMessage}. Comment id: |{uploadFile.Comment.Id}|", errorMessage, [errorMessage]);
            }
            else
            {
                var relatedComment = commentService.ReadById(uploadFile.Comment.Id);
                if (relatedComment is null)
                {
                    return new ApiErrorResult(ApiResultStatus.NotFound, $"{errorMessage}. Comment not found, Id: |{uploadFile.Comment.Id}|", errorMessage, [errorMessage]);
                }
                else
                {
                    var commentWithFile = mapperService.MapEntity(commentWithFileDto);
                    var updateResult = commentService.Update(relatedComment, commentWithFile);
                    var updatedCommentDto = mapperService.MapDto(updateResult);
                    return new ApiOkResult(ApiResultStatus.Ok, data: updatedCommentDto);
                }
            }

        }

        public async Task<IApiResult> UploadFileAsync(CommentUploadFile uploadFile)
        {
            return await Task.FromResult(UploadFile(uploadFile));
        }

        private Comment? FindCommentById(object id)
        {
            return commentService.ReadById(id);
        }
    }
}
