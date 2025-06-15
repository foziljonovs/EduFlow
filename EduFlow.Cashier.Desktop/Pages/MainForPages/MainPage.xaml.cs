using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Cashier.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
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
    private readonly ICategoryService _categoryService;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private readonly IStudentService _studentService;
    public MainPage()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._categoryService = new CategoryService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
        this._studentService = new StudentService();
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
        
        ShowCategories(categories);
    }

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        ShowCourses(courses);
    }

    private async Task GetAllTeacher()
    {
        var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

        ShowTeachers(teachers);
    }

    private async Task GetAllGroup()
    {
        groupLoader.Visibility = Visibility.Visible;

        var groups = await Task.Run(async () => await _groupService.GetAllAsync());

        ShowGroup(groups);
        ActiveGroupCount(groups);
    }

    private async Task GetAllActiveStudent()
    {
        var students = await Task.Run(async () => await _studentService.GetAllAsync());

        if (students.Any())
        {
            var activeStudents = students
                .Count(x => x.StudentCourses
                    .Any(s => s.Status == Domain.Enums.EnrollmentStatus.Active));

            ActiveStudentsCount.Text = activeStudents.ToString();
        }
        else
            ActiveStudentsCount.Text = "0";
    }

    private void ShowTeachers(List<TeacherForResultDto> teachers)
    {
        if (teachers.Any())
        {
            teacherComboBox.Items.Clear();

            teacherComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach(var teacher in teachers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Tag = teacher.Id,
                    Content = $"{teacher.User?.Firstname} {teacher.User?.Lastname}" 
                };

                teacherComboBox.Items.Add(item);
            }
        }
    }

    private void ShowCourses(List<CourseForResultDto> courses)
    {
        if (courses.Any())
        {
            courseComboBox.Items.Clear();

            courseComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach(var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Tag = course.Id,
                    Content = course.Name
                };

                courseComboBox.Items.Add(item);
            }
        }
    }

    private void ShowCategories(List<CategoryForResultDto> categories)
    {
        if (categories.Any())
        {
            categoryComboBox.Items.Clear();

            categoryComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach(var category in categories)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Tag = category.Id,
                    Content = category.Name
                };

                categoryComboBox.Items.Add(item);
            }
        }
    }

    private void ActiveGroupCount(List<GroupForResultDto> groups)
    {
        if (groups.Any())
        {
            var activeGroupCount = groups.Count(x => x.IsStatus == Domain.Enums.Status.Active);
            tbActiveGroupCount.Text = activeGroupCount.ToString();
        }
        else
            tbActiveGroupCount.Text = "0";
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
        await GetAllCategory();
        await GetAllCourse();
        await GetAllTeacher();
        await GetAllActiveStudent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPage();
    }
}
