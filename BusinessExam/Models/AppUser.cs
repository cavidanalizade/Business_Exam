using Microsoft.AspNetCore.Identity;

namespace BusinessExam.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
