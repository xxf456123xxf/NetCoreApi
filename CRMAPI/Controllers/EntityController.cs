using CRMAPI.Bll;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Controllers
{
    [EnableCors("any")]
    public class EntityController : ErrorController
    {

        [HttpGet]
        public async Task<IActionResult> getOptions([FromQuery]string[] name)
        {
            EntityData data = new EntityData();
            return await Task.Run(() => { return Ok(data.getOptions(name.ToList())); });
        }
    }
}
