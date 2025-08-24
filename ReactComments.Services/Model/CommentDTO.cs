namespace ReactComments.Services.Model
{
    public class CommentDTO
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public byte[]? Image { get; set; }
        public string? ImageMimeType { get; set; }
        public byte[]? TextFile { get; set; }
        public string? TextFileName { get; set; }
        public CommentDTO? ParentComment { get; set; }
        public ICollection<CommentDTO>? Replies { get; set; }
        public PersonDTO Person { get; set; }
        public CommentDTO()
        {
            Replies = [];
        }
        public CommentDTO(string id, string text, string createdAt, string updatedAt,
            byte[]? image, string? imageMimeType, byte[]? textFile, string? textFileName,
            CommentDTO? parentComment, ICollection<CommentDTO> replies, PersonDTO person)
        {
            Id = id;
            Text = text;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Image = image;
            ImageMimeType = imageMimeType;
            TextFile = textFile;
            TextFileName = textFileName;
            ParentComment = parentComment;
            Replies = replies;
            Person = person;
        }
    }
}
