using Microsoft.AspNetCore.Http;

namespace EduFlow.BLL.DTOs.Companies.Logo;

public class LogoForUploadDto
{
    public IFormFile File { get; set; }
}
