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

        if(status is Status.Active)
            ActiveBorder.Visibility = System.Windows.Visibility.Visible;
        else if(status is Status.Archived)
            ArchivedBorder.Visibility = System.Windows.Visibility.Visible;
        else if(status is Status.Graduated)
            GratuatedBorder.Visibility = System.Windows.Visibility.Visible;
        else
            DeletedBorder.Visibility = System.Windows.Visibility.Visible;
    }

    public long GetId()
        => this.Id;
}
