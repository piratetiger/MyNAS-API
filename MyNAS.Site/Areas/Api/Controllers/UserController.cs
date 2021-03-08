using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Services.Abstraction;
using MyNAS.Site.Helper;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ServiceCollection<IUserService> _userServices;
        private IUserService _userService;
        protected IUserService UserService
        {
            get
            {
                if (_userService == null)
                {
                    _userServices.FilterOrder = this.GetServiceFilterOrder();
                    _userService = _userServices.First();
                }

                return _userService;
            }
        }

        public UserController(IEnumerable<IUserService> userServices)
        {
            _userServices = new ServiceCollection<IUserService>(userServices);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<object> Login(LoginRequest req)
        {
            req.HostInfo = HttpContext.GetUserAgent();
            var result = await UserService.Login(req);
            var user = result.First;
            if (user != null)
            {
                return result;
            }
            else
            {
                return new MessageDataResult(result.Source, false, "Login");
            }
        }

        [HttpPost("update")]
        public async Task<object> UpdateUser(UserRequest req)
        {
            UserModel user = null;

            if (req.User != null)
            {
                user = (await UserService.GetItem(req.User.UserName)).First;

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
                        return new MessageDataResult(nameof(UserController), false, "Update User");
                    }
                }
            }
            return new MessageDataResult(await UserService.UpdateItem(user), "Update User");
        }

        [HttpPost("list")]
        public async Task<object> GetUserList()
        {
            var users = await UserService.GetList();
            if (User.IsInRole(UserRole.Guest.ToString()))
            {
                users.Data = users.Data.Where(u => u.KeyName == User.Identity.Name).ToList();
            }
            else if (User.IsInRole(UserRole.User.ToString()))
            {
                users.Data = users.Data.Where(u => u.Role == UserRole.Guest || u.Role == UserRole.User).ToList();
            }

            return users;
        }
    }
}