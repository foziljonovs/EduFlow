using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Domain.Enums;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Pages.StudentForPages;

/// <summary>
/// Interaction logic for StudentPage.xaml
/// </summary>
public partial class StudentPage : Page
{
    private readonly IStudentService _service;
    public StudentPage()
    {
        InitializeComponent();
        this._service = new StudentService();
    }

    private async Task GetAllStudent()
    {
        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;
        var students = await Task.Run(async () => await _service.GetAllAsync());

        ShowStudents(students);
    }

    private void ShowStudents(List<StudentForResultDto> students)
    {
        int count = 1;

        if (students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Collapsed;

            foreach(var student in students)
            {
                StudentForComponent component = new StudentForComponent();
                component.Tag = student;
                component.SetValues(
                    count,
                    student.Id,
                    student.Fullname,
                    student.Age,
                    student.Address,
                    student.PhoneNumber,
                    student.Courses.FirstOrDefault(x => x.Archived == Status.Active).Name);

                stStudents.Children.Add(component);
                count++;
            }
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        GetAllStudent();
    }
}
