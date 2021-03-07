using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Logs;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public class LogsService : LiteDbBaseService<ILogsService>, ILogsService
    {
        protected LiteDbAccessor AuditLogAccessor
        {
            get
            {
                return new LiteDbAccessor("logs/AuditLog.db");
            }
        }

        protected LiteDbAccessor ErrorLogAccessor
        {
            get
            {
                return new LiteDbAccessor("logs/ErrorLog.db");
            }
        }

        public Task<DataResult<AuditLogModel>> GetAuditLogList(GetListRequest req)
        {
            var logs = AuditLogAccessor.SearchItems<AuditLogModel>(Constants.TABLE_LOG_AUDIT, req);
            return Task.FromResult(new DataResult<AuditLogModel>(Name, logs));
        }

        public Task<DataResult<ErrorLogModel>> GetErrorLogList(GetListRequest req)
        {
            var logs = ErrorLogAccessor.SearchItems<ErrorLogModel>(Constants.TABLE_LOG_ERROR, req);
            return Task.FromResult(new DataResult<ErrorLogModel>(Name, logs));
        }
    }
}