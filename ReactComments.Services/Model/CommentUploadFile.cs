using Microsoft.AspNetCore.Http;

namespace ReactComments.Services.Model
{
    public class CommentUploadFile
    {
        public IFormFile File { get; set; }
        public string FileType { get; set; }
        public string CommentId { get; set; }
        public int PersonId { get; set; }
    }
}
