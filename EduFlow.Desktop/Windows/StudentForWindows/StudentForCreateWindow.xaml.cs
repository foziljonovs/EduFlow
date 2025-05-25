using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.StudentCourse;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.StudentForWindows;

/// <summary>
/// Interaction logic for StudentForCreateWindow.xaml
/// </summary>
public partial class StudentForCreateWindow : Window
{
    private readonly IStudentService _studentService;
    private readonly IStudentCourseService _studentCourseService;
    private readonly ICourseService _courseService;
    public StudentForCreateWindow()
    {
        InitializeComponent();
        this._studentService = new StudentService();
        this._studentCourseService = new StudentCourseService();
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

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            CourseComboBox.Items.Clear();

            ComboBoxItem defaultItem = new ComboBoxItem
            {
                Content = "Kursni tanlang",
                IsSelected = true,
                IsEnabled = false
            };

            CourseComboBox.Items.Add(defaultItem);

            foreach (var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = course.Name,
                    Tag = course.Id
                };

                CourseComboBox.Items.Add(item);
            }
        }
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task SavedAsync()
    {
        try
        {
            StudentForCreateDto dto = new StudentForCreateDto();

            if (!string.IsNullOrEmpty(fullNameTxt.Text))
                dto.Fullname = fullNameTxt.Text;
            else
            {
                notifierThis.ShowWarning("Ism va familiya kiritilmadi!");
                fullNameTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(addressTxt.Text))
                dto.Address = addressTxt.Text;
            else
            {
                notifierThis.ShowWarning("Manzil kiritilmadi!");
                addressTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(phoneNumberTxt.Text) && phoneNumberTxt.Text.Length == 13)
                dto.PhoneNumber = phoneNumberTxt.Text;
            else
            {
                notifierThis.ShowWarning("Telefon raqami kiritilmadi yoki no'to'gri formatda!");
                phoneNumberTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (AgeComboBox.SelectedItem is ComboBoxItem selectedAgeItem && int.TryParse(selectedAgeItem.Content.ToString(), out int age))
                dto.Age = age;
            else
            {
                notifierThis.ShowWarning("Yosh kiritilmadi!");
                AgeComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            var studentResult = await _studentService.AddAndReturnIdAsync(dto);

            if (studentResult == 0)
            {
                notifierThis.ShowWarning("O'quvchini saqlashda xatolik yuz berdi!");
                saveBtn.IsEnabled = true;
                return;
            }

            StudentCourseForCreateDto studentCourseDto = new StudentCourseForCreateDto();

            if (CourseComboBox.SelectedItem is ComboBoxItem selectedCourseItem && long.TryParse(selectedCourseItem.Tag.ToString(), out long courseId))
                studentCourseDto.CourseId = courseId;
            else
            {
                notifierThis.ShowWarning("Kurs topilmadi!");
                CourseComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            studentCourseDto.StudentId = studentResult;

            var result = await _studentCourseService.AddAsync(studentCourseDto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess("O'quvchi muvaffaqiyatli saqlandi!");
            }
            else
            {
                notifierThis.ShowError("O'quvchini saqlashda xatolik yuz berdi!");
                saveBtn.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi!");
        }
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!saveBtn.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos, kuting!");
            return;
        }

        saveBtn.IsEnabled = false;

        try
        {
            await SavedAsync();
        }
        finally
        {
            saveBtn.IsEnabled = true;
        }
    }

    private void phoneNumberTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;

        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var regex = new System.Text.RegularExpressions.Regex(@"^\+998\d{0,9}$");
        e.Handled = !regex.IsMatch(futureText);
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetAllCourse();
    }
}
