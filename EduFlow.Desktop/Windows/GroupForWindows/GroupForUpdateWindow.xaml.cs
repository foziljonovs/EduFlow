using System.Windows;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForUpdateWindow.xaml
/// </summary>
public partial class GroupForUpdateWindow : Window
{
    public GroupForUpdateWindow()
    {
        InitializeComponent();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
