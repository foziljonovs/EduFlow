﻿using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.Interfaces.Users;
using EduFlow.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduFlow.WebApi.Controllers.Common.Users;

[Route("api/students")]
[ApiController, Authorize]
public class StudentController(
    IStudentService service) : ControllerBase
{
    private readonly IStudentService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _service.GetAllAsync(cancellationToken);
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
    public async Task<IActionResult> AddAsync([FromBody] StudentForCreateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.AddAsync(dto, cancellation);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
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

    [HttpPost("with-id")]
    public async Task<IActionResult> AddAndReturnIdAsync([FromBody] StudentForCreateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.AddAndReturnIdAsync(dto, cancellation);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
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

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] StudentForUpdateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.UpdateAsync(id, dto, cancellation);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
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

    [HttpGet("{phoneNumber}/phone-number")]
    public async Task<IActionResult> GetAllByPhoneNumberAsync([FromRoute] string phoneNumber, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetByPhoneNumberAsync(phoneNumber, cancellation);
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

    [HttpGet("{teacherId:long}/teacher")]
    public async Task<IActionResult> GetAllByTeacherIdAsync([FromRoute] long teacherId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByTeacherIdAsync(teacherId, cancellation);
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

    [HttpGet("{categoryId:long}/category")]
    public async Task<IActionResult> GetAllByCategoryIdAsync([FromRoute] long categoryId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByCategoryIdAsync(categoryId, cancellation);
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

    [HttpPost("{studentId:long}/courses/{groupId:long}")]
    public async Task<IActionResult> AddStudentByCourseAsync([FromRoute] long studentId, [FromRoute] long groupId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.AddStudentByGroupAsync(studentId, groupId, cancellation);
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

    [HttpGet("{courseId:long}/course")]
    public async Task<IActionResult> GetAllByCourseIdAsync([FromRoute] long courseId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByCourseIdAsync(courseId, cancellation);
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

    [HttpPost("filter")]
    public async Task<IActionResult> FilterAsync([FromBody] StudentForFilterDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.FilterAsync(dto, cancellation);
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

    [HttpPut("{id:long}/group/{groupId:long}")]
    public async Task<IActionResult> UpdateStudentByGroupAsync([FromRoute] long id, [FromRoute] long groupId, [FromBody] Status status, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.UpdateStudentByGroupAsync(id, groupId, status, cancellation);
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
