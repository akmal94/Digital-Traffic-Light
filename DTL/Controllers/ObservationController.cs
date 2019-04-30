using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTL.BLL.Services;
using DTL.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationController : ControllerBase
    {
        private static DtlManager _dtlManager;

        public ObservationController(DtlManager dtlManager)
        {
            _dtlManager = dtlManager;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddObservation([FromBody] ObservationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var responce = _dtlManager.GetResponse(model);
                return Ok(responce);
            }
            catch(DTLException ex)
            {
                var responce = new ErrorModel { Status = "error", Msg = ex.ErrMsg };
                return BadRequest(responce);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}