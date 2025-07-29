using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Cashier.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Pages.StudentForPages;

/// <summary>
/// Interaction logic for StudentPage.xaml
/// </summary>
public partial class StudentPage : Page
{
    private readonly IStudentService _studentService;
    private readonly ICourseService _courseService;
    private int pageSize = 10;
    private int pageNumber = 1;
    private bool hasNext = false;
    private bool hasPrevious = false;
    private bool isFiltered = false;
    private StudentForFilterDto? isFilterDto = new StudentForFilterDto();
    public StudentPage()
    {
        InitializeComponent();
        this._studentService = new StudentService();
        this._courseService = new CourseService();

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
            offsetX: 20,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;

        cfg.DisplayOptions.Width = 200;
        cfg.DisplayOptions.TopMost = true;
    });

    private Window? GetActiveWindow()
        => Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

    private async Task GetAllStudent()
    {
        try
        {
            studentLoader.Visibility = Visibility.Visible;

            var students = await Task.Run(async () => await _studentService.GetAllPaginationAsync(pageSize, pageNumber));

            ShowStudents(students.Data);
            Pagination(students);
        }
        catch(Exception ex)
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
            notifier.ShowError("Kurslarni yuklashda xatolik yuz berdi!");
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
            notifier.ShowError("Xatolik yuz berdi!");
        }
    }

    private async Task FilterAsync()
    {
        try
        {
            stStudents.Children.Clear();
            studentLoader.Visibility = Visibility.Visible;

            StudentForFilterDto dto = new StudentForFilterDto();
            if (courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem &&
                selectedCourseItem.Tag is not null)
                dto.CourseId = (long)selectedCourseItem.Tag;

            if(statusComboBox.SelectedItem is ComboBoxItem selectedStatusItem &&
                selectedStatusItem.Tag is not null)
                dto.CourseStatus = selectedStatusItem.Tag.ToString() switch
                {
                    "0" => EnrollmentStatus.Pending,
                    "1" => EnrollmentStatus.Active,
                    "2" => EnrollmentStatus.Suspended,
                    "3" => EnrollmentStatus.Completed,
                    "4" => EnrollmentStatus.Dropped,
                    _ => EnrollmentStatus.Active
                };

            this.pageNumber = 1;
            this.isFiltered = true;
            this.isFilterDto = dto;

            var students = await Task.Run(async () => await _studentService.FilterAsync(dto, pageSize, pageNumber));
            
            ShowStudents(students.Data);
            Pagination(students);
        }
        catch(Exception ex)
        {
            notifier.ShowError("Ma'lumotlarni filterlashda xatolik yuz berdi!");
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private void Pagination(PagedResponse<StudentForResultDto> pagedResponse)
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

    private void ShowCourses(List<CourseForResultDto> courses)
    {
        courseComboBox.Items.Clear();

        if (courses.Any())
        {
            ComboBoxItem defaultItem = new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false,
                Tag = "0"
            };

            courseComboBox.Items.Add(defaultItem);

            foreach (var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = course.Name,
                    Tag = course.Id
                };

                courseComboBox.Items.Add(item);
            }
        }
        else
        {
            notifier.ShowWarning("Kurslar topilmadi!");

            ComboBoxItem item = new ComboBoxItem
            {
                Content = "Topilmadi",
                Tag = "0"
            };

            courseComboBox.Items.Add(item);
        }
    }

    private void ShowStudents(List<StudentForResultDto> students)
    {
        int count = 1;

        if (students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Collapsed;

            foreach(var student in students)
            {
                string activeGroupName = student.Groups.Any(x => x.IsStatus == Status.Active)
                                        ? student.Groups.FirstOrDefault(x => x.IsStatus == Status.Active).Name : "Yo'q";

                StudentForSecondComponent component = new StudentForSecondComponent();
                component.SetValues(
                    student.Id,
                    count,
                    student.Fullname,
                    student.Age,
                    student.Address ?? "Nomalum",
                    student.PhoneNumber,
                    activeGroupName);

                stStudents.Children.Add(component);
                count++;
            }
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private async Task LoadedPage()
    {
        await GetAllCourse();
        await GetAllStudent();

        IsPageLoaded = true;
    }

    private bool IsPageLoaded = false;
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if(!IsPageLoaded)
            LoadedPage();
    }

    private async void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsPageLoaded)
            await FilterAsync();
    }

    private async void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsPageLoaded)
            await FilterAsync();
    }

    private async void btnPrevious_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber -= 1;

        if(this.isFiltered && this.isFilterDto is not null)
        {
            var students = await Task.Run(async () => await _studentService.FilterAsync(isFilterDto, pageSize, pageNumber));

            ShowStudents(students.Data);
            Pagination(students);
        }
        else
            await GetAllStudent();
    }

    private async void btnNext_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber += 1;

        if(this.isFiltered && this.isFilterDto is not null)
        {
            var students = await Task.Run(async () => await _studentService.FilterAsync(isFilterDto, pageSize, pageNumber));

            ShowStudents(students.Data);
            Pagination(students);
        }
        else
            await GetAllStudent();
    }
}
