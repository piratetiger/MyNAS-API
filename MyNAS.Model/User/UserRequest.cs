namespace MyNAS.Model.User
{
    public class UserRequest
    {
        public UserModel User { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}