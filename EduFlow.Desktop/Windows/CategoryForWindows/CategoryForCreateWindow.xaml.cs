using System.Windows;

namespace EduFlow.Desktop.Windows.CategoryForWindows;

/// <summary>
/// Interaction logic for CategoryForCreateWindow.xaml
/// </summary>
public partial class CategoryForCreateWindow : Window
{
    public CategoryForCreateWindow()
    {
        InitializeComponent();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
