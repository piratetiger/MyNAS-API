using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model.Logs;
using MyNAS.Services.Abstraction;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    [Authorize(Policy = "Admin")]
    public class LogsController : ControllerBase
    {
        private readonly ServiceCollection<ILogsService> _logsServices;
        private ILogsService _logsService;
        protected ILogsService LogsService
        {
            get
            {
                if (_logsService == null)
                {
                    _logsServices.FilterOrder = this.GetServiceFilterOrder();
                    _logsService = _logsServices.First();
                }

                return _logsService;
            }
        }

        public LogsController(IEnumerable<ILogsService> logsServices)
        {
            _logsServices = new ServiceCollection<ILogsService>(logsServices);
        }

        [HttpPost("audit/list")]
        public async Task<object> GetAuditLogList(GetListRequest req)
        {
            return await LogsService.GetAuditLogList(req);
        }

        [HttpPost("error/list")]
        public async Task<object> GetErrorLogList(GetListRequest req)
        {
            return await LogsService.GetErrorLogList(req);
        }
    }
}