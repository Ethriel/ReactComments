using Microsoft.AspNetCore.Identity;

namespace ReactComments.DAL.Model
{
    public class Person : IdentityUser<Guid>
    {
        public string? HomePage { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Person()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
