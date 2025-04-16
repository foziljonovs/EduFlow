using System.Windows;

namespace EduFlow.Desktop.Windows.TeacherForWindows;

/// <summary>
/// Interaction logic for TeacherForCreateWindow.xaml
/// </summary>
public partial class TeacherForCreateWindow : Window
{
    public TeacherForCreateWindow()
    {
        InitializeComponent();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
