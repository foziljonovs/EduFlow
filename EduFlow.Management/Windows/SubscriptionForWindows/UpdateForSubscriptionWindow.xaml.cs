using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.SubscriptionForWindows;

/// <summary>
/// Interaction logic for UpdateForSubscriptionWindow.xaml
/// </summary>
public partial class UpdateForSubscriptionWindow : Window
{
    private readonly ICompanySubscriptionService _subscriptionService;
    private long Id { get; set; }
    private CompanySubscriptionForResultDto CompanySubscription {  get; set; } = new CompanySubscriptionForResultDto();
    public UpdateForSubscriptionWindow()
    {
        InitializeComponent();
        this._subscriptionService = new CompanySubscriptionService();
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

    public void SetId(long id)
        => this.Id = id;

    private async Task GetSubscription()
    {
        try
        {
            var subscription = await _subscriptionService.GetByIdAsync(this.Id);

            if(subscription is null)
            {
                notifierThis.ShowWarning("Obuna malumotlarni yuklashda muammo yuz berdi, iltimos oynani qayta yuklang!");
                return;
            }

            ShowValues(subscription);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Obuna malumotlari topilmadi, iltimos qayta urining!");
        }
    }

    private void ShowValues(CompanySubscriptionForResultDto subscription)
    {
        this.CompanySubscription = subscription;

        int type = (int)subscription.Type;

        foreach (ComboBoxItem item in SubscriptionTypeComboBox.Items)
            if(int.TryParse(item.Tag?.ToString(), out int tagValue) && tagValue == type)
            {
                SubscriptionTypeComboBox.SelectedItem = item;
                break;
            }

        if(subscription.SubcriptionDate is not null)
            SubscriptionDatePicker.SelectedDate = subscription.SubcriptionDate;

        IsPaidCheckBox.IsChecked = subscription.IsPaid;
        IsActiveCheckBox.IsChecked = subscription.IsActive;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetSubscription();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task SavedAsync()
    {
        try
        {
            CompanySubscriptionForUpdateDto dto = new CompanySubscriptionForUpdateDto
            {
                CompanyId = this.CompanySubscription.CompanyId
            };

            if(SubscriptionTypeComboBox.SelectedItem is ComboBoxItem item &&
                item.Tag is not null)
                dto.Type = item.Tag.ToString() switch
                {
                    "1" => SubscriptionType.OneTime,
                    "2" => SubscriptionType.Monthly
                };
            else
            {
                notifierThis.ShowWarning("Obuna tanlanmagan, iltimos tanlang!");
                SaveButton.IsEnabled = true;
                return;
            }

            if (SubscriptionDatePicker.SelectedDate is not null)
                dto.SubcriptionDate = SubscriptionDatePicker.SelectedDate.Value;
            else
            {
                notifierThis.ShowWarning("Obuna sanasi tanlanmagan, iltimos tanlang!");
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
                dto.IsActive = false;

            var result = await _subscriptionService.UpdateAsync(this.Id, dto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess($"Obuna muvaffaqiyatli o'zgartirildi!");
            }
            else
            {
                notifierThis.ShowWarning("Obunani yangilashda muammo yuz berdi, iltimos qayta urining!");
                SaveButton.IsEnabled = true;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Malumotlarni yangilashda xatolik yuz berdi, iltimos qayta urining!");
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
