namespace ReactComments.DAL.Model
{
    public enum UploadFileTypes
    {
        Image,
        Text
    }
    public class FileAttachment
    {
        public Guid Id { get; set; }
        public byte[] Contents { get; set; }
        public string Name { get; set; }
        public UploadFileTypes Type { get; set; }
        public Guid? ImageCommentId { get; set; }
        public virtual Comment? ImageComment { get; set; }
        public Guid? TextFileCommentId { get; set; }
        public virtual Comment? TextFileComment { get; set; }
    }
}
