using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.Desktop.Integrated.Services.Courses.Lesson;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.LessonForWindows;

/// <summary>
/// Interaction logic for LessonForUpdateWindow.xaml
/// </summary>
public partial class LessonForUpdateWindow : Window
{
    private readonly ILessonService _lessonService;
    private long _lessonId { get; set; }
    private LessonForResultDto _lesson = new LessonForResultDto();
    public LessonForUpdateWindow()
    {
        InitializeComponent();
        this._lessonService = new LessonService();
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

    public void SetId(long id)
        => this._lessonId = id;

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task GetLesson()
    {
        try
        {
            var lesson = await Task.Run(async () => await _lessonService.GetByIdAsync(this._lessonId));

            ShowValues(lesson);
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Dars malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta yuklang!");
        }
    }

    private void ShowValues(LessonForResultDto lesson)
    {
        if(lesson is not null)
        {
            this._lesson = lesson;
            dtDateTime.SelectedDate = lesson.Date;
        }
        else
        {
            notifierThis.ShowWarning("Dars malumotlari topilmadi, Iltimos qayta yuklang!");
        }
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!saveBtn.IsEnabled)
            {
                notifierThis.ShowWarning("Iltimos, kuting!");
                return;
            }

            saveBtn.IsEnabled = false;

            if (dtDateTime.SelectedDate is null ||
                dtDateTime.SelectedDate.Value.Date == _lesson.Date.Date)
            {
                notifierThis.ShowWarning("Dars sanasi o'zgarmagan yoki bo'sh, Iltimos tekshiring!");
                saveBtn.IsEnabled = true;
                return;
            }

            LessonForUpdateDto dto = new LessonForUpdateDto
            {
                Name = _lesson.Name,
                LessonNumber = _lesson.LessonNumber,
                GroupId = _lesson.GroupId,
                Date = dtDateTime.SelectedDate.Value.Date
            };

            var res = await _lessonService.UpdateAsync(_lessonId, dto);

            if (res)
            {
                this.Close();
                notifier.ShowSuccess("Dars malumotlari o'zgartirildi!");
            }
            else
            {
                notifierThis.ShowWarning("Dars malumotlarini o'zgartirib bo'lmadi, Iltimos qayta urining!");
                saveBtn.IsEnabled = true;
                return;
            }
        }
        catch(Exception ex)
        {
            saveBtn.IsEnabled = true;
            notifierThis.ShowWarning("Malumotlarni saqlashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await GetLesson();
    }
}
