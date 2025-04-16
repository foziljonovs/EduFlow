using System.Windows;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForCreateWindow.xaml
/// </summary>
public partial class GroupForCreateWindow : Window
{
    public GroupForCreateWindow()
    {
        InitializeComponent();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
