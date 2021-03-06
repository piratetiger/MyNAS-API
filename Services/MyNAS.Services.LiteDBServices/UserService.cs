using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;
using MyNAS.Util;

namespace MyNAS.Services.LiteDbServices
{
    public class UserService : LiteDBBaseService<IUserService>, IUserService
    {
        public async Task<DataResult<UserModel>> Login(LoginRequest req)
        {
            if (string.IsNullOrEmpty(req.HostInfo))
            {
                return new DataResult<UserModel>(Name, null);
            }

            var dbUser = DBAccessor.GetItem<UserModel>(Constants.TABLE_USERS, req.UserName);

            if (dbUser != null && dbUser.Password == req.Password)
            {
                dbUser.HostInfo = req.HostInfo;
                dbUser.Token = (await NewToken(dbUser)).First;
                return new DataResult<UserModel>(Name, new List<UserModel>() { dbUser });
            }

            return new DataResult<UserModel>(Name, null);
        }

        public Task<DataResult<bool>> ValidateUser(UserModel user)
        {
            var dbUser = DBAccessor.GetItem<UserModel>(Constants.TABLE_USERS, user.KeyName);

            if (dbUser != null)
            {
                user.Password = dbUser.Password;
                user.TokenDate = dbUser.TokenDate;
                var userToken = GetToken(user);
                var dbUserToken = GetToken(dbUser);

                if (userToken == user.Token && dbUserToken == dbUser.Token && userToken == dbUserToken && (DateTime.Now - dbUser.TokenDate) < TimeSpan.FromDays(7))
                {
                    user.Role = dbUser.Role;
                    return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { true }));
                }
            }

            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { false }));
        }

        public Task<DataResult<string>> NewToken(UserModel user)
        {
            var dbUser = DBAccessor.GetItem<UserModel>(Constants.TABLE_USERS, user.KeyName);

            if (dbUser != null)
            {
                var date = DateTime.Now;
                user.TokenDate = date;
                var token = GetToken(user);

                dbUser.HostInfo = user.HostInfo;
                dbUser.Token = token;
                dbUser.TokenDate = date;
                DBAccessor.UpdateItem(Constants.TABLE_USERS, dbUser);
                return Task.FromResult(new DataResult<string>(Name, new List<string>() { token }));
            }
            else
            {
                return Task.FromResult(new DataResult<string>(Name, null));
            }
        }

        public Task<DataResult<UserModel>> GetList()
        {
            var result = DBAccessor.GetAll<UserModel>(Constants.TABLE_USERS);
            return Task.FromResult(new DataResult<UserModel>(Name, result));
        }

        public Task<DataResult<UserModel>> GetItem(string name)
        {
            var result = DBAccessor.GetItem<UserModel>(Constants.TABLE_USERS, name);
            return Task.FromResult(new DataResult<UserModel>(Name, new List<UserModel>() { result }));
        }

        public Task<DataResult<bool>> SaveItem(UserModel item)
        {
            var result = DBAccessor.SaveItem(Constants.TABLE_USERS, item);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> UpdateItem(UserModel item)
        {
            var user = DBAccessor.GetItem<UserModel>(Constants.TABLE_USERS, item?.KeyName);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(item.Password))
                {
                    user.Password = item.Password;
                }
                user.Role = item.Role;
                user.NickName = item.NickName;
            }
            var result = DBAccessor.UpdateItem(Constants.TABLE_USERS, user);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> DeleteItem(UserModel item)
        {
            var result = DBAccessor.DeleteItem(Constants.TABLE_USERS, item);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        private string GetToken(UserModel user)
        {
            var key = $@"{user.HostInfo}\{user.UserName}";
            var data = $@"{user.TokenDate.ToString()}\{user.Password}";
            return EncryptHelper.Encrypt(data, key);
        }
    }
}