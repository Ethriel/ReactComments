namespace ReactComments.Services.Model
{
    public class CommentDTO
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public FileAttachmentDTO? ImageAttachment { get; set; }
        public FileAttachmentDTO? TextFileAttachment { get; set; }
        public string? ParentCommentId { get; set; }
        public ICollection<CommentDTO>? Replies { get; set; }
        // make just as id
        public PersonDTO Person { get; set; }
    }
}
