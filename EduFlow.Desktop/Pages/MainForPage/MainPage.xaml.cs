using EduFlow.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Security;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Pages.MainForPage;

/// <summary>
/// Interaction logic for MainPage.xaml
/// </summary>
public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
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

    private void GetAllCourse()
    {
        int count = 0;
        stCourses.Children.Clear();

        for(int i = count; i < 20; i++)
        {
            MainForCourseComponent component = new MainForCourseComponent();
            component.SetValues(count, long.Parse(count.ToString()), "Foundation", 10, "Abdulvosid");
            stCourses.Children.Add(component);
            count++;
        }
    }

    private void LoadPage()
    {
        var role = IdentitySingelton.GetInstance().Role;
        var fullName = IdentitySingelton.GetInstance().Name;

        if (role is Domain.Enums.UserRole.Teacher)
        {
            categoryComboBox.Visibility = Visibility.Collapsed;
            teacherComboBox.Visibility = Visibility.Collapsed;
            notifier.ShowInformation($"{fullName} xush kelibsiz ustoz!");
        }
        else if(role is Domain.Enums.UserRole.Administrator)
        {
            notifier.ShowInformation($"{fullName} xush kelibsiz!");
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        GetAllCourse();
        LoadPage();
    }
}
