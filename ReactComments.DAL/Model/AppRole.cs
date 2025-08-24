using Microsoft.AspNetCore.Identity;

namespace ReactComments.DAL.Model
{
    public class AppRole : IdentityRole<int>
    {
        public virtual ICollection<Person> People { get; set; }
        public AppRole()
        {
            People = new HashSet<Person>();
        }
    }
}
