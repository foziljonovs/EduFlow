using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows;
using EduFlow.Desktop.Windows.CategoryForWindows;
using EduFlow.Desktop.Windows.CourseForWindows;
using EduFlow.Desktop.Windows.GroupForWindows;
using EduFlow.Desktop.Windows.TeacherForWindows;
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
    private readonly IGroupService _groupService;
    private readonly IStudentService _studentService;
    private TeacherForResultDto _teacher;

    public MainPage()
    {
        InitializeComponent();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
        this._categoryService = new CategoryService();
        this._groupService = new GroupService();
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

    private async Task GetAllCategories()
    {
        var categories = await Task.Run(async () => await _categoryService.GetAllAsync());
        if (categories.Any())
        {
            categoryComboBox.Items.Clear();

            categoryComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

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

        GroupForFilterDto dto = new GroupForFilterDto();

        if(dtStartDate.SelectedDate is not null)
            dto.StartedDate = dtStartDate.SelectedDate.Value;

        if(dtEndDate.SelectedDate is not null)
            dto.FinishedDate = dtEndDate.SelectedDate.Value;

        if (categoryComboBox.SelectedItem is ComboBoxItem selectedCategoryItem
            && selectedCategoryItem.Tag != null)
            dto.CategoryId = (long)selectedCategoryItem.Tag;

        if (courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem
            && selectedCourseItem.Tag != null)
            dto.CourseId = (long)selectedCourseItem.Tag;

        if(teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem
            && selectedTeacherItem.Tag != null)
            dto.TeacherId = (long)selectedTeacherItem.Tag;

        dto.IsStatus = Domain.Enums.Status.Active;

        var window = Window.GetWindow(this) as MainWindow;
        if (window.MainMenuNavigation.Content is TeacherNavigationPage)
            dto.TeacherId = _teacher.Id;

        var groups = await Task.Run(async () => await _groupService.FilterAsync(dto));

        if(groups.Any()) 
            ShowGroup(groups);
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
            teacherComboBox.Items.Clear();

            var defaultItem = new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            };

            teacherComboBox.Items.Add(defaultItem);

            foreach (var teacher in teachers)
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

    private async Task GetAllGroup()
    {
        courseForLoader.Visibility = Visibility.Visible;

        var groups = await Task.Run(async () => await _groupService.GetAllAsync());
        ShowGroup(groups);
    }

    private async Task GetAllGroupByTeacherId()
    {
        courseForLoader.Visibility = Visibility.Visible;
        var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(_teacher.Id));
        ShowGroup(groups);
    }

    private void ShowGroup(List<GroupForResultDto> groups)
    {
        stCourses.Children.Clear();

        int count = 1;
        if (groups.Any())
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Collapsed;

            foreach (var group in groups)
            {
                MainForCourseComponent component = new MainForCourseComponent();
                component.Tag = group;

                component.SetValues(
                    count,
                    group.Id,
                    group.Name,
                    group.Students?.Count ?? 0);

                if (_teacher is not null)
                    component.OnGroupView += GetAllGroupByTeacherId;
                else
                    component.OnGroupView += GetAllGroup;
                stCourses.Children.Add(component);
                count++;
            }

            YourCoursesCount.Text = groups.Count.ToString();
            YourStudentsCount.Text = groups.Sum(x => x.Students.Count).ToString();
        }
        else
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Visible;
        }
    }

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());
        //ShowCourse(courses);

        if (courses.Any())
        {
            courseComboBox.Items.Clear();

            courseComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach (var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = course.Name;
                item.Tag = course.Id;
                courseComboBox.Items.Add(item);
            }
        }
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

                component.SetValues(
                    count,
                    course.Id,
                    course.Name,
                    course.Groups?.Sum(x => x.Students.Count) ?? 0);

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

    private async Task GetAllStudent()
    {
        var students = await _studentService.GetAllAsync();

        if (!students.Any())
        {
            notifier.ShowInformation("Talabalar topilmadi!");
            return;
        }
        else
        {
            var studentsCount = students.Count(x => x.Groups.Count == 0);
            WaitingStudentsCount.Text = studentsCount.ToString();
        }
    }

    private async Task LoadPage()
    { 
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;

        if (role is Domain.Enums.UserRole.Teacher)
        {
            categoryComboBox.Visibility = Visibility.Collapsed;
            createCategoryBtn.Visibility = Visibility.Collapsed;
            courseComboBox.Visibility = Visibility.Collapsed;
            createCourseBtn.Visibility = Visibility.Collapsed;
            teacherComboBox.Visibility = Visibility.Collapsed;
            createTeacherBtn.Visibility = Visibility.Collapsed;
            createCourseBtn.Visibility = Visibility.Collapsed;
            createGroupBtn.Visibility = Visibility.Collapsed;
            WaitingBox.Visibility = Visibility.Collapsed;

            var teacherId = await GetTeacher(id);

            if(teacherId == 0)
            {
                stCourses.Children.Clear();
                emptyDataForCourse.Visibility = Visibility.Visible;
                return;
            }

            await GetAllGroupByTeacherId();
            await GetAllTeacherCourses(teacherId);
        }
        else
        {
            await GetAllCategories();
            await GetAllCourse();
            await GetAllTeachers();
            await GetAllGroup();
            await GetAllStudent();
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

    private void categoryComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            FilterCourses();
    }

    private async void createCategoryBtn_Click(object sender, RoutedEventArgs e)
    {
        CategoryForCreateWindow window = new CategoryForCreateWindow();
        window.ShowDialog();
        await GetAllCategories();
    }

    private async void createCourseBtn_Click(object sender, RoutedEventArgs e)
    {
        CourseForCreateWindow window = new CourseForCreateWindow();
        window.ShowDialog();
        await GetAllCourse();
    }

    private async void createTeacherBtn_Click(object sender, RoutedEventArgs e)
    {
        TeacherForCreateWindow window = new TeacherForCreateWindow();
        window.ShowDialog();
        await GetAllTeachers();
    }

    private async void createGroupBtn_Click(object sender, RoutedEventArgs e)
    {
        GroupForCreateWindow window = new GroupForCreateWindow();
        window.ShowDialog();
        await GetAllGroup();
    }
}
