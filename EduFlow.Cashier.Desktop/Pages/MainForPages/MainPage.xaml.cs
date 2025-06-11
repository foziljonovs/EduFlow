using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.Cashier.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Pages.MainForPages;

/// <summary>
/// Interaction logic for MainPage.xaml
/// </summary>
public partial class MainPage : Page
{
    private readonly IGroupService _groupService;
    public MainPage()
    {
        InitializeComponent();
        this._groupService = new GroupService();
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

    private async Task GetAllGroup()
    {
        groupLoader.Visibility = Visibility.Visible;

        var groups = await Task.Run(async () => await _groupService.GetAllAsync());

        ShowGroup(groups);
    }


    private void ShowGroup(List<GroupForResultDto> groups)
    {
        stGroups.Children.Clear();
        int count = 1;

        if (groups.Any())
        {
            emptyDataForGroup.Visibility = Visibility.Collapsed;
            groupLoader.Visibility = Visibility.Collapsed;

            foreach (var item in groups)
            {
                MainForGroupComponent component = new MainForGroupComponent();
                component.SetValues(
                    count,
                    item.Id,
                    item.Name,
                    item.Students?.Count ?? 0,
                    item.Lessons?.Count ?? 0,
                    item.CreatedAt);

                stGroups.Children.Add(component);
                count++;
            }
        }
        else
        {
            emptyDataForGroup.Visibility = Visibility.Visible;
            groupLoader.Visibility = Visibility.Collapsed;
        }
    }

    private async Task LoadPage()
    {
        await GetAllGroup();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPage();
    }
}
