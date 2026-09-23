using JobApplication.Application.CQRS.Command.CLoseJob;
using JobApplication.Application.CQRS.Command.CreateJop;
using JobApplication.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new job posting.
        /// </summary>
        /// <param name="createJobDto">
        /// Contains the job title and description.
        /// </param>
        /// <returns>
        /// Returns the ID of the newly created job.
        /// </returns>
        /// <response code="200">The job was created successfully.</response>
        /// <response code="400">The provided job data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateJobDto createJobDto)
        {
            var id = await _mediator.Send(
                new CreateJobCommand(
                    createJobDto.Title,
                    createJobDto.Description));

            return Ok(new
            {
                id = id
            });
        }

        /// <summary>
        /// Closes an existing job posting.
        /// </summary>
        /// <param name="id">The ID of the job to close.</param>
        /// <returns>
        /// Returns no content when the job is successfully closed.
        /// </returns>
        /// <response code="204">The job was closed successfully.</response>
        /// <response code="401">The user ID was not found or is invalid in the authentication token.</response>
        /// <response code="403">The authenticated user is not authorized to close this job.</response>
        /// <response code="404">The specified job was not found.</response>
        /// <response code="409">The job cannot be closed because of its current state.</response>
        [HttpPut("{id:int}/close")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CloseJob(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized(
                    "User ID was not found in the token.");
            }

            if (!int.TryParse(userIdClaim, out var recruiterId))
            {
                return Unauthorized(
                    "Invalid user ID in token.");
            }

            try
            {
                await _mediator.Send(
                    new CloseJobCOmmand(id, recruiterId));

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
    }
}