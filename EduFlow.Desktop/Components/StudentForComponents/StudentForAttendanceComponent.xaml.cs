using System.Windows.Controls;

namespace EduFlow.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForAttendanceComponent.xaml
/// </summary>
public partial class StudentForAttendanceComponent : UserControl
{
    private long Id { get; set; }
    public StudentForAttendanceComponent()
    {
        InitializeComponent();
    }

    public void setValues(long id, int number, string fullName)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbFullname.Text = fullName;
    }
}
