using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows;
using EduFlow.Desktop.Windows.TeacherForWindows;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Components.TeacherForComponents;

/// <summary>
/// Interaction logic for TeacherForComponent.xaml
/// </summary>
public partial class TeacherForComponent : UserControl
{
    private long Id { get; set; }
    private readonly ITeacherService _service;
    public event Func<Task> OnDeleteTeacher;
    public TeacherForComponent()
    {
        InitializeComponent();
        this._service = new TeacherService();
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

    public void setValues(long id, int number, string firstName, string courseName, string phoneNumber, int groupCount, string[] skills)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbFirstname.Text = firstName;
        tbCourse.Text = courseName;
        tbPhoneNumber.Text = phoneNumber;
        tbGroupCount.Text = groupCount.ToString();
        tbSkills.Text = skills[0];
    }

    private void ViewBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        TeacherForViewWindow window = new TeacherForViewWindow();
        window.SetId(Id);
        window.ShowDialog();
    }

    private async void DeleteBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if(this.Id > 0)
        {
            var messageBox = new MessageBoxWindow($"{tbFirstname.Text} o'chirilsinmi?", MessageBoxWindow.MessageType.Confirmation, MessageBoxWindow.MessageButtons.OkCancel);
            var res = messageBox.ShowDialog();

            if(res is true)
            {
                var result = await _service.DeleteAsync(this.Id);

                if (result)
                {
                    notifier.ShowSuccess($"{tbFirstname.Text} o'chirildi");
                    await OnDeleteTeacher?.Invoke();
                }
                else
                    notifier.ShowError("Xatolik yuz berdi");
            }
        }
    }

    private void EditBtn_Click(object sender, RoutedEventArgs e)
    {
        TeacherForUpdateWindow window = new TeacherForUpdateWindow();
        window.setId(Id);
        window.ShowDialog();
    }
}
