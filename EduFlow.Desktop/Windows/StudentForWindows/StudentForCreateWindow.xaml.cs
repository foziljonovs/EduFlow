using System.Windows;

namespace EduFlow.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForCreateWindow.xaml
/// </summary>
public partial class StudentForCreateWindow : Window
{
    public StudentForCreateWindow()
    {
        InitializeComponent();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
