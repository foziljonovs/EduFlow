using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForSecondComponent.xaml
/// </summary>
public partial class StudentForSecondComponent : UserControl
{
    private long Id { get; set; }
    public StudentForSecondComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, int number, string fullName, int age, string address, string phoneNumber, string courseName)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbFullName.Text = fullName;
        tbAge.Text = age.ToString();
        tbAddress.Text = address.ToString();
        tbPhoneNumber.Text = phoneNumber.ToString();
        tbCourse.Text = courseName;
    }
}