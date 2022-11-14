using System.Collections.Generic;

namespace POSTmanager.Models
{
    internal class Rest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ServerName { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public virtual ICollection<UserRest> UserRests { get; set; }
    }
}
