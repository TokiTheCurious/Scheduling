using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Scheduling.Infrastructure.Business;
using Scheduling.Infrastructure.Business.Results;
using Scheduling.Infrastructure.Models;
using Scheduling.WebApi.ApiModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduling.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        ILogger<TimeSlotController> _logger;
        private readonly ITimeSlotBusinessLogic _businessLogic;

        public TimeSlotController(ILogger<TimeSlotController> logger, ITimeSlotBusinessLogic businessLogic)
        {
            _logger = logger;
            _businessLogic = businessLogic;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> Get()
        {
            var res = await _businessLogic.Get();
            if (res.Count() == 0)
                return NotFound(res);
            return Ok(res);
        }


        [HttpGet("is-available")]
        public async Task<ActionResult<IsAvailableTimeSlotResult>> IsAvailable([FromQuery] TimeSlotRequest req)
        {
            return await _businessLogic.IsAvailable(req.ToTimeSlot());
        }

        [HttpPost()]
        public async Task<ActionResult<CreateTimeSlotResult>> Create([FromQuery] TimeSlotRequest req)
        {
            var res = await _businessLogic.Create(req.ToTimeSlot());
            return res?.CreatedTimeSlot != null ? StatusCode(201, res) : Conflict(res);
        }

        [HttpDelete()]
        public async Task<ActionResult<DeleteTimeSlotResult>> Delete([FromQuery] TimeSlotRequest req)
        {
            var res = await _businessLogic.Delete(req.ToTimeSlot());
            if (res.DeletedTimeSlots.Count() == 0)
                return NotFound(res);
            return res;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteTimeSlotResult>> Delete(int id)
        {
            var res = await _businessLogic.DeleteId(id);
            if (res.DeletedTimeSlots.Count() == 0)
                return NotFound(res);
            return res;
        }
    }
}
