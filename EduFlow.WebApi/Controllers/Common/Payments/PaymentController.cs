using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.Interfaces.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EduFlow.WebApi.Controllers.Common.Payments;

[Route("api/payments")]
[ApiController, Authorize]
public class PaymentController(
    IPaymentService service) : ControllerBase
{
    private readonly IPaymentService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int pageSize = 10, int pageNumber = 1, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllAsync(pageSize, pageNumber, cancellation);

            var metadata = new
            {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response.Data);
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

    [HttpPost("filter")]
    public async Task<IActionResult> GetAllByFilterAsync([FromBody] PaymentForFilterDto filter, int pageSize = 10, int pageNumber = 1, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.FilterAsync(pageSize, pageNumber, filter, cancellation); 
            
            var metadata = new
            {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response.Data);
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
    public async Task<IActionResult> GetAllByStudentIdAsync([FromRoute] long studentId, int pageSize = 10, int pageNumber = 1, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByStudentIdAsync(pageSize, pageNumber, studentId, cancellation);

            var metadata = new
            {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response.Data);
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
    public async Task<IActionResult> GetAllByGroupIdAsync([FromRoute] long groupId, int pageSize = 10, int pageNumber = 1, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByGroupIdAsync( pageSize, pageNumber, groupId, cancellation);

            var metadata = new
            {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response.Data);
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

    [HttpGet("{teacherId:long}/teacher")]
    public async Task<IActionResult> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _service.GetAllByTeacherIdAsync(teacherId, cancellation);
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
