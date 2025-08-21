using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Management.Components.SubscriptionForComponents;

/// <summary>
/// Interaction logic for SubscriptionForComponent.xaml
/// </summary>
public partial class SubscriptionForComponent : UserControl
{
    private long Id { get; set; }
    public SubscriptionForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, int number, string company, SubscriptionType type, bool isPaid, DateTime subscriptionDate, bool isActived)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbCompanyName.Text = company;

        if(type == SubscriptionType.Monthly)
            TypeMonthlyBorder.Visibility = System.Windows.Visibility.Visible;
        else
            TypeYearBorder.Visibility = System.Windows.Visibility.Visible;

        if (isPaid)
            IsPaidBorder.Visibility = System.Windows.Visibility.Visible;
        else
            IsNoPaidBorder.Visibility = System.Windows.Visibility.Visible;

        tbSubscriptionDate.Text = subscriptionDate.ToString("dd/MM/yyyy");

        if(isActived)
            IsActiveBorder.Visibility = System.Windows.Visibility.Visible;
        else
            IsNoActiveBorder.Visibility = System.Windows.Visibility.Visible;
    }
}
