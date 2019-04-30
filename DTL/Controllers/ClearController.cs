using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTL.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClearController : ControllerBase
    {
        private static DtlManager _dtlManager;

        public ClearController(DtlManager dtlManager)
        {
            _dtlManager = dtlManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Clear()
        {
            try
            {
                _dtlManager.Clear();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}