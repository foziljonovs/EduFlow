using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForUpdateWindow.xaml
/// </summary>
public partial class GroupForUpdateWindow : Window
{
    private readonly IGroupService _service;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private long Id { get; set; }
    private GroupForResultDto Group {  get; set; } = new GroupForResultDto();
    public GroupForUpdateWindow()
    {
        InitializeComponent();
        this._service = new GroupService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
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

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    public void setId(long id)
        => this.Id = id;

    private async Task GetGroup()
    {
        try
        {
            if (this.Id > 0)
            {
                var group = await _service.GetByIdAsync(this.Id);

                if (group is not null)
                {
                    this.Group = group;
                    nameTxt.Text = group.Name;
                    statusComboBox.SelectedIndex = group.IsStatus switch 
                    {
                        Domain.Enums.Status.Active => 0,
                        Domain.Enums.Status.Archived => 1,
                        Domain.Enums.Status.Graduated => 2
                    };
                }
                else
                    notifierThis.ShowWarning("Malumotlar topilmadi, iltimos qayta yuklang!");
            }
            else
                notifierThis.ShowError("Xatolik yuz berdi!");
        }
        catch (Exception ex)
        {
            notifierThis.ShowError("Guruh malumotlarini yuklashda xatolik yuz berdi! Iltimos qayta urinib ko'ring!");
        }
    }

    private async Task GetAllTeacher()
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

            if (teachers.Any())
            {
                foreach(var item in teachers)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem
                    {
                        Tag = item.Id,
                        Content = item.User.Firstname + " " + item.User.Lastname
                    };

                    teacherComboBox.Items.Add(comboBoxItem);
                }
            }
            else
                notifierThis.ShowWarning("O'qituvchilar topilmadi, iltimos qayta yuklang!");
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'qituvchi malumotini yuklashda xatolik yuz berdi! Iltimos qayta urinib ko'ring!");
        }
    }

    private async Task GetAllTeacherByCourseId(long courseId)
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllByCourseIdAsync(courseId));

            if (teachers.Any())
            {
                teacherComboBox.Items.Clear();

                foreach (var teacher in teachers)
                {
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Tag = teacher.Id,
                        Content = teacher.User.Firstname + " " + teacher.User.Lastname
                    };

                    teacherComboBox.Items.Add(item);
                }
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'qituvchilarni yuklashda xatolik yuz berdi! Iltimos qayta urinib ko'ring!");
        }
    }

    private async Task GetAllCourse()
    {
        try
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

                    courseComboBox.Items.Add(comboBoxItem);
                }
            }
            else
                notifierThis.ShowWarning("Kurslar topilmadi, iltimos qayta yuklang!");
        }
        catch(Exception ex)
        {
             notifierThis.ShowError("Kurslarni yuklashda xatolik yuz berdi! Iltimos qayta urinib ko'ring!");
        }
    }

    private void Favourites()
    {
        if(this.Group is not null)
        {
            foreach(ComboBoxItem teacherComboBoxItem in teacherComboBox.Items)
                if(teacherComboBoxItem.Tag is long teacherId && teacherId == this.Group.TeacherId)
                {
                    teacherComboBox.SelectedItem = teacherComboBoxItem;
                    break;
                }

            foreach(ComboBoxItem courseComboBoxItem in courseComboBox.Items)
                if(courseComboBoxItem.Tag is long courseId && courseId == this.Group.CourseId)
                {
                    courseComboBox.SelectedItem = courseComboBoxItem;
                    break;
                }

            int hour = this.Group.PreferredTime.Value.Hours;
            int minute = this.Group.PreferredTime.Value.Minutes;

            foreach (ComboBoxItem hourItem in hourCombobox.Items)
                if (hourItem.Tag is string hourTag && int.TryParse(hourTag, out int hourValue) && hourValue == hour)
                {
                    hourCombobox.SelectedItem = hourItem;
                    break;
                }

            foreach (ComboBoxItem minuteItem in minuteCombobox.Items)
                if (minuteItem.Tag is string minuteTag && int.TryParse(minuteTag, out int minuteValue) && minuteValue == minute)
                {
                    minuteCombobox.SelectedItem = minuteItem;
                    break;
                }

            string preferredDayTag = this.Group.PreferredDay switch
            {
                Day.None => "0",
                Day.ToqKunlar => "1",
                Day.JuftKunlar => "2",
                _ => "0"
            };

            foreach (ComboBoxItem item in dayCombobox.Items)
                if (item.Tag?.ToString() == preferredDayTag)
                {
                    dayCombobox.SelectedItem = item;
                    break;
                }


            if (IdentitySingelton.GetInstance().Role is Domain.Enums.UserRole.Teacher)
                courseComboBox.IsEnabled = false;
        }
        else
            notifierThis.ShowWarning("Guruh ma'lumotlari noto'g'ri, iltimos qayta yuklang!");
    }

    private async void LoadedWindow()
    {
        await GetGroup();
        await GetAllTeacher();
        await GetAllCourse();

        Favourites();
    }

    private bool IsLoaded = false;
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (!IsLoaded)
        {
            this.IsLoaded = true;
            LoadedWindow();
        }
    }

    private async Task SavedAsync()
    {
        try
        {
            GroupForUpdateDto dto = new GroupForUpdateDto();

            if(!string.IsNullOrWhiteSpace(nameTxt.Text))
                dto.Name = nameTxt.Text;
            else
            {
                notifierThis.ShowWarning("Iltimos, guruh nomini kiriting!");
                nameTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if(teacherComboBox.SelectedItem is ComboBoxItem teacherComboBoxItem && teacherComboBoxItem.Tag is long teacherId)
                dto.TeacherId = teacherId;
            else
            {
                notifierThis.ShowWarning("Iltimos, o'qituvchini tanlang!");
                teacherComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (courseComboBox.SelectedItem is ComboBoxItem courseComboBoxItem && courseComboBoxItem.Tag is long courseId)
                dto.CourseId = courseId;
            else
            {
                notifierThis.ShowWarning("Iltimos, kursni tanlang!");
                courseComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if(statusComboBox.SelectedItem is ComboBoxItem statusComboBoxItem)
            {
                dto.IsStatus = statusComboBoxItem.Content.ToString() switch
                {
                    "Faol" => Domain.Enums.Status.Active,
                    "Saqlangan" => Domain.Enums.Status.Archived,
                    "Yakunlangan" => Domain.Enums.Status.Graduated,
                    _ => Domain.Enums.Status.Active
                };
            }
            else
            {
                notifierThis.ShowWarning("Iltimos, holatni tanlang!");
                statusComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (hourCombobox.SelectedItem is ComboBoxItem selectedHourItem &&
                minuteCombobox.SelectedItem is ComboBoxItem selectedMinuteItem)
            {
                string selectedHourString = selectedHourItem.Content.ToString();
                string selectedMinuteString = selectedMinuteItem.Content.ToString();

                if (int.TryParse(selectedHourString, out int hour) &&
                    int.TryParse(selectedMinuteString, out int minute))
                    dto.PreferredTime = new TimeSpan(hour, minute, 0);
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
                dto.PreferredDay = selectedDay.Tag.ToString() switch
                {
                    "0" => Day.None,
                    "1" => Day.ToqKunlar,
                    "2" => Day.JuftKunlar
                };

            if (this.Id > 0)
            {
                var result = await _service.UpdateAsync(this.Id, dto);

                if (result)
                {
                    this.Close();
                    notifier.ShowSuccess("Guruh malumotlari saqlandi!");
                }
                else
                {
                    notifierThis.ShowWarning("Guruhni malumotlari saqlanmadi!");
                    saveBtn.IsEnabled = true;
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("Guruh malumotlari noto'g'ri, iltimos qayta yuklang!");
                saveBtn.IsEnabled = true;
                return;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi! Iltimos qayta urinib ko'ring.");
            saveBtn.IsEnabled = true;
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

    private async void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(IsLoaded)
            if(courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem
                && selectedCourseItem.Tag is long courseId)
            {
                await GetAllTeacherByCourseId(courseId);

                Favourites();
            }
    }
}
