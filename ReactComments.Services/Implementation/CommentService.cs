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
        private readonly IMapperService<Comment, CommentDTO> commentMapperService;
        private readonly IMapperService<CommentDTO, SubmitComment> submitCommentMapperService;
        private readonly ILogger<ICommentService> logger;
        private readonly IFileService fileService;

        public CommentService(IEntityExtendedService<Comment> commentService,
                              IMapperService<Comment, CommentDTO> commentMapperService,
                              IMapperService<CommentDTO, SubmitComment> submitCommentMapperService,
                              ILogger<ICommentService> logger,
                              IFileService fileService)
        {
            this.commentService = commentService;
            this.commentMapperService = commentMapperService;
            this.submitCommentMapperService = submitCommentMapperService;
            this.logger = logger;
            this.fileService = fileService;
        }
        public IApiResult AddComment(SubmitComment submitComment)
        {
            if (!Guid.TryParse(submitComment.Id, out var guid))
            {
                guid = new Guid();
            }
            var existingComment = FindCommentById(guid);
            if (existingComment != null)
            {
                var errorMessage = $"Comment |{submitComment.Id}| already exists!";
                logger.LogError(errorMessage);
                return new ApiErrorResult(ApiResultStatus.Conflict, errors: [errorMessage]);
            }

            var commentDtoFromSubmit = submitCommentMapperService.MapEntity(submitComment);
            if (commentDtoFromSubmit == null)
            {
                var mappingErrorMsg = "Invalid mapping result";
                var loggerErrorMsg = $"{mappingErrorMsg}. Attempted for comment |id: {submitComment.Id}|";
                logger.LogError(loggerErrorMsg);
                return new ApiErrorResult(ApiResultStatus.BadRequest, errors: [mappingErrorMsg], loggerErrorMessage: loggerErrorMsg);
            }

            if (submitComment.File != null)
            {
                commentDtoFromSubmit = fileService.UploadFile(commentDtoFromSubmit, submitComment.File);
                if (commentDtoFromSubmit == null)
                {
                    var mappingErrorMsg = "Could not upload a file";
                    var loggerErrorMsg = $"{mappingErrorMsg}. Attempted for comment |id: {submitComment.Id}|";
                    logger.LogError(loggerErrorMsg);
                    return new ApiErrorResult(ApiResultStatus.BadRequest, errors: [mappingErrorMsg], loggerErrorMessage: loggerErrorMsg);
                }
            }

            commentDtoFromSubmit.CreatedAt = DateTime.Now.ToString("o");
            var commentFromDto = commentMapperService.MapEntity(commentDtoFromSubmit);
            var addResult = commentService.Create(commentFromDto);
            if (addResult == null)
            {
                var errorMessage = "Failed to add new comment";
                var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {submitComment.Id}|";
                logger.LogError(loggerErrorMsg);
                return new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }

            var commentToReturnDto = commentMapperService.MapDto(addResult);
            return new ApiOkResult(ApiResultStatus.Ok, data: commentToReturnDto);
        }

        public async Task<IApiResult> AddCommentAsync(SubmitComment submitComment)
        {
            return await Task.FromResult(AddComment(submitComment));
        }

        public IApiResult AddReply(SubmitComment submitComment)
        {
            if (submitComment.ParentCommentId == null)
            {
                return new ApiErrorResult(ApiResultStatus.BadRequest,
                                          "Parent comment id is missing",
                                          "Add reply failed",
                                          ["Parent comment is missing!"]);
            }

            return AddComment(submitComment);
        }

        public async Task<IApiResult> AddReplyAsync(SubmitComment submitComment)
        {
            return await Task.FromResult(AddReply(submitComment));
        }

        public IApiResult CommentDetails(object id)
        {
            var errorMessage = "Failed to retreive a comment";

            if (!Guid.TryParse(id.ToString(), out var guid))
            {
                return new ApiErrorResult(ApiResultStatus.BadRequest, $"{errorMessage}. Id: |{id}|", errorMessage, ["Invalid guid"]);
            }

            var existingComment = FindCommentById(guid);
            if (existingComment is null)
            {
                errorMessage += "Comment does not exist!";
                var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {id}|";
                logger.LogError(loggerErrorMsg);
                return new ApiErrorResult(ApiResultStatus.NotFound, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }

            var commentDto = commentMapperService.MapDto(existingComment);
            return new ApiOkResult(ApiResultStatus.Ok, data: commentDto);
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
            var comments = commentService.Read().ToArray();
            if (comments is null || comments.Length == 0)
            {
                var noCommentsMessage = "No comments to show";
                logger.LogWarning(noCommentsMessage);
                return new ApiOkResult(ApiResultStatus.NoContent, noCommentsMessage);
            }

            var commentsDtos = commentMapperService.MapDtos(comments);
            return new ApiOkResult(ApiResultStatus.Ok, data: commentsDtos);
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
                var commentsDtos = commentMapperService.MapDtos(comments);
                apiResult = new ApiOkResult(ApiResultStatus.Ok, data: commentsDtos);
            }

            return apiResult;
        }

        public async Task<IApiResult> GetCommentsByConditionAsync(Expression<Func<Comment, bool>> condition)
        {
            return await Task.FromResult(GetCommentsByCondition(condition));
        }

        public IApiResult GetTopLevelComments()
        {
            var topLevelComments = commentService.ReadEntitiesByCondition(c => c.ParentCommentId == null).ToArray();
            var topLevelCommentsDtos = commentMapperService.MapDtos(topLevelComments);

            return new ApiOkResult(ApiResultStatus.Ok, data: topLevelCommentsDtos);
        }

        public async Task<IApiResult> GetTopLevelCommentsAsync()
        {
            return await Task.FromResult(GetTopLevelComments());
        }

        public IApiResult UpdateComment(SubmitComment submitComment)
        {
            var errorMessage = "Failed to update a comment";
            var loggerErrorMsg = $"{errorMessage}. Attempted for comment |id: {submitComment.Id}|";

            var existingComment = FindCommentById(submitComment.Id);
            if (existingComment is null)
            {
                logger.LogError(loggerErrorMsg);
                return new ApiErrorResult(ApiResultStatus.BadRequest, errors: ["Comment does not exist!"], loggerErrorMessage: loggerErrorMsg);
            }

            var toUpdateCommentDTO = submitCommentMapperService.MapEntity(submitComment);
            if (toUpdateCommentDTO is null)
            {
                logger.LogError(loggerErrorMsg);
                return new ApiErrorResult(ApiResultStatus.BadRequest, errors: ["Mapper error"], loggerErrorMessage: loggerErrorMsg);
            }

            var toUpdateComment = commentMapperService.MapEntity(toUpdateCommentDTO);

            var updateResultComment = commentService.Update(existingComment, toUpdateComment);
            if (updateResultComment is null)
            {
                logger.LogError(loggerErrorMsg);
                return new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }

            var commentToReturnDto = commentMapperService.MapDto(updateResultComment);
            return new ApiOkResult(ApiResultStatus.Ok, data: commentToReturnDto);
        }

        public async Task<IApiResult> UpdateCommentAsync(SubmitComment submitComment)
        {
            return await Task.FromResult(UpdateComment(submitComment));
        }

        public IApiResult UploadFile(CommentUploadFile uploadFile)
        {
            var fileTypeStr = string.Empty;
            var errorMessage = "Upload failed";

            var relatedComment = commentService.ReadById(uploadFile.CommentId);
            if (relatedComment is null)
            {
                return new ApiErrorResult(ApiResultStatus.NotFound, $"{errorMessage}. Comment not found, Id: |{uploadFile.CommentId}|", errorMessage, [errorMessage]);
            }

            var relatedCommentDto = commentMapperService.MapDto(relatedComment);

            relatedCommentDto = fileService.UploadFile(relatedCommentDto, uploadFile.File);
            if (relatedCommentDto is null)
            {
                return new ApiErrorResult(ApiResultStatus.BadRequest, $"{errorMessage}. Comment id: |{uploadFile.CommentId}|", errorMessage, [errorMessage]);
            }

            var commentWithFile = commentMapperService.MapEntity(relatedCommentDto);
            var updateResult = commentService.Update(relatedComment, commentWithFile);
            var updatedCommentDto = commentMapperService.MapDto(updateResult);

            return new ApiOkResult(ApiResultStatus.Ok, data: updatedCommentDto);
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
