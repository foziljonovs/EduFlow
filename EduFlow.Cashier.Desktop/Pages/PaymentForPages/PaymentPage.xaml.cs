using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Cashier.Desktop.Components.PaymentForComponents;
using EduFlow.Cashier.Desktop.Windows.PaymentForWindows;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Pages.PaymentForPages;

/// <summary>
/// Interaction logic for PaymentPage.xaml
/// </summary>
public partial class PaymentPage : Page
{
    private readonly IPaymentService _paymentService;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    public PaymentPage()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
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

    private async Task GetAllPayment()
    {
        try
        {
            paymentLoader.Visibility = Visibility.Visible;

            var payments = await Task.Run(async () => await _paymentService.GetAllAsync());

            ShowPayments(payments);
        }
        catch(Exception ex)
        {
            paymentLoader.Visibility = Visibility.Collapsed;
            emptyDataForPayment.Visibility = Visibility.Visible;
            notifier.ShowWarning("Kurslarni yuklashda xatolik yuz berdi!");
        }
    }

    private async Task GetAllTeacher()
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

            ShowTeachers(teachers);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("O'qituvchilarni yuklashda xatolik yuz berdi!");
        }
    }

    private async Task FilterAsync()
    {
        try
        {
            stPayments.Children.Clear();
            paymentLoader.Visibility = Visibility.Visible;

            PaymentForFilterDto dto = new PaymentForFilterDto();
            if (StartedDate.SelectedDate is not null)
                dto.StartedDate = StartedDate.SelectedDate.Value;

            if(FinishedDate.SelectedDate is not null)
                dto.FinishedDate = FinishedDate.SelectedDate.Value;

            if (courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem &&
                selectedCourseItem.Tag is not null)
                dto.CourseId = (long)selectedCourseItem.Tag;

            if (teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem &&
                selectedTeacherItem.Tag is not null)
                dto.TeacherId = (long)selectedTeacherItem.Tag;

            if (statusComboBox.SelectedItem is ComboBoxItem selectedStatusItem &&
                selectedStatusItem.Tag is not null)
                dto.Status = selectedStatusItem.Tag.ToString() switch
                {
                    "0" => PaymentStatus.Pending,
                    "1" => PaymentStatus.Completed,
                    "2" => PaymentStatus.Failed,
                    "3" => PaymentStatus.Refunded
                };

            if(typeComboBox.SelectedItem is ComboBoxItem selectedTypeItem &&
                selectedTypeItem.Tag is not null)
                dto.Type = selectedTypeItem.Tag.ToString() switch
                {
                    "0" => PaymentType.Cash,
                    "1" => PaymentType.Card,
                    "2" => PaymentType.Transfer,
                    "3" => PaymentType.Credit,
                    "4" => PaymentType.Other
                };

            var payments = await Task.Run(async () => await _paymentService.FilterAsync(dto));

            if(payments.Any())
                ShowPayments(payments);
            else
            {
                paymentLoader.Visibility = Visibility.Collapsed;
                emptyDataForPayment.Visibility = Visibility.Visible;
            }
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Ma'lumotlarni filterlashda xatolik yuz berdi!");
        }
    }

    private async Task GetAllCourse()
    {
        try
        {
            var courses = await Task.Run(async () => await _courseService.GetAllAsync());

            ShowCourses(courses);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Kurslarni yuklashda xatolik yuz berdi!");
        }
    }

    private void ShowTeachers(List<TeacherForResultDto> teachers)
    {
        if(teachers.Any())
        {
            teacherComboBox.Items.Clear();

            teacherComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach(var teacher in teachers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Tag = teacher.Id,
                    Content = $"{teacher.User?.Firstname} {teacher.User?.Lastname}"
                };

                teacherComboBox.Items.Add(item);
            }
        }
    }

    private void ShowCourses(List<CourseForResultDto> courses)
    {
        if (courses.Any())
        {
            courseComboBox.Items.Clear();

            courseComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach(var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Tag = course.Id,
                    Content = course.Name
                };

                courseComboBox.Items.Add(item);
            }
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

            foreach(var item in payments)
            {
                PaymentForComponent component = new PaymentForComponent();
                component.Tag = item.Id;
                component.SetValues(
                    count,
                    item.Id,
                    item.ReceiptNumber,
                    item.Amount,
                    $"{item.Teacher.User.Firstname} {item.Teacher.User.Lastname}",
                    item.Discount,
                    item.Status,
                    item.Type,
                    item.PaymentDate);

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

    private async Task LoadPage()
    {
        await GetAllCourse();
        await GetAllTeacher();
        await GetAllPayment();
    }

    private bool isPageLoaded = false;
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if(!isPageLoaded)
        {
            isPageLoaded = true;
            LoadPage();
        }
    }

    private async void paymentBtn_Click(object sender, RoutedEventArgs e)
    {
        IncomeForPaymentWindow window = new IncomeForPaymentWindow();
        window.ShowDialog();
        await LoadPage();
    }

    private async void FinishedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            await FilterAsync();
    }

    private async void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            await FilterAsync();
    }

    private async void teacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            await FilterAsync();
    }

    private async void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            await FilterAsync();
    }

    private async void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            await FilterAsync();
    }
}
