using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.GroupForComponents;

/// <summary>
/// Interaction logic for GroupForStudentViewWindowComponent.xaml
/// </summary>
public partial class GroupForStudentViewWindowComponent : UserControl
{
    private long Id { get; set; }
    public GroupForStudentViewWindowComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, string name, Status status)
    {
        this.Id = id;
        tbName.Text = name;
        tbStatus.Text = status.ToString();
    }

    public long GetId()
        => this.Id;
}
