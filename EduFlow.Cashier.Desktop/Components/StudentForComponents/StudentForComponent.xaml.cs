using System.Windows.Controls;
using System.Windows.Media;

namespace EduFlow.Cashier.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForComponent.xaml
/// </summary>
public partial class StudentForComponent : UserControl
{
    private long Id { get; set; }
    public Func<Task> isClicked { get; set; }
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

    public void SelectedState(bool isSelected)
    {
        if (isSelected)
            mainBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B6B6B6"));
        else
            mainBorder.Background = Brushes.White;
    }
    public long GetId()
        => this.Id;

    private void mainBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        => isClicked?.Invoke();
}
