using EduFlow.Desktop.Components.StudentForComponents;
using System.Windows;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForViewWindow.xaml
/// </summary>
public partial class GroupForViewWindow : Window
{
    public GroupForViewWindow()
    {
        InitializeComponent();
    }

    private void StaticDatas()
    {
        stStudents.Children.Clear();

        for(int i = 1; i <= 10; i++)
        {
            StudentForAttendanceComponent component = new StudentForAttendanceComponent();
            component.setValues(i, i, "Shavkatjonov Muhammadaziz");
            stStudents.Children.Add(component);
        }
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        StaticDatas();
    }
}
