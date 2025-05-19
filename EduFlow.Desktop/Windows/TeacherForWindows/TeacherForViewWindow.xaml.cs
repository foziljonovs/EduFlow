using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.GroupForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.TeacherForWindows;

/// <summary>
/// Interaction logic for TeacherForViewWindow.xaml
/// </summary>
public partial class TeacherForViewWindow : Window
{
    private readonly ITeacherService _teacherService;
    private readonly IGroupService _groupService;
    private long Id { get; set; }
    private TeacherForResultDto _teacher { get; set; } = new TeacherForResultDto();
    public TeacherForViewWindow()
    {
        InitializeComponent();
        this._teacherService = new TeacherService();
        this._groupService = new GroupService();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async Task<TeacherForResultDto> GetTeacher()
    {
        var teacher = await _teacherService.GetByIdAsync(Id);

        if (teacher is not null)
            return teacher;
        else
            return new TeacherForResultDto();
    }

    private async Task<List<GroupForResultDto>> GetGroups()
    {
        var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(Id));

        if (groups is not null)
            return groups;
        else
            return new List<GroupForResultDto>();
    }

    private async Task FilterAsync()
    {
        stGroups.Children.Clear();
        groupsForLoader.Visibility = Visibility.Visible;

        GroupForFilterDto dto = new GroupForFilterDto();

        if(startedDate.SelectedDate is not null)
            dto.StartedDate = startedDate.SelectedDate.Value;

        if (endDate.SelectedDate is not null)
            dto.FinishedDate = endDate.SelectedDate.Value;

        dto.TeacherId = Id;

        var groups = await Task.Run(async () => await _groupService.FilterAsync(dto));

        ShowGroups(groups);
    }

    private async void ShowValues()
    {
        groupsForLoader.Visibility = Visibility.Visible;
        this._teacher = await GetTeacher();

        if(_teacher is not null)
        {
            tbName.Text = _teacher.User.Firstname + " " + _teacher.User.Lastname;
            tbCourseName.Text = _teacher.Course.Name;
            tbPhoneNumber.Text = _teacher.User.PhoneNumber;
            tbSalary.Text = "0";
            tbSkills.Text = string.Join(", ", _teacher.Skills);
            tbStudentsCount.Text = _teacher.Groups.Sum(x => x.Students.Count()).ToString() ?? "0";
        }
        else
        {
            notifierThis.ShowWarning("O'qituvchi ma'lumotlari topilmadi!");
            EmptyValues();
            return;
        }

        var groups = await GetGroups();
        ShowGroups(groups);
    }

    private void ShowGroups(List<GroupForResultDto> groups)
    {
        int count = 1;
        if (groups.Any())
        {
            groupsForLoader.Visibility = Visibility.Collapsed;
            groupForEmptyData.Visibility = Visibility.Collapsed;

            foreach (var group in groups)
            {
                GroupForMinComponent component = new GroupForMinComponent();
                component.SetValues(
                    count,
                    group.Id,
                    group.Name,
                    group.Students.Count(),
                    _teacher.User.Firstname,
                    group.IsStatus);

                stGroups.Children.Add(component);
                count++;
            }
        }
        else
        {
            notifierThis.ShowWarning("O'qituvchi guruhlari topilmadi!");
            groupsForLoader.Visibility = Visibility.Collapsed;
            groupForEmptyData.Visibility = Visibility.Visible;
        }
    }

    private void EmptyValues()
    {
        tbName.Text = string.Empty;
        tbCourseName.Text = string.Empty;
        tbPhoneNumber.Text = "+998000000000";
        tbSalary.Text = "0";
        tbSkills.Text = string.Empty;
        tbStudentsCount.Text = "0";

        groupForEmptyData.Text = string.Empty;
    }

    private bool isLoaded = false;
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if(!isLoaded)
        {
            ShowValues();
            isLoaded = true;
        }
    }

    private void endDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (IsLoaded)
            FilterAsync();
    }
}
