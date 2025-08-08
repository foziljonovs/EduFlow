using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduFlow.WebApi.Controllers.Common.Companies;

[Route("api/logos")]
[ApiController]
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
    public async Task<IActionResult> AddAsync([FromForm] LogoForUploadDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (dto.File is null || dto.File.Length == 0)
                return BadRequest("File is required.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type. Allowed types are: .jpg, .jpeg, .png");

            long maxFileSize = 4 * 1024 * 1024; // 4 MB
            if (dto.File.Length > maxFileSize)
                return BadRequest("File size exceeds the maximum limit of 4 MB.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logos");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var result = await _service.AddAsync(new LogoForCreateDto
            {
                FileName = fileName,
                ContentType = dto.File.ContentType,
                FilePath = filePath
            }, cancellationToken);

            return Ok(result);
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

    [HttpGet("{fileName}/download")]
    public IActionResult GetLogo(string fileName)
    {
        try
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logos");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if(!System.IO.File.Exists(filePath))
                return NotFound("Logo not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = FileExtension.GetContentType(filePath);

            return File(fileBytes, contentType);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:long}/download")]
    public async Task<IActionResult> GetLogoById(long id)
    {
        try
        {
            var logo = await _service.GetByIdAsync(id);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logos");
            var filePath = Path.Combine(uploadsFolder, logo.FileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Logo not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = FileExtension.GetContentType(filePath);

            return File(fileBytes, contentType, logo.FileName);
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
