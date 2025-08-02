using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Helpers;
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
using System.Threading.Tasks;
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
    private int pageSize = 10;
    private int pageNumber = 1;
    private bool hasNext = false;
    private bool hasPrevious = false;
    private bool isFiltered = false;
    private GroupForFilterDto? isFilterDto = new GroupForFilterDto();

    public MainPage()
    {
        InitializeComponent();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
        this._categoryService = new CategoryService();
        this._groupService = new GroupService();
        this._studentService = new StudentService();

        var window = GetActiveWindow();

        if (window.WindowState == WindowState.Maximized)
            pageSize = 15;
        else if (window.WindowState == WindowState.Normal)
            pageSize = 10;
        else
            pageSize = 10;
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 10,
            offsetY: 10);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;

        cfg.DisplayOptions.Width = 200;
        cfg.DisplayOptions.TopMost = true;
    });

    private void Pagination(PagedResponse<GroupForResultDto> pagedResponse)
    {
        this.pageNumber = pagedResponse.CurrentPage;
        this.pageSize = (pagedResponse.PageSize > 0 ? pagedResponse.PageSize : 10);
        this.hasNext = pagedResponse.HasNext;
        this.hasPrevious = pagedResponse.HasPrevious;

        btnPrevious.Visibility = pagedResponse.HasPrevious switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed
        };

        btnNext.Visibility = pagedResponse.HasNext switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed
        };

        tbCurrentPageNumber.Text = pagedResponse.CurrentPage.ToString();
        btnPrevious.IsChecked = false;
        btnNext.IsChecked = false;
    }

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
        try
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

            this.isFiltered = true;
            this.isFilterDto = dto;
            this.pageNumber = 1;

            var groups = await Task.Run(async () => await _groupService.FilterAsync(dto, pageSize, pageNumber));

            if(groups.Data.Any())
            {
                ShowGroup(groups.Data);
                Pagination(groups);
            }
            else
            {
                courseForLoader.Visibility = Visibility.Collapsed;
                emptyDataForCourse.Visibility = Visibility.Visible;
            }    
        }
        catch(Exception ex)
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Visible;
        }
    }

    private async Task GetAllTeacherCourses(long teacherId)
    {
        try
        {
            stCourses.Children.Clear();
            courseForLoader.Visibility = Visibility.Visible;

            var courses = await Task.Run(async () => await _courseService.GetAllByTeacherIdAsync(teacherId));
            //ShowCourse(courses);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Kurslarni yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async Task GetAllTeachers()
    {
        try
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
        catch(Exception ex)
        {
            notifier.ShowWarning("O'qituvchilarning malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async Task GetAllGroup()
    {
        try
        {
            courseForLoader.Visibility = Visibility.Visible;

            var groups = await Task.Run(async () => await _groupService.GetAllPaginationAsync(pageSize, pageNumber));

            ShowGroup(groups.Data);
            Pagination(groups);
        }
        catch(Exception ex)
        {
            notifier.ShowError("Guruh malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Visible;
        }
    }

    private async Task GetAllGroupByTeacherId()
    {
        try
        {
            courseForLoader.Visibility = Visibility.Visible;
            var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(_teacher.Id));
            ShowGroup(groups);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Guruhlaringizni yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Visible;
        }
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
                    group.Students?.Count ?? 0,
                    group.PreferredTime ?? TimeSpan.Zero,
                    group.PreferredDay);

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
        try
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
        catch(Exception ex)
        {
            notifier.ShowWarning("Kurslarni yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    //private void ShowCourse(List<CourseForResultDto> courses)
    //{
    //    int count = 1;

    //    if (courses.Any())
    //    {
    //        courseForLoader.Visibility = Visibility.Collapsed;
    //        emptyDataForCourse.Visibility = Visibility.Collapsed;

    //        foreach (var course in courses)
    //        {
    //            MainForCourseComponent component = new MainForCourseComponent();
    //            component.Tag = course;

    //            component.SetValues(
    //                count,
    //                course.Id,
    //                course.Name,
    //                course.Groups?.Sum(x => x.Students.Count) ?? 0);

    //            stCourses.Children.Add(component);
    //            count++;
    //        }

    //        YourCoursesCount.Text = courses.Count.ToString();
    //        YourStudentsCount.Text = courses.Sum(x => x.Groups.Sum(x => x.Students.Count)).ToString();
    //    }
    //    else
    //    {
    //        courseForLoader.Visibility = Visibility.Collapsed;
    //        emptyDataForCourse.Visibility = Visibility.Visible;
    //    }
    //}

    private async Task<long> GetTeacher(long userId)
    {
        try
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
        catch(Exception ex)
        {
            notifier.ShowWarning("Malumotlaringizni yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            return 0;
        }
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
            //await GetAllTeacherCourses(teacherId);
        }
        else
        {
            await GetAllCategories();
            await GetAllCourse();
            await GetAllTeachers();
            await GetAllStudent();
            await GetAllGroup();
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

    private Window? GetActiveWindow()
        => Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

    private async void dtEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            await FilterCourses();
    }

    private async void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            await FilterCourses();
    }

    private async void teacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            await FilterCourses();
    }

    private async void categoryComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            await FilterCourses();
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

    private async void btnPrevious_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber -= 1;

        if(this.isFiltered && this.isFilterDto is not null)
        {
            var groups = await Task.Run(async () => await _groupService.FilterAsync(isFilterDto, pageSize, pageNumber));

            ShowGroup(groups.Data);
            Pagination(groups);
        }
        else
            await GetAllGroup();
    }

    private async void btnNext_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber += 1;

        if(this.isFiltered && this.isFilterDto is not null)
        {
            var groups = await Task.Run(async () => await _groupService.FilterAsync(isFilterDto, pageSize, pageNumber));

            ShowGroup(groups.Data);
            Pagination(groups);
        }
        else
            await GetAllGroup();
    }
}
