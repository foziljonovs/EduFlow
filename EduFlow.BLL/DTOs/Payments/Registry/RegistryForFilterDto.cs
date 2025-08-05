namespace EduFlow.BLL.DTOs.Payments.Registry;

public class RegistryForFilterDto
{
    public DateTime? StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public long CompanyId { get; set; }
}
