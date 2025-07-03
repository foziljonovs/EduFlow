using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.GroupForComponents;

/// <summary>
/// Interaction logic for GroupForMinComponent.xaml
/// </summary>
public partial class GroupForMinComponent : UserControl
{
    private long Id { get; set; }
    
    public GroupForMinComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string name, int studentCount, Status status)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbStudentCount.Text = studentCount.ToString();
        tbStatus.Text = status.ToString();
    }

    public long GetId()
        => this.Id;
}
