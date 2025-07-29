using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows.StudentForWindows;
using EduFlow.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Pages.StudentForPages;

/// <summary>
/// Interaction logic for StudentPage.xaml
/// </summary>
public partial class StudentPage : Page
{
    private readonly IStudentService _service;
    private readonly ITeacherService _teacherService;
    private readonly ICourseService _courseService;
    private readonly ICategoryService _categoryService;
    private TeacherForResultDto _teacher;
    private int pageSize = 10;
    private int pageNumber = 1;
    private bool hasNext = false;
    private bool hasPrevious = false;
    public StudentPage()
    {
        InitializeComponent();
        this._service = new StudentService();
        this._teacherService = new TeacherService();
        this._courseService = new CourseService();
        this._categoryService = new CategoryService();

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

    private Window? GetActiveWindow()
        => Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
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
    private async Task GetAllStudent()
    {
        try
        {
            studentLoader.Visibility = Visibility.Visible;
            var students = await Task.Run(async () => await _service.GetAllPaginationAsync(pageSize, pageNumber));

            if (students.Data.Any())
            {
                ShowStudents(students.Data);
                Pagination(students);
            }
            else
            {
                studentLoader.Visibility = Visibility.Collapsed;
                emptyDataForStudent.Visibility = Visibility.Visible;
                tbCurrentPageNumber.Text = "0";
            }
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("O'quvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta yuklang!");
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private async Task GetAllStudentByTeacher(long teacherId)
    {
        try
        {
            studentLoader.Visibility = Visibility.Visible;
            var students = await Task.Run(async () => await _service.GetAllByTeacherIdAsync(teacherId));

            ShowStudents(students);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("O'quvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta yuklang!");
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private async Task GetAllCourse()
    {
        try
        {
            var courses = await Task.Run(async () => await _courseService.GetAllAsync());

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = course.Name;
                    item.Tag = course.Id;
                    courseComboBox.Items.Add(item);
                }
            }
            else
            {
                notifier.ShowInformation("Kurslar topilmadi!");
            }
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Kurs malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta yuklang!");
        }
    }

    private async Task GetAllStudentByCourse()
    {
        try
        {
            long courseId = 0;
            studentLoader.Visibility = Visibility.Visible;

            if(courseComboBox.SelectedItem is ComboBoxItem selectedComboBoxItem
                && selectedComboBoxItem.Tag != null)
                courseId = (long)selectedComboBoxItem.Tag;

            var students = await Task.Run(async () => await _service.GetAllByCourseIdAsync(courseId));
        
            ShowStudents(students);
        }
        catch(Exception ex)
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private void ShowStudents(List<StudentForResultDto> students)
    {
        List<StudentForResultDto> studentsShow = new List<StudentForResultDto>();

        if (_teacher is not null)
            studentsShow = students.Where(x =>
                x.StudentCourses.Any(y => y.CourseId == _teacher.CourseId && y.Status == EnrollmentStatus.Active))
                .ToList();
        else
            studentsShow = students;

        int count = 1;
        stStudents.Children.Clear();

        if (studentsShow.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Collapsed;

            foreach(var student in studentsShow)
            {
                string activeGroupName = student.Groups.Any(x => x.IsStatus == Status.Active)
                                        ? student.Groups.FirstOrDefault(x => x.IsStatus == Status.Active).Name : "Yo'q";

                StudentForComponent component = new StudentForComponent();
                component.Tag = student;
                component.SetValues(
                    count,
                    student.Id,
                    student.Fullname,
                    student.Age,
                    student.Address,
                    student.PhoneNumber,
                    activeGroupName);

                component.OnStudentDelete += GetAllStudent;
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

    private async Task<long> GetTeacher(long userId)
    {
        try
        {
            var teacher = await Task.Run(async () => await _teacherService.GetByUserIdAsync(userId));

            if (teacher is null)
            {
                notifier.ShowInformation("Ustoz topilmadi!");
                return 0;
            }

            _teacher = teacher;
            return teacher.Id;
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("O'qituvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
            return 0;
        }
    }

    private async Task LoadPage()
    {
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;

        if(role is UserRole.Teacher)
        {
            courseComboBox.Visibility = Visibility.Collapsed;
            createStudent.Visibility = Visibility.Collapsed;

            var teacherId = await GetTeacher(id);
            if(teacherId == 0)
            {
                stStudents.Children.Clear();
                emptyDataForStudent.Visibility = Visibility.Visible;
                return;
            }

            await GetAllStudentByTeacher(teacherId);
        }
        else
        {
            await GetAllStudent();
            await GetAllCourse();
        }
    }

    private bool isPageLoaded = false;
    private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if(!isPageLoaded)
        {
            isPageLoaded = true;
            LoadPage();
        }
    }

    private void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isPageLoaded)
            GetAllStudentByCourse();
    }

    private async Task GetStudentByPhoneNumber(string phoneNumber)
    {
        try
        {
            studentLoader.Visibility = Visibility.Visible;
            this.pageNumber = 1;

            var students = await Task.Run(async () => await _service.GetByPhoneNumberAsync(phoneNumber, pageSize, pageNumber));

            if(!students.Data.Any())
            {
                stStudents.Children.Clear();
                emptyDataForStudent.Visibility = Visibility.Visible;
            }

            ShowStudents(students.Data);
            Pagination(students);
        }
        catch(Exception ex)
        {
            notifier.ShowWarning("Telefon raqam orqali o'quvchini qidirishda xatolik yuz berdi, Iltimos qayta urining!");
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private async void searchPhoneNumberForStudentTxt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Enter)
        {
            string phoneNumber = searchPhoneNumberForStudentTxt.Text.Trim();

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                notifier.ShowWarning("Telefon raqamni ohirgi 4 raqamini kiriting!");
                return;
            }

            if (!phoneNumber.All(char.IsDigit))
            {
                notifier.ShowWarning("Telefon raqam faqat raqamlardan iborat bo‘lishi kerak!");
                return;
            }

            if (phoneNumber.Length != 4)
            {
                notifier.ShowWarning("Telefon raqam aniq 4 ta raqamdan iborat bo‘lishi kerak!");
                return;
            }

            await GetStudentByPhoneNumber(phoneNumber);
        }
    }

    private async void createStudent_Click(object sender, RoutedEventArgs e)
    {
        StudentForCreateWindow window = new StudentForCreateWindow();
        window.ShowDialog();
        await GetAllStudent();
    }

    private async void btnPrevious_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber -= 1;

        await GetAllStudent();
    }

    private async void btnNext_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber += 1;

        await GetAllStudent();
    }
}
