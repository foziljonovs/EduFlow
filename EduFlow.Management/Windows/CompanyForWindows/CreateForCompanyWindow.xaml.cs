using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Management.Windows.CompanyForWindows;

/// <summary>
/// Interaction logic for CreateForCompanyWindow.xaml
/// </summary>
public partial class CreateForCompanyWindow : Window
{
    public CreateForCompanyWindow()
    {
        InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void phoneNumberTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;

        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var regex = new System.Text.RegularExpressions.Regex(@"^\+998\d{0,9}$");
        e.Handled = !regex.IsMatch(futureText);
    }
}
