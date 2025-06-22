using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForComponent.xaml
/// </summary>
public partial class StudentForComponent : UserControl
{
    private long Id { get; set; }
    public StudentForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, string fullName, string phoneNumber, int paymentCount)
    {
        this.Id = id;
        tbFullName.Text = fullName;
        tbPhoneNumber.Text = phoneNumber;
        tbPayCount.Text = paymentCount.ToString();
    }

    public long GetId()
        => this.Id;
}
