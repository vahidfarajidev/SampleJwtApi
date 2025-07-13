
using System.Collections.Generic;

namespace SampleJwtApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
    }
}
