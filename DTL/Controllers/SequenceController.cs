using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTL.BLL.Services;
using DTL.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SequenceController : ControllerBase
    {
        private static DtlManager _dtlManager;
        public SequenceController(DtlManager dTLService)
        {
            _dtlManager = dTLService;
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateSequence()
        {
            try
            {
                var sequence = _dtlManager.CreateSequence();
                return Ok(sequence);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}