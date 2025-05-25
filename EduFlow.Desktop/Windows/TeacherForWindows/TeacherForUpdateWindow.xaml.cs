using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Integrated.Services.Users.User.Interfaces;
using EduFlow.Desktop.Integrated.Services.Users.User.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.TeacherForWindows;

/// <summary>
/// Interaction logic for TeacherForUpdateWindow.xaml
/// </summary>
public partial class TeacherForUpdateWindow : Window
{
    private readonly ITeacherService _service;
    private readonly ICourseService _courseService;
    private readonly IUserService _userService;
    private long Id { get; set; }
    private TeacherForResultDto Teacher { get; set; } = new TeacherForResultDto();
    public TeacherForUpdateWindow()
    {
        InitializeComponent();
        this._service = new TeacherService();
        this._courseService = new CourseService();
        this._userService = new UserService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 50,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    Notifier notifierThis = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 50,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    public void setId(long id)
        => this.Id = id;

    private async Task GetTeacher()
    {
        if (this.Id > 0)
        {
            var teacher = await _service.GetByIdAsync(this.Id);

            if(teacher is not null)
            {
                this.Teacher = teacher;
                fullNameTxt.Text = teacher.User.Firstname + " " + teacher.User.Lastname;
                phoneNumberTxt.Text = teacher.User.PhoneNumber;
                skillTxt.Text = string.Join(", ", teacher.Skills);
            }
        }
        else
            notifierThis.ShowError("Xatolik yuz berdi, iltimos qayta yuklang!");
    }

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Tag = course.Id,
                    Content = course.Name
                };
                courseComboBox.Items.Add(item);
            }
        }
        else
            notifierThis.ShowWarning("Kurslar topilmadi, iltimos qayta yuklang!");
    }

    private void Favourites()
    {
        if(this.Teacher is not null)
        {
            foreach(ComboBoxItem item in courseComboBox.Items)
                if(item.Tag is long courseId && courseId == this.Teacher.CourseId)
                {
                    courseComboBox.SelectedItem = item;
                    break;
                }

            foreach (ComboBoxItem item in ageComboBox.Items)
            {
                var age = this.Teacher.User.Age.ToString();

                if (item.Content?.ToString() == age)
                {
                    ageComboBox.SelectedItem = item;
                    break;
                }
            }
        }
        else
            notifierThis.ShowWarning("O'qituvchi malumotlari noto'g'ri, iltimos qayta yuklang!");
    }

    private async Task LoadedWindow()
    {
        await GetTeacher();
        await GetAllCourse();

        Favourites();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadedWindow();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void phoneNumberTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var regex = new System.Text.RegularExpressions.Regex(@"^\+998\d{0,9}$");
        e.Handled = !regex.IsMatch(futureText);
    }

    private bool SaveBtnIsEnable()
        => this.saveBtn.IsEnabled = true;

    private async Task SavedAsync()
    {
        try
        {
            TeacherForUpdateDto teacherDto = new TeacherForUpdateDto()
            {
                UserId = this.Teacher.UserId,
            };

            UserForUpdateDto userDto = new UserForUpdateDto()
            {
                Role = Domain.Enums.UserRole.Teacher
            };

            if(!string.IsNullOrEmpty(fullNameTxt.Text))
            {
                var firstName = fullNameTxt.Text.Split(' ')[0];
                var lastName = fullNameTxt.Text.Split(' ')[1];

                if(!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    userDto.Firstname = firstName;
                    userDto.Lastname = lastName;
                }
                else
                {
                    notifierThis.ShowWarning("Ism va familya to'g'ri kiritilmagan!");
                    fullNameTxt.Focus();
                    SaveBtnIsEnable();
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("Iltimos, ism va familyangizni kiriting!");
                fullNameTxt.Focus();
                SaveBtnIsEnable();
                return;
            }

            if (!string.IsNullOrEmpty(phoneNumberTxt.Text))
                userDto.PhoneNumber = phoneNumberTxt.Text;
            else
            {
                notifierThis.ShowWarning("Iltimos, telefon raqamingizni kiriting!");
                phoneNumberTxt.Focus();
                SaveBtnIsEnable();
                return;
            }

            if (ageComboBox.SelectedItem is ComboBoxItem ageComboBoxItem && int.TryParse(ageComboBoxItem.Content.ToString(), out int age))
                userDto.Age = age;
            else
            {
                notifierThis.ShowWarning("Iltimos, yoshingizni tanlang!");
                ageComboBox.Focus();
                SaveBtnIsEnable();
                return;
            }

            if (!string.IsNullOrEmpty(skillTxt.Text))
                teacherDto.Skills = skillTxt.Text.Split(',').Select(skill => skill.Trim()).ToArray();
            else
            {
                notifierThis.ShowWarning("O'qituvchi ko'nikmalari kiriting! har bir ko'nikmani vergul bilan ajrating.");
                skillTxt.Focus();
                SaveBtnIsEnable();
                return;
            }

            if (courseComboBox.SelectedItem is ComboBoxItem courseComboBoxItem && courseComboBoxItem.Tag is long courseId)
                teacherDto.CourseId = courseId;
            else
            {
                notifierThis.ShowWarning("Iltimos, kursni tanlang!");
                courseComboBox.Focus();
                SaveBtnIsEnable();
                return;
            }

            var userResult = await Task.Run(async () => await _userService.UpdateAsync(this.Teacher.UserId, userDto));

            if (userResult)
            {
                var teacherResult = await Task.Run(async () => await _service.UpdateAsync(this.Id, teacherDto));

                if(teacherResult)
                {
                    this.Close();
                    notifier.ShowSuccess("o'qituvchi malumotlari saqlandi!");
                }
                else
                {
                    notifierThis.ShowWarning("O'qituvchi malumotlari saqlanmadi!");
                    SaveBtnIsEnable();
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("O'qituvchi malumotlari saqlanmadi, iltimos qayta yuklang!");
                SaveBtnIsEnable();
                return;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi! Iltimos qayta urinib ko'ring.");
            SaveBtnIsEnable();
        }
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        saveBtn.IsEnabled = false;

        if (!saveBtn.IsEnabled)
            await SavedAsync();
        else
            notifierThis.ShowWarning("Iltimos, kuting!");
    }
}
