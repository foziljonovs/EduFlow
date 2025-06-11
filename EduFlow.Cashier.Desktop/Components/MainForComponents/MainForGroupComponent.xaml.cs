using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.MainForComponents;

/// <summary>
/// Interaction logic for MainForGroupComponent.xaml
/// </summary>
public partial class MainForGroupComponent : UserControl
{
    private long Id { get; set; }
    public MainForGroupComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string name, int studentsCount, /*int lessonsCount,*/ DateTime startedDate)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbStudentCount.Text = studentsCount.ToString();
        //tbLessonCount.Text = lessonsCount.ToString();
        tbStartedDate.Text = startedDate.ToString("dd/MM/yyyy");
    }

    public long GetId()
        => this.Id;
}
