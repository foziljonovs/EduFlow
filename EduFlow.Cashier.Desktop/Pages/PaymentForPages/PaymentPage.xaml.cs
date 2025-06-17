using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Cashier.Desktop.Components.PaymentForComponents;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Pages.PaymentForPages;

/// <summary>
/// Interaction logic for PaymentPage.xaml
/// </summary>
public partial class PaymentPage : Page
{
    private readonly IPaymentService _paymentService;
    public PaymentPage()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
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

    private async Task GetAllPayment()
    {
        paymentLoader.Visibility = Visibility.Visible;

        var payments = await Task.Run(async () => await _paymentService.GetAllAsync());

        ShowPayments(payments);
    }

    private void ShowPayments(List<PaymentForResultDto> payments)
    {
        int count = 1;

        if (payments.Any())
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Collapsed;

            foreach(var item in payments)
            {
                PaymentForComponent component = new PaymentForComponent();
                component.Tag = item.Id;
                component.SetValues(
                    count,
                    item.Id,
                    item.ReceiptNumber,
                    item.Amount,
                    "static qiymat",
                    item.Discount,
                    item.Status,
                    item.Type,
                    item.PaymentDate);

                stPayments.Children.Add(component);
                count++;
            }
        }
        else
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
        }
    }

    private async Task LoadPage()
    {
        await GetAllPayment();
    }
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPage();
    }
}
