using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Service;
using MyNAS.Site.Helper;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    public class UserController : ControllerBase
    {
        protected UserService UserService
        {
            get
            {
                return new UserService();
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public object Login(LoginRequest req)
        {
            req.HostInfo = HttpContext.GetUserAgent();
            var user = UserService.Login(req);
            return new MessageDataResult<UserModel>("Login", user != null, user);
        }

        [HttpPost("update")]
        public object UpdateUser(UserRequest req)
        {
            UserModel user = null;

            if (req.User != null)
            {
                user = UserService.GetItem(req.User.UserName);

                if (user != null)
                {
                    user.NickName = req.User.NickName;
                }

                if (!string.IsNullOrEmpty(req.Password))
                {
                    if (req.OldPassword == user.Password)
                    {
                        user.Password = req.Password;
                    }
                    else
                    {
                        return new MessageDataResult("Update User", false);
                    }
                }
            }
            return new MessageDataResult("Update User", UserService.UpdateItem(user));
        }

        [HttpPost("list")]
        public object GetUserList()
        {
            var users = UserService.GetList();
            var result = new List<UserModel>();
            if (User.IsInRole(UserRole.Guest.ToString()))
            {
                result = users.Where(u => u.KeyName == User.Identity.Name).ToList();
            }
            else if (User.IsInRole(UserRole.User.ToString()))
            {
                result = users.Where(u => u.Role == UserRole.Guest || u.Role == UserRole.User).ToList();
            }
            else
            {
                result = users;
            }

            return new DataResult<List<UserModel>>(result);
        }
    }
}