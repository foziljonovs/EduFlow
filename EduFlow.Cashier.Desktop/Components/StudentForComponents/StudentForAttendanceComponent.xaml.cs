using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Components.StudentForComponents;

/// <summary>
/// Interaction logic for StudentForAttendanceComponent.xaml
/// </summary>
public partial class StudentForAttendanceComponent : UserControl
{
    private readonly IGroupService _groupService;
    private long Id { get; set; }
    private int Number { get; set; }
    private EnrollmentStatus Status { get; set; }
    private long GroupId { get; set; }
    private Student student { get; set; }
    public StudentForAttendanceComponent()
    {
        InitializeComponent();
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

    public void setValues(long id, int number, string fullName, EnrollmentStatus status, long groupId)
    {
        this.Id = id;
        this.Number = number;
        tbNumber.Text = number.ToString();
        tbFullname.Text = fullName;
        this.Status = status;
        this.GroupId = groupId;

        //switch (status)
        //{
        //    case EnrollmentStatus.Active:
        //        mnActive.Visibility = System.Windows.Visibility.Visible;
        //        break;

        //    case EnrollmentStatus.Suspended:
        //        mnSaved.Visibility = System.Windows.Visibility.Visible;
        //        break;
        //}
    }

    public void setValues(int number, Student student, long groupId)
    {
        this.Id = student.Id;
        this.Number = number;
        tbNumber.Text = number.ToString();
        tbFullname.Text = student.Fullname;
        this.Status = EnrollmentStatus.Active;
        this.GroupId = groupId;
        this.student = student;
    }

    public (int, long) GetValues()
        => (this.Number, this.Id);
}
