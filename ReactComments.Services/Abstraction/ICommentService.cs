using ReactComments.DAL.Model;
using ReactComments.Services.Model;
using ReactComments.Services.Utility.ApiResult.Abstraction;
using System.Linq.Expressions;

namespace ReactComments.Services.Abstraction
{
    public interface ICommentService
    {
        IApiResult AddComment(SubmitComment submitComment);
        Task<IApiResult> AddCommentAsync(SubmitComment submitComment);
        IApiResult AddReply(SubmitComment submitComment);
        Task<IApiResult> AddReplyAsync(SubmitComment submitComment);
        IApiResult CommentDetails(object id);
        Task<IApiResult> CommentDetailsAsync(object id);
        IApiResult DeleteComment(object id);
        Task<IApiResult> DeleteCommentAsync(object id);
        IApiResult GetComments();
        Task<IApiResult> GetCommentsAsync();
        IApiResult GetTopLevelComments();
        Task<IApiResult> GetTopLevelCommentsAsync();
        IApiResult GetCommentsByCondition(Expression<Func<Comment, bool>> condition);
        Task<IApiResult> GetCommentsByConditionAsync(Expression<Func<Comment, bool>> condition);
        IApiResult UpdateComment(SubmitComment submitComment);
        Task<IApiResult> UpdateCommentAsync(SubmitComment submitComment);
        IApiResult UploadFile(CommentUploadFile uploadFile);
        Task<IApiResult> UploadFileAsync(CommentUploadFile uploadFile);
    }
}
