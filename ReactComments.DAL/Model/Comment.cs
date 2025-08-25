namespace ReactComments.DAL.Model
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual FileAttachment? ImageAttachment { get; set; }
        public virtual FileAttachment? TextFileAttachment { get; set; }
        public Guid? ParentCommentId { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment>? Replies { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }

        public Comment()
        {
            Replies = new HashSet<Comment>();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
