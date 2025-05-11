using System.Windows;

namespace EduFlow.Desktop.Windows.CourseForWindows;

/// <summary>
/// Interaction logic for CourseForViewWindow.xaml
/// </summary>
public partial class CourseForViewWindow : Window
{
    public CourseForViewWindow()
    {
        InitializeComponent();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;
}
