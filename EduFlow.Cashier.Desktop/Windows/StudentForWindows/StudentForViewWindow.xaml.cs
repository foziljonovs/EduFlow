using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Cashier.Desktop.Components.GroupForComponents;
using EduFlow.Cashier.Desktop.Components.PaymentForComponents;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForViewWindow.xaml
/// </summary>
public partial class StudentForViewWindow : Window
{
    private readonly IStudentService _studentService;
    private long Id { get; set; }
    public StudentForViewWindow()
    {
        InitializeComponent();
        this._studentService = new StudentService();
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

    public void SetId(long id)
        => this.Id = id;

    private async Task GetStudent()
    {
        try
        {
            var student = await _studentService.GetByIdAsync(this.Id);

            ShowValues(student);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'quvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private void Empty()
    {
        tbFullName.Text = string.Empty;
        tbPhoneNumber.Text = "+998000000000";
        tbAddress.Text = string.Empty;
        tbAge.Text = "0";
    }

    private void ShowValues(StudentForResultDto student)
    {
        stGroups.Children.Clear();
        stPayments.Children.Clear();

        groupForLoader.Visibility = Visibility.Visible;
        paymentForLoader.Visibility= Visibility.Visible;

        if(student is not null)
        {
            tbFullName.Text = student.Fullname;
            tbPhoneNumber.Text = student.PhoneNumber;
            tbAddress.Text = student.Address;
            tbAge.Text = student.Age.ToString();


            if (student.Groups.Any())
            {
                groupForLoader.Visibility = Visibility.Collapsed;
                groupForEmptyDate.Visibility = Visibility.Collapsed;

                foreach(var group in student.Groups)
                {
                    GroupForStudentViewComponent component = new GroupForStudentViewComponent();
                    component.SetValues(
                        group.Id,
                        group.Name,
                        group.IsStatus);

                    stGroups.Children.Add(component);
                }
            }
            else
            {
                groupForLoader.Visibility = Visibility.Collapsed;
                groupForEmptyDate.Visibility = Visibility.Visible;
            }

            if (student.Payments.Any())
            {
                paymentForLoader.Visibility = Visibility.Collapsed;
                paymentForEmptyDate.Visibility = Visibility.Collapsed;

                foreach(var payment in student.Payments)
                {
                    PaymentForStudentViewComponent component = new PaymentForStudentViewComponent();
                    component.SetValues(
                        payment.Id,
                        payment.Amount,
                        payment.PaymentDate);

                    stPayments.Children.Add(component);
                }
            }
            else
            {
                paymentForLoader.Visibility = Visibility.Collapsed;
                paymentForEmptyDate.Visibility = Visibility.Collapsed;
            }
        }
        else
        {
            notifierThis.ShowWarning("O'quvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            Empty();
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetStudent();
    }
}
