namespace WA.Repository.Account.Data
{
    public class UserData
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public UserStatusData Status { get; set; }

        public SessionData Session { get; set; }
    }
}