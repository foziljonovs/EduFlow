using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Services.Users.User.Interfaces;
using EduFlow.Desktop.Integrated.Services.Users.User.Services;
using EduFlow.Management.Components.UserForComponents;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.CompanyForWindows;

/// <summary>
/// Interaction logic for ViewForCompanyEmployeeWindow.xaml
/// </summary>
public partial class ViewForCompanyEmployeeWindow : Window
{
    private readonly IUserService _userService;
    private long CompanyId { get; set; }
    public ViewForCompanyEmployeeWindow()
    {
        InitializeComponent();
        this._userService = new UserService();
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

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    public void SetCompanyId(long companyId)
        => this.CompanyId = companyId;

    private async Task GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllByCompanyIdAsync(this.CompanyId);

            if (!users.Any())
            {
                notifierThis.ShowWarning("Xodimlar topilmadi!");
                AdminLoader.Visibility = Visibility.Collapsed;
                emptyDataForAdmin.Visibility = Visibility.Visible;
            }

            ShowValues(users);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Malumotlarni yuklashda xatolik yuz berdi, iltimos qayta urining!");
            AdminLoader.Visibility = Visibility.Collapsed;
            emptyDataForAdmin.Visibility = Visibility.Visible;
        }
    }

    private void ShowValues(List<UserForResultDto> users)
    {
        int count = 1;

        stAdmins.Children.Clear();

        if (users.Any())
        {
            AdminLoader.Visibility = Visibility.Collapsed;
            emptyDataForAdmin.Visibility = Visibility.Collapsed;

            var existsAdmins = users.Where(x => x.Role != Domain.Enums.UserRole.Teacher &&
                                                x.Role != Domain.Enums.UserRole.Dev).ToList();

            if(existsAdmins.Any())
            {
                foreach(var admin in existsAdmins)
                {
                    CompanyForUserComponent component = new CompanyForUserComponent();
                    component.SetValues(
                        admin.Id,
                        count,
                        $"{admin.Firstname} {admin.Lastname}",
                        admin.PhoneNumber,
                        admin.Role);

                    stAdmins.Children.Add(component);
                    count++;
                }
            }
            else
            {
                emptyDataForAdmin.Visibility = Visibility.Visible;
            }
        }
        else
        {
            emptyDataForAdmin.Visibility = Visibility.Visible;
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetAllUsers();
    }
}
