using System.Windows;

namespace EduFlow.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForViewWindow.xaml
/// </summary>
public partial class StudentForViewWindow : Window
{
    public StudentForViewWindow()
    {
        InitializeComponent();
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
