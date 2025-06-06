using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Auth;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.AuthForWindow;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    private readonly IAuthService _service;
    public LoginWindow()
    {
        InitializeComponent();
        this._service = new AuthService();
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

    Notifier notifierThis = new Notifier(cfg =>
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
    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private bool VerifyForPhoneNumber(string phoneNumber)
    {
        if (phoneNumber is null)
        {
            notifierThis.ShowWarning("Iltimos, telefon raqamingizni kiriting!");
            PhoneNumberTxt.Focus();
            return false;
        }

        if (!phoneNumber.StartsWith("+998") || phoneNumber.Length != 13)
        {
            notifierThis.ShowWarning("Telefon raqam +998XX XXX XX XX formatida bo'lishi kerak!");
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
    private async void LoginBtn_Click(object sender, RoutedEventArgs e)
    {
        LoginBtn.Visibility = Visibility.Collapsed;
        LoginLoader.Visibility = Visibility.Visible;
        try
        {
            if(!string.IsNullOrEmpty(PhoneNumberTxt.Text) &&
                !string.IsNullOrEmpty(PasswordPwd.Password))
            {
                UserForLoginDto dto = new UserForLoginDto();

                if(VerifyForPhoneNumber(PhoneNumberTxt.Text))
                    dto.PhoneNumber = PhoneNumberTxt.Text;
                else
                {
                    LoginLoader.Visibility = Visibility.Collapsed;
                    LoginBtn.Visibility = Visibility.Visible;   
                    return;
                }

                if(VerifyForPassword(PasswordPwd.Password))
                    dto.Password = PasswordPwd.Password;
                else
                {
                    notifierThis.ShowWarning("Parol noto'g'ri formatda! raqam va harflar bo'lishi kerak");
                    LoginLoader.Visibility = Visibility.Collapsed;
                    LoginBtn.Visibility = Visibility.Visible;
                    return;
                }

                var res = await _service.LoginAsync(dto);
                if (res.result)
                {
                    TokenHandler.ParseToken(res.token);
                    SetIdentity(res.token);
                    var role = IdentitySingelton.GetInstance().Role;

                    if (role == Domain.Enums.UserRole.Cashier)
                    {
                        notifierThis.ShowWarning("Siz ushbu tizimga kirish huquqiga ega emassiz!");
                        LoginLoader.Visibility = Visibility.Collapsed;
                        LoginBtn.Visibility = Visibility.Visible;
                        return;
                    }

                    var fullName = IdentitySingelton.GetInstance().Name;

                    MainWindow window = new MainWindow();
                    window.Show();

                    this.Close();
                    notifier.ShowInformation($"{fullName} - xush kelibsiz!");
                }
                else
                {
                    notifierThis.ShowError("Kirishda xatolik yuz berdi!");
                    LoginLoader.Visibility = Visibility.Collapsed;
                    LoginBtn.Visibility = Visibility.Visible;
                }
            }
            else
            {
                notifierThis.ShowWarning("Ma'lumotlar kiritilganini tekshiring!");
                LoginLoader.Visibility = Visibility.Collapsed;
                LoginBtn.Visibility = Visibility.Visible;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi, qayta urining");
            LoginLoader.Visibility = Visibility.Collapsed;  
            LoginBtn.Visibility = Visibility.Visible;
        }
    }
}
