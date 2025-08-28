using System.Windows.Controls;

namespace EduFlow.Desktop.Components.PaymentForComponents;

/// <summary>
/// Interaction logic for PaymentForMinComponent.xaml
/// </summary>
public partial class PaymentForMinComponent : UserControl
{
    private long PaymentId { get; set; }
    public PaymentForMinComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, double amount, string courseName, DateTime paymentDate)
    {
        this.PaymentId = id;
        tbAmount.Text = amount.ToString();
        tbCourseName.Text = courseName;
        tbPaymentDate.Text = paymentDate.ToString("dd/MM/yyyy");
    }
}
