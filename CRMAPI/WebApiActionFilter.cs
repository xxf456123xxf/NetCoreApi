using CRMAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI
{
    /// <summary>
    /// 正常ok处理
    /// </summary>
    public class StateActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 正常ok处理
        /// </summary>
        /// <param name="context"></param>
        public async override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (context.Result is OkObjectResult)
            {
                ResponseData resData = new ResponseData();
                resData.status = 1;
                resData.data = ((OkObjectResult)context.Result).Value;
                context.Result = new ObjectResult(resData);
            }
            else if (context.Result is OkResult) {
                ResponseData resData = new ResponseData();
                resData.status = 1;
    
                context.Result = new ObjectResult(resData);
            }
        }
    }
}
