using JobApplication.Application.CQRS.Command.Login;
using JobApplication.Application.CQRS.Command.Register;
using JobApplication.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="request">
        /// Contains the user's full name, email, password, and password confirmation.
        /// </param>
        /// <returns>
        /// Returns a success message when the user is registered successfully.
        /// </returns>
        /// <response code="200">User registered successfully.</response>
        /// <response code="400">Registration failed because the provided data is invalid.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(Register_DTO request)
        {
            var result =
                await _mediator.Send(
                    new RegisterCommand(
                        request.FullName,
                        request.Email,
                        request.Password,
                        request.ConfirmPassword));

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(x => x.Description)
                });
            }

            return Ok(new
            {
                message = "User registered successfully."
            });
        }

        /// <summary>
        /// Authenticates a user and returns an authentication result.
        /// </summary>
        /// <param name="request">
        /// Contains the user's email and password.
        /// </param>
        /// <returns>
        /// Returns the authentication result when the credentials are valid.
        /// </returns>
        /// <response code="200">Login successful.</response>
        /// <response code="401">Invalid email or password.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            Application.DTOs.LoginRequest request)
        {
            var result =
                await _mediator.Send(
                    new LoginCommand(
                        request.Email,
                        request.Password));

            if (result is null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(result);
        }
    }
}