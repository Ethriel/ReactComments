namespace ReactComments.Services.Model
{
    public class FileAttachmentDTO
    {
        public string Id { get; set; }
        public byte[] Contents { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? ImageCommentId { get; set; }
        public string? TextFileCommentId { get; set; }
        public FileAttachmentDTO() { }
        public FileAttachmentDTO(string id, byte[] contents, string name, string type, string imageCommentId, string textFileCommentId)
        {
            Id = id;
            Contents = contents;
            Name = name;
            Type = type;
            ImageCommentId = imageCommentId;
            TextFileCommentId = textFileCommentId;
        }
    }
}
