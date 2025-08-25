using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Services.Users.User.Interfaces;
using EduFlow.Desktop.Integrated.Services.Users.User.Services;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Management.Windows.EmployeeForWindows;

/// <summary>
/// Interaction logic for CreateForEmployeeWindow.xaml
/// </summary>
public partial class CreateForEmployeeWindow : Window
{
    private readonly IUserService _userService;
    private long CompanyId { get; set; }
    public CreateForEmployeeWindow()
    {
        InitializeComponent();
        this._userService = new UserService();
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

    public void SetCompanyId(long companyId)
        => this.CompanyId = companyId;

    private void phoneNumberTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;

        string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var regex = new System.Text.RegularExpressions.Regex(@"^\+998\d{0,9}$");
        e.Handled = !regex.IsMatch(futureText);
    }

    private async Task SavedAsync()
    {
        try
        {
            UserForCreateDto dto = new UserForCreateDto
            {
                CompanyId = this.CompanyId
            };

            if(!string.IsNullOrEmpty(txtFirstName.Text))
                dto.Firstname = txtFirstName.Text;
            else
            {
                notifierThis.ShowWarning("Ism kiritilishi kerak!");
                SaveButton.IsEnabled = true;
                return;
            }

            if(!string.IsNullOrEmpty(txtLastName.Text))
                dto.Lastname = txtLastName.Text;
            else
            {
                notifierThis.ShowWarning("Familya kiritilishi kerak!");
                SaveButton.IsEnabled = true;
                return;
            }

            if(!string.IsNullOrEmpty(txtPhoneNumber.Text) && txtPhoneNumber.Text.Length == 13)
                dto.PhoneNumber = txtPhoneNumber.Text;
            else
            {
                notifierThis.ShowWarning("Telefon raqam noto'g'ri kiritilgan!");
                SaveButton.IsEnabled = true;
                return;
            }

            if(!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Length >= 4)
                dto.Password = txtPassword.Text;
            else
            {
                notifierThis.ShowWarning("Parol kiritilmagan yoki uzunligi 4 ta belgidan noto'g'ri kitilgan!");
                SaveButton.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(txtAge.Text))
            {
                int age = int.Parse(txtAge.Text);

                if (age >= 18 && age <= 70)
                    dto.Age = age;
                else
                {
                    notifierThis.ShowWarning("Yosh 18 va 70 orasida bo'lishi kerak!");
                    SaveButton.IsEnabled = true;
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("Yosh kiritilishi kerak!");
                SaveButton.IsEnabled = true;
                return;
            }

            if (RoleComboBox.SelectedItem is ComboBoxItem roleComboBoxItem &&
                roleComboBoxItem.Tag is not null)
                dto.Role = roleComboBoxItem.Tag.ToString() switch
                {
                    "0" => UserRole.Manager,
                    "1" => UserRole.Administrator,
                    "2" => UserRole.Cashier
                };
            else
            {
                notifierThis.ShowWarning("Lavozim tanlanishi kerak!");
                SaveButton.IsEnabled = true;
                return;
            }

            var result = await _userService.RegisterAsync(dto);

            if (result)
            {
                this.Close();
                notifierThis.ShowSuccess($"{dto.Firstname} - muvaffaqiyatli qo'shildi!");
            }
            else
            {
                notifierThis.ShowWarning("Malumotlarni saqlashda muammo yuz berdi, iltimos qayta urining!");
                SaveButton.IsEnabled = true;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowWarning("Malumotlarni saqlashda muammo yuz berdi, iltimos qayta urining!");
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (!SaveButton.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos, kuting!");
            return;
        }

        SaveButton.IsEnabled = false;

        try
        {
            await SavedAsync();
        }
        finally
        {
            SaveButton.IsEnabled = true;
        }
    }
}
