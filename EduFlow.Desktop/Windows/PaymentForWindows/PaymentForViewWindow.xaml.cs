using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.PaymentForComponents;
using EduFlow.Desktop.Helpers;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using QuestPDF.Fluent;
using System;
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
    private readonly ITeacherService _teacherService;
    private long _teacherId { get; set; }
    private TeacherForResultDto Teacher { get; set; }
    private int pageSize = 10;
    private int pageNumber = 1;
    private bool hasNext = false;
    private bool hasPrevious = false;
    private bool isFiltered = false;
    private PaymentForFilterDto? isFilterDto = new PaymentForFilterDto();
    private long CompanyId { get; set; }
    public PaymentForViewWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._teacherService = new TeacherService();
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

    private async Task GetTeacher()
    {
        try
        {
            var teacher = await _teacherService.GetByIdAsync(this._teacherId);

            if (teacher is null)
                notifierThis.ShowWarning("O'qituvchi ma'lumotlari topilmadi, iltimos qayta urining!");
            else
                this.Teacher = teacher;
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Xatolik yuz berdi, iltimos qayta urinig!");
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
                TeacherId = this._teacherId,
                CompanyId = this.CompanyId
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
            tbAllPaymentAmount.Text = "0";
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
        }
    }

    private void ShowPayments(List<PaymentForResultDto> payments)
    {
        double totalAmount = 0;
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
                totalAmount += payment.Amount;
                count++;
            }

            tbAllPaymentAmount.Text = totalAmount.ToString();
        }
        else
        {
            tbAllPaymentAmount.Text = "0";
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
        }
    }

    public void SetId(long id)
        => this._teacherId = id;

    private bool IsWindowLoaded = false;
    private async Task LoadedWindow()
    {
        if (!IsWindowLoaded)
        {
            IsWindowLoaded = true;
            this.CompanyId = IdentitySingelton.GetInstance().CompanyId;

            await GetAllPaginationPayment();
            await GetTeacher();
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

    private string SanitizeFileName(string fileName)
    {
        foreach (char invalidChar in System.IO.Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidChar, '_');
        }
        return fileName;
    }

    private async Task<(List<PaymentForResultDto>, DateTime, DateTime)> GetAllByTeacherIdAsync()
    {
        try
        {
            var payments = await Task.Run(async () => await _paymentService.GetAllByTeacherIdAsync(this._teacherId));

            DateTime startedDate;
            DateTime finishedDate;

            if (isFilterDto is not null && isFilterDto.StartedDate.HasValue && isFilterDto.FinishedDate.HasValue)
            {
                startedDate = isFilterDto.StartedDate.Value;
                finishedDate = isFilterDto.FinishedDate.Value;
            }
            else
            {
                startedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                finishedDate = DateTime.UtcNow.AddHours(5);
            }

            if (payments.Any())
            {
                var filteredPayments = payments
                    .Where(x => x.PaymentDate >= startedDate
                        && x.PaymentDate <= finishedDate)
                    .ToList();

                return (filteredPayments, startedDate, finishedDate);
            }
            else
                return (new List<PaymentForResultDto>(), DateTime.Now, DateTime.Now);
        }
        catch(Exception ex)
        {
            return (new List<PaymentForResultDto>(), DateTime.Now, DateTime.Now);
        }
    }

    private async void PaymentForUploadPDF_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        try
        {
            (List<PaymentForResultDto> payments, DateTime startedDate, DateTime finishedDate) = await GetAllByTeacherIdAsync();

            if (!payments.Any())
            {
                notifierThis.ShowWarning("To'lovlar topilmadi, iltimos qayta urining!");
                return;
            }

            string title = SanitizeFileName($"{Teacher.User.Firstname} {Teacher.User.Lastname}" +
                $" {startedDate.ToString("yyyy:MM:dd")} dan {finishedDate.ToString("yyyy:MM:dd")} gacha tushumlari");

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = System.IO.Path.Combine(path, $"{title}.pdf");

            var document = new PDFGenerator(
                Teacher,
                payments,
                startedDate,
                finishedDate);

            document.GeneratePdf(filePath);
            notifierThis.ShowSuccess($"{title} yuklandi!");
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Faylni yuklab bo'lmadi, iltimos qayta urining!");
        }
    }
}
