using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Pages.MainForPage;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void NavigatePage(Page page)
        => Navigate.Content = page;

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if (NormalButton.Visibility == Visibility.Visible)
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(MaxButton.Visibility == Visibility.Visible)
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        NormalButton.IsChecked = true;
        var role = IdentitySingelton.GetInstance().Role;
        if (role is Domain.Enums.UserRole.Teacher)
        {
            TeacherNavigationPage page = new TeacherNavigationPage();
            MainMenuNavigation.Content = page;
        }
        else
        {
            ManagerNavigationPage page = new ManagerNavigationPage();
            MainMenuNavigation.Content = page;
        }
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}