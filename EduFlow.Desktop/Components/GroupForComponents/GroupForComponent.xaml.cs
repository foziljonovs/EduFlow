using EduFlow.Desktop.Windows.GroupForWindows;
using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.GroupForComponents;

/// <summary>
/// Interaction logic for GroupForComponent.xaml
/// </summary>
public partial class GroupForComponent : UserControl
{
    private long Id { get; set; }
    public GroupForComponent()
    {
        InitializeComponent();
    }

    public void setValues(long id, int number, string name, string courseName, string teacherName, int studentCount, Status status, DateTime startedDate)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbCourse.Text = courseName;
        tbTeacher.Text = teacherName;
        tbStudentCount.Text = studentCount.ToString();
        string groupStatus = status switch
        {
            Status.Active => "Faol",
            Status.Archived => "Arxivlangan",
            Status.Graduated => "Yakunlangan",
        };

        tbStatus.Text = groupStatus;
        tbStartedDate.Text = startedDate.ToString("dd/MM/yyyy");
    }

    private void ViewButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        GroupForViewWindow window = new GroupForViewWindow();
        window.ShowDialog();
    }
}
