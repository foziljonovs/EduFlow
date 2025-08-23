using EduFlow.BLL.DTOs.Companies.Company;
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
/// Interaction logic for UpdateForCompanyWindow.xaml
/// </summary>
public partial class UpdateForCompanyWindow : Window
{
    private readonly ICompanyService _companyService;
    private readonly ILogoService _logoService;
    private long _companyId { get; set; }
    private long _logoId { get; set; }
    private CompanyForResultDto Company { get; set; } = new CompanyForResultDto();
    public UpdateForCompanyWindow()
    {
        InitializeComponent();
        this._companyService = new CompanyService();
        this._logoService = new LogoService();
    }

    Notifier notifier = new Notifier(cfg =>
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

    Notifier notifierThis = new Notifier(cfg =>
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

    public void SetCompanyId(long companyId)
        => this._companyId = companyId;

    private void EmptyData()
    {
        txtName.Clear();
        txtAbout.Clear();
        txtAddress.Clear();
        txtPhoneNumber.Text = "+998";
    }

    private async Task GetCompany()
    {
        try
        {
            var company = await _companyService.GetByIdAsync(this._companyId);

            if(company is null)
            {
                notifierThis.ShowWarning("O'quv markaz ma'lumotlari topilmadi, iltimos qayta urining!");
                EmptyData();
                return;
            }

            ShowValues(company);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("O'quv markaz ma'lumotlarini yuklab bo'lmadi, iltimos qayta urining!");
        }
    }

    private void ShowValues(CompanyForResultDto company)
    {
        this.Company = company;
        this._logoId = company.LogoId;

        txtName.Text = company.Name;
        txtAbout.Text = company.About;
        txtAddress.Text = company.Address;
        txtPhoneNumber.Text = company.PhoneNumber;
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

    private async void UploadLogoButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image files (*.bmp)|*.bmp";

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;

            try
            {
                var result = await _logoService.AddAsync(filePath);

                if (result > 0)
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
            catch (Exception ex)
            {
                notifierThis.ShowWarning("Logotipni yuklab bo'lmadi, iltimos qayta urinig");
                return;
            }
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetCompany();
    }
}
