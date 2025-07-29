using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Components.PaymentForComponents;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.PaynentForWindows;

/// <summary>
/// Interaction logic for PaymentForViewWindow.xaml
/// </summary>
public partial class PaymentForViewWindow : Window
{
    private readonly IPaymentService _paymentService;
    private long _teacherId { get; set; }
    private int pageSize = 10;
    private int pageNumber = 1;
    private bool hasNext = false;
    private bool hasPrevious = false;
    private bool isFiltered = false;
    private PaymentForFilterDto? isFilterDto = new PaymentForFilterDto();
    public PaymentForViewWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
    }

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

    private void Pagination(PagedResponse<PaymentForResultDto> pagedResponse)
    {
        this.pageNumber = pagedResponse.CurrentPage;
        this.pageSize = (pagedResponse.PageSize > 0 ? pagedResponse.PageSize : 10);
        this.hasNext = pagedResponse.HasNext;
        this.hasPrevious = pagedResponse.HasPrevious;

        btnPrevious.Visibility = pagedResponse.HasPrevious switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed
        };

        btnNext.Visibility = pagedResponse.HasNext switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed
        };

        tbCurrentPageNumber.Text = pagedResponse.CurrentPage.ToString();
        btnPrevious.IsChecked = false;
        btnNext.IsChecked = false;
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(NormalButton.Visibility == Visibility.Visible)
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if(NormalButton.Visibility == Visibility.Visible)
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility= Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async Task GetAllPaginationPayment()
    {
        try
        {
            paymentLoader.Visibility = Visibility.Visible;

            var payments = await Task.Run(async () => await _paymentService.GetAllPaginationByTeacherIdAsync(this._teacherId, pageSize, pageNumber));

            ShowPayments(payments.Data);
            Pagination(payments);
        }
        catch(Exception ex)
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
        }
    }

    private async Task GetAllPayment()
    {
        try
        {
            var payments = await Task.Run(async () => await _paymentService.GetAllByTeacherIdAsync(this._teacherId));

            PaymentAmount(payments);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Bu oydagi tushumlarni olishda xatolik yuz berdi, Iltimos qayta yuklang!");
        }
    }

    private async Task FilterAsync()
    {
        try
        {
            stPayments.Children.Clear();
            paymentLoader.Visibility = Visibility.Visible;

            PaymentForFilterDto dto = new PaymentForFilterDto
            {
                TeacherId = this._teacherId
            };

            if (dtStartDate.SelectedDate is not null)
                dto.StartedDate = dtStartDate.SelectedDate.Value;

            if(dtEndDate.SelectedDate is not null)
                dto.FinishedDate = dtEndDate.SelectedDate.Value;

            this.pageNumber = 1;
            this.isFiltered = true;
            this.isFilterDto = dto;

            var payments = await Task.Run(async () => await _paymentService.FilterAsync(dto, pageSize, pageNumber));

            ShowPayments(payments.Data);
            Pagination(payments);
        }
        catch(Exception ex)
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
        }
    }

    private void ShowPayments(List<PaymentForResultDto> payments)
    {
        int count = 1;
        stPayments.Children.Clear();

        if (payments.Any())
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Collapsed;

            foreach(var payment in payments)
            {
                PaymentForComponent component = new PaymentForComponent();
                component.SetValues(
                    count,
                    payment.Id,
                    payment.Student.Fullname,
                    payment.Amount,
                    payment.Discount,
                    payment.Status,
                    payment.Type,
                    payment.PaymentDate);

                stPayments.Children.Add(component);
                count++;
            }
        }
        else
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
        }
    }

    private void PaymentAmount(List<PaymentForResultDto> payments)
    {
        if (payments.Any())
        {
            DateTime now = DateTime.UtcNow.AddHours(5);

            double allAmount = payments
                .Where(x => x.PaymentDate.Year == now.Year &&
                            x.PaymentDate.Month == now.Month &&
                           (x.Status == Domain.Enums.PaymentStatus.Pending ||
                            x.Status == Domain.Enums.PaymentStatus.Completed))
                .Sum(x => x.Amount);

            tbAllPaymentAmount.Text = allAmount.ToString();
        }
        else
            tbAllPaymentAmount.Text = "0";
    }

    public void SetId(long id)
        => this._teacherId = id;

    private bool IsWindowLoaded = false;
    private async Task LoadedWindow()
    {
        if (!IsWindowLoaded)
        {
            IsWindowLoaded = true;
            await GetAllPaginationPayment();
            await GetAllPayment();
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadedWindow();
    }

    private async void dtEndDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (IsWindowLoaded)
            await FilterAsync();
    }

    private async void btnPrevious_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber -= 1;

        if (this.isFiltered && this.isFilterDto is not null)
        {
            var payments = await Task.Run(async () => await _paymentService.FilterAsync(isFilterDto, pageSize, pageNumber));

            ShowPayments(payments.Data);
            Pagination(payments);
        }
        else
            await GetAllPaginationPayment();
    }

    private async void btnNext_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber += 1;

        if (this.isFiltered && this.isFilterDto is not null)
        {
            var payments = await Task.Run(async () => await _paymentService.FilterAsync(isFilterDto, pageSize, pageNumber));

            ShowPayments(payments.Data);
            Pagination(payments);
        }
        else
            await GetAllPaginationPayment();
    }
}
