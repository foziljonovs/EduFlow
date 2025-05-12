using System.Windows.Controls;

namespace EduFlow.Desktop.Components.TeacherForComponents;

/// <summary>
/// Interaction logic for TeacherForMinComponent.xaml
/// </summary>
public partial class TeacherForMinComponent : UserControl
{
    public Func<Task> SelectedComponent { get; set; } = null!;
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

    private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (SelectedComponent != null)
            SelectedComponent.Invoke();
    }
}
