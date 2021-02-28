using System;

namespace MyNAS.Model.User
{
    public class UserModel : IKeyNameModel
    {
        [JsonIgnoreSerialization]
        public long Id { get; set; }
        [JsonIgnoreSerialization]
        public string KeyName
        {
            get
            {
                return UserName;
            }
        }
        public string UserName { get; set; }
        public string NickName { get; set; }
        [JsonIgnoreSerialization]
        public string Password { get; set; }
        public UserRole Role { get; set; }
        [JsonIgnoreSerialization]
        public string HostInfo { get; set; }
        public string Token { get; set; }
        [JsonIgnoreSerialization]
        public DateTime TokenDate { get; set; }
    }
}