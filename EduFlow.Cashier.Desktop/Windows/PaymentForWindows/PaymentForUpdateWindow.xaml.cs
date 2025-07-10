using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for PaymentForUpdateWindow.xaml
/// </summary>
public partial class PaymentForUpdateWindow : Window
{
    private readonly IPaymentService _paymentService;
    private readonly ITeacherService _teacherService;
    private long Id { get; set; }
    private PaymentForResultDto payment = new PaymentForResultDto();
    public PaymentForUpdateWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._teacherService = new TeacherService();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(this.WindowState == WindowState.Maximized)
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
    }

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if(this.WindowState == WindowState.Normal)
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            NormalButton.Visibility = Visibility.Visible;
            MaxButton.Visibility = Visibility.Collapsed;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async Task GetAllTeacher()
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

            ShowTeachers(teachers);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'qituvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async Task GetPayment()
    {
        try
        {
            var payment = await Task.Run(async () => await _paymentService.GetByIdAsync(this.Id));

            ShowPayment(payment);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Ma'lumotlarni yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private void ShowTeachers(List<TeacherForResultDto> teachers)
    {
        teacherComboBox.Items.Clear();

        if (teachers.Any())
        {
            foreach(var teacher in teachers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = $"{teacher.User.Firstname} {teacher.User.Lastname}",
                    Tag = teacher.Id,
                };

                teacherComboBox.Items.Add(item);
            }
        }
        else
        {
            notifierThis.ShowError("O'qituvchi malumotlari yuklab bo'lmadi, Iltimos qayta urining!");
        }
    }

    public void SetId(long id)
        => this.Id = id;

    private void ShowPayment(PaymentForResultDto payment)
    {
        if(payment is not null)
        {
            this.payment = payment;

            AmountTxt.Text = payment.Amount.ToString();
            DiscountTxt.Text = payment.Discount.ToString();
            NotesTxt.Text = payment.Notes?.ToString() ?? "...";
        }
        else
        {
            notifierThis.ShowError("To'lov malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private void ShowActivites()
    {
        if(payment is not null)
        {
            paymentTypeComboBox.SelectedItem = payment.Type switch
            {
                Domain.Enums.PaymentType.Cash => paymentTypeComboBox.Items[0],
                Domain.Enums.PaymentType.Card => paymentTypeComboBox.Items[1],
                Domain.Enums.PaymentType.Transfer => paymentTypeComboBox.Items[2],
                Domain.Enums.PaymentType.Credit => paymentTypeComboBox.Items[3],
                Domain.Enums.PaymentType.Other => paymentTypeComboBox.Items[4]
            };

            foreach (var item in teacherComboBox.Items)
            {
                if(item is ComboBoxItem comboboxItem && 
                    comboboxItem.Tag?.ToString() == payment.TeacherId.ToString())
                {
                    teacherComboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }

    private void teacherComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {

    }

    private async Task Load()
    {
        await GetAllTeacher();
        await GetPayment();

        ShowActivites();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Load();
    }
}
