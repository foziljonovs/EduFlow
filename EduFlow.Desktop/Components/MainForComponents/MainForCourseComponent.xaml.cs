using System.Windows.Controls;

namespace EduFlow.Desktop.Components.MainForComponents;

/// <summary>
/// Interaction logic for MainForCourseComponent.xaml
/// </summary>
public partial class MainForCourseComponent : UserControl
{
    public long Id { get; set; }
    public MainForCourseComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string name, int studentCount, string teacherName)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbStudentCount.Text = studentCount.ToString();
        tbTeacherName.Text = teacherName;
    }
}
