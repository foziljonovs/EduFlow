using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.SubscriptionForWindows;

/// <summary>
/// Interaction logic for UpdateForSubscriptionWindow.xaml
/// </summary>
public partial class UpdateForSubscriptionWindow : Window
{
    private readonly ICompanySubscriptionService _subscriptionService;
    private long Id { get; set; }
    public UpdateForSubscriptionWindow()
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

    private async Task GetSubscription()
    {
        try
        {
            var subscription = await _subscriptionService.GetByIdAsync(this.Id);

            if(subscription is null)
            {
                notifierThis.ShowWarning("Obuna malumotlarni yuklashda muammo yuz berdi, iltimos oynani qayta yuklang!");
                return;
            }

            ShowValues(subscription);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Obuna malumotlari topilmadi, iltimos qayta urining!");
        }
    }

    private void ShowValues(CompanySubscriptionForResultDto subscription)
    {
        foreach(ComboBoxItem item in SubscriptionTypeComboBox.Items)
        {
            int itemTag = int.Parse(item.Tag.ToString());

            if(itemTag == (int)subscription.Type)
            {
                SubscriptionTypeComboBox.SelectedItem = item;
                break;
            }
        }

        if (subscription.IsPaid)
            IsPaidCheckBox.IsChecked = true;
        else
            IsPaidCheckBox.IsChecked = false;

        if (subscription.IsActive)
            IsActiveCheckBox.IsChecked = true;
        else
            IsActiveCheckBox.IsChecked = false;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetSubscription();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
