using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.Desktop.Integrated.Services.Payments.Registry;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for OutlayForPaymentWindow.xaml
/// </summary>
public partial class OutlayForPaymentWindow : Window
{
    private readonly IRegistryService _registryService;
    public OutlayForPaymentWindow()
    {
        InitializeComponent();
        this._registryService = new RegistryService();
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

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task SaveAsync()
    {
        try
        {
            RegistryForCreateDto dto = new RegistryForCreateDto();
            dto.Debit = 0;
            dto.IsConfirmed = true;

            if(!string.IsNullOrEmpty(creditTxt.Text))
            {
                if(double.TryParse(creditTxt.Text, out double credit) &&
                    credit > 0)
                    dto.Credit = credit;
                else
                {
                    notifierThis.ShowWarning("Iltimos, to'g'ri to'lov summasi kiriting!");
                    SaveBtn.IsEnabled = true;
                    creditTxt.Focus();
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("Summa kiritilishi shart!");
                SaveBtn.IsEnabled = true;
                creditTxt.Focus();
                return;
            }

            dto.Description = descriptionTxt.Text;

            if(paymentTypeComboBox.SelectedItem is ComboBoxItem selectedPaymentItem &&
                selectedPaymentItem.Tag is not null)
                dto.Type = selectedPaymentItem.Tag.ToString() switch
                {
                    "0" => PaymentType.Cash,
                    "1" => PaymentType.Card,
                    "2" => PaymentType.Transfer,
                    "3" => PaymentType.Credit,
                    "4" => PaymentType.Other,
                    _ => PaymentType.Cash
                };
            else
            {
                notifierThis.ShowWarning("Iltimos, to'lov turini tanlang!");
                SaveBtn.IsEnabled = true;
                paymentTypeComboBox.Focus();
                return;
            }

            var outlayRegistry = await Task.Run(async () => await _registryService.OutlayAsync(dto));

            if(outlayRegistry > 0)
            {
                this.Close();

                notifier.ShowSuccess("Chiqim muvaffaqiyatli saqlandi!");
            }
            else
            {
                notifierThis.ShowWarning("Saqlashda xatolik yuz berdi, qayta urining!");
                SaveBtn.IsEnabled = true;
                return;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi, iltimos qayta urining!");
            SaveBtn.IsEnabled = true;
        }
    }

    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        if(!SaveBtn.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos, kuting!");
            return;
        }

        SaveBtn.IsEnabled = false;

        try
        {
            await SaveAsync();
        }
        catch(Exception ex)
        {
            SaveBtn.IsEnabled = true;
        }
    }
}
