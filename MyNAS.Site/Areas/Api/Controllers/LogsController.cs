// using System.Collections.Generic;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using MyNAS.Model;
// using MyNAS.Model.Logs;
// using MyNAS.Service;

// namespace MyNAS.Site.Areas.Api.Controllers
// {
//     [Area("Api")]
//     [ApiController]
//     [Route("[area]/[controller]")]
//     [Authorize(Policy = "Admin")]
//     public class LogsController
//     {
//         protected LogsService LogsService
//         {
//             get
//             {
//                 return new LogsService();
//             }
//         }

//         [HttpPost("audit/list")]
//         public object GetAuditLogList(GetListRequest req)
//         {
//             return new DataResult<List<AuditLogModel>>(LogsService.GetAuditLogList(req));
//         }

//         [HttpPost("error/list")]
//         public object GetErrorLogList(GetListRequest req)
//         {
//             return new DataResult<List<ErrorLogModel>>(LogsService.GetErrorLogList(req));
//         }
//     }
// }