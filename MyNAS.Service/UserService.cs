using System;
using System.Collections.Generic;
using MyNAS.Model.User;
using MyNAS.Service.Helper;

namespace MyNAS.Service
{
    public class UserService : ServiceBase
    {
        public UserModel Login(LoginRequest req)
        {
            if (string.IsNullOrEmpty(req.HostInfo))
            {
                return null;
            }

            var dbUser = DBAccessor.GetItem<UserModel>(Constants.TABLE_USERS, req.UserName);

            if (dbUser != null && dbUser.Password == req.Password)
            {
                dbUser.HostInfo = req.HostInfo;
                dbUser.Token = NewToken(dbUser);
                return dbUser;
            }

            return null;
        }

        public bool ValidateUser(UserModel user)
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
                    return true;
                }
            }

            return false;
        }

        public string NewToken(UserModel user)
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
                return token;
            }
            else
            {
                return null;
            }
        }

        public List<UserModel> GetList()
        {
            return DBAccessor.GetAll<UserModel>(Constants.TABLE_USERS);
        }

        public UserModel GetItem(string name)
        {
            return DBAccessor.GetItem<UserModel>(Constants.TABLE_USERS, name);
        }

        public bool SaveItem(UserModel item)
        {
            return DBAccessor.SaveItem(Constants.TABLE_USERS, item);
        }

        public bool UpdateItem(UserModel item)
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
            return DBAccessor.UpdateItem(Constants.TABLE_USERS, user);
        }

        public bool DeleteItem(UserModel item)
        {
            return DBAccessor.DeleteItem(Constants.TABLE_USERS, item);
        }

        private string GetToken(UserModel user)
        {
            var key = $@"{user.HostInfo}\{user.UserName}";
            var data = $@"{user.TokenDate.ToString()}\{user.Password}";
            return EncryptHelper.Encrypt(data, key);
        }
    }
}