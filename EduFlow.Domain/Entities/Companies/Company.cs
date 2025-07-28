using EduFlow.Domain.Entities.Base;

namespace EduFlow.Domain.Entities.Companies
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPaied { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
