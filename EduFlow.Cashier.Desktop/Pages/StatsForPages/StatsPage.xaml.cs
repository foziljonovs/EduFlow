using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.Desktop.Integrated.Services.Payments.Registry;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Pages.StatsForPages;

/// <summary>
/// Interaction logic for StatsPage.xaml
/// </summary>
public partial class StatsPage : Page
{
    private readonly IRegistryService _registryService;
    public StatsPage()
    {
        InitializeComponent();
        this._registryService = new RegistryService();
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

    private async Task GetAllRegistry()
    {
        try
        {
            statsLoader.Visibility = Visibility.Visible;

            var registries = await Task.Run(async () => await _registryService.GetAllAsync());

            ShowRegistries(registries);
        }
        catch(Exception ex)
        {
            statsLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Visible;
            notifier.ShowError("To'lov malumotlarini yuklashda xatolik yuz berdi!");
        }
    }

    private async Task FilterAsync()
    {
        try
        {
            ClearStats();
            statsLoader.Visibility = Visibility.Visible;

            RegistryForFilterDto dto = new RegistryForFilterDto();

            if (startedDate.SelectedDate is not null)
                dto.StartedDate = startedDate.SelectedDate.Value;

            if (finishadDate.SelectedDate is not null)
                dto.FinishedDate = finishadDate.SelectedDate.Value;

            var registries = await Task.Run(async () => await _registryService.FilterAsync(dto));

            ShowRegistries(registries);
        }
        catch (Exception ex)
        {
            statsLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Visible;
            notifier.ShowError("To'lov malumotlarini filtrlashda xatolik yuz berdi!");
        }
    }

    private void ClearStats()
    {
        tbCash.Text = "0";
        tbCard.Text = "0";
        tbTransfer.Text = "0";
        tbCredit.Text = "0";
        tbOther.Text = "0";
        tbAllAmount.Text = "0";
        tbOutlayCash.Text = "0";
        tbOutlayCard.Text = "0";
        tbOutlayTransfer.Text = "0";
        tbOutlayCredit.Text = "0";
        tbOutlayOther.Text = "0";
        tbOutlayAllAmount.Text = "0";
    }

    private void ShowRegistries(List<RegistryForResultDto> registries)
    {
        if (registries.Any())
        {
            statsLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Collapsed;

            ShowIncome(registries);
            ShowOutlay(registries);
        }
        else
        {
            statsLoader.Visibility = Visibility.Collapsed;
            st.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Visible;
        }
    }

    private void ShowIncome(List<RegistryForResultDto> registries)
    {
        var totalCashIncome = registries.Where(p => p.Type == Domain.Enums.PaymentType.Cash &&
            p.Debit > 0).Sum(p => p.Debit);

        if(totalCashIncome > 0)
            tbCash.Text = totalCashIncome.ToString();
        else
            tbCash.Text = "0";

        var totalCardIncome = registries.Where(p => p.Type == Domain.Enums.PaymentType.Card &&
            p.Debit > 0).Sum(p => p.Debit);

        if(totalCardIncome > 0)
            tbCard.Text = totalCardIncome.ToString();
        else
            tbCard.Text = "0";

        var totalTransferIncome = registries.Where(p => p.Type == Domain.Enums.PaymentType.Transfer &&
            p.Debit > 0).Sum(p => p.Debit);

        if(totalTransferIncome > 0)
            tbTransfer.Text = totalTransferIncome.ToString();
        else
            tbTransfer.Text = "0";

        var totalCreditIncome = registries.Where(p => p.Type == Domain.Enums.PaymentType.Credit 
            && p.Debit > 0).Sum(p => p.Debit);

        if(totalCreditIncome > 0)
            tbCredit.Text = totalCreditIncome.ToString();
        else
            tbCredit.Text = "0";

        var totalOtherIncome = registries.Where(p => p.Type == Domain.Enums.PaymentType.Other
            && p.Debit > 0).Sum(p => p.Debit);

        if(totalOtherIncome > 0)
            tbOther.Text = totalOtherIncome.ToString();
        else
            tbOther.Text = "0";

        var totalIncome = registries.Sum(p => p.Debit);

        if(totalIncome > 0)
            tbAllAmount.Text = totalIncome.ToString();
        else
            tbAllAmount.Text = "0";
    }

    private void ShowOutlay(List<RegistryForResultDto> registries)
    {
        var totalCashOutlay = registries.Where(p => p.Type == Domain.Enums.PaymentType.Cash &&
            p.Credit > 0).Sum(p => p.Credit);

        if(totalCashOutlay > 0)
            tbOutlayCash.Text = totalCashOutlay.ToString();
        else
            tbOutlayCash.Text = "0";

        var totalCardOutlay = registries.Where(p => p.Type == Domain.Enums.PaymentType.Card &&
            p.Credit > 0).Sum(p => p.Credit);

        if(totalCardOutlay > 0)
            tbOutlayCard.Text = totalCardOutlay.ToString();
        else
            tbOutlayCard.Text = "0";

        var totalTransferOutlay = registries.Where(p => p.Type == Domain.Enums.PaymentType.Transfer &&
            p.Credit > 0).Sum(p => p.Credit);

        if(totalTransferOutlay > 0)
            tbOutlayTransfer.Text = totalTransferOutlay.ToString();
        else
            tbOutlayTransfer.Text = "0";

        var totalCreditOutlay = registries.Where(p => p.Type == Domain.Enums.PaymentType.Credit 
            && p.Credit > 0).Sum(p => p.Credit);

        if(totalCreditOutlay > 0)
            tbOutlayCredit.Text = totalCreditOutlay.ToString();
        else
            tbOutlayCredit.Text = "0";

        var totalOtherOutlay = registries.Where(p => p.Type == Domain.Enums.PaymentType.Other
            && p.Credit > 0).Sum(p => p.Credit);

        if(totalOtherOutlay > 0)
            tbOutlayOther.Text = totalOtherOutlay.ToString();
        else
            tbOutlayOther.Text = "0";

        var totalAllAmount = registries.Sum(p => p.Credit);

        if(totalAllAmount > 0)
            tbOutlayAllAmount.Text = totalAllAmount.ToString();
        else
            tbOutlayAllAmount.Text = "0";
    }

    private bool IsPageLoaded = false;
    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (!IsPageLoaded)
        {
            await GetAllRegistry();
            IsPageLoaded = true;
        }
    }

    private async void finishadDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if(IsPageLoaded)
            await FilterAsync();
    }
}
