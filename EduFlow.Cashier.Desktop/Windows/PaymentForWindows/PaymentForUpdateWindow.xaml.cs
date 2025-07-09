using System.Windows;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for PaymentForUpdateWindow.xaml
/// </summary>
public partial class PaymentForUpdateWindow : Window
{
    public PaymentForUpdateWindow()
    {
        InitializeComponent();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(this.WindowState == WindowState.Maximized)
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
    }

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if(this.WindowState == WindowState.Normal)
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            NormalButton.Visibility = Visibility.Visible;
            MaxButton.Visibility = Visibility.Collapsed;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void teacherComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {

    }
}
