using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.CategoryForWindows;

/// <summary>
/// Interaction logic for CategoryForCreateWindow.xaml
/// </summary>
public partial class CategoryForCreateWindow : Window
{
    private readonly ICategoryService _service;
    public CategoryForCreateWindow()
    {
        InitializeComponent();
        this._service = new CategoryService();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task SavedAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(nameTxt.Text))
            {
                notifierThis.ShowWarning("Kategoriya nomi kiriting!");
                return;
            }

            var dto = new CategoryForCraeteDto
            {
                Name = nameTxt.Text
            };

            var result = await _service.AddAsync(dto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess("Kategoriya muvaffaqiyatli saqlandi!");
            }
            else
            {
                notifierThis.ShowError("Saqlashda xatolik yuz berdi!");
            }
        }
        catch (Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi!");
        }
    }

    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        SaveBtn.IsEnabled = false;

        if (!SaveBtn.IsEnabled)
            await SavedAsync();
        else
            notifierThis.ShowWarning("Iltimos, kuting!");
    }
}
