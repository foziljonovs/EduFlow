using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows;
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
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private readonly ICategoryService _categoryService;
    private TeacherForResultDto _teacher;

    public MainPage()
    {
        InitializeComponent();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
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

    private async Task GetAllCategories()
    {
        var categories = await Task.Run(async () => await _categoryService.GetAllAsync());
        if (categories.Any())
        {
            foreach (var category in categories)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = category.Name;
                item.Tag = category.Id;
                categoryComboBox.Items.Add(item);
            }
        }
    }

    private async Task FilterCourses()
    {
        stCourses.Children.Clear();
        courseForLoader.Visibility = Visibility.Visible;

        CourseForFilterDto dto = new CourseForFilterDto();

        if(dtStartDate.SelectedDate is not null)
            dto.StartedDate = dtStartDate.SelectedDate.Value;

        if(dtEndDate.SelectedDate is not null)
            dto.FinishedDate = dtEndDate.SelectedDate.Value;

        if (categoryComboBox.SelectedItem is ComboBoxItem selectedCategoryItem
            && selectedCategoryItem.Tag != null)
            dto.CategoryId = (long)selectedCategoryItem.Tag;

        if(teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem
            && selectedTeacherItem.Tag != null)
            dto.TeacherId = (long)selectedTeacherItem.Tag;

        var window = Window.GetWindow(this) as MainWindow;
        if (window.MainMenuNavigation.Content is TeacherNavigationPage)
            dto.TeacherId = _teacher.Id;

        var courses = await Task.Run(async () => await _courseService.FilterAsync(dto));

        if(courses.Any()) 
            ShowCourse(courses);
        else
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Visible;
        }    
    }

    private async Task GetAllTeacherCourses(long teacherId)
    {
        stCourses.Children.Clear();
        courseForLoader.Visibility = Visibility.Visible;

        var courses = await Task.Run(async () => await _courseService.GetAllByTeacherIdAsync(teacherId));
        ShowCourse(courses);
    }

    private async Task GetAllTeachers()
    {
        var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

        if (teachers.Any())
        {
            foreach(var teacher in teachers)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = teacher.User.Firstname;
                item.Tag = teacher.Id;
                teacherComboBox.Items.Add(item);
            }
        }
        else
        {
            notifier.ShowInformation("Ustozlar topilmadi!");
        }
    }

    private async Task GetAllCourse()
    {
        stCourses.Children.Clear();
        courseForLoader.Visibility = Visibility.Visible;

        var courses = await Task.Run(async () => await _courseService.GetAllAsync());
        ShowCourse(courses);
    }

    private void ShowCourse(List<CourseForResultDto> courses)
    {
        int count = 1;

        if (courses.Any())
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Collapsed;

            foreach (var course in courses)
            {
                MainForCourseComponent component = new MainForCourseComponent();
                component.Tag = course;
                var teacherName = course.Teachers.Any() ? (course.Teachers.First().User?.Firstname) : "Topilmadi"; //vaqtincha

                component.SetValues(
                    count,
                    course.Id,
                    course.Name,
                    course.Groups?.Sum(x => x.Students.Count) ?? 0,
                    teacherName);

                stCourses.Children.Add(component);
                count++;
            }

            YourCoursesCount.Text = courses.Count.ToString();
            YourStudentsCount.Text = courses.Sum(x => x.Groups.Sum(x => x.Students.Count)).ToString();
        }
        else
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Visible;
        }
    }

    private async Task<long> GetTeacher(long userId)
    {
        var teacher = await Task.Run(async () => await _teacherService.GetByUserIdAsync(userId));

        if(teacher is null)
        {
            notifier.ShowInformation("Ustoz topilmadi!");
            return 0;
        }

        _teacher = teacher;
        return teacher.Id;
    }

    private async Task LoadPage()
    {
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;

        if (role is Domain.Enums.UserRole.Teacher)
        {
            categoryComboBox.Visibility = Visibility.Collapsed;
            createCategoryBtn.Visibility = Visibility.Collapsed;
            teacherComboBox.Visibility = Visibility.Collapsed;
            createTeacherBtn.Visibility = Visibility.Collapsed;
            createCourseBtn.Visibility = Visibility.Collapsed;

            var teacherId = await GetTeacher(id);

            if(teacherId == 0)
            {
                stCourses.Children.Clear();
                emptyDataForCourse.Visibility = Visibility.Visible;
                return;
            }

            await GetAllTeacherCourses(teacherId);
        }
        else
        {
            await GetAllCourse();
            await GetAllTeachers();
            await GetAllCategories();
        }
    }
    private bool isPageLoaded = false;
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if(!isPageLoaded)
        {
            isPageLoaded = true;
            LoadPage();
        }
    }

    private void dtEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            FilterCourses();
    }

    private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            FilterCourses();
    }

    private void teacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            FilterCourses();
    }
}
