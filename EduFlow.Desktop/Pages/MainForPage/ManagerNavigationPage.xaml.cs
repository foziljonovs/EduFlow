using EduFlow.Desktop.Pages.CourseForPages;
using EduFlow.Desktop.Windows;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Pages.MainForPage;

/// <summary>
/// Interaction logic for ManagerNavigationPage.xaml
/// </summary>
public partial class ManagerNavigationPage : Page
{
    public ManagerNavigationPage()
    {
        InitializeComponent();
    }

    private MainWindow GetMainWindow()
        => Window.GetWindow(this) as MainWindow;

    private void GetMainPage()
    {
        MainButton.IsChecked = true;
        MainPage page = new MainPage();
        var window = GetMainWindow();
        if(window != null)
            window.NavigatePage(page);
    }
    private void MainButton_Click(object sender, RoutedEventArgs e)
    {
        GetMainPage();
    }

    private void CoursesButton_Click(object sender, RoutedEventArgs e)
    {
        CoursesButton.IsChecked = true;
        CoursePage page = new CoursePage();
        var window = GetMainWindow(); 
        if(window != null)
            window.NavigatePage(page);
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        GetMainPage();
    }
}
