using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows.Controls;
using System.Windows;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.TeacherForComponents;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using EduFlow.Desktop.Windows.TeacherForWindows;

namespace EduFlow.Desktop.Pages.TeacherForPages;

/// <summary>
/// Interaction logic for TeacherPage.xaml
/// </summary>
public partial class TeacherPage : Page
{
    private readonly ITeacherService _teacherService;
    private readonly ICourseService _courseService;
    public TeacherPage()
    {
        InitializeComponent();
        this._teacherService = new TeacherService();
        this._courseService = new CourseService();
    }

    Notifier notifier = new Notifier(cfg =>
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
        if (courses != null)
        {
            foreach (var item in courses)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Tag = item.Id;
                comboBoxItem.Content = item.Name;
                courseComboBox.Items.Add(comboBoxItem);
            }
        }
        else
        {
            notifier.ShowWarning("Kurslar topilmadi!");
        }
    }

    private async Task GetAllTeacher()
    {
        teacherLoader.Visibility = Visibility.Visible;
        var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

        ShowTeachers(teachers);
    }

    private async Task GetAllByCourseId()
    {
        teacherLoader.Visibility = Visibility.Visible;
        var courseId = (long)((ComboBoxItem)courseComboBox.SelectedItem).Tag;

        //var teachers = await Task.Run(async () => await _teacherService.get(courseId));
        //ShowTeachers(teachers);
    }

    private void ShowTeachers(List<TeacherForResultDto> teachers)
    {
        int count = 1;
        stTeachers.Children.Clear();

        if (teachers.Any())
        {
            teacherLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Collapsed;

            foreach(var item in teachers)
            {
                TeacherForComponent component = new TeacherForComponent();
                component.Tag = item;
                component.setValues(
                    item.Id,
                    count,
                    item.User.Firstname,
                    item.Course.Name,
                    item.User.PhoneNumber,
                    item.Groups.Count,
                    item.Skills);

                component.OnDeleteTeacher += GetAllTeacher;
                stTeachers.Children.Add(component);
                count++;
            }
        }
        else
        {
            teacherLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Visible;
        }
    }

    private void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private async Task PageLoaded()
    {
        await GetAllTeacher();
        await GetAllCourse();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        PageLoaded();
    }

    private async void craeteTeacherBtn_Click(object sender, RoutedEventArgs e)
    {
        TeacherForCreateWindow window = new TeacherForCreateWindow();
        window.ShowDialog();
        await GetAllTeacher();
    }
}
