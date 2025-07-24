using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.StudentCourse;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
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
    private readonly IStudentCourseService _studentCourseService;
    private long Id { get; set; }
    private StudentForResultDto Student { get; set; } = new StudentForResultDto();
    public StudentForUpdateWindow()
    {
        InitializeComponent();
        this._service = new StudentService();
        this._courseService = new CourseService();
        this._studentCourseService = new StudentCourseService();
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

            if(IdentitySingelton.GetInstance().Role is Domain.Enums.UserRole.Teacher)
                CourseComboBox.IsEnabled = false;
        }
        else
            notifierThis.ShowWarning("Kurs malumotlari topilmadi, iltimos qayta yuklang!");
    }

    private void Favourites()
    {
        if(this.Student is not null)
        {
            foreach (ComboBoxItem item in AgeComboBox.Items)
            {
                var studentAge = this.Student.Age.ToString();

                if (item.Content?.ToString() == studentAge)
                {
                    AgeComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in CourseComboBox.Items)
            {
                var studentActiveCourse = this.Student.StudentCourses?
                    .FirstOrDefault(x => x.Status == Domain.Enums.EnrollmentStatus.Active
                                    || x.Status == Domain.Enums.EnrollmentStatus.Pending);

                if (studentActiveCourse is not null)
                {
                    if (item.Tag is long courseId && studentActiveCourse.CourseId == courseId)
                    {
                        CourseComboBox.SelectedItem = item;
                        break;
                    }
                }
                else
                {
                    ComboBoxItem defaultItem = new ComboBoxItem
                    {
                        Content = "Kursni tanlang",
                        IsEnabled = false,
                        IsSelected = true
                    };

                    CourseComboBox.Items.Add(defaultItem);
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

    private async Task SavedAsync()
    {
        try
        {
            StudentForUpdateDto dto = new StudentForUpdateDto();

            if (!string.IsNullOrEmpty(fullNameTxt.Text))
                dto.Fullname = fullNameTxt.Text;
            else
            {
                notifierThis.ShowWarning("Iltimos, to'liq ismingizni kiriting!");
                fullNameTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(addressTxt.Text))
                dto.Address = addressTxt.Text;
            else
            {
                notifierThis.ShowWarning("Iltimos, manzilingizni kiriting!");
                addressTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(phoneNumberTxt.Text) && phoneNumberTxt.Text.Length == 13)
                dto.PhoneNumber = phoneNumberTxt.Text;
            else
            {
                notifierThis.ShowWarning("Iltimos, telefon raqamingizni kiriting!");
                phoneNumberTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if(AgeComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int age))
                dto.Age = age;
            else
            {
                notifierThis.ShowWarning("Iltimos, yoshingizni tanlang!");
                AgeComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            var studentResult = await _service.UpdateAsync(this.Id, dto);

            if (studentResult)
            {
                StudentCourseForUpdateDto studentCourseDto = new StudentCourseForUpdateDto()
                {
                    StudentId = this.Id
                };

                if (CourseComboBox.SelectedItem is ComboBoxItem selectedCourseItem && long.TryParse(selectedCourseItem.Tag.ToString(), out long courseId))
                {
                    studentCourseDto.CourseId = courseId;

                    var updateStudentCourse = this.Student.StudentCourses.FirstOrDefault(x => x.CourseId == courseId
                                                                                      && x.Status == Domain.Enums.EnrollmentStatus.Active
                                                                                      || x.Status == Domain.Enums.EnrollmentStatus.Pending);

                    if(updateStudentCourse is not null)
                    {
                        notifierThis.ShowWarning("Talaba guruhda o'qimoqda, iltimos qayta tekshiring!");
                        saveBtn.IsEnabled = true;
                        return;
                    }

                    StudentCourseForCreateDto studentCourseForCreateDto = new StudentCourseForCreateDto
                    {
                        StudentId = this.Id,
                        CourseId = courseId
                    };

                    if (hourCombobox.SelectedItem is ComboBoxItem selectedHourItem &&
                        minuteCombobox.SelectedItem is ComboBoxItem selectedMinuteItem)
                    {
                        string selectedHourString = selectedHourItem.Content.ToString();
                        string selectedMinuteString = selectedMinuteItem.Content.ToString();

                        if (int.TryParse(selectedHourString, out int hour) &&
                            int.TryParse(selectedMinuteString, out int minute))
                        {
                            TimeOnly time = new TimeOnly(hour, minute);
                            studentCourseDto.PreferredTime = time;
                        }
                        else
                        {
                            notifierThis.ShowWarning("Iltimos, o'quv vaqti tanlanganligini tekshiring!");
                            saveBtn.IsEnabled = true;
                            return;
                        }
                    }
                    else
                    {
                        notifierThis.ShowWarning("Iltimos, o'quv vaqti tanlanganligini tekshiring!");
                        saveBtn.IsEnabled = true;
                        return;
                    }

                    if (dayCombobox.SelectedItem is ComboBoxItem selectedDay)
                        studentCourseDto.PreferredDay = selectedDay.Tag.ToString() switch
                        {
                            "0" => Day.None,
                            "1" => Day.ToqKunlar,
                            "2" => Day.JuftKunlar
                        };

                    var studentCourseResult = await _studentCourseService.AddAsync(studentCourseForCreateDto);

                    if(studentCourseResult)
                    {
                        this.Close();
                        notifier.ShowSuccess("Talaba malumotlari muvaffaqiyatli yangilandi!");
                    }
                    else
                    {
                        notifierThis.ShowWarning("Talaba kursini yangilashda xatolik yuz berdi!");
                        saveBtn.IsEnabled = true;
                        return;
                    }
                }
                else
                {
                    notifierThis.ShowWarning("Iltimos, kursni tanlang!");
                    CourseComboBox.Focus();
                    saveBtn.IsEnabled = true;
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("Talaba malumotlarini yangilashda xatolik yuz berdi!");
                saveBtn.IsEnabled = true;
                return;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi, iltimos qayta urinib ko'ring!");
            return;
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
}
