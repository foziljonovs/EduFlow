using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Services.Companies.Company;
using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.SubscriptionForWindows;

/// <summary>
/// Interaction logic for EnableForSubscriptionWindow.xaml
/// </summary>
public partial class EnableForSubscriptionWindow : Window
{
    private readonly ICompanyService _companyService;
    private readonly ICompanySubscriptionService _companySubscriptionService;
    private long CompanyId { get; set; }
    private CompanyForResultDto Company { get; set; } = new CompanyForResultDto();
    public EnableForSubscriptionWindow()
    {
        InitializeComponent();
        this._companyService = new CompanyService();
        this._companySubscriptionService = new CompanySubscriptionService();
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
        => this.CompanyId = companyId;

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task GetCompany()
    {
        try
        {
            var company = await _companyService.GetByIdAsync(this.CompanyId);

            if (company is not null)
                this.Company = company;
            else
                notifierThis.ShowWarning("O'quv markazni topa olmadik, iltimos oynani qayta oching!");
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Xatolik yuz berdi, iltimos oynani qayta oching!");
        }
    }

    private async Task SavedAsync()
    {
        try
        {
            CompanySubscriptionForCreateDto dto = new CompanySubscriptionForCreateDto()
            {
                CompanyId = this.CompanyId,
                SubcriptionDate = DateTime.UtcNow.AddHours(5)
            };

            if (SubscriptionTypeComboBox.SelectedItem is ComboBoxItem selectedSubscriptionTypeItem)
                dto.Type = selectedSubscriptionTypeItem.Tag.ToString() switch
                {
                    "0" => SubscriptionType.Monthly,
                    "1" => SubscriptionType.OneTime
                };
            else
            {
                notifierThis.ShowWarning("Obuna turini tanlang");
                SaveButton.IsEnabled = true;
                return;
            }

            if (IsPaidCheckBox.IsChecked is true)
                dto.IsPaid = true;
            else
                dto.IsPaid = false;

            if (IsActiveCheckBox.IsChecked is true)
                dto.IsActive = true;
            else
                dto.IsActive = true;

            var result = await _companySubscriptionService.AddAsync(dto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess($"{Company.Name} uchun obuna yoqildi!");
            }
            else
            {
                notifierThis.ShowError("Obunani saqlashda xatolik yuz berdi, iltimos qayta uruning!");
                SaveButton.IsEnabled = true;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Xatolik yuz berdi, iltimos qayta urining!");
            SaveButton.IsEnabled = true;
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetCompany();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if(!SaveButton.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos kuting!");
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
