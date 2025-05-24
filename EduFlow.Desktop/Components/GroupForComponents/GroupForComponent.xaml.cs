using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Windows;
using EduFlow.Desktop.Windows.GroupForWindows;
using EduFlow.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Components.GroupForComponents;

/// <summary>
/// Interaction logic for GroupForComponent.xaml
/// </summary>
public partial class GroupForComponent : UserControl
{
    private readonly IGroupService _groupService;
    public event Func<Task> OnGroupView;
    private long Id { get; set; }
    public GroupForComponent()
    {
        InitializeComponent();
        this._groupService = new GroupService();
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

    public void setValues(long id, int number, string name, string courseName, string teacherName, int studentCount, Status status, DateTime startedDate)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbCourse.Text = courseName;
        tbTeacher.Text = teacherName;
        tbStudentCount.Text = studentCount.ToString();
        string groupStatus = status switch
        {
            Status.Active => "Faol",
            Status.Archived => "Arxivlangan",
            Status.Graduated => "Yakunlangan",
        };

        tbStatus.Text = groupStatus;
        tbStartedDate.Text = startedDate.ToString("dd/MM/yyyy");
    }

    private async void ViewButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        GroupForViewWindow window = new GroupForViewWindow();
        window.SetId(Id);
        window.ShowDialog();
        await OnGroupView?.Invoke();
    }

    private async void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if(Id > 0)
        {
            var messageBox = new MessageBoxWindow($"{tbName.Text} o'chirilsinmi?", MessageBoxWindow.MessageType.Confirmation, MessageBoxWindow.MessageButtons.OkCancel);

            var res = messageBox.ShowDialog();

            if (res is true)
            {
                var result = await _groupService.DeleteAsync(this.Id);

                if(result)
                {
                    notifier.ShowInformation($"{tbName.Text} o'chirildi!");
                    await OnGroupView?.Invoke();
                }
                else
                    notifier.ShowError("Xatolik yuz berdi!");
            }
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        GroupForUpdateWindow window = new GroupForUpdateWindow();
        window.setId(Id);
        window.ShowDialog();
    }
}
