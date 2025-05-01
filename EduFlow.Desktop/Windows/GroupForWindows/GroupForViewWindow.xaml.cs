using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.LessonForComponents;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Attendance;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Domain.Entities.Users;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForViewWindow.xaml
/// </summary>
public partial class GroupForViewWindow : Window
{
    private readonly IGroupService _groupService;
    private readonly IStudentService _studentService;
    private readonly IAttendanceService _attendanceService;
    private readonly ITeacherService _teacherService;
    private long Id { get; set; }
    public GroupForViewWindow()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._studentService = new StudentService();
        this._attendanceService = new AttendanceService();
        this._teacherService = new TeacherService();
    }


    Notifier notifierThis = new Notifier(cfg =>
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

    public void SetId(long id)
        => this.Id = id;

    private async Task<GroupForResultDto> GetGroup()
    {
        var group = await Task.Run(async () => await _groupService.GetByIdAsync(Id));

        if (group is not null)
            return group;
        else
            return new GroupForResultDto();
    }

    private async Task<TeacherForResultDto> GetTeacher(long teacherId)
    {
        var teacher = await Task.Run(async () => await _teacherService.GetByIdAsync(teacherId));

        if (teacher is not null)
            return teacher;
        else
            return new TeacherForResultDto();
    }

    //private async Task<LessonForResultDto> GetLessons()
    //{

    //}

    private async void ShowValues()
    {
        var group = await GetGroup();
        if(group is null)
        {
            notifierThis.ShowError("Guruh ma'lumotlari topilmadi! Qayta yuklang.");
            return;
        }

        var teacher = await GetTeacher(group.TeacherId);
        if (teacher is null)
        {
            notifierThis.ShowError("O'qituvchi ma'lumotlari topilmadi! Qayta yuklang.");
            return;
        }

        nameTxt.Text = group.Name;
        teacherNameTxt.Text = teacher.User.Firstname + " " + teacher.User.Lastname;
        statusTxt.Text = group.IsStatus.ToString();
        statedDateTxt.Text = group.CreatedAt.ToString("dd/MM/yyyy");

        ShowStudents(group.Students);
    }

    private void ShowStudents(List<Student> students)
    {
        int count = 1;

        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;

        if (students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Collapsed;

            foreach(var student in students)
            {
                StudentForAttendanceComponent component = new StudentForAttendanceComponent();
                component.setValues(
                    student.Id,
                    count,
                    student.Fullname);

                stStudents.Children.Add(component);
                count++;
            }

            studentCountTbc.Text = count.ToString();
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
            studentCountTbc.Text = "0";
        }
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ShowValues();
    }
}
