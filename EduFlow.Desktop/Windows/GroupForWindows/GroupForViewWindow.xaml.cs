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
}
