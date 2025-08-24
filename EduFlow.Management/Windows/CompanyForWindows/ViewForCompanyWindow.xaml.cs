using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Services.Companies.Company;
using EduFlow.Desktop.Integrated.Services.Companies.Logo;
using EduFlow.Desktop.Integrated.Services.Users.User.Interfaces;
using EduFlow.Desktop.Integrated.Services.Users.User.Services;
using EduFlow.Management.Components.SubscriptionForComponents;
using EduFlow.Management.Components.UserForComponents;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.CompanyForWindows;

/// <summary>
/// Interaction logic for ViewForCompanyWindow.xaml
/// </summary>
public partial class ViewForCompanyWindow : Window
{
    private readonly ICompanyService _companyService;
    private readonly ILogoService _logoService;
    private readonly IUserService _userService;

    private long Id { get; set; }
    private CompanyForResultDto Company { get; set; } = new CompanyForResultDto();
    public ViewForCompanyWindow()
    {
        InitializeComponent();
        this._companyService = new CompanyService();
        this._logoService = new LogoService();
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
        => this.Id = companyId;

    private async Task GetCompany()
    {
        CompanyLogoLoader.Visibility = Visibility.Visible;
        CompanySubscriptionLoader.Visibility = Visibility.Visible;

        try
        {
            var company = await _companyService.GetByIdAsync(this.Id);

            if(company is null)
            {
                notifierThis.ShowWarning("O'quv markaz malumotlarini yuklab bo'lmadi, iltimos oynani qayta yuklang!");
                return;
            }

            this.Company = company;
            ShowCompanyValues(company);
            ShowCompanySubscriptions(company.Subscriptions);
        }
        catch (Exception ex) 
        {
            notifierThis.ShowWarning("Malumotlarni yuklab bo'lmadi, iltimos qayta yuklang!");
            DefaultValues();
        }
    }

    private async Task GetCompanyLogo()
    {
        CompanyLogo.Visibility = Visibility.Collapsed;
        CompanyFallbackIcon.Visibility = Visibility.Collapsed;

        try
        {
            var logoBytes = await _logoService.GetLogoByIdAsync(this.Company.LogoId);

            if (logoBytes.Length > 0)
            {
                try
                {
                    using var ms = new MemoryStream(logoBytes);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    CompanyLogo.Source = bitmap;
                    CompanyLogo.Visibility = Visibility.Visible;
                }
                catch
                {
                    CompanyFallbackIcon.Visibility = Visibility.Visible;
                }
            }
            else
                CompanyFallbackIcon.Visibility = Visibility.Visible;
        }
        catch
        {
            CompanyFallbackIcon.Visibility = Visibility.Visible;
        }
        finally
        {
            CompanyLogoLoader.Visibility = Visibility.Collapsed;
        }
    }

    private async Task GetAdmins()
    {
        CompanyLogoLoader.Visibility = Visibility.Visible;
        
        try
        {
            var admins = await _userService.GetAllByCompanyIdAsync(this.Id);

            if(!admins.Any())
            {
                CompanyAdminLoader.Visibility = Visibility.Collapsed;
                emptyDataForCompanyAdmin.Visibility = Visibility.Visible;
                return;
            }

            ShowCompanyAdmins(admins);
        }
        catch(Exception ex)
        {
            CompanyAdminLoader.Visibility = Visibility.Collapsed;
            emptyDataForCompanyAdmin.Visibility = Visibility.Visible;
        }
    }

    private void ShowCompanyAdmins(List<UserForResultDto> users)
    {
        CompanyAdminLoader.Visibility = Visibility.Collapsed;
        stAdmins.Children.Clear();

        if (users.Any())
        {
            emptyDataForCompanyAdmin.Visibility = Visibility.Collapsed;

            var admins = users.Where(x => x.Role == Domain.Enums.UserRole.Manager).ToList();

            if (admins.Any())
            {
                foreach (var admin in admins)
                {
                    CompanyForUserComponent adminComponent = new CompanyForUserComponent();
                    adminComponent.SetValues(
                        admin.Id,
                        $"{admin.Firstname} {admin.Lastname}",
                        admin.PhoneNumber);

                    stAdmins.Children.Add(adminComponent);
                }
            }
            else
                emptyDataForCompanyAdmin.Visibility = Visibility.Visible;
        }
        else
            emptyDataForCompanyAdmin.Visibility = Visibility.Visible;
    }

    private void ShowCompanyValues(CompanyForResultDto company)
    {
        tbCompanyName.Text = company.Name;
        tbPhoneNumber.Text = company.PhoneNumber;
        tbAbout.Text = company.About;
        tbAddress.Text = company.Address;
    }

    private void ShowCompanySubscriptions(List<CompanySubscriptionForShortResultDto> subscriptions)
    {
        stSubscriptions.Children.Clear();

        if (subscriptions.Any())
        {
            CompanySubscriptionLoader.Visibility = Visibility.Collapsed;
            emptyDataForCompanySubscription.Visibility = Visibility.Collapsed;

            foreach(var subscription in subscriptions)
            {
                SubscriptionForCompanyViewComponent companyViewComponent = new SubscriptionForCompanyViewComponent();
                companyViewComponent.SetValues(
                    subscription.Id,
                    subscription.IsActive,
                    subscription.SubcriptionDate ?? DateTime.UtcNow);

                stSubscriptions.Children.Add(companyViewComponent);
            }
        }
        else
        {
            CompanySubscriptionLoader.Visibility = Visibility.Collapsed;
            emptyDataForCompanySubscription.Visibility = Visibility.Visible;
        }
    }

    private void DefaultValues()
    {
        tbCompanyName.Text = "...";
        tbAbout.Text = "Izoh mavjud emas!";
        tbPhoneNumber.Text = "+998000000000";
        tbAddress.Text = "...";
        
        CompanyFallbackIcon.Visibility = Visibility.Visible;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetCompany();
        await GetCompanyLogo();
        await GetAdmins();
    }
}
