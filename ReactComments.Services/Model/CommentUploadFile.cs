using Microsoft.AspNetCore.Http;

namespace ReactComments.Services.Model
{
    public class CommentUploadFile
    {
        // make just as id
        public CommentDTO Comment { get; set; }
        public int UserId { get; set; }
        public IFormFile File { get; set; }

        public CommentUploadFile() { }
        public CommentUploadFile(CommentDTO comment, int userId, IFormFile file)
        {
            Comment = comment;
            UserId = userId;
            File = file;
        }
    }
}
