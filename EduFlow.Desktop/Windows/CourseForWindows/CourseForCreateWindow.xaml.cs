using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EduFlow.Desktop.Windows.CourseForWindows;
/// <summary>
/// Interaction logic for CourseForCreateWindow.xaml
/// </summary>
public partial class CourseForCreateWindow : Window
{
    public CourseForCreateWindow()
    {
        InitializeComponent();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void priceTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        e.Handled = !double.TryParse(futureText, System.Globalization.NumberStyles.Any, culture, out _);
    }

}
