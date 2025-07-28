using EduFlow.Domain.Entities.Base;

namespace EduFlow.BLL.DTOs.Companies
{
    public class CompanyForUpdateDto : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPaied { get; set; } = false;
    }
}
