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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;
}
