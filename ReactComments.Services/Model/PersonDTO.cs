namespace ReactComments.Services.Model
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? HomePage { get; set; }
        public string RegisteredDate { get; set; }
        public virtual ICollection<CommentDTO> Comments { get; set; }
        public PersonDTO()
        {
            Comments = [];
        }
        public PersonDTO(int id, string username, string email, string role, string? homePage, string registeredDate, ICollection<CommentDTO> comments)
        {
            Id = id;
            UserName = username;
            Email = email;
            Role = role;
            HomePage = homePage;
            RegisteredDate = registeredDate;
            Comments = comments;
        }
    }
}
