using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Domain.Enums;
using EduFlow.Management.Windows;
using EduFlow.Management.Windows.SubscriptionForWindows;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Components.SubscriptionForComponents;

/// <summary>
/// Interaction logic for SubscriptionForComponent.xaml
/// </summary>
public partial class SubscriptionForComponent : UserControl
{
    private readonly ICompanySubscriptionService _subscriptionService;
    private long Id { get; set; }
    public Func<Task>? IsClicked { get; set; }
    public SubscriptionForComponent()
    {
        InitializeComponent();
        this._subscriptionService = new CompanySubscriptionService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 50,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    public void SetValues(long id, int number, string company, SubscriptionType type, bool isPaid, DateTime subscriptionDate, bool isActived)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbCompanyName.Text = company;

        if(type == SubscriptionType.Monthly)
            TypeMonthlyBorder.Visibility = System.Windows.Visibility.Visible;
        else
            TypeYearBorder.Visibility = System.Windows.Visibility.Visible;

        if (isPaid)
            IsPaidBorder.Visibility = System.Windows.Visibility.Visible;
        else
            IsNoPaidBorder.Visibility = System.Windows.Visibility.Visible;

        tbSubscriptionDate.Text = subscriptionDate.ToString("dd/MM/yyyy");

        if(isActived)
            IsActiveBorder.Visibility = System.Windows.Visibility.Visible;
        else
            IsNoActiveBorder.Visibility = System.Windows.Visibility.Visible;
    }

    private async void NoActiveButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MessageBoxWindow window = new MessageBoxWindow("Obuna faolsizlashtirilsinmi?", MessageBoxWindow.MessageType.Confirmation, MessageBoxWindow.MessageButtons.YesNo);
        var messageResult = window.ShowDialog();

        if(messageResult is true)
        {
            var res = await _subscriptionService.DeleteAsync(this.Id);

            if (res)
            {
                notifier.ShowInformation($"{tbCompanyName.Text} obunasi o'chirildi!");
                await IsClicked?.Invoke();
            }
            else
                notifier.ShowWarning("O'chirib bo'lmadi, iltimos qayta urining!");
        }
    }

    private void ViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewForSubscriptionWindow window = new ViewForSubscriptionWindow();
        window.SetId(this.Id);
        window.ShowDialog();
    }
}
