using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Users.User;
using EduFlow.BLL.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduFlow.WebApi.Controllers.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController(
    IUserService service) : ControllerBase
{
    private readonly IUserService _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserForLoginDto dto)
    {
        try
        {
            var response = await _service.LoginAsync(dto);
            return Ok(response);
        }
        catch(ValidationException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch(StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
