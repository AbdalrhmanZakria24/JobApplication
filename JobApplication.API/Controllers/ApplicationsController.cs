using JobApplication.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ICancelApplication _cancelApplication;

        public ApplicationsController(ICancelApplication cancelApplication)
        {
            _cancelApplication = cancelApplication;
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id,int candidateId)
        {

             await _cancelApplication.Cancel(id, candidateId);

            return Ok(new
            {
                message = "Application cancelled successfully."
            });
        }
    }
}
