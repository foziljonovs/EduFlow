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

    public void SetValues(long id, double amount, DateTime paymentDate)
    {
        this.Id = id;
        tbPaymentAmount.Text = amount.ToString();
        tbPaymentDate.Text = paymentDate.ToString();
    }
}
