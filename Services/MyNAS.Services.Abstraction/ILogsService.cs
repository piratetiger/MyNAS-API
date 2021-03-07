using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Logs;
using MyNAS.Services.Abstraction.Helper;

namespace MyNAS.Services.Abstraction
{
    public interface ILogsService : ICollectionService<ILogsService>, IServiceBase
    {
        Task<DataResult<AuditLogModel>> GetAuditLogList(GetListRequest req)
        {
            var result = new DataResult<AuditLogModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetAuditLogList(req);
        }

        Task<DataResult<ErrorLogModel>> GetErrorLogList(GetListRequest req)
        {
            var result = new DataResult<ErrorLogModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetErrorLogList(req);
        }
    }
}