using System.Windows;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for OutlayForPaymentWindow.xaml
/// </summary>
public partial class OutlayForPaymentWindow : Window
{
    public OutlayForPaymentWindow()
    {
        InitializeComponent();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
