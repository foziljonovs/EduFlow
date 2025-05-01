using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.BLL.Interfaces.Courses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduFlow.WebApi.Controllers.Common.Courses;

[Route("api/lessons")]
[ApiController, Authorize]
public class LessonController(
    ILessonService service) : ControllerBase
{
    private readonly ILessonService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllAsync(cancellation);
            return Ok(response);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetByIdAsync(id, cancellation);
            return Ok(response);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] LessonForCreateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.AddAsync(dto, cancellation);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] LessonForUpdateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.UpdateAsync(id, dto, cancellation);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.DeleteAsync(id, cancellation);
            return Ok(response);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("group/{groupId:long}")]
    public async Task<IActionResult> GetAllByGroupIdAsync([FromRoute] long groupId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByGroupIdAsync(groupId, cancellation);
            return Ok(response);
        }
        catch (StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
