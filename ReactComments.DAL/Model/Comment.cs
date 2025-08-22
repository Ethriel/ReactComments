namespace ReactComments.DAL.Model
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[]? Image { get; set; }
        public string? ImageMimeType { get; set; }
        public byte[]? TextFile { get; set; }
        public string? TextFileName { get; set; }
        public Guid? ParentCommentId { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment>? Replies { get; set; }
        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }

        public Comment()
        {
            Replies = new HashSet<Comment>();
        }
    }
}
