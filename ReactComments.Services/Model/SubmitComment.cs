using Microsoft.AspNetCore.Http;

namespace ReactComments.Services.Model
{
    public class SubmitComment
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public IFormFile? File { get; set; }
        public string? ParentCommentId { get; set; }
        public int PersonId { get; set; }
    }
}
