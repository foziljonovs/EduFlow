using EduFlow.Desktop.Windows.CourseForWindows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.CourseForComponents;

/// <summary>
/// Interaction logic for CourseForComponent.xaml
/// </summary>
public partial class CourseForComponent : UserControl
{
    private long Id;
    public CourseForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string name, int courseCount, int studentCount)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbCourseCount.Text = courseCount.ToString();
        tbStudentCount.Text = studentCount.ToString();
    }

    private void ViewButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        CourseForViewWindow window = new CourseForViewWindow();
        window.SetCourseId(this.Id);
        window.ShowDialog();
    }
}
