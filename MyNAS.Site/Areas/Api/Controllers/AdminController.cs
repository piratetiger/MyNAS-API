using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Service;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;

        protected AdminService AdminService
        {
            get
            {
                return new AdminService();
            }
        }

        protected UserService UserService
        {
            get
            {
                return new UserService();
            }
        }

        public AdminController(IWebHostEnvironment host)
        {
            _host = host;
        }

        [HttpPost("initDB")]
        [AllowAnonymous]
        public object InitDB()
        {
            return new MessageDataResult("Initialize database", AdminService.InitDB());
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

            return new MessageDataResult("Prune", true);
        }

        [HttpPost("users")]
        public object GetUserList()
        {
            return new DataResult<List<UserModel>>(UserService.GetList());
        }

        [HttpPost("users/add")]
        public object AddUser(UserRequest req)
        {
            if (req.User != null)
            {
                req.User.Password = req.Password;
            }
            return new MessageDataResult("Create User", UserService.SaveItem(req.User));
        }

        [HttpPost("users/update")]
        public object UpdateUser(UserRequest req)
        {
            if (req.User != null)
            {
                if (!string.IsNullOrEmpty(req.Password))
                {
                    req.User.Password = req.Password;
                }
            }
            return new MessageDataResult("Update User", UserService.UpdateItem(req.User));
        }

        [HttpPost("users/delete")]
        public object DeleteUser(UserRequest req)
        {
            return new MessageDataResult("Delete User", UserService.DeleteItem(req.User));
        }
    }
}