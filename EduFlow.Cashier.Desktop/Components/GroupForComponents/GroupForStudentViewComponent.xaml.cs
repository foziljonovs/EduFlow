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
        tbStatus.Text = status switch
        {
            Status.Active => "Faol",
            Status.Archived => "Saqlangan",
            Status.Graduated => "Bitirgan",
            Status.Deleted => "Chiqarib yuborilgan"
        };
    }

    public long GetId()
        => this.Id;
}
