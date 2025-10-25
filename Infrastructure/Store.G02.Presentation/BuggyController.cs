using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            return NotFound();
        }



        [HttpGet("servererror")]
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception();
            return Ok();
        }



        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }



        [HttpGet("badrequest/{id}")]
        public IActionResult GetBadRequest(int id)
        {
            return BadRequest();
        }



        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorizedRequest()
        {
            return Unauthorized();
        }

    }
}
