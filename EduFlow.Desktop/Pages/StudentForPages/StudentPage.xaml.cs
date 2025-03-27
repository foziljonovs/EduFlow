using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Pages.StudentForPages;

/// <summary>
/// Interaction logic for StudentPage.xaml
/// </summary>
public partial class StudentPage : Page
{
    private readonly IStudentService _service;
    private readonly ITeacherService _teacherService;
    private readonly ICategoryService _categoryService;
    private TeacherForResultDto _teacher;
    public StudentPage()
    {
        InitializeComponent();
        this._service = new StudentService();
        this._teacherService = new TeacherService();
        this._categoryService = new CategoryService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 20,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;

        cfg.DisplayOptions.Width = 200;
        cfg.DisplayOptions.TopMost = true;
    });

    private async Task GetAllStudent()
    {
        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;
        var students = await Task.Run(async () => await _service.GetAllAsync());

        ShowStudents(students);
    }

    private async Task GetAllStudentByTeacher(long teacherId)
    {
        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;
        var students = await Task.Run(async () => await _service.GetAllByTeacherIdAsync(teacherId));

        ShowStudents(students);
    }

    private async Task GetAllCategory()
    {
        var categories = await Task.Run(async () => await _categoryService.GetAllAsync());

        if (categories.Any())
        {
            foreach (var category in categories)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = category.Name;
                item.Tag = category.Id;
                courseComboBox.Items.Add(item);
            }
        }
        else
        {
            notifier.ShowInformation("Kurslar topilmadi!");
        }
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

    private async Task<long> GetTeacher(long userId)
    {
        var teacher = await Task.Run(async () => await _teacherService.GetByUserIdAsync(userId));

        if (teacher is null)
        {
            notifier.ShowInformation("Ustoz topilmadi!");
            return 0;
        }

        _teacher = teacher;
        return teacher.Id;
    }

    private async Task LoadPage()
    {
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;

        if(role is UserRole.Teacher)
        {
            courseComboBox.Visibility = Visibility.Collapsed;

            var teacherId = await GetTeacher(id);
            if(teacherId == 0)
            {
                stStudents.Children.Clear();
                emptyDataForStudent.Visibility = Visibility.Visible;
                return;
            }

            await GetAllStudentByTeacher(teacherId);
        }
        else
        {
            await GetAllStudent();
            await GetAllCategory();
        }
    }

    private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        LoadPage();
    }
}
