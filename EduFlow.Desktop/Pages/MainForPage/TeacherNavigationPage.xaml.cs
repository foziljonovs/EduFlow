using EduFlow.Desktop.Pages.CourseForPages;
using EduFlow.Desktop.Windows;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Pages.MainForPage;

/// <summary>
/// Interaction logic for TeacherNavigationPage.xaml
/// </summary>
public partial class TeacherNavigationPage : Page
{
    public TeacherNavigationPage()
    {
        InitializeComponent();
    }

    private MainWindow getWindow()
        => Window.GetWindow(this) as MainWindow;    
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        GetMainPage();
    }

    private void GetMainPage()
    {
        MainPage page = new MainPage();
        var window = getWindow();
        if (window != null)
            window.NavigatePage(page);
    }

    private void MainButton_Click(object sender, RoutedEventArgs e)
    {
        GetMainPage();
    }

    private void CoursesButton_Click(object sender, RoutedEventArgs e)
    {
        CoursePage page = new CoursePage();
        var window = getWindow();
        if(window != null)
            window.NavigatePage(page);
    }
}
