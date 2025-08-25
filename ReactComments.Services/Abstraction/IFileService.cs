using ReactComments.Services.Model;

namespace ReactComments.Services.Abstraction
{


    public interface IFileService
    {
        CommentDTO UploadFile(CommentUploadFile uploadFile);
        Task<CommentDTO> UploadFileAsync(CommentUploadFile uploadFile);
    }
}
