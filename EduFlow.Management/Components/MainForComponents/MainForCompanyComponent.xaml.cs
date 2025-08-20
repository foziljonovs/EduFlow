using System.Windows.Controls;

namespace EduFlow.Management.Components.MainForComponents;

/// <summary>
/// Interaction logic for MainForCompanyComponent.xaml
/// </summary>
public partial class MainForCompanyComponent : UserControl
{
    private long Id { get; set; }

    public MainForCompanyComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, int number, string name, string phoneNumber, bool subscriptionStatus)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbPhoneNumber.Text = phoneNumber;

        if (subscriptionStatus)
        {
            activeBorder.Visibility = System.Windows.Visibility.Visible;
            noActiveBorder.Visibility = System.Windows.Visibility.Collapsed;
        }
        else
        {
            activeBorder.Visibility = System.Windows.Visibility.Collapsed;
            noActiveBorder.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
