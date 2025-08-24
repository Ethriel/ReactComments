using Microsoft.AspNetCore.Identity;

namespace ReactComments.DAL.Model
{
    public class Person : IdentityUser<int>
    {
        public string? HomePage { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int RoleId { get; set; }
        public virtual AppRole AppRole { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Person()
        {
            Comments = new HashSet<Comment>();
            RegisteredDate = DateTime.UtcNow;
        }
    }
}
