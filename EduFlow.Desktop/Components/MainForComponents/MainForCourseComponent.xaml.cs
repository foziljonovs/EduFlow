using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Windows;
using EduFlow.Desktop.Windows.GroupForWindows;
using EduFlow.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Components.MainForComponents;

/// <summary>
/// Interaction logic for MainForCourseComponent.xaml
/// </summary>
public partial class MainForCourseComponent : UserControl
{
    public long Id { get; set; }
    public event Func<Task> OnGroupView;
    private readonly IGroupService _service;
    public MainForCourseComponent()
    {
        InitializeComponent();
        this._service = new GroupService();
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

    public void SetValues(int number, long id, string name, int studentCount, TimeSpan preferredTime, Day preferredDay)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbStudentCount.Text = studentCount.ToString();
        tbTime.Text = preferredTime.ToString(@"hh\:mm");
        tbDay.Text = preferredDay switch
        {
            Day.None => "Belgilanmagan",
            Day.ToqKunlar => "Toq kunlar",
            Day.JuftKunlar => "Juft kunlar",
            _ => "Belgilanmagan"
        };
    }

    private async void ViewButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        GroupForViewWindow window = new GroupForViewWindow();
        window.SetId(Id);
        window.ShowDialog();

        if(OnGroupView is not null)
            await OnGroupView.Invoke();
    }

    private async void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if(this.Id > 0)
        {
            var role = IdentitySingelton.GetInstance().Role;

            if(role is Domain.Enums.UserRole.Administrator || role is Domain.Enums.UserRole.Manager)
            {
                var messageBox = new MessageBoxWindow($"{tbName.Text} o'chirilsinmi?", MessageBoxWindow.MessageType.Confirmation, MessageBoxWindow.MessageButtons.OkCancel);
                var res = messageBox.ShowDialog();

                if (res is true)
                {
                    var result = await _service.DeleteAsync(this.Id);

                    if(result)
                    {
                        notifier.ShowSuccess($"{tbName.Text} o'chirildi");
                        await OnGroupView.Invoke();
                    }
                    else
                        notifier.ShowError("Xatolik yuz berdi");
                }
            }
            else
                notifier.ShowInformation("Sizda bu funksiyani ishlatish huquqi yo'q!");
        }
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        GroupForUpdateWindow window = new GroupForUpdateWindow();
        window.setId(this.Id);
        window.ShowDialog();
        await OnGroupView?.Invoke();
    }
}
