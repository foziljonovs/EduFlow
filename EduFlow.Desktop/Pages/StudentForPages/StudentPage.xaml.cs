using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows.StudentForWindows;
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
    private readonly ICourseService _courseService;
    private readonly ICategoryService _categoryService;
    private TeacherForResultDto _teacher;
    public StudentPage()
    {
        InitializeComponent();
        this._service = new StudentService();
        this._teacherService = new TeacherService();
        this._courseService = new CourseService();
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
        studentLoader.Visibility = Visibility.Visible;
        var students = await Task.Run(async () => await _service.GetAllAsync());

        ShowStudents(students);
    }

    private async Task GetAllStudentByTeacher(long teacherId)
    {
        studentLoader.Visibility = Visibility.Visible;
        var students = await Task.Run(async () => await _service.GetAllByTeacherIdAsync(teacherId));

        ShowStudents(students);
    }

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = course.Name;
                item.Tag = course.Id;
                courseComboBox.Items.Add(item);
            }
        }
        else
        {
            notifier.ShowInformation("Kurslar topilmadi!");
        }
    }

    private async Task GetAllStudentByCourse()
    {
        long courseId = 0;
        studentLoader.Visibility = Visibility.Visible;

        if(courseComboBox.SelectedItem is ComboBoxItem selectedComboBoxItem
            && selectedComboBoxItem.Tag != null)
            courseId = (long)selectedComboBoxItem.Tag;

        var students = await Task.Run(async () => await _service.GetAllByCourseIdAsync(courseId));
        
        ShowStudents(students);
    }

    private void ShowStudents(List<StudentForResultDto> students)
    {
        var yourActiveStudents = students.Where(x => 
            x.StudentCourses.Any(y => y.CourseId == _teacher.CourseId && y.Status == EnrollmentStatus.Active))
            .ToList();

        int count = 1;
        stStudents.Children.Clear();

        if (yourActiveStudents.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Collapsed;

            foreach(var student in yourActiveStudents)
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
                    student.Groups.Any() ? student.Groups.First().Name : "Yo'q");

                component.OnStudentDelete += GetAllStudent;
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
            createStudent.Visibility = Visibility.Collapsed;

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
            await GetAllCourse();
        }
    }

    private bool isPageLoaded = false;
    private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if(!isPageLoaded)
        {
            isPageLoaded = true;
            LoadPage();
        }
    }

    private void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            GetAllStudentByCourse();
    }

    private async Task GetStudentByPhoneNumber(string phoneNumber)
    {
        studentLoader.Visibility = Visibility.Visible;

        var student = await Task.Run(async () => await _service.GetByPhoneNumberAsync(phoneNumber));

        if(!string.IsNullOrEmpty(student.PhoneNumber))
        {
            stStudents.Children.Clear();
            emptyDataForStudent.Visibility = Visibility.Visible;
        }

        ShowStudents(new List<StudentForResultDto> { student });
    }

    private void searchPhoneNumberForStudentTxt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Enter)
        {
            if (string.IsNullOrWhiteSpace(searchPhoneNumberForStudentTxt.Text))
            {
                notifier.ShowError("Telefon raqam no'to'g'ri kiritildi!");
                return;
            }

            var phoneNumber = "+998" + searchPhoneNumberForStudentTxt.Text;

            GetStudentByPhoneNumber(phoneNumber);
        }
    }

    private async void createStudent_Click(object sender, RoutedEventArgs e)
    {
        StudentForCreateWindow window = new StudentForCreateWindow();
        window.ShowDialog();
        await GetAllStudent();
    }
}
