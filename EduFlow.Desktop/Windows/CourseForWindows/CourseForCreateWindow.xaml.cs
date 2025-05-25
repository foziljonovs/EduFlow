using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.CourseForWindows;
/// <summary>
/// Interaction logic for CourseForCreateWindow.xaml
/// </summary>
public partial class CourseForCreateWindow : Window
{
    private readonly ICourseService _courseService;
    private readonly ICategoryService _categoryService;
    public CourseForCreateWindow()
    {
        InitializeComponent();
        this._courseService = new CourseService();
        this._categoryService = new CategoryService();
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

    private async Task GetAllCategory()
    {
        var categories = await Task.Run(async () => await _categoryService.GetAllAsync());

        if (categories.Any())
        {
            foreach(var item in categories)
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
        {
            notifierThis.ShowWarning("Kategoriyalar topilmadi, iltimos qayta yuklang!");
        }
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void priceTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        e.Handled = !double.TryParse(futureText, System.Globalization.NumberStyles.Any, culture, out _);
    }

    private async Task SavedAsync()
    {
        try
        {
            var dto = new CourseForCreateDto();

            if (!string.IsNullOrEmpty(nameTxt.Text))
                dto.Name = nameTxt.Text;
            else
            {
                notifierThis.ShowWarning("Kurs nomini kiriting!");
                nameTxt.Focus();
                SaveBtn.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(priceTxt.Text))
                dto.Price = Convert.ToDouble(priceTxt.Text);
            else
            {
                notifierThis.ShowWarning("Kurs narxini kiriting!");
                priceTxt.Focus();
                SaveBtn.IsEnabled = true;
                return;
            }

            if (categoryComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is long categoryId)
                dto.CategoryId = categoryId;
            else
            {
                notifierThis.ShowWarning("Kurs kategoriyasini tanlang!");
                categoryComboBox.Focus();
                SaveBtn.IsEnabled = true;
                return;
            }

            if (termComboBox.SelectedItem is ComboBoxItem selectedTermItem)
                dto.Term = byte.Parse(selectedTermItem.Content.ToString());
            else
            {
                notifierThis.ShowWarning("Kurs muddatini tanlang!");
                termComboBox.Focus();
                SaveBtn.IsEnabled = true;
                return;
            }

            if (statusComboBox.SelectedItem is ComboBoxItem selectedStatusItem)
                dto.Archived = selectedStatusItem.Content.ToString() switch
                {
                    "Faol" => Domain.Enums.Status.Active,
                    "Saqlangan" => Domain.Enums.Status.Archived,
                    _ => Domain.Enums.Status.Archived
                };
            else
            {
                notifierThis.ShowWarning("Kurs holatini tanlang!");
                statusComboBox.Focus();
                SaveBtn.IsEnabled = true;
                return;
            }

            var result = await _courseService.AddAsync(dto);

            if (result)
            {
                this.Close();
                notifier.ShowSuccess("Kurs muvaffaqiyatli saqlandi!");
            }
            else
            {
                notifierThis.ShowError("Kurs saqlashda xatolik yuz berdi!");
                SaveBtn.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi!");
            SaveBtn.IsEnabled = true;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        GetAllCategory();
    }

    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!SaveBtn.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos, kuting!");
            return;
        }

        SaveBtn.IsEnabled = false;

        try
        {
            await SavedAsync();
        }
        finally
        {
            SaveBtn.IsEnabled = true;
        }
    }
}
