﻿using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.Interfaces.Courses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduFlow.WebApi.Controllers.Common.Courses;

[Route("api/attendances")]
[ApiController, Authorize]
public class AttendanceController(
    IAttendanceService service) : ControllerBase
{
    private readonly IAttendanceService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllAsync(cancellation);
            return Ok(response);
        }
        catch(StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch(Exception ex)
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
        catch(StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] AttendanceForCraeteDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.AddAsync(dto, cancellation);
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
        catch(Exception ex)
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
        catch(StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] AttendanceForUpdateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.UpdateAsync(id, dto, cancellation);
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
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{studentId:long}/student")]
    public async Task<IActionResult> GetAllByStudentIdAsync([FromRoute] long studentId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByStudentIdAsync(studentId, cancellation);
            return Ok(response);
        }
        catch(StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{lessonId:long}/lesson")]
    public async Task<IActionResult> GetAllByGroupIdAsync([FromRoute] long lessonId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByLessonIdAsync(lessonId, cancellation);
            return Ok(response);
        }
        catch(StatusCodeException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);   
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("update-range")]
    public async Task<IActionResult> UpdateRangeAsync([FromBody] List<AttendanceForUpdateRangeDto> dtos, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.UpdateRangeAsync(dtos, cancellation);
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
