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
        public PersonDTO Person { get; set; }
        public CommentDTO()
        {
            Replies = [];
        }
        public CommentDTO(string id, string text, string createdAt, string updatedAt, FileAttachmentDTO imageAttachment,
                          FileAttachmentDTO textFileAttachment, string? parentCommentId,
                          ICollection<CommentDTO> replies, PersonDTO person)
        {
            Id = id;
            Text = text;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ParentCommentId = parentCommentId;
            ImageAttachment = imageAttachment;
            TextFileAttachment = textFileAttachment;
            Replies = replies;
            Person = person;
        }
    }
}
