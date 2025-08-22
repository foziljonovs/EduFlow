using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Management.Components.SubscriptionForComponents;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Pages.SubscriptionForPages;

/// <summary>
/// Interaction logic for SubscriptionForPage.xaml
/// </summary>
public partial class SubscriptionForPage : Page
{
    private readonly ICompanySubscriptionService _companySubscriptionService;
    public SubscriptionForPage()
    {
        InitializeComponent();
        this._companySubscriptionService = new CompanySubscriptionService();
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

    private async Task GetAllSubscription()
    {
        try
        {
            subscriptionLoader.Visibility = Visibility.Visible;
            var subscriptions = await Task.Run(async () => await _companySubscriptionService.GetAllAsync());

            ShowSubscriptions(subscriptions);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Obunalarni yuklab bo'lmadi!");
            subscriptionLoader.Visibility = Visibility.Collapsed;
            emptyDataForSubscription.Visibility = Visibility.Visible;
        }
    }

    private void ShowSubscriptions(List<CompanySubscriptionForResultDto> subscriptions)
    {
        int count = 1;

        stSubscriptions.Children.Clear();

        if (subscriptions.Any())
        {
            subscriptionLoader.Visibility = Visibility.Collapsed;
            emptyDataForSubscription.Visibility = Visibility.Collapsed;

            foreach(var item in subscriptions)
            {
                SubscriptionForComponent component = new SubscriptionForComponent();
                component.SetValues(
                    item.Id,
                    count,
                    item.Company.Name,
                    item.Type,
                    item.IsPaid,
                    item.SubcriptionDate ?? DateTime.UtcNow.AddHours(5),
                    item.IsActive);

                stSubscriptions.Children.Add(component);
                count++;
            }
        }
        else
        {
            subscriptionLoader.Visibility = Visibility.Collapsed;
            emptyDataForSubscription.Visibility = Visibility.Visible;
        }
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await GetAllSubscription();
    }
}
