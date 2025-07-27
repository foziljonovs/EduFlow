using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.PaymentForComponents;

/// <summary>
/// Interaction logic for PaymentForComponent.xaml
/// </summary>
public partial class PaymentForComponent : UserControl
{
    private long Id { get; set; }
    public PaymentForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string studentName, double amount, double discount, PaymentStatus status, PaymentType type, DateTime paymentDate)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbStudentName.Text = studentName;
        tbAmount.Text = amount.ToString();
        tbDiscount.Text = discount.ToString();
        tbStatus.Text = status switch
        {
            PaymentStatus.Pending => "Kutilmoqda",
            PaymentStatus.Completed => "Yakunlangan",
            PaymentStatus.Failed => "Muvaffaqiyatsiz",
            PaymentStatus.Refunded => "Qaytarilgan",
            _ => "Tekshirilishi kerak"
        };

        tbType.Text = type switch
        {
            PaymentType.Cash => "Naqd",
            PaymentType.Card => "Karta",
            PaymentType.Transfer => "O'tqazma",
            PaymentType.Credit => "Nasiya",
            PaymentType.Other => "Boshqa",
            _ => "Boshqa"
        };

        tbPaymentDate.Text = paymentDate.ToString("dd:MM:yyyy");
    }
}
