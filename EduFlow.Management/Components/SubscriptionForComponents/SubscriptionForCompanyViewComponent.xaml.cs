using System.Windows.Controls;

namespace EduFlow.Management.Components.SubscriptionForComponents;

/// <summary>
/// Interaction logic for SubscriptionForCompanyViewComponent.xaml
/// </summary>
public partial class SubscriptionForCompanyViewComponent : UserControl
{
    private long SubscriptionId { get; set; }
    public SubscriptionForCompanyViewComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, bool isActive, DateTime subscriptionDate)
    {
        this.SubscriptionId = id;
        tbSubscriptionDate.Text = subscriptionDate.ToString("dd-MM-yyyy");

        if (isActive)
            ActiveBorder.Visibility = System.Windows.Visibility.Visible;
        else
            NoActiveBorder.Visibility = System.Windows.Visibility.Visible;
    }
}
