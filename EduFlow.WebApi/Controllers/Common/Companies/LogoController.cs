using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.BLL.Interfaces.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduFlow.WebApi.Controllers.Common.Companies;

[Route("api/logos")]
[ApiController, Authorize]
public class LogoController(
    ILogoService service) : ControllerBase
{
    private readonly ILogoService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] LogoForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.AddAsync(dto, cancellationToken);
            return result ? Ok() : BadRequest("Failed to add logo.");
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] LogoForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto, cancellationToken);
            return result ? Ok() : BadRequest("Failed to update logo.");
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.DeleteAsync(id, cancellationToken);
            return result ? Ok() : BadRequest("Failed to delete logo.");
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
