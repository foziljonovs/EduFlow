using System.Windows;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for IncomeForPaymentWindow.xaml
/// </summary>
public partial class IncomeForPaymentWindow : Window
{
    public IncomeForPaymentWindow()
    {
        InitializeComponent();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
