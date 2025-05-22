using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Windows;
using EduFlow.Desktop.Windows.CourseForWindows;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using static EduFlow.Desktop.Windows.MessageBoxWindow;

namespace EduFlow.Desktop.Components.CourseForComponents;

/// <summary>
/// Interaction logic for CourseForComponent.xaml
/// </summary>
public partial class CourseForComponent : UserControl
{
    private long Id;
    private readonly ICourseService _service;
    public Func<Task> GetCourses { get; set; } = null!;
    public CourseForComponent()
    {
        InitializeComponent();
        this._service = new CourseService();
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

    public void SetValues(int number, long id, string name, int courseCount, int studentCount)
    {
        Id = id;
        tbNumber.Text = number.ToString();
        tbName.Text = name;
        tbCourseCount.Text = courseCount.ToString();
        tbStudentCount.Text = studentCount.ToString();
    }

    private void ViewButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        CourseForViewWindow window = new CourseForViewWindow();
        window.SetCourseId(this.Id);
        window.ShowDialog();
    }

    private async void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if(this.Id > 0)
        {
            var message = $"{tbName.Text} o'chirilsinmi?";
            var messageBox = new MessageBoxWindow(message, MessageBoxWindow.MessageType.Confirmation, MessageButtons.OkCancel); 
            
            var result = messageBox.ShowDialog();

            if(result is true)
            {
                var res = await _service.DeleteAsync(this.Id);

                if (res)
                {
                    notifier.ShowSuccess($"{tbName.Text} o'chirildi!");
                    await GetCourses();
                }
                else
                    notifier.ShowWarning($"O'chirishda xatolik yuz berdi!");
            }
        }
    }
}
