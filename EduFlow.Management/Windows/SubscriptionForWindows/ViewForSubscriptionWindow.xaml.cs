using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.SubscriptionForWindows;

/// <summary>
/// Interaction logic for ViewForSubscriptionWindow.xaml
/// </summary>
public partial class ViewForSubscriptionWindow : Window
{
    private readonly ICompanySubscriptionService _subscriptionService;
    private long Id { get; set; }
    public ViewForSubscriptionWindow()
    {
        InitializeComponent();
        this._subscriptionService = new CompanySubscriptionService();
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

    public void SetId(long id)
        => this.Id = id;

    private void EmptyData()
    {
        tbName.Text = "...";
        tbPhoneNumber.Text = "...";
        tbSubscriptionDate.Text = "00/00/0000";
        tbSubscriptionType.Text = "...";
    }

    private async Task GetSubscription()
    {
        try
        {
            var subscription = await _subscriptionService.GetByIdAsync(this.Id);

            if(subscription is null)
            {
                notifier.ShowWarning("Obuna topilmadi, iltimos oynani qayta yuklang!");
                EmptyData();
                return;
            }

            ShowValues(subscription);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Ma'lumotlarni yuklashda xatolik yuz berdi, iltimos qayta urining!");
        }
    }

    private void ShowValues(CompanySubscriptionForResultDto subscription)
    {
        tbName.Text = subscription.Company.Name;
        tbPhoneNumber.Text = subscription.Company.PhoneNumber;
        tbSubscriptionDate.Text = subscription.SubcriptionDate?.ToString("dd/MM/yyyy");
        tbSubscriptionType.Text = subscription.Type switch
        {
            SubscriptionType.Monthly => "1 oylik",
            SubscriptionType.OneTime => "Doimiy"
        };

        if (subscription.IsPaid)
            IsPaidBorder.Visibility = Visibility.Visible;
        else
            NoIsPaidBorder.Visibility = Visibility.Visible;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetSubscription();
    }
}
