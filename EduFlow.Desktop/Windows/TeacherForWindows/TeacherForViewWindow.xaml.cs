using System.Windows;

namespace EduFlow.Desktop.Windows.TeacherForWindows;

/// <summary>
/// Interaction logic for TeacherForViewWindow.xaml
/// </summary>
public partial class TeacherForViewWindow : Window
{
    public TeacherForViewWindow()
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
