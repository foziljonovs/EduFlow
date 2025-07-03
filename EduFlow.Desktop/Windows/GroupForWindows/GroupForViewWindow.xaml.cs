using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.LessonForComponents;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Attendance;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Courses.Lesson;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
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
    private readonly ILessonService _lessonService;
    private long Id { get; set; }
    private Dictionary<int, long> _students = new Dictionary<int, long>();
    private List<AttendanceForUpdateRangeDto> allUpdateAttendances = new List<AttendanceForUpdateRangeDto>();
    public GroupForViewWindow()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._studentService = new StudentService();
        this._attendanceService = new AttendanceService();
        this._teacherService = new TeacherService();
        this._lessonService = new LessonService();
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

    private async Task<GroupForResultDto> GetGroup()
    {
        var group = await _groupService.GetByIdAsync(Id);

        if (group is not null)
            return group;
        else
            return new GroupForResultDto();
    }

    private async Task<TeacherForResultDto> GetTeacher(long teacherId)
    {
        var teacher = await _teacherService.GetByIdAsync(teacherId);

        if (teacher is not null)
            return teacher;
        else
            return new TeacherForResultDto();
    }

    private async Task<Stack<LessonForResultDto>> GetLessons()
    {
        var lessons = await Task.Run(async () => await _lessonService.GetByGroupIdAsync(Id));

        if (lessons.Any())
            return new Stack<LessonForResultDto>(lessons);
        else
            return new Stack<LessonForResultDto>();
    }

    private async void ShowLessons()
    {
        var lessons = await GetLessons();

        int count = lessons.Count;

        stLessons.Children.Clear();
        lessonLoader.Visibility = Visibility.Visible;

        if (lessons.Any())
        {
            lessonLoader.Visibility = Visibility.Collapsed;
            emptyDataForLesson.Visibility = Visibility.Collapsed;

            foreach(var lesson in lessons)
            {
                LessonForAttendanceComponent component = new LessonForAttendanceComponent();
                component.SetValues(
                    count,
                    _students,
                    lesson);

                component.OnGetValues += LoadedWindow;
                stLessons.Children.Add(component);
                count--;
            }

            lessonCountTbc.Text = lessons.Count.ToString();
        }
        else
        {
            lessonLoader.Visibility = Visibility.Collapsed;
            emptyDataForLesson.Visibility = Visibility.Visible;
            lessonCountTbc.Text = "0";
        }
    }

    private async Task ShowValues()
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

    private void ShowStudents(List<StudentForShortResultDto> students)
    {
        _students.Clear();
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
                    student.Fullname,
                    EnrollmentStatus.Active,
                    this.Id);

                component.OnDelete = async () =>
                {
                    Window_Loaded(this, new RoutedEventArgs());
                };

                stStudents.Children.Add(component);
                _students.Add(count, student.Id);
                count++;
            }

            studentCountTbc.Text = students.Count.ToString();
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
            studentCountTbc.Text = "0";
        }
    }

    private async void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        GetAllUpdateAttendance();

        if(!allUpdateAttendances.Any())
            this.Close();
        else
        {
            var messageResult = MessageBox.Show("O'zgarishlar saqlansinmi?", "EduFlow", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(messageResult is MessageBoxResult.Yes)
            {
                await SaveAsync();
                this.Close();
                notifier.ShowInformation("O'zgarishlar saqlandi.");
            }
            else
            {
                this.Close();
                notifier.ShowWarning("O'zgarishlar saqlanmadi!");
            }
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if (NormalButton.Visibility == Visibility.Visible)
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if (MaxButton.Visibility == Visibility.Visible)
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
    }

    private async Task LoadedWindow()
    {
        if (IdentitySingelton.GetInstance().Role is Domain.Enums.UserRole.Teacher)
            addStudents.Visibility = Visibility.Collapsed;

        await ShowValues();
        ShowLessons();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadedWindow();
    }

    private async void addStudents_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        GroupForAddStudentWindow window = new GroupForAddStudentWindow();
        window.SetId(Id);
        window.ShowDialog();
        await LoadedWindow();
    }

    private async void createLessonBtn_Click(object sender, RoutedEventArgs e)
    {
        stLessons.Children.Clear();
        lessonLoader.Visibility = Visibility.Visible;

        var lesson = new LessonForCreateDto();

        lesson.GroupId = Id;
        lesson.Date = DateTime.UtcNow.AddHours(5);
        lesson.LessonNumber = Convert.ToInt32(lessonCountTbc.Text) + 1;
        lesson.Name = lesson.LessonNumber + " - dars";

        var result = await _lessonService.AddAsync(lesson);

        if (result)
        {
            notifierThis.ShowSuccess("Dars muvaffaqiyatli qo'shildi!");
            ShowLessons();
        }
        else
        {
            notifierThis.ShowError("Dars qo'shishda xatolik yuz berdi!");
        }
    }

    private void GetAllUpdateAttendance()
    {
        allUpdateAttendances.Clear();

        foreach (var child in stLessons.Children)
        {
            if (child is LessonForAttendanceComponent component &&
                component.isChanged)
            {
                var updates = component.GetAttandanceStatus();

                if (updates is not null && updates.Any())
                    allUpdateAttendances.AddRange(updates);

                component.MarkAsSaved();
            }
        }
    }

    private async Task<char> SaveAsync()
    {
        if (!allUpdateAttendances.Any())
            return '0';

        var result = await _attendanceService.UpdateRangeAsync(allUpdateAttendances);

        if (result)
            return '1';
        else
            return '2';
    }

    private async void saveButton_Click(object sender, RoutedEventArgs e)
    {
        GetAllUpdateAttendance();
        char res = await SaveAsync();

        if(res is '1')
        {
            notifierThis.ShowSuccess("O'zgarishlar saqlandi!");
            allUpdateAttendances.Clear();
            ShowLessons();
        }
        else if (res is '2')
            notifierThis.ShowError("O'zgarishlarni saqlashda xatolik yuz berdi!");
        else
            notifierThis.ShowWarning("O'zgarishlar mavjud emas!");
    }
}
