using EduFlow.Domain.Enums;
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

    public void SetValues(long id, int number, string fullName, string phoneNumber, UserRole role)
    {
        this.UserId = id;
        tbNumber.Text = number.ToString();
        tbName.Text = fullName;
        tbPhoneNumber.Text = phoneNumber;

        if(role == UserRole.Manager)
            AdminBorder.Visibility = System.Windows.Visibility.Visible;
        else if(role == UserRole.Administrator)
            AdministratorBorder.Visibility = System.Windows.Visibility.Visible;
        else
            CashierBorder.Visibility = System.Windows.Visibility.Visible;
    }
}
