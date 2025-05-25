using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForUpdateWindow.xaml
/// </summary>
public partial class StudentForUpdateWindow : Window
{
    private readonly IStudentService _service;
    private readonly ICourseService _courseService;
    private long Id { get; set; }
    private StudentForResultDto Student { get; set; } = new StudentForResultDto();
    public StudentForUpdateWindow()
    {
        InitializeComponent();
        this._service = new StudentService();
        this._courseService = new CourseService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
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

    Notifier notifierThis = new Notifier(cfg =>
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

    public void setId(long id)
        => this.Id = id;

    private async Task GetStudent()
    {
        if(this.Id > 0)
        {
            var student = await _service.GetByIdAsync(this.Id);

            if (student is not null)
            {
                this.Student = student;
                fullNameTxt.Text = student.Fullname;
                addressTxt.Text = student.Address;
                phoneNumberTxt.Text = student.PhoneNumber;
            }
            else
                notifierThis.ShowWarning("Talaba malumotlari noto'g'ri, iltimos qayta yuklang!");
        }
        else
            notifierThis.ShowError("Xatolik yuz berdi, iltimos qayta yuklang!");
    }

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            foreach(var item in courses)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Tag = item.Id,
                    Content = item.Name
                };

                CourseComboBox.Items.Add(comboBoxItem);
            }
        }
        else
            notifierThis.ShowWarning("Kurs malumotlari topilmadi, iltimos qayta yuklang!");
    }

    private void Favourites()
    {
        if(this.Student is not null)
        {
            foreach(ComboBoxItem item in CourseComboBox.Items)
            {
                var studentActiveCourse = this.Student.StudentCourses
                    .FirstOrDefault(x => x.Status == Domain.Enums.EnrollmentStatus.Active 
                                    || x.Status == Domain.Enums.EnrollmentStatus.Pending);

                if(item.Tag is long courseId && studentActiveCourse.CourseId == courseId)
                {
                    CourseComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach(ComboBoxItem item in AgeComboBox.Items)
            {
                var studentAge = this.Student.Age.ToString();

                if(item.Content?.ToString() == studentAge)
                {
                    AgeComboBox.SelectedItem = item;
                    break;
                }
            }
        }
        else
            notifierThis.ShowWarning("Talaba malumotlari topilmadi, iltimos qayta yuklang!");
    }

    private void phoneNumberTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;

        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var regex = new System.Text.RegularExpressions.Regex(@"^\+998\d{0,9}$");
        e.Handled = !regex.IsMatch(futureText);
    }

    private async Task LoadedWindow()
    {
        await GetStudent();
        await GetAllCourse();

        Favourites();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadedWindow();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}
