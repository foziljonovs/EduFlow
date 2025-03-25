using EduFlow.Desktop.Pages.CourseForPages;
using EduFlow.Desktop.Pages.StudentForPages;
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
        MainButton.IsChecked = true;
        MainPage page = new MainPage();
        var window = getWindow();
        if (window != null)
            window.NavigatePage(page);
    }

    private void MainButton_Click(object sender, RoutedEventArgs e)
    {
        GetMainPage();
    }

    private void StudentsButton_Click(object sender, RoutedEventArgs e)
    {
        StudentsButton.IsChecked = true;
        StudentPage page = new StudentPage();
        var window = getWindow();
        if (window != null)
            window.NavigatePage(page);
    }
}
