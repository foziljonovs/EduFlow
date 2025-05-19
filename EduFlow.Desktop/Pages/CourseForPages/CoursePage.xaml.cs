using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.CourseForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows.CourseForWindows;
using System.Threading.Tasks;
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
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private TeacherForResultDto _teacher;
    public CoursePage()
    {
        InitializeComponent();
        this._categoryService = new CategoryService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
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
        var categories = await Task.Run(async () => await _categoryService.GetAllAsync());

        if (categories.Any())
        {
            foreach(var item in categories)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.Name;
                comboBoxItem.Tag = item.Id;
                categoryComboBox.Items.Add(comboBoxItem);
            }
        }
        else
        {
            notifier.ShowWarning("Kategoriya topilmadi!");
        }
    }

    private async Task GetAllCourse()
    {
        stCategories.Children.Clear();
        courseLoader.Visibility = Visibility.Visible;
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        ShowCourses(courses);
    }

    private async Task GetAllByTeacherId()
    {
        stCategories.Children.Clear();
        courseLoader.Visibility = Visibility.Visible;
        var courses = await Task.Run(async () => await _courseService.GetAllByTeacherIdAsync(this._teacher.Id));
        ShowCourses(courses);
    }

    private async Task GetAllCourseByCategory(long id)
    {
        stCategories.Children.Clear();
        courseLoader.Visibility = Visibility.Visible;
        var courses = await Task.Run(async () => await _courseService.GetAllByCategoryIdAsync(id));
        ShowCourses(courses);
    }

    private void ShowCourses(List<CourseForResultDto> courses)
    {
        int count = 1;
        stCategories.Children.Clear();
        if (courses.Any())
        {
            courseLoader.Visibility = Visibility.Collapsed;
            emptyDataForCategories.Visibility = Visibility.Collapsed;
            foreach (var course in courses)
            {
                CourseForComponent component = new CourseForComponent();
                component.Tag = course;

                if(_teacher is not null)
                    component.GetCourses += GetAllByTeacherId;
                else
                    component.GetCourses += GetAllCourse;

                component.SetValues(count, course.Id, course.Name, course.Groups.Count, course.Groups.Sum(x => x.Students.Count));
                stCategories.Children.Add(component);
                count++;
            }
        }
        else
        {
            notifier.ShowInformation("Kurslar topilmadi!");
            courseLoader.Visibility = Visibility.Collapsed;
            emptyDataForCategories.Visibility = Visibility.Visible;
        }
    }
    private async Task<long> GetTeacher(long userId)
    {
        var teacher = await Task.Run(async () => await _teacherService.GetByUserIdAsync(userId));

        if (teacher is null)
        {
            notifier.ShowInformation("Ustoz topilmadi!");
            return 0;
        }

        this._teacher = teacher;
        return teacher.Id;
    }

    private async Task LoadPage()
    {
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;

        if(role == Domain.Enums.UserRole.Teacher)
        {
            categoryComboBox.Visibility = Visibility.Collapsed;
            var teacherId = await GetTeacher(id);

            if(teacherId == 0)
            {
                stCategories.Children.Clear();
                emptyDataForCategories.Visibility = Visibility.Visible;
                return;
            }

            await GetAllByTeacherId();
        }
        else
        {
            categoryComboBox.Visibility = Visibility.Visible;
            await GetAllCategory();
            await GetAllCourse();
        }
    }

    private bool isPageLoaded = false;
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if(!isPageLoaded)
        {
            LoadPage();
            isPageLoaded = true;
        }
    }

    private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
        {
            if(categoryComboBox.SelectedItem is ComboBoxItem item
                && item.Tag != null)
            {
                long categoryId = (long)item.Tag;
                GetAllCourseByCategory(categoryId);
            }
        }
    }

    private async void AddCourseBtn_Click(object sender, RoutedEventArgs e)
    {
        CourseForCreateWindow window = new CourseForCreateWindow();
        window.ShowDialog();
        await GetAllCourse();
    }
}
