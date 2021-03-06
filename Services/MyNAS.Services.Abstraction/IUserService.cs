using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Services.Abstraction.Helper;

namespace MyNAS.Services.Abstraction
{
    public interface IUserService : ICollectionService<IUserService>, IServiceBase
    {
        Task<DataResult<UserModel>> Login(LoginRequest req)
        {
            var result = new DataResult<UserModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.Login(req);
        }

        Task<DataResult<bool>> ValidateUser(UserModel user)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.ValidateUser(user);
        }

        Task<DataResult<string>> NewToken(UserModel user)
        {
            var result = new DataResult<string>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.NewToken(user);
        }

        Task<DataResult<UserModel>> GetList()
        {
            var result = new DataResult<UserModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetList();
        }

        Task<DataResult<UserModel>> GetItem(string name)
        {
            var result = new DataResult<UserModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetItem(name);
        }

        Task<DataResult<bool>> SaveItem(UserModel item)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.SaveItem(item);
        }

        Task<DataResult<bool>> UpdateItem(UserModel item)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.UpdateItem(item);
        }

        Task<DataResult<bool>> DeleteItem(UserModel item)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.DeleteItem(item);
        }
    }
}