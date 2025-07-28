using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.GroupForComponents;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows.PaynentForWindows;
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
    private readonly IPaymentService _paymentService;
    private long Id { get; set; }
    private int pageSize = 15;
    private int pageNumber = 1;
    private TeacherForResultDto _teacher { get; set; } = new TeacherForResultDto();
    public TeacherForViewWindow()
    {
        InitializeComponent();
        this._teacherService = new TeacherService();
        this._groupService = new GroupService();
        this._paymentService = new PaymentService();
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

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async Task<TeacherForResultDto> GetTeacher()
    {
        try
        {
            var teacher = await _teacherService.GetByIdAsync(Id);

            if (teacher is not null)
                return teacher;
            else
                return new TeacherForResultDto();
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'qituvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            return new TeacherForResultDto();
        }
    }

    private async Task<List<GroupForResultDto>> GetGroups()
    {
        try
        {
            var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(Id));

            if (groups is not null)
                return groups;
            else
                return new List<GroupForResultDto>();
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Guruh malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            return new List<GroupForResultDto>();
        }
    }

    private async Task FilterAsync()
    {
        try
        {
            stGroups.Children.Clear();
            groupsForLoader.Visibility = Visibility.Visible;

            GroupForFilterDto dto = new GroupForFilterDto();

            if(startedDate.SelectedDate is not null)
                dto.StartedDate = startedDate.SelectedDate.Value;

            if (endDate.SelectedDate is not null)
                dto.FinishedDate = endDate.SelectedDate.Value;

            dto.TeacherId = Id;

            var groups = await Task.Run(async () => await _groupService.FilterAsync(dto, pageSize, pageNumber));

            ShowGroups(groups.Data);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Malumotlarni filterlashda xatolik yuz berdi, Iltimos qayta urining!");
            groupsForLoader.Visibility = Visibility.Collapsed;
            groupForEmptyData.Visibility = Visibility.Visible;
        }
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
            tbSkills.Text = string.Join(", ", _teacher.Skills);
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
        tbSkills.Text = string.Empty;

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

    private void btnTeacherForViewPayments_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        PaymentForViewWindow window = new PaymentForViewWindow();
        window.SetId(this.Id);
        window.ShowDialog();
    }
}
