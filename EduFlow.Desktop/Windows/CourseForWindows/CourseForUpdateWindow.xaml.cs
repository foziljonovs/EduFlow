using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Category;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Domain.Entities.Courses;
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
            notifierThis.ShowWarning("Kategoriyalar topilmadi, iltimos qayta yuklang!");
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
            }
            else
                notifierThis.ShowWarning("Malumotlarni topilmadi, iltimos qayta yuklang!");
        }
        else
            notifierThis.ShowError("Xatolik yuz berdi!");
    }

    private void Favourites()
    {
        if(this.Course is not null)
        {
            foreach (ComboBoxItem item in termComboBox.Items)
                if (item.Content.ToString() == this.Course.Term.ToString())
                {
                    termComboBox.SelectedItem = item;
                    break;
                }

            foreach (ComboBoxItem item in categoryComboBox.Items)
                if (item.Tag is long categoryId && categoryId == this.Course.CategoryId)
                {
                    categoryComboBox.SelectedItem = item;
                    break;
                }

            switch (this.Course.Archived)
            {
                case Domain.Enums.Status.Active:
                    statusComboBox.SelectedItem = statusComboBox.Items[0];
                    break;

                case Domain.Enums.Status.Archived:
                    statusComboBox.SelectedItem = statusComboBox.Items[1];
                    break;
            }
        }
        else
            notifierThis.ShowWarning("Kurs malumotlari noto'g'ri, iltimos qayta yuklang!");
    }

    private async void LoadedWindow()
    {
        await GetAllCategory();
        await GetCourse();

        Favourites();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadedWindow();
    }

    private async Task SavedAsync()
    {
        try
        {
            var dto = new CourseForUpdateDto();

            if(!string.IsNullOrEmpty(nameTxt.Text))
                dto.Name = nameTxt.Text;
            else
            {
                notifierThis.ShowWarning("Iltimos kurs nomini kiriting!");
                nameTxt.Focus();
                SaveButonIsEnable();
                return;
            }

            if(!string.IsNullOrEmpty(priceTxt.Text) && double.TryParse(priceTxt.Text, out double price))
            {
                if(price <= 0)
                {
                    notifierThis.ShowWarning("Kurs narxi 0 dan baland bo'lishi kerak!");
                    priceTxt.Focus();
                    SaveButonIsEnable();
                    return;
                }

                dto.Price = price;
            }
            else
            {
                notifierThis.ShowWarning("Iltimos kurs narxini kiriting!");
                priceTxt.Focus();
                SaveButonIsEnable();
                return;
            }

            if (termComboBox.SelectedItem is ComboBoxItem termItem)
                dto.Term = byte.Parse(termItem.Content.ToString());
            else
            {
                notifierThis.ShowWarning("Iltimos kurs muddatini tanlang!");
                termComboBox.Focus();
                SaveButonIsEnable();
                return;
            }

            if (categoryComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is long categoryId)
                dto.CategoryId = categoryId;
            else
            {
                notifierThis.ShowWarning("Iltimos kurs kategoriyasini tanlang!");
                categoryComboBox.Focus();
                SaveButonIsEnable();
                return;
            }

            if(statusComboBox.SelectedItem is ComboBoxItem statusItem)
            {
                dto.Archived = statusItem.Content.ToString() switch
                {
                    "Faol" => Domain.Enums.Status.Active,
                    "Arxivlangan" => Domain.Enums.Status.Archived,
                    _ => Domain.Enums.Status.Active
                };
            }
            else
            {
                notifierThis.ShowWarning("Iltimos kurs statusini tanlang!");
                statusComboBox.Focus();
                SaveButonIsEnable();
                return;
            }

            if(this.Id > 0)
            {
                var result = await _service.UpdateAsync(this.Id, dto);

                if (result)
                {
                    this.Close();
                    notifier.ShowInformation("Kurs malumotlari saqlandi!");
                }
                else
                {
                    notifierThis.ShowWarning("Kurs malumotlari saqlanmadi!");
                    SaveButonIsEnable();
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("Kurs malumotlari noto'g'ri, iltimos qayta yuklang!");
                SaveButonIsEnable();
                return;
            }
        }
        catch(Exception ex)
        {
            notifier.ShowError("Xatolik yuz berdi!");
            SaveButonIsEnable();
        }
    }

    private bool SaveButonIsEnable()
        => this.SaveBtn.IsEnabled = true;
    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
         SaveBtn.IsEnabled = false;

        if (!SaveBtn.IsEnabled)
            await SavedAsync();
        else
            notifierThis.ShowWarning("Iltimos, kuting!");
    }
}
