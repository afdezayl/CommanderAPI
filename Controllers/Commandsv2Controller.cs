using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class Commandsv2Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Hello()
        {
            return Ok("hello world v2");
        }
    }
}
