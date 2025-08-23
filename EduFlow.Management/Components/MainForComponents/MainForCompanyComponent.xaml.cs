using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Management.Windows.CompanyForWindows;
using EduFlow.Management.Windows.SubscriptionForWindows;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Components.MainForComponents;

/// <summary>
/// Interaction logic for MainForCompanyComponent.xaml
/// </summary>
public partial class MainForCompanyComponent : UserControl
{
    private readonly ICompanySubscriptionService _subscriptionService;
    private long Id { get; set; }
    public Func<Task>? IsClicked { get; set; }

    public MainForCompanyComponent()
    {
        InitializeComponent();
        this._subscriptionService = new CompanySubscriptionService();
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

    public void SetValues(long id, int number, string name, string phoneNumber, bool subscriptionStatus)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbPhoneNumber.Text = phoneNumber;

        if (subscriptionStatus)
        {
            activeBorder.Visibility = System.Windows.Visibility.Visible;
            noActiveBorder.Visibility = System.Windows.Visibility.Collapsed;
        }
        else
        {
            activeBorder.Visibility = System.Windows.Visibility.Collapsed;
            noActiveBorder.Visibility = System.Windows.Visibility.Visible;
        }
    }

    private async void EditSubscriptionButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if(!EditButton.IsEnabled)
        {
            notifier.ShowWarning("Iltimos kuting, obuna borligi tekshirilyapdi!");
            return;
        }

        EditButton.IsEnabled = false;

        try
        {
            var existsSubscriptionResult = await ExistsSubscription();
            if(existsSubscriptionResult)
            {
                EnableForSubscriptionWindow window = new EnableForSubscriptionWindow();
                window.SetCompanyId(this.Id);
                window.ShowDialog();
                await IsClicked?.Invoke();
            }
            else
                notifier.ShowInformation("Faol obuna mavjud!");
        }
        finally
        {
            EditButton.IsEnabled = true;
        }
    }

    private async Task<bool> ExistsSubscription()
    {
        try
        {
            var subscriptions = await _subscriptionService.GetAllByCompanyIdAsync(this.Id);

            if (subscriptions.Any())
            {
                var existsResult = subscriptions
                    .FirstOrDefault(x => x.IsActive == true);

                if (existsResult != null)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }
        catch(Exception ex)
        {
            return true;
        }
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        UpdateForCompanyWindow window = new UpdateForCompanyWindow();
        window.SetCompanyId(this.Id);
        window.ShowDialog();
        await IsClicked?.Invoke();
    }
}
