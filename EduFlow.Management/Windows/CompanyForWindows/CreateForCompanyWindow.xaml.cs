using EduFlow.Desktop.Integrated.Services.Companies.Company;
using EduFlow.Desktop.Integrated.Services.Companies.Logo;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.CompanyForWindows;

/// <summary>
/// Interaction logic for CreateForCompanyWindow.xaml
/// </summary>
public partial class CreateForCompanyWindow : Window
{
    private readonly ILogoService _logoService;
    private readonly ICompanyService _companyService;
    private long _logoId { get; set; } = 0;
    public CreateForCompanyWindow()
    {
        InitializeComponent();
        this._logoService = new LogoService();
        this._companyService = new CompanyService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 20,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;

        cfg.DisplayOptions.Width = 200;
        cfg.DisplayOptions.TopMost = true;
    });

    Notifier notifierThis = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
            corner: Corner.TopRight,
            offsetX: 20,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;

        cfg.DisplayOptions.Width = 200;
        cfg.DisplayOptions.TopMost = true;
    });

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task SavedAsync()
    {
        try
        {

        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Xatolik yuz berdi, iltimos qayta urining!");
        }
    }

    private void phoneNumberTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;

        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var regex = new System.Text.RegularExpressions.Regex(@"^\+998\d{0,9}$");
        e.Handled = !regex.IsMatch(futureText);
    }

    private async void UploadLogoButton_Click(object sender, RoutedEventArgs e)
    { 
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image files (*.bmp)|*.bmp";

        if(openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;

            try
            {
                var result = await _logoService.AddAsync(filePath);

                if(result > 0)
                {
                    _logoId = result;
                    notifierThis.ShowSuccess("Logotip yuklandi!");
                }
                else
                {
                    notifierThis.ShowWarning("Logotipni yuklab bo'lmadi!");
                    SaveButton.IsEnabled = true;
                }
            }
            catch(Exception ex)
            {
                notifierThis.ShowWarning("Logotipni yuklab bo'lmadi, iltimos qayta urinig");
                return;
            }
        }
    }
}
