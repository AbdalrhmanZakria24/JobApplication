using JobApplication.Application.DTOs;
using JobApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobService _JobService;
        private readonly CloseJob _closeJob;

        public JobsController(JobService jobService,CloseJob closeJob)
        {
            _JobService = jobService;
            _closeJob = closeJob;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobDto createJobDto)
        {
            var id = await _JobService.CreateAsync(createJobDto);
            return Ok(new
            {
                id = id
            });
        }

        [HttpPut("{id:int}/close")]
        public async Task<IActionResult> CloseJob(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized("User ID was not found in the token.");
            }
            if (!int.TryParse(userIdClaim, out var recruiterId))
            {
                return Unauthorized("Invalid user ID in token.");
            }
            try
            {
                await _closeJob.CloseAsync(id, recruiterId); return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }
    }
}
