using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Services.Abstraction.Helper;

namespace MyNAS.Services.Abstraction
{
    public interface IAdminService : ICollectionService<IAdminService>, IServiceBase
    {
        Task<DataResult<bool>> InitDB()
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.InitDB();
        }
    }
}