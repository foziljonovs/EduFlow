using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.Interfaces.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduFlow.WebApi.Controllers.Common.Payments;

[Route("api/payments")]
[ApiController, Authorize]
public class PaymentController(
    IPaymentService service) : ControllerBase
{
    private readonly IPaymentService _service = service;

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
    public async Task<IActionResult> AddAsync([FromBody] PaymentForCreateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.AddToPayAsync(dto, cancellation);
            return Ok(response);
        }
        catch(ValidationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
    public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] PaymentForUpdateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.UpdateToPayAsync(id, dto, cancellation);
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

    [HttpGet("{groupId:long}/group")]
    public async Task<IActionResult> GetAllByGroupIdAsync([FromRoute] long groupId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByGroupIdAsync(groupId, cancellation);
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
}
