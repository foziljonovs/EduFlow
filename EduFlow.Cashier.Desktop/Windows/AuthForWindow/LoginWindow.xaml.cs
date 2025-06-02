using System.Windows;

namespace EduFlow.Cashier.Desktop.Windows.AuthForWindow;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void LoginBtn_Click(object sender, RoutedEventArgs e)
    {
        MainWindow window = new MainWindow();
        window.Show();

        this.Close();
    }
}
