namespace MyNAS.Model.User
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostInfo { get; set; }
    }
}