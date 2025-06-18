using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Cashier.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Payments.Registry;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
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
    private readonly IRegistryService _registryService;
    private readonly IPaymentService _paymentService;
    public MainPage()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._categoryService = new CategoryService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
        this._studentService = new StudentService();
        this._registryService = new RegistryService();
        this._paymentService = new PaymentService();
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
        try
        {
            var categories = await Task.Run(async () => await _categoryService.GetAllAsync());
        
            ShowCategories(categories);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Kategoriyalarni yuklashda xatolik yuz berdi!");
        }
    }

    private async Task GetAllCourse()
    {
        try
        {
            var courses = await Task.Run(async () => await _courseService.GetAllAsync());

            ShowCourses(courses);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Kurslarni yuklashda xatolik yuz berdi!");
        }
    }

    private async Task GetAllTeacher()
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

            ShowTeachers(teachers);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("O'qituvchilarni yuklashda xatolik yuz berdi!");
        }
    }

    private async Task GetAllGroup()
    {
        try
        {
            groupLoader.Visibility = Visibility.Visible;

            var groups = await Task.Run(async () => await _groupService.GetAllAsync());

            ShowGroup(groups);
            ActiveGroupCount(groups);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Guruhlarni yuklashda xatolik yuz berdi!");
        }
    }

    private async Task GetAllRegistry()
    {
        try
        {
            var registries = await Task.Run(async () => await _registryService.GetAllAsync());

            if (registries.Any())
                ShowMonthlyDebit(registries);
            else
                MonthlyIncome.Text = "0";
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Oylik tushumlarni olishda xatolik yuz berdi!");
        }
    }

    private async Task GetAllPayment()
    {
        try
        {
            var payments = await Task.Run(async () => await _paymentService.GetAllAsync());

            if (payments.Any())
                ShowStudentsOfPaid(payments);
            else
                NumberOfStudentsPaid.Text = "0";
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("To'lovlarni yuklashda xatolik yuz berdi!");
        }
    }

    private void ShowStudentsOfPaid(List<PaymentForResultDto> payments)
    {
        var allCountOfPaid = payments.Count(x => x.CreatedAt.Month == DateTime.UtcNow.Month);

        if (allCountOfPaid > 0)
            NumberOfStudentsPaid.Text = allCountOfPaid.ToString();
        else
            NumberOfStudentsPaid.Text = "0";
    }

    private void ShowMonthlyDebit(List<RegistryForResultDto> registries)
    {
        var allDebit = registries.Sum(x => x.Debit);

        if (allDebit > 0)
            MonthlyIncome.Text = allDebit.ToString("0");
        else
            MonthlyIncome.Text = "0";
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

    private async Task FilterAsync()
    {
        try
        {
            stGroups.Children.Clear();
            groupLoader.Visibility = Visibility.Visible;

            GroupForFilterDto dto = new GroupForFilterDto();
            dto.IsStatus = Domain.Enums.Status.Active;

            if (categoryComboBox.SelectedItem is ComboBoxItem selectedCategoryItem &&
                selectedCategoryItem.Tag != null)
                dto.CategoryId = (long)selectedCategoryItem.Tag;

            if(courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem &&
                selectedCourseItem.Tag != null)
                dto.CourseId = (long)selectedCourseItem.Tag;

            if(teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem &&
                selectedTeacherItem.Tag != null)
                dto.TeacherId = (long)selectedTeacherItem.Tag;

            if (groupStatusComboBox.SelectedItem is ComboBoxItem selectedGroupStatusItem &&
                selectedGroupStatusItem.Tag != null)
            {
                dto.IsStatus = selectedGroupStatusItem.Tag.ToString() switch
                {
                    "1" => Domain.Enums.Status.Archived,
                    "2" => Domain.Enums.Status.Graduated,
                    "3" => Domain.Enums.Status.Deleted,
                    _ => Domain.Enums.Status.Active
                };
            }

            var groups = await Task.Run(async () => await _groupService.FilterAsync(dto));

            if (groups.Any())
                ShowGroup(groups);
            else
            {
                groupLoader.Visibility = Visibility.Collapsed;
                emptyDataForGroup.Visibility = Visibility.Visible;
            }
        }
        catch(Exception ex)
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroup.Visibility = Visibility.Visible;
        }
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
        await GetAllRegistry();
        await GetAllPayment();
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

    private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            FilterAsync();
    }

    private void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            FilterAsync();
    }

    private void teacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            FilterAsync();
    }

    private void groupStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isPageLoaded)
            FilterAsync();
    }
}
