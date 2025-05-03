using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForAddStudentWindow.xaml
/// </summary>
public partial class GroupForAddStudentWindow : Window
{
    private long Id { get; set; }
    private GroupForResultDto _group = new GroupForResultDto();
    private List<long> ChooseStudents = new List<long>();

    private readonly IStudentService _studentService;
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;
    public GroupForAddStudentWindow()
    {
        InitializeComponent();
        this._studentService = new StudentService();
        this._groupService = new GroupService();
        this._courseService = new CourseService();
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

    private void OnStudentComponentSelectionChanged(StudentForGroupComponent component, bool isSelected)
    {
        if (isSelected)
        {
            var studentId = component.GetId();

            if (!ChooseStudents.Contains(studentId))
                ChooseStudents.Add(studentId);
        }
        else
        {
            var studentId = component.GetId();
            if (ChooseStudents.Contains(studentId))
                ChooseStudents.Remove(studentId);
        }
    }

    private async Task GetGroup()
    {
        var group = await _groupService.GetByIdAsync(Id);

        if (group is not null)
            _group = group;
        else
            notifierThis.ShowError("Guruh topilmadi! Iltimos oynani qaytadan oching.");
    }

    private async Task GetStudents()
    {
        var students = await Task.Run(async () => await _studentService.GetAllAsync());
        
        var result = students.Where(x => x.StudentCourses
            .Any(y => y.Status == Domain.Enums.EnrollmentStatus.Pending &&
                y.CourseId == _group.CourseId)).ToList();

        ShowStudents(result);
    }

    private async Task FilterAsync()
    {
        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;

        StudentForFilterDto dto = new StudentForFilterDto();

        if (dtStartDate.SelectedDate != null)
            dto.StartedDate = dtStartDate.SelectedDate.Value;

        if (dtEndDate.SelectedDate != null)
            dto.FinishedDate = dtEndDate.SelectedDate.Value;

        dto.CourseId = _group.CourseId;

        var students = await Task.Run(async () => await _studentService.FilterAsync(dto));

        var result = students.Where(x => x.StudentCourses
            .Any(y => y.Status == Domain.Enums.EnrollmentStatus.Pending)).ToList();

        if (students.Any())
            ShowStudents(result);
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            studentforEmptyData.Visibility = Visibility.Visible;
        }
    }

    private void ShowStudents(List<StudentForResultDto> students)
    {
        int count = 1;

        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;

        if (students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            studentforEmptyData.Visibility = Visibility.Collapsed;

            foreach(var item in students)
            {
                var courseName = item.StudentCourses
                    .FirstOrDefault(x => x.Course is not null).Course.Name ?? "Nomalum";

                StudentForGroupComponent component = new StudentForGroupComponent();
                component.SetValues(
                    count,
                    item.Id,
                    item.Fullname,
                    courseName,
                    item.Age,
                    item.Address,
                    item.PhoneNumber);

                component.SelectionChanged += OnStudentComponentSelectionChanged;
                stStudents.Children.Add(component);
                count++;
            }
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            studentforEmptyData.Visibility = Visibility.Visible;
        }
    }

    public void SetId(long id)
        => this.Id = id;

    private async void LoadWindow()
    {
        await GetGroup();
        await GetStudents();
    }

    private bool IsWindowLoaded = false;
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if(!IsWindowLoaded)
        {
            IsWindowLoaded = true;
            LoadWindow();
        }
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void dtEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if(IsWindowLoaded)
            FilterAsync();
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ChooseStudents.Any())
        {
            if(Id > 0)
            {
                var result = await _groupService.AddStudentsToGroupAsync(Id, ChooseStudents);

                if (result)
                {
                    this.Close();
                    notifier.ShowSuccess("Talabalar muvaffaqiyatli qo'shildi!");
                }
                else
                    notifierThis.ShowError("Talabalar qo'shilmadi! Qayta urinib ko'ring.");
            }
        }
        else
            notifierThis.ShowWarning("Yangi talabalar tanlanmadi!");
    }
}
