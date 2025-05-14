using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using System.Windows;

namespace EduFlow.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForViewWindow.xaml
/// </summary>
public partial class StudentForViewWindow : Window
{
    private readonly IStudentService _studentService;
    private long Id { get; set; }
    public StudentForViewWindow()
    {
        InitializeComponent();
        this._studentService = new StudentService();
    }

    public void SetId(long id)
        => this.Id = id;

    private async Task<StudentForResultDto> GetStudent()
    {
        var student = await _studentService.GetByIdAsync(Id);

        if(student is not null)
            return student;
        else
            return new StudentForResultDto();
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async void ShowValues()
    {
        var student = await GetStudent();
        if (student is not null)
        {
            tbFullName.Text = student.Fullname;
            tbAge.Text = student.Age.ToString();
            tbAddress.Text = student.Address;
            tbPhoneNumber.Text = student.PhoneNumber;
            //guruhlarni component orqali chiqaramiz
        }
        else
        {
            tbFullName.Text = "...";
            tbAge.Text = "0";
            tbAddress.Text = "N/A";
            tbPhoneNumber.Text = "N/A";
            groupForEmptyDate.Visibility = Visibility.Visible;
            paymentForEmptyDate.Visibility = Visibility.Visible;
        }
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ShowValues();
    }
}
