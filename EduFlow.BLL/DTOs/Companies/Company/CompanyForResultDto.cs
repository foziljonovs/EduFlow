using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.BLL.DTOs.Payments.Registry;

namespace EduFlow.BLL.DTOs.Companies.Company;

public class CompanyForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? About { get; set; }
    public string PhoneNumber { get; set; }
    public long LogoId { get; set; }
    public LogoForResultDto Logo { get; set; }
    public List<RegistryForResultDto> Registries { get; set; }
    public List<CompanySubscriptionForShortResultDto> Subscriptions { get; set; }
}
