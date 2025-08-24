using ReactComments.DAL.Model;
using ReactComments.Services.Model;
using ReactComments.Services.Utility.ApiResult.Abstraction;
using System.Linq.Expressions;

namespace ReactComments.Services.Abstraction
{
    public interface ICommentService
    {
        IApiResult AddComment(CommentDTO commentDTO);
        Task<IApiResult> AddCommentAsync(CommentDTO commentDTO);
        IApiResult DeleteComment(object id);
        Task<IApiResult> DeleteCommentAsync(object id);
        IApiResult GetComments();
        Task<IApiResult> GetCommentsAsync();
        IApiResult GetCommentsByCondition(Expression<Func<Comment, bool>> condition);
        Task<IApiResult> GetCommentsByConditionAsync(Expression<Func<Comment, bool>> condition);
        IApiResult UpdateComment(CommentDTO commentDTO);
        Task<IApiResult> UpdateCommentAsync(CommentDTO commentDTO);
    }
}
