using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Domain.Enums;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for PaymentForViewWindow.xaml
/// </summary>
public partial class PaymentForViewWindow : Window
{
    private readonly IPaymentService _paymentService;
    private long Id { get; set; }
    private PaymentForResultDto payment = new PaymentForResultDto();
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

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    public void SetId(long id)
        => this.Id = id;

    private void Empty()
    {
        tbReceiptNumber.Text = "00000000-0000-0000-0000-0000000-0000000";
        tbStudentName.Text = string.Empty;
        tbGroupName.Text = string.Empty;
        tbPrice.Text = "0";
        tbDiscountPrice.Text = "0";
        tbPaymentStatus.Text = string.Empty;
        tbPaymentDate.Text = "00.00.0000";
        tbPaymentType.Text = string.Empty;
        tbNotes.Text = string.Empty;
    }

    private async Task GetPayment()
    {
        try
        {
            var payment = await _paymentService.GetByIdAsync(Id);

            ShowValues(payment);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("To'ov malumotini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private void ShowValues(PaymentForResultDto payment)
    {
        if (payment is not null)
        {
            tbReceiptNumber.Text = payment.ReceiptNumber;
            tbStudentName.Text = payment.Student.Fullname;
            tbGroupName.Text = payment.Group.Name;
            tbPrice.Text = payment.Amount.ToString();
            tbDiscountPrice.Text = payment.Discount.ToString();

            tbPaymentType.Text = payment.Type switch
            {
                Domain.Enums.PaymentType.Cash => "Naqt",
                Domain.Enums.PaymentType.Card => "Karta",
                Domain.Enums.PaymentType.Transfer => "O'tqazma",
                Domain.Enums.PaymentType.Credit => "Nasiya",
                Domain.Enums.PaymentType.Other => "Belgilanmagan",
                _ => "No'malum"
            };

            tbPaymentStatus.Text = payment.Status switch
            {
                PaymentStatus.Pending => "Kutilmoqda",
                PaymentStatus.Completed => "Yakunlangan",
                PaymentStatus.Failed => "Muvaffaqiyatsiz",
                PaymentStatus.Refunded => "Qaytarilgan",
                _ => "No'malum"
            };

            tbPaymentDate.Text = payment.PaymentDate.ToString("dd.MM.yyyy HH:MM");
            tbNotes.Text = payment.Notes ?? "...";
        }
        else
        {
            notifierThis.ShowWarning("To'lov malumotlari topilmadi!");
            Empty();
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetPayment();
    }
}
