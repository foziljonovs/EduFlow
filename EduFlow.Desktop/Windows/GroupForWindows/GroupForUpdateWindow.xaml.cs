using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
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
        if (this.Id > 0)
        {
            var group = await _service.GetByIdAsync(this.Id);

            if (group is not null)
            {
                this.Group = group;
                nameTxt.Text = group.Name;
            }
            else
                notifierThis.ShowWarning("Malumotlar topilmadi, iltimos qayta yuklang!");
        }
        else
            notifierThis.ShowError("Xatolik yuz berdi!");
    }

    private async Task GetAllTeacher()
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

    private async Task GetAllTeacherByCourseId(long courseId)
    {
        var teachers = await Task.Run(async () => await _teacherService.GetAllByCourseIdAsync(courseId));

        if (teachers.Any())
        {
            teacherComboBox.Items.Clear();

            ComboBoxItem defaultItem = new ComboBoxItem
            {
                Content = "O'qituvchi tanlang",
                IsEnabled = false,
                IsSelected = true
            };

            teacherComboBox.Items.Add(defaultItem);

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

                courseComboBox.Items.Add(comboBoxItem);
            }
        }
        else
            notifierThis.ShowWarning("Kurslar topilmadi, iltimos qayta yuklang!");
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

            if(this.Id > 0)
            {
                var result = await _service.UpdateAsync(this.Id, dto);

                if(result)
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
                await GetAllTeacherByCourseId(courseId);
    }
}
