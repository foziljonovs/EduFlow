using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Auth;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.AuthForWindow;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    private readonly IAuthService _authService;
    public LoginWindow()
    {
        InitializeComponent();
        this._authService = new AuthService();
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

    private bool VerifyForPhoneNumber(string phoneNumber)
    {
        if (phoneNumber is null)
        {
            notifier.ShowWarning("Iltimos, telefon raqamingizni kiriting!");
            PhoneNumberTxt.Focus();
            return false;
        }

        if (!phoneNumber.StartsWith("+998") || phoneNumber.Length != 13)
        {
            notifier.ShowWarning("Telefon raqam +998XX XXX XX XX formatida bo'lishi kerak!");
            PhoneNumberTxt.Focus();
            return false;
        }

        return true;
    }

    private bool VerifyForPassword(string password)
    {
        if (password is null)
            return false;

        if (password.Length < 4 || password.Length > 8)
            return false;

        if (!password.Any(char.IsLetter))
            return false;

        return true;
    }

    private void SetIdentity(string token)
    {
        var tkn = TokenHandler.ParseToken(token);
        var identity = IdentitySingelton.GetInstance();
        identity.Id = tkn.Id;
        identity.Token = token;
        identity.PhoneNumber = tkn.PhoneNumber;
        identity.Name = tkn.Name;
        identity.Role = tkn.Role;
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async void LoginBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LoginBtn.IsEnabled = false;

            if(!string.IsNullOrEmpty(PhoneNumberTxt.Text) &&
                !string.IsNullOrEmpty(PasswordPwd.Password))
            {
                UserForLoginDto dto = new UserForLoginDto();

                if(VerifyForPhoneNumber(PhoneNumberTxt.Text))
                    dto.PhoneNumber = PhoneNumberTxt.Text;
                else
                {
                    notifier.ShowWarning("Telefon raqam noto'g'ri formatda!");
                    LoginBtn.IsEnabled = true;
                    return;
                }

                if (VerifyForPassword(PasswordPwd.Password))
                    dto.Password = PasswordPwd.Password;
                else
                {
                    notifier.ShowWarning("Parol noto'g'ri formatda, harf va raqamlardan iborat bo'lishi kerak!");
                    LoginBtn.IsEnabled = true;
                    return;
                }

                var res = await _authService.LoginAsync(dto);

                if (res.result)
                {
                    TokenHandler.ParseToken(res.token);
                    SetIdentity(res.token);
                    var role = IdentitySingelton.GetInstance().Role;

                    if(role != Domain.Enums.UserRole.Cashier)
                    {
                        notifier.ShowWarning("Siz ushbu tizimga kirish huquqiga ega emassiz!");
                        LoginBtn.IsEnabled = true;
                        return;
                    }

                    var fullName = IdentitySingelton.GetInstance().Name;

                    MainWindow window = new MainWindow();
                    window.Show();

                    this.Close();
                }
                else
                {
                    notifier.ShowError("Telefon raqam yoki parol noto'g'ri!");
                    LoginBtn.IsEnabled = true;
                }
            }
        }
        catch(Exception ex)
        {
            notifier.ShowError("Kirishda xatolik yuz berdi!");
            LoginBtn.IsEnabled = true;
        }
    }
}
