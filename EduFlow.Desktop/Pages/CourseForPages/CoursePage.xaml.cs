using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.Desktop.Components.CourseForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Pages.CourseForPages;

/// <summary>
/// Interaction logic for CoursePage.xaml
/// </summary>
public partial class CoursePage : Page
{
    private readonly ICategoryService _categoryService;
    public CoursePage()
    {
        InitializeComponent();
        this._categoryService = new CategoryService();
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

    private async Task GetAllCategory()
    {
        courseLoader.Visibility = Visibility.Visible;
        var categories = await Task.Run(async () => await _categoryService.GetAllAsync());

        ShowCategories(categories);
    }

    private void ShowCategories(List<CategoryForResultDto> categories)
    {
        int count = 1;
        stCategories.Children.Clear();

        if(categories.Any())
        {
            courseLoader.Visibility = Visibility.Collapsed;
            emptyDataForCategories.Visibility = Visibility.Collapsed;

            foreach (var category in categories)
            {
                CourseForComponent component = new CourseForComponent();
                component.Tag = category;
                component.SetValues(count, category.Id, category.Name, category.Courses.Count, category.Courses.Sum(x => x.Students.Count));
                stCategories.Children.Add(component);
                count++;
            }
        }
        else
        {
            notifier.ShowInformation("Kategoriyalar topilmadi!");
            courseLoader.Visibility = Visibility.Collapsed;
            emptyDataForCategories.Visibility = Visibility.Visible;
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        GetAllCategory();
    }
}
