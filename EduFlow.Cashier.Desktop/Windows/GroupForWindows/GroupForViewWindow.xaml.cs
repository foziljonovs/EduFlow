using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Cashier.Desktop.Components.LessonForComponents;
using EduFlow.Cashier.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Courses.Lesson;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows;

/// <summary>
/// Interaction logic for GroupForViewWindow.xaml
/// </summary>
public partial class GroupForViewWindow : Window
{
    private readonly IGroupService _groupService;
    private readonly ILessonService _lessonService;
    private readonly ITeacherService _teacherService;
    private long Id { get; set; }
    private GroupForResultDto _group = new GroupForResultDto();
    private Dictionary<int, long> _students = new Dictionary<int, long>();
    public GroupForViewWindow()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._lessonService = new LessonService();
        this._teacherService = new TeacherService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
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

    private async Task<Stack<LessonForResultDto>> GetLessons()
    {
        try
        {
            var lessons = await Task.Run(async () => await _lessonService.GetByGroupIdAsync(this.Id));

            if (lessons.Any())
                return new Stack<LessonForResultDto>(lessons);
            else
                return new Stack<LessonForResultDto>();
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Darslarni yuklab bo'lmadi, Iltimos qayta urining!");
            return new Stack<LessonForResultDto>();
        }
    }

    private async Task<GroupForResultDto> GetGroup()
    {
        try
        {
            var group = await _groupService.GetByIdAsync(this.Id);

            if (group is null)
                return new GroupForResultDto();

            return group;
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Guruh malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            return new GroupForResultDto();
        }
    }

    private async Task<TeacherForResultDto> GetTeacher(long teacherId)
    {
        try
        {
            var teacher = await _teacherService.GetByIdAsync(teacherId);

            if (teacher is null)
                return new TeacherForResultDto();

            return teacher;
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'qituvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            return new TeacherForResultDto();
        }
    }

    private async void ShowLessons()
    {
        var lessons = await GetLessons();

        int count = lessons.Count();

        stLessons.Children.Clear();
        lessonLoader.Visibility = Visibility.Visible;

        if (lessons.Any())
        {
            lessonLoader.Visibility = Visibility.Collapsed;
            emptyDataForLesson.Visibility = Visibility.Collapsed;

            foreach (var lesson in lessons)
            {
                LessonForAttendanceComponent component = new LessonForAttendanceComponent();
                component.SetValues(
                    count,
                    _students,
                    lesson);

                stLessons.Children.Add(component);
                count--;
            }

            lessonCountTbc.Text = count.ToString();
        }
        else
        {
            lessonLoader.Visibility = Visibility.Collapsed;
            emptyDataForLesson.Visibility = Visibility.Collapsed;
            lessonCountTbc.Text = "0";
        }
    }

    private async Task ShowValues()
    {
        var group = await GetGroup();

        if(group is null)
        {
            notifierThis.ShowWarning("Guruh malumotlari topilmadi, Iltimos qayta urining!");
            return;
        }

        this._group = group;

        var teacher = await GetTeacher(group.Id);

        if(teacher is null)
        {
            notifierThis.ShowWarning("O'qituvchi malumotlari topilmadi, Iltimos qayta urining!");
            return;
        }

        nameTxt.Text = group.Name;
        teacherNameTxt.Text = $"{teacher.User?.Firstname} {teacher.User?.Lastname}";
        statusTxt.Text = group.IsStatus switch
        {
            Status.Active => "Faol",
            Status.Archived => "Saqlangan",
            Status.Deleted => "O'chirilgan",
            Status.Graduated => "To'xtatilgan",
            _ => "Nomalum"
        };
        statedDateTxt.Text = group.CreatedAt.ToString("dd/MM/yyyy");

        ShowStudents(group.Students);
    }

    private void ShowStudents(List<StudentForShortResultDto> students)
    {
        _students.Clear();
        int count = 1;

        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;

        if(students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Collapsed;

            foreach(var student in students)
            {
                StudentForAttendanceComponent component = new StudentForAttendanceComponent();
                component.setValues(
                    student.Id,
                    count,
                    student.Fullname,
                    EnrollmentStatus.Active,
                    this.Id);

                stStudents.Children.Add(component);
                _students.Add(count, student.Id);
                count++;
            }
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

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(this.WindowState == WindowState.Maximized)
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
    }

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if (this.WindowState == WindowState.Normal)
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await ShowValues();
        ShowLessons();
    }
}
