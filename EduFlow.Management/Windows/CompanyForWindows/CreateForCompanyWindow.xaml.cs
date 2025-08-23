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

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task SavedAsync()
    {
        try
        {
            CompanyForCreateDto dto = new CompanyForCreateDto();

            if (!string.IsNullOrEmpty(txtName.Text))
                dto.Name = txtName.Text;
            else
            {
                notifierThis.ShowWarning("O'quv markaz nomi kiritilmagan!");
                SaveButton.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(txtAddress.Text))
                dto.Address = txtAddress.Text;
            else
            {
                notifierThis.ShowWarning("Manzil kiritilmagan!");
                SaveButton.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(txtPhoneNumber.Text) && txtPhoneNumber.Text.Length == 13)
                dto.PhoneNumber = txtPhoneNumber.Text;
            else
            {
                notifierThis.ShowWarning("Telefon raqam noto'g'ri kiritilgan!");
                SaveButton.IsEnabled = true;
                return;
            }

            dto.About = txtAbout.Text;

            if (this._logoId > 0)
                dto.LogoId = this._logoId;
            else
            {
                notifierThis.ShowWarning("Logotipni yuklanmagan yoki yuklanmoqda!");
                SaveButton.IsEnabled = true;
                return;
            }

            var result = await _companyService.AddAsync(dto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess($"{dto.Name} - muvaffaqiyatli saqlandi!");
            }
            else
            {
                notifierThis.ShowWarning("Malumotlarni saqlashda muammo yuz berdi, iltimos qayta urining!");
                SaveButton.IsEnabled = true;
            }
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

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (!SaveButton.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos, kuting!");
            return;
        }

        SaveButton.IsEnabled = false;

        try
        {
            await SavedAsync();
        }
        finally
        {
            SaveButton.IsEnabled = true;
        }
    }
}
