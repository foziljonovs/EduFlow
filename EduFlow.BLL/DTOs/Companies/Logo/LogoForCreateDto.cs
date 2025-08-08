using Microsoft.AspNetCore.Http;

namespace EduFlow.BLL.DTOs.Companies.Logo;

public class LogoForCreateDto
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FilePath { get; set; }
}
