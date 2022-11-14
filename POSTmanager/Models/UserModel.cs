using System.Collections.Generic;

namespace POSTmanager.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<UserRest> UserRests { get; set; }
    }
}
