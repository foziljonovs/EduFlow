using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Integrated.Services.Users.User.Interfaces;
using EduFlow.Desktop.Integrated.Services.Users.User.Services;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.TeacherForWindows;

/// <summary>
/// Interaction logic for TeacherForCreateWindow.xaml
/// </summary>
public partial class TeacherForCreateWindow : Window
{
    private readonly ITeacherService _teacherService;
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;
    public TeacherForCreateWindow()
    {
        InitializeComponent();
        _teacherService = new TeacherService();
        _userService = new UserService();
        _courseService = new CourseService();
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

    private async Task GetAllCourse()
    {
        courseComboBox.Items.Clear();

        var courses = await Task.Run(async () => await _courseService.GetAllAsync());
        if (courses != null)
        {
            var defaultItem = new ComboBoxItem
            {
                Content = "Kursni tanlang",
                IsSelected = true,
                IsEnabled = false
            };

            courseComboBox.Items.Add(defaultItem);

            foreach (var course in courses)
            {
                var item = new ComboBoxItem
                {
                    Content = course.Name,
                    Tag = course.Id
                };

                courseComboBox.Items.Add(item);
            }
        }
    }

    private async Task SavedAsync()
    {
        try
        {
            UserForCreateDto userDto = new UserForCreateDto();
            TeacherForCreateDto teacherDto = new TeacherForCreateDto();

            if (!string.IsNullOrEmpty(fullNameTxt.Text))
            {
                var firstName = fullNameTxt.Text.Split(' ')[0];
                var lastName = fullNameTxt.Text.Split(' ')[1];

                userDto.Firstname = firstName;
                userDto.Lastname = lastName;
            }
            else
            {
                notifierThis.ShowWarning("Ism va familyani kiriting!");
                return;
            }

            if (!string.IsNullOrEmpty(phoneNumberTxt.Text))
                userDto.PhoneNumber = phoneNumberTxt.Text;
            else
            {
                notifierThis.ShowWarning("Telefon raqamini kiriting!");
                return;
            }

            if (!string.IsNullOrEmpty(passwordTxt.Text))
                userDto.Password = passwordTxt.Text;
            else
            {
                notifierThis.ShowWarning("Parolni kiriting!");
                return;
            }

            if(ageComboBox.SelectedItem is ComboBoxItem selectedAgeItem && selectedAgeItem.Content is int age)
                userDto.Age = age;
            else
            {
                notifierThis.ShowWarning("Yoshni tanlang!");
                return;
            }

            if (courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem && selectedCourseItem.Tag is long courseId)
                teacherDto.CourseId = courseId;
            else
            {
                notifierThis.ShowWarning("Kursni tanlang!");
                return;
            }

            if (!string.IsNullOrEmpty(skillTxt.Text)
                && skillTxt.Text.Split(',').All(skill => !string.IsNullOrWhiteSpace(skill)))
            {
                teacherDto.Skills = skillTxt.Text.Split(',').Select(skill => skill.Trim()).ToArray();
            }
            else
            {
                notifierThis.ShowWarning("O'qituvchi ko'nikmalari kiriting! har bir ko'nikmani vergul bilan ajrating.");
                return;
            }

            userDto.Role = Domain.Enums.UserRole.Teacher;

            var userResult = await Task.Run(async () => await _userService.RegisterAsync(userDto));

            if (userResult)
            {
                var teacherResult = await Task.Run(async () => await _teacherService.AddAsync(teacherDto));

                if (teacherResult)
                {
                    this.Close();
                    notifier.ShowSuccess("O'qituvchi muvaffaqiyatli qo'shildi!");
                }
                else
                {
                    notifierThis.ShowError("O'qituvchi qo'shishda xatolik yuz berdi!");
                    return;
                }
            }
            else
            {
                notifierThis.ShowError("O'qituvchi qo'shishda xatolik yuz berdi!");
                return;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi!");
        }
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        GetAllCourse();
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        saveBtn.IsEnabled = false;

        if(!saveBtn.IsEnabled)
            await SavedAsync();
        else
            notifierThis.ShowWarning("Iltimos, kuting!");
    }
}
