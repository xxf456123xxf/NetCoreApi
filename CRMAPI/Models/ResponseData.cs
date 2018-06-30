using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Models
{
    public class ResponseData
    {

        /// <summary>
        /// 返回值状态 1成功 0失败
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 状态 0 成功
        /// </summary>
        public dynamic data { get; set; }
    }
}
