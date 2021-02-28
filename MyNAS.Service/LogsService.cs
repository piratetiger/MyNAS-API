using System.Collections.Generic;
using MyNAS.Model.Logs;
using MyNAS.Service.Helper;

namespace MyNAS.Service
{
    public class LogsService
    {
        protected LiteDBAccessor AuditLogAccessor
        {
            get
            {
                return new LiteDBAccessor("logs/AuditLog.db");
            }
        }

        protected LiteDBAccessor ErrorLogAccessor
        {
            get
            {
                return new LiteDBAccessor("logs/ErrorLog.db");
            }
        }

        public List<AuditLogModel> GetAuditLogList(GetListRequest req)
        {
            return AuditLogAccessor.SearchItems<AuditLogModel>(Constants.TABLE_LOG_AUDIT, req);
        }

        public List<ErrorLogModel> GetErrorLogList(GetListRequest req)
        {
            return ErrorLogAccessor.SearchItems<ErrorLogModel>(Constants.TABLE_LOG_ERROR, req);
        }
    }
}