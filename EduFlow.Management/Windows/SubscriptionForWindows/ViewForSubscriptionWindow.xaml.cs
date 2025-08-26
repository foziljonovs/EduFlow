using System.Windows;

namespace EduFlow.Management.Windows.SubscriptionForWindows;

/// <summary>
/// Interaction logic for ViewForSubscriptionWindow.xaml
/// </summary>
public partial class ViewForSubscriptionWindow : Window
{
    public ViewForSubscriptionWindow()
    {
        InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
