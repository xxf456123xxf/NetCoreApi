using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Controllers
{
    [EnableCors("any")]
    public class TestController : ErrorController
    {
        /// <summary>
        /// 测试
        /// </summary>

        /// <returns></returns>
        [HttpGet]


        public async Task<IActionResult> error()
        {
            throw new Exception("测试");
            return await Task.Run(() => { return Ok(); });
        }
        /// <summary>
        /// 测试
        /// </summary>

        /// <returns></returns>
        [HttpGet]

        public async Task<IActionResult> get()
        {

            return await Task.Run(() => { return Ok(); });
        }

        /// <summary>
        /// 测试延迟一秒
        /// </summary>

        /// <returns></returns>
        [HttpGet]


        public async Task<IActionResult> get1000()
        {

            return await Task.Run(() => { System.Threading.Thread.Sleep(1000); return Ok(); });
        }
        /// <summary>
        /// 测试延迟两秒
        /// </summary>

        /// <returns></returns>
        [HttpGet]

        public async Task<IActionResult> get2000()
        {

            return await Task.Run(() => { System.Threading.Thread.Sleep(2000); return Ok(); });
        }

    }
}
