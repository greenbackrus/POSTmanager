namespace POSTmanager.Models
{
    internal class UserRest
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RestId { get; set; }
        public Rest Rest { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
