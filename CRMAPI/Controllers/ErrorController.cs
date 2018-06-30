using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRMAPI.Controllers
{
    [StateActionFilter]
    [WebApiExceptionFilter]
    [Route("api/[controller]/[action]")]
    public class ErrorController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CustomHttpContext.Current.User.Identity.Name != null)
            {
                
                string domainname = CustomHttpContext.Current.User.Identity.Name.ToLower();
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
