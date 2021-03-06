using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Services.Abstraction;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;

        private readonly ServiceCollection<IAdminService> _adminServices;
        private IAdminService _adminService;
        protected IAdminService AdminService
        {
            get
            {
                if (_adminService == null)
                {
                    _adminServices.FilterOrder = this.GetServiceFilterOrder();
                    _adminService = _adminServices.First();
                }

                return _adminService;
            }
        }

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

        public AdminController(IWebHostEnvironment host)
        {
            _host = host;
        }

        [HttpPost("initDB")]
        [AllowAnonymous]
        public async Task<object> InitDB()
        {
            return new MessageDataResult(await AdminService.InitDB(), "Initialize database");
        }

        [HttpPost("prune")]
        public object Prune()
        {
            // prune tmp folder
            var tmpFolder = Path.Combine(_host.WebRootPath, "tmp");
            foreach (var file in Directory.GetFiles(tmpFolder))
            {
                System.IO.File.Delete(file);
            }

            // prune obsolete tmp folder
            var obsoleteTmpFolder = Path.Combine(_host.WebRootPath, "storage/tmp");
            Directory.Delete(obsoleteTmpFolder, true);

            // remove obsolete video thumb
            var obsoleteVideoThumbFolder = Path.Combine(_host.WebRootPath, "storage/videos");
            foreach (var file in Directory.GetFiles(obsoleteVideoThumbFolder, "*.jpg"))
            {
                System.IO.File.Delete(file);
            }

            return new MessageDataResult(nameof(AdminController), true, "Prune");
        }

        [HttpPost("users")]
        public async Task<object> GetUserList()
        {
            return await UserService.GetList();
        }

        [HttpPost("users/add")]
        public async Task<object> AddUser(UserRequest req)
        {
            if (req.User != null)
            {
                req.User.Password = req.Password;
            }
            return new MessageDataResult(await UserService.SaveItem(req.User), "Create User");
        }

        [HttpPost("users/update")]
        public async Task<object> UpdateUser(UserRequest req)
        {
            if (req.User != null)
            {
                if (!string.IsNullOrEmpty(req.Password))
                {
                    req.User.Password = req.Password;
                }
            }
            return new MessageDataResult(await UserService.UpdateItem(req.User), "Update User");
        }

        [HttpPost("users/delete")]
        public async Task<object> DeleteUser(UserRequest req)
        {
            return new MessageDataResult(await UserService.DeleteItem(req.User), "Delete User");
        }
    }
}