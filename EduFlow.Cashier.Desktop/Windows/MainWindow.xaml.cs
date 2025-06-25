using EduFlow.Cashier.Desktop.Pages.MainForPages;
using EduFlow.Cashier.Desktop.Pages.PaymentForPages;
using EduFlow.Cashier.Desktop.Pages.StatsForPages;
using EduFlow.Cashier.Desktop.Pages.StudentForPages;
using EduFlow.Cashier.Desktop.Windows.SettingsForWindows;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if( this.WindowState == WindowState.Maximized)
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
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    public void NavigatePage(Page page)
        => Navigate.Content = page;

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        homeBtn.IsChecked = true;

        MainPage page = new MainPage();
        NavigatePage(page);
    }

    private void homeBtn_Click(object sender, RoutedEventArgs e)
    {
        homeBtn.IsChecked = true;

        MainPage page = new MainPage();
        NavigatePage(page);
    }

    private void paymentBtn_Click(object sender, RoutedEventArgs e)
    {
        paymentBtn.IsChecked = true;

        PaymentPage page = new PaymentPage();
        NavigatePage(page);
    }

    private void studentBtn_Click(object sender, RoutedEventArgs e)
    {
        studentBtn.IsChecked = true;

        StudentPage page = new StudentPage();
        NavigatePage(page);
    }

    private void statisticBtn_Click(object sender, RoutedEventArgs e)
    {
        statisticBtn.IsChecked = true;

        StatsPage page = new StatsPage();
        NavigatePage(page);
    }

    private void settingsBtn_Click(object sender, RoutedEventArgs e)
    {
        settingsBtn.IsChecked = true;

        SettingsForPrinterWindow window = new SettingsForPrinterWindow();
        window.ShowDialog();
    }
}