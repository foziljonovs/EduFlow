using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.CourseForWindows;

/// <summary>
/// Interaction logic for CourseForUpdateWindow.xaml
/// </summary>
public partial class CourseForUpdateWindow : Window
{
    private readonly ICourseService _service;
    private readonly ICategoryService _categoryService;
    private long Id { get; set; }
    private CourseForResultDto Course { get; set; } = new CourseForResultDto();
    public CourseForUpdateWindow()
    {
        InitializeComponent();
        this._service = new CourseService();
        this._categoryService = new CategoryService();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    public void setId(long id)
        => this.Id = id;

    private async Task GetAllCategory()
    {
        var categories = await Task.Run(async () => await _categoryService.GetAllAsync());

        if (categories.Any())
        {
            foreach (var item in categories)
            {
                var categoryComboBoxItem = new ComboBoxItem
                {
                    Content = item.Name,
                    Tag = item.Id
                };

                categoryComboBox.Items.Add(categoryComboBoxItem);
            }
        }
        else
            notifier.ShowWarning("Kategoriyalar topilmadi, iltimos qayta yuklang!");
    }

    private async Task GetCourse()
    {
        if(Id > 0)
        {
            var course = await _service.GetByIdAsync(Id);

            if (course is not null)
            {
                this.Course = course;
                nameTxt.Text = course.Name;
                priceTxt.Text = course.Price.ToString();

                foreach(ComboBoxItem item in termComboBox.Items)
                {
                    if (item.Content.ToString() == course.Term.ToString())
                    {
                        termComboBox.SelectedItem = item;
                        break;
                    }
                }

                switch (course.Archived)
                {
                    case Domain.Enums.Status.Active:
                        statusComboBox.SelectedItem = statusComboBox.Items[0];
                        break;

                    case Domain.Enums.Status.Archived:
                        statusComboBox.SelectedItem = statusComboBox.Items[1];
                        break;
                }

                foreach(ComboBoxItem item in categoryComboBox.Items)
                {
                    if(item.Tag is long categoryId && categoryId == course.CategoryId)
                    {
                        categoryComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
                notifier.ShowWarning("Malumotlarni topilmadi, iltimos qayta yuklang!");
        }
        else
            notifier.ShowError("Xatolik yuz berdi!");
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        GetAllCategory();
        GetCourse();
    }
}
