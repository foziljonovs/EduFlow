using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.GroupForComponents;

/// <summary>
/// Interaction logic for GroupForStudentViewComponent.xaml
/// </summary>
public partial class GroupForStudentViewComponent : UserControl
{
    private long Id { get; set; }
    public GroupForStudentViewComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, string name, Status status)
    {
        this.Id = id;
        tbName.Text = name;

        if (status == Status.Active)
            ActiveBorder.Visibility = System.Windows.Visibility.Visible;
        else if (status == Status.Archived)
            ArchivedBorder.Visibility = System.Windows.Visibility.Visible;
        else if(status == Status.Graduated)
            GratuatedBorder.Visibility = System.Windows.Visibility.Visible;
        else
            DeletedBorder.Visibility = System.Windows.Visibility.Visible;
    }

    public long GetId()
        => this.Id;
}
