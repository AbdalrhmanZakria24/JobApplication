using JobApplication.Application.CQRS.Command.CancelApp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cancels a job application.
        /// </summary>
        /// <param name="id">The ID of the job application.</param>
        /// <param name="candidateId">The ID of the candidate who owns the application.</param>
        /// <returns>
        /// Returns a success message and the ID of the cancelled application.
        /// </returns>
        /// <response code="200">The application was cancelled successfully.</response>
        /// <response code="400">The application cannot be cancelled due to its current status.</response>
        /// <response code="403">The candidate is not authorized to cancel this application.</response>
        /// <response code="404">The application was not found.</response>
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id, int candidateId)
        {
            var result = await _mediator.Send(
                new CancelAppCommand(id, candidateId));

            return Ok(new
            {
                message = "Application cancelled successfully.",
                Id = result
            });
        }
    }
}