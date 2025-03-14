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
        => this.WindowState = WindowState.Normal;

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        NormalButton.IsChecked = true;
        TeacherNavigationPage page = new TeacherNavigationPage();
        MainMenuNavigation.Content = page;
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}