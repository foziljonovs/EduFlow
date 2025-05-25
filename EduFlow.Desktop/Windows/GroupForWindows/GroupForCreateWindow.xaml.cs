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
/// Interaction logic for GroupForCreateWindow.xaml
/// </summary>
public partial class GroupForCreateWindow : Window
{
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    public GroupForCreateWindow()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
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

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                var item = new ComboBoxItem();
                item.Content = course.Name;
                item.Tag = course.Id;
                this.courseComboBox.Items.Add(item);
            }
        }
    }

    private async Task GetAllTeacher()
    {
        var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

        if (teachers.Any())
        {
            foreach (var teacher in teachers)
            {
                var item = new ComboBoxItem();
                item.Content = teacher.User.Firstname + " " + teacher.User.Lastname;
                item.Tag = teacher.Id;
                this.teacherComboBox.Items.Add(item);
            }
        }
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

    private async Task SavedAsync()
    {
        try
        {
            GroupForCreateDto dto = new GroupForCreateDto();

            if(!string.IsNullOrEmpty(nameTxt.Text))
                dto.Name = nameTxt.Text;
            else
            {
                notifierThis.ShowWarning("Guruh nomi kiritilmadi!");
                nameTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if(courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem && selectedCourseItem.Tag is long courseId)
                dto.CourseId = courseId;
            else
            {
                notifierThis.ShowWarning("Kurs tanlanmadi!");
                courseComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem && selectedTeacherItem.Tag is long teacherId)
                dto.TeacherId = teacherId;
            else
            {
                notifierThis.ShowWarning("O'qituvchi tanlanmadi!");
                teacherComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            var result = await _groupService.AddAsync(dto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess("Guruh muvaffaqiyatli saqlandi!");
            }
            else
            {
                notifierThis.ShowError("Guruhni saqlashda xatolik yuz berdi!");
                saveBtn.IsEnabled = true;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi!");
            saveBtn.IsEnabled = true;
        }
    }

    private async void LoadedWindow()
    {
        await GetAllCourse();
        await GetAllTeacher();   
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
            if(courseComboBox.SelectedItem is ComboBoxItem selectedItem &&
                selectedItem.Tag is long courseId)
                await GetAllTeacherByCourseId(courseId);
    }
}
