using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.GroupForComponents;

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

    public void SetValues(long id, string name, int lessonCount, DateTime CreatedDate)
    {
        this.Id = id;
        tbName.Text = name;
        tbLessonCount.Text = lessonCount.ToString();
        tbCreatedDate.Text = CreatedDate.ToString("dd/MM/yyyy");
    }

    public long GetId()
        => this.Id;
}
