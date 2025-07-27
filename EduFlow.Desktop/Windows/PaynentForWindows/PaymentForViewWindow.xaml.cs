using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Components.PaymentForComponents;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.PaynentForWindows;

/// <summary>
/// Interaction logic for PaymentForViewWindow.xaml
/// </summary>
public partial class PaymentForViewWindow : Window
{
    private readonly IPaymentService _paymentService;
    private long _groupId { get; set; }
    public PaymentForViewWindow()
    {
        InitializeComponent();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(NormalButton.Visibility == Visibility.Visible)
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if(NormalButton.Visibility == Visibility.Visible)
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility= Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async Task GetAllPayment()
    {
        try
        {
            paymentLoader.Visibility = Visibility.Visible;

            var payments = await Task.Run(async () => await _paymentService.GetAllByGroupIdAsync(this._groupId));

            ShowPayments(payments);
        }
        catch(Exception ex)
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
        }
    }

    private void ShowPayments(List<PaymentForResultDto> payments)
    {
        int count = 1;
        stPayments.Children.Clear();

        if (payments.Any())
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Collapsed;

            foreach(var payment in payments)
            {
                PaymentForComponent component = new PaymentForComponent();
                component.SetValues(
                    count,
                    payment.Id,
                    payment.Student.Fullname,
                    payment.Amount,
                    payment.Discount,
                    payment.Status,
                    payment.Type,
                    payment.PaymentDate);

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

    public void SetId(long id)
        => this._groupId = id;

    private bool IsWindowLoaded = false;
    private async Task LoadedWindow()
    {
        if (!IsWindowLoaded)
        {
            IsWindowLoaded = true;
            await GetAllPayment();
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadedWindow();
    }
}
