using System.Windows.Controls;

namespace EduFlow.Management.Components.UserForComponents;

/// <summary>
/// Interaction logic for CompanyForUserComponent.xaml
/// </summary>
public partial class CompanyForUserComponent : UserControl
{
    private long UserId { get; set; }
    public CompanyForUserComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, string fullName, string phoneNumber)
    {
        this.UserId = id;
        tbName.Text = fullName;
        tbPhoneNumber.Text = phoneNumber;
    }
}
