using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Components.GroupForComponents;
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
        groupForLoader.Visibility = Visibility.Visible;

        var student = await GetStudent();
        if (student is not null)
        {
            tbFullName.Text = student.Fullname;
            tbAge.Text = student.Age.ToString();
            tbAddress.Text = student.Address;
            tbPhoneNumber.Text = student.PhoneNumber;

            if (student.Groups.Any())
            {
                groupForLoader.Visibility = Visibility.Collapsed;
                groupForEmptyDate.Visibility = Visibility.Collapsed;

                foreach(var item in student.Groups)
                {
                    GroupForStudentViewWindowComponent component = new GroupForStudentViewWindowComponent();
                    component.SetValues(
                        item.Id,
                        item.Name,
                        item.IsStatus);

                    stGroups.Children.Add(component);
                }
            }
            else
            {
                groupForLoader.Visibility = Visibility.Collapsed;
                groupForEmptyDate.Visibility = Visibility.Visible;
            }
        }
        else
        {
            groupForLoader.Visibility = Visibility.Collapsed;
            groupForEmptyDate.Visibility = Visibility.Visible;
            paymentForEmptyDate.Visibility = Visibility.Visible;

            tbFullName.Text = "...";
            tbAge.Text = "0";
            tbAddress.Text = "N/A";
            tbPhoneNumber.Text = "N/A";
        }
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ShowValues();
    }

    private void addStudentCourseBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        StudentForRegisterCourseWindow window = new StudentForRegisterCourseWindow();
        window.SetStudentId(this.Id);
        window.ShowDialog();
    }
}
