using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Windows;
using EduFlow.Desktop.Windows.StudentForWindows;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForComponent.xaml
/// </summary>
public partial class StudentForComponent : UserControl
{
    private long Id { get; set; }
    private readonly IStudentService _service;
    public event Func<Task> OnStudentDelete;
    public StudentForComponent()
    {
        InitializeComponent();
        this._service = new StudentService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 50,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    public void SetValues(int number, long id, string fullName, int age, string address, string phoneNumber, string group)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbFullname.Text = fullName;
        tbAge.Text = age.ToString();
        tbAddress.Text = address;
        tbPhoneNumber.Text = phoneNumber;
        tbCourse.Text = group;
        PrimaryView();
    }

    private void PrimaryView()
    {
        var role = IdentitySingelton.GetInstance().Role;

        if(role is Domain.Enums.UserRole.Teacher)
        {
            EditBtn.IsEnabled = false;
            EditBtn.ToolTip = "O'quvchi ma'lumotlarini o'zgartira olmaysiz!";

            DeleteBtn.IsEnabled = false;
            DeleteBtn.ToolTip = "O'quvchini o'chirib yubora olmaysiz!";
        }
    }

    private void ViewBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        StudentForViewWindow window = new StudentForViewWindow();
        window.SetId(Id);
        window.ShowDialog();
    }

    private async void DeleteBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if(this.Id > 0)
        {
            var messageBox = new MessageBoxWindow($"{tbFullname.Text} o'chirilsinmi?", MessageBoxWindow.MessageType.Confirmation, MessageBoxWindow.MessageButtons.OkCancel);
            var res = messageBox.ShowDialog();

            if(res is true)
            {
                var result = await _service.DeleteAsync(this.Id);

                if (result)
                {
                    notifier.ShowSuccess($"{tbFullname.Text} o'chirildi");
                    await OnStudentDelete?.Invoke();
                }
                else
                    notifier.ShowError("Xatolik yuz berdi");
            }
        }
    }

    private async void EditBtn_Click(object sender, RoutedEventArgs e)
    {
        StudentForUpdateWindow window = new StudentForUpdateWindow();
        window.setId(Id);
        window.ShowDialog();
        await OnStudentDelete?.Invoke();
    }
}
