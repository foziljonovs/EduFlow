using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.PaymentForComponents;

/// <summary>
/// Interaction logic for PaymentForStudentViewComponent.xaml
/// </summary>
public partial class PaymentForStudentViewComponent : UserControl
{
    private long Id { get; set; }
    public PaymentForStudentViewComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, double amount, PaymentStatus status, DateTime paymentDate)
    {
        this.Id = id;
        tbPaymentAmount.Text = amount.ToString();

        tbStatus.Text = status switch
        {
            PaymentStatus.Pending => "Kutilmoqda",
            PaymentStatus.Completed => "Yakunlangan",
            PaymentStatus.Failed => "Xatolik yuz bergan",
            PaymentStatus.Refunded => "Qaytarilgan"
        };

        tbPaymentDate.Text = paymentDate.ToString("dd.MM.yyyy");
    }
}
