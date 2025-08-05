namespace EduFlow.BLL.DTOs.Companies.Company;

public class CompanyForCreateDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string? About { get; set; }
    public string PhoneNumber { get; set; }
    public long LogoId { get; set; }
    public long RegistryId { get; set; }
}
