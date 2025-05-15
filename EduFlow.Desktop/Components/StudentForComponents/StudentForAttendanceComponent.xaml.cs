using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForAttendanceComponent.xaml
/// </summary>
public partial class StudentForAttendanceComponent : UserControl
{
    private readonly IStudentService _studentService;
    private long Id { get; set; }
    private int Number { get; set; }
    private EnrollmentStatus Status { get; set; }
    public Func<Task> OnDelete { get; set; } = null!;
    public StudentForAttendanceComponent()
    {
        InitializeComponent();
        this._studentService = new StudentService();
    }

    public void setValues(long id, int number, string fullName, EnrollmentStatus status)
    {
        this.Id = id;
        this.Number = number;
        tbNumber.Text = number.ToString();
        tbFullname.Text = fullName;
        this.Status = status;

        //switch (status)
        //{
        //    case EnrollmentStatus.Active:
        //        mnActive.Visibility = System.Windows.Visibility.Visible;
        //        break;

        //    case EnrollmentStatus.Suspended:
        //        mnSaved.Visibility = System.Windows.Visibility.Visible;
        //        break;
        //}
    }

    public (int, long) GetValues() 
        => (this.Number, this.Id);

    private async void mnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var message = MessageBox.Show("O'quvchini guruhdan o'chirishni tasdiqlang", "Tasdiqlash", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if(message is MessageBoxResult.Yes)
            if(this.Id > 0)
            {
                var res = await _studentService.DeleteAsync(this.Id);

                if (res)
                    await OnDelete?.Invoke();
            }
    }
}
