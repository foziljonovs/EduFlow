using EduFlow.Desktop.Windows.TeacherForWindows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.TeacherForComponents;

/// <summary>
/// Interaction logic for TeacherForComponent.xaml
/// </summary>
public partial class TeacherForComponent : UserControl
{
    private long Id { get; set; }
    public TeacherForComponent()
    {
        InitializeComponent();
    }

    public void setValues(long id, int number, string firstName, string courseName, string phoneNumber, int groupCount, string[] skills)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbFirstname.Text = firstName;
        tbCourse.Text = courseName;
        tbPhoneNumber.Text = phoneNumber;
        tbGroupCount.Text = groupCount.ToString();
        tbSkills.Text = skills[0];
    }

    private void ViewBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        TeacherForViewWindow window = new TeacherForViewWindow();
        window.SetId(Id);
        window.ShowDialog();
    }
}
