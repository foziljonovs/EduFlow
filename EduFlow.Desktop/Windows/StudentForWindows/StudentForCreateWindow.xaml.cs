using EduFlow.Desktop.Integrated.Services.Users.Student;
using System.Windows;

namespace EduFlow.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForCreateWindow.xaml
/// </summary>
public partial class StudentForCreateWindow : Window
{
    private readonly IStudentService _service;
    public StudentForCreateWindow()
    {
        InitializeComponent();
        this._service = new StudentService();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
