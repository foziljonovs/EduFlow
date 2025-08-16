using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
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
/// Interaction logic for StudentForCreateByTeacherWindow.xaml
/// </summary>
public partial class StudentForCreateByTeacherWindow : Window
{
    private readonly IGroupService _groupService;
    private readonly IStudentService _studentService;
    private readonly IStudentCourseService _studentCourseService;
    private long GroupId { get; set; }
    private long CompanyId { get; set; }
    private GroupForResultDto Group { get; set; }
    public StudentForCreateByTeacherWindow()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._studentService = new StudentService();
        this._studentCourseService = new StudentCourseService();
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

    public void SetGroupId(long groupId)
        => this.GroupId = groupId;

    private async Task GetGroup()
    {
        try
        {
            var group = await _groupService.GetByIdAsync(this.GroupId);

            if(group is null)
            {
                notifierThis.ShowWarning("Guruh malumotlari topilmadi, iltimos qayta urinib ko'ring!");
                return;
            }

            this.Group = group;
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Xatolik yuz berdi, iltimos qayta urinib ko'ring!");
        }
    }

    private void phoneNumberTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;

        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var regex = new System.Text.RegularExpressions.Regex(@"^\+998\d{0,9}$");
        e.Handled = !regex.IsMatch(futureText);
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.CompanyId = IdentitySingelton.GetInstance().CompanyId;

        await GetGroup();
    }

    private async Task SavedAsync()
    {
        try
        {
            StudentForCreateDto dto = new StudentForCreateDto
            {
                CompanyId = this.CompanyId
            };

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
                notifierThis.ShowWarning("O'quvchini saqlab bo'lmadi, iltimos qayta urining!");
                saveBtn.IsEnabled = true;
                return;
            }

            StudentCourseForCreateDto studentCourseDto = new StudentCourseForCreateDto
            {
                CourseId = this.Group.CourseId,
                StudentId = studentResult,
                PreferredTime = this.Group.PreferredTime.HasValue
                    ? TimeOnly.FromTimeSpan(this.Group.PreferredTime.Value)
                    : null,
                PreferredDay = this.Group.PreferredDay
            };

            var studentCourseResult = await _studentCourseService.AddAsync(studentCourseDto);

            if (studentCourseResult)
            {
                List<long> studentIds = new List<long>{ studentResult };
                var addedGroupResult = await _groupService.AddStudentsToGroupAsync(this.GroupId, studentIds);

                if (addedGroupResult)
                {
                    this.Close();
                    notifier.ShowSuccess("O'quvchi guruhga muvaffaqiyatli qo'shildi!");
                }
                else
                {
                    notifierThis.ShowError("O'quvchini guruhga qo'shib bo'lmadi, iltimos qayta urining!");
                    saveBtn.IsEnabled = true;
                }
            }
            else
            {
                notifierThis.ShowWarning("O'quvchi malumotini saqlab bo'lmadi, iltimos qayta urining!");
                saveBtn.IsEnabled = true;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Xatolik yuz berdi!");
        }
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!saveBtn.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos kuting!");
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
}
