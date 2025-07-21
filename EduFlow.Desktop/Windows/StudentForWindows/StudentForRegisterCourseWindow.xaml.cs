using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.StudentCourse;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForRegisterCourseWindow.xaml
/// </summary>
public partial class StudentForRegisterCourseWindow : Window
{
    private readonly IStudentCourseService _studentCourseService;
    private readonly ICourseService _courseService;
    private long _studentId;
    public StudentForRegisterCourseWindow()
    {
        InitializeComponent();
        this._studentCourseService = new StudentCourseService();
        this._courseService = new CourseService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
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

    Notifier notifierThis = new Notifier(cfg =>
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

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    public void SetStudentId(long studentId)
        => this._studentId = studentId;

    private async Task GetAllCourse()
    {
        try
        {
            var courses = await Task.Run(async () => await _courseService.GetAllAsync());

            ShowCourses(courses);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Kurslarni yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private void ShowCourses(List<CourseForResultDto> courses)
    {
        if (courses.Any())
        {
            foreach(var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem
                {   
                    Tag = course.Id,
                    Content = course.Name
                };

                courseCombobox.Items.Add(item);
            }
        }
        else
        {
            notifierThis.ShowWarning("Kurs malumotlari topilmadi, Iltimos qayta urining!");
        }
    }

    private async Task SavedAsync()
    {
        try
        {
            StudentCourseForCreateDto dto = new StudentCourseForCreateDto
            {
                StudentId = _studentId
            };

            if (courseCombobox.SelectedItem is ComboBoxItem selectedCourseItem &&
                long.TryParse(selectedCourseItem.Tag.ToString(), out long courseId))
                dto.CourseId = courseId;
            else
            {
                notifierThis.ShowWarning("Kurs tanlanmadi!");
                courseCombobox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            var result = await _studentCourseService.AddAsync(dto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess("O'quvchi kursga ro'yxatdan o'tdi!");
            }
            else
            {
                notifierThis.ShowWarning("O'quvchini ro'yxatga olishda xatolik yuz berdi, Iltimos qayta urining!");
                saveBtn.IsEnabled = true;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        if(!saveBtn.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos, kuting!");
            return;
        }

        saveBtn.IsEnabled = false;

        try
        {
            await SavedAsync();
        }
        catch(Exception ex)
        {
            saveBtn.IsEnabled = true;
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetAllCourse();
    }
}
