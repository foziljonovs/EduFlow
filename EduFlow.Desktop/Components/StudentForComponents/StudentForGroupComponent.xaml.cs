using System.Windows.Controls;

namespace EduFlow.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForGroupComponent.xaml
/// </summary>
public partial class StudentForGroupComponent : UserControl
{
    private long Id { get; set; }
    private bool IsChosen { get; set; } = false;
    public event Action<StudentForGroupComponent, bool> SelectionChanged;
    public StudentForGroupComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, long id, string fullName, string courseName, int age, string address, string phoneNumber)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbFullname.Text = fullName;
        tbCourse.Text = courseName;
        tbAge.Text = age.ToString();
        tbAddress.Text = address;
        tbPhoneNumber.Text = phoneNumber;
    }

    public bool GetIsChosen()
        => this.IsChosen;

    public long GetId()
        => this.Id;

    private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (IsChosen)
        {
            ChooseStudent.IsChecked = false;
            IsChosen = false;
        }
        else
        {
            ChooseStudent.IsChecked = true;
            IsChosen = true;
        }
    }

    private void ChooseStudent_Checked(object sender, System.Windows.RoutedEventArgs e)
    {
        IsChosen = true;
        SelectionChanged?.Invoke(this, IsChosen);
    }

    private void ChooseStudent_Unchecked(object sender, System.Windows.RoutedEventArgs e)
    {
        IsChosen = false;
        SelectionChanged?.Invoke(this, IsChosen);
    }
}
