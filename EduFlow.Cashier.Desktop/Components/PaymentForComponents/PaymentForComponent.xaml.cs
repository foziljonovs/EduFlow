using EduFlow.Cashier.Desktop.Windows.PaymentForWindows;
using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.PaymentForComponents;

/// <summary>
/// Interaction logic for PaymentForComponent.xaml
/// </summary>
public partial class PaymentForComponent : UserControl
{
    private long Id { get; set; }
    private string ReceiptNumber { get; set; }
    public PaymentForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string receiptNumber, double amount, double discount, PaymentStatus status, PaymentType type, DateTime paymentDate)
    {
        this.Id = id;
        this.ReceiptNumber = receiptNumber;
        tbNumber.Text = number.ToString();
        tbReceiptNumber.Text = receiptNumber;
        tbAmount.Text = amount.ToString();
        tbDiscount.Text = discount.ToString();
        tbStatus.Text = status switch
        {
            PaymentStatus.Pending => "Kutilmoqda",
            PaymentStatus.Completed => "Tugallangan",
            PaymentStatus.Failed => "Muvaffaqiyatsiz",
            PaymentStatus.Refunded => "Qaytarilgan",
            _ => "Noma'lum"
        };

        tbType.Text = type switch
        {
            PaymentType.Cash => "Naqt",
            PaymentType.Card => "Karta",
            PaymentType.Transfer => "O'tqazma",
            PaymentType.Credit => "Nasiya",
            PaymentType.Other => "Belgilanmagan",
            _ => "Noma'lum"
        };

        tbPaymentDate.Text = paymentDate.ToString("dd.MM.yyyy HH:MM");
    }

    private void viewBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        PaymentForViewWindow window = new PaymentForViewWindow();
        window.SetId(this.Id);
        window.ShowDialog();
    }

    private void editBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        PaymentForUpdateWindow window = new PaymentForUpdateWindow();
        window.SetId(this.Id);
        window.ShowDialog();
    }
}
