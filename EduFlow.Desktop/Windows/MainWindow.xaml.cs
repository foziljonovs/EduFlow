using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton clickedButton)
            clickedButton.IsChecked = true;
    }

    private void CoursesButton_Click(object sender, RoutedEventArgs e)
    {
        if(sender is RadioButton clickedButton)
            clickedButton.IsChecked = true;
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;
    
    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        NormalButton.IsChecked = true;
    }
}