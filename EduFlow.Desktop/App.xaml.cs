using EduFlow.SharedConfig;
using System.Windows;

namespace EduFlow.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var baseUrl = ConfigurationManager.GetValue("ApiConfig:BaseUrl");

        base.OnStartup(e);
    }
}
