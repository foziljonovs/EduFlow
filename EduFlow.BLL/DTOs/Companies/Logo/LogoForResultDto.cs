namespace EduFlow.BLL.DTOs.Companies.Logo;

public class LogoForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FilePath { get; set; }
}
