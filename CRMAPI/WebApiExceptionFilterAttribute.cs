

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using ApiCoreCommon;
using CRMAPI.Models;

namespace CRMAPI
{
    /// <summary>
    /// 异常ok处理
    /// </summary>
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常ok处理
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(ExceptionContext actionExecutedContext)
        {
            ResponseData resData = new ResponseData();
            resData.status = 0;
            resData.msg = actionExecutedContext.Exception.Message;

            actionExecutedContext.Result = new ObjectResult(resData);
          
            logger.LogError(actionExecutedContext.Exception.Message, actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);

        }
       
    }
}