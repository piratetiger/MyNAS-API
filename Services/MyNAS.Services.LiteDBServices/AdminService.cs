using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public class AdminService : LiteDbBaseService<IAdminService>, IAdminService
    {
        public Task<DataResult<bool>> InitDB()
        {
            var admin = DbAccessor.GetItem<UserModel>(Constants.TABLE_USERS, "admin");
            if (admin == null)
            {
                var users = new List<UserModel>();
                users.Add(new UserModel
                {
                    UserName = "admin",
                    NickName = "Admin",
                    Password = "Admin",
                    Role = UserRole.SystemAdmin
                });
                var result = DbAccessor.SaveItems(Constants.TABLE_USERS, users);
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
            }
            else
            {
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { true }));
            }
        }
    }
}