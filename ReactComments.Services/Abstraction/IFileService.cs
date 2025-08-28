using Microsoft.AspNetCore.Http;
using ReactComments.Services.Model;

namespace ReactComments.Services.Abstraction
{


    public interface IFileService
    {
        CommentDTO? UploadFile(CommentDTO comment, IFormFile file);
        Task<CommentDTO?> UploadFileAsync(CommentDTO comment, IFormFile file);
    }
}
