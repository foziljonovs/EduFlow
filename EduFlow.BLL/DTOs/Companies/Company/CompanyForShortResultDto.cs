namespace EduFlow.BLL.DTOs.Companies.Company;

public class CompanyForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? About { get; set; }
    public string PhoneNumber { get; set; }
    public long LogoId { get; set; }
}
