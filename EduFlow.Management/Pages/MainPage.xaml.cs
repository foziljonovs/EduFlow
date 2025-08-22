using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.Desktop.Integrated.Services.Companies.Company;
using EduFlow.Management.Components.MainForComponents;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Pages;

/// <summary>
/// Interaction logic for MainPage.xaml
/// </summary>
public partial class MainPage : Page
{
    private readonly ICompanyService _companyService;
    public MainPage()
    {
        InitializeComponent();
        this._companyService = new CompanyService();
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

    private async Task GetCompanies()
    {
        try
        {
            companyLoader.Visibility = Visibility.Visible;
            var companies = await Task.Run(async () => await _companyService.GetAllAsync());

            ShowCompanies(companies);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("O'quv markazlar topilmadi!"); 
            companyLoader.Visibility = Visibility.Collapsed;
            emptyDataForCompany.Visibility = Visibility.Visible;
        }
    }

    private void ShowCompanies(List<CompanyForResultDto> companies)
    {
        int count = 1;
        stCompanies.Children.Clear();

        if (companies.Any())
        {
            companyLoader.Visibility = Visibility.Collapsed;
            emptyDataForCompany.Visibility = Visibility.Collapsed;

            foreach(var item in companies)
            {
                bool companySubscription = item.Subscriptions.Any(x => x.IsActive == true);

                MainForCompanyComponent companyComponent = new MainForCompanyComponent();
                companyComponent.SetValues(
                    item.Id,
                    count,
                    item.Name,
                    item.PhoneNumber,
                    companySubscription);

                companyComponent.IsClicked += async () => await GetCompanies();
                stCompanies.Children.Add(companyComponent);
                count++;
            }
        }
        else
        {
            companyLoader.Visibility = Visibility.Collapsed;
            emptyDataForCompany.Visibility = Visibility.Collapsed;
        }
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await GetCompanies();
    }
}
