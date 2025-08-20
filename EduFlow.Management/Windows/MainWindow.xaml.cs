using EduFlow.Management.Pages;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Management.Windows;

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
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

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
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    public void NavigatePage(Page page)
        => Navigate.Content = page;
    private void HomeBtn_Click(object sender, RoutedEventArgs e)
    {
        HomeBtn.IsChecked = true;

        MainPage page = new MainPage();
        NavigatePage(page);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        HomeBtn.IsChecked = true;

        MainPage page = new MainPage();
        NavigatePage(page);
    }
}