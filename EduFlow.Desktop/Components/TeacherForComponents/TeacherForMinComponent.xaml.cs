using System.Windows.Controls;

namespace EduFlow.Desktop.Components.TeacherForComponents;

/// <summary>
/// Interaction logic for TeacherForMinComponent.xaml
/// </summary>
public partial class TeacherForMinComponent : UserControl
{
    private long Id { get; set; }
    public TeacherForMinComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, string fullName)
    {
        this.Id = id;
        tbName.Text = fullName;
    }

    public long GetId() 
        => this.Id;
}
