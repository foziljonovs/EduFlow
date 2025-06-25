using EduFlow.Cashier.Desktop.Pages.SettingsForPages;
using System.Windows;

namespace EduFlow.Cashier.Desktop.Windows.SettingsForWindows;

/// <summary>
/// Interaction logic for SettingsForPrinterWindow.xaml
/// </summary>
public partial class SettingsForPrinterWindow : Window
{
    public SettingsForPrinterWindow()
    {
        InitializeComponent();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        SettingsForPrinterPage page = new SettingsForPrinterPage();
        pageNavigator.Content = page;
    }
}
