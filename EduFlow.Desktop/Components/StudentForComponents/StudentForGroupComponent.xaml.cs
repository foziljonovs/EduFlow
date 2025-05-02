using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForGroupComponent.xaml
/// </summary>
public partial class StudentForGroupComponent : UserControl
{
    private long Id { get; set; }
    public StudentForGroupComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, StudentForResultDto dto)
    {
        Id = dto.Id;
        tbNumber.Text = number.ToString();
        tbFullname.Text = dto.Fullname;
        tbCourse.Text = dto.StudentCourses.Where(x => x.Status == EnrollmentStatus.Pending).FirstOrDefault().Course.Name ?? "Nomalum";
        tbAge.Text = dto.Age.ToString();
        tbAddress.Text = dto.Address;
        tbPhoneNumber.Text = dto.PhoneNumber;
    }
}
