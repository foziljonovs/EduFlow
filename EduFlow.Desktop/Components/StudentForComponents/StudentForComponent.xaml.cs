using EduFlow.Desktop.Windows.StudentForWindows;
using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForComponent.xaml
/// </summary>
public partial class StudentForComponent : UserControl
{
    private long Id { get; set; }
    public StudentForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string fullName, int age, string address, string phoneNumber, string group)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbFullname.Text = fullName;
        tbAge.Text = age.ToString();
        tbAddress.Text = address;
        tbPhoneNumber.Text = phoneNumber;
        tbCourse.Text = group;
    }

    private void ViewBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        StudentForViewWindow window = new StudentForViewWindow();
        window.SetId(Id);
        window.ShowDialog();
    }
}
