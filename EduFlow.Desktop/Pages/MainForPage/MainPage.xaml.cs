using EduFlow.Desktop.Components.MainForComponents;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Pages.MainForPage;

/// <summary>
/// Interaction logic for MainPage.xaml
/// </summary>
public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void GetAllCourse()
    {
        int count = 0;
        stCourses.Children.Clear();

        for(int i = count; i < 20; i++)
        {
            MainForCourseComponent component = new MainForCourseComponent();
            component.SetValues(count, long.Parse(count.ToString()), "Foundation", 10, "Abdulvosid");
            stCourses.Children.Add(component);
            count++;
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        GetAllCourse();
    }
}
