using EduFlow.Desktop.Helpers;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Companies.Company;
using EduFlow.Desktop.Integrated.Services.Companies.Logo;
using EduFlow.Desktop.Pages.MainForPage;
using EduFlow.Desktop.State;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EduFlow.Desktop.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ICompanyService _companyService;
    private readonly ILogoService _logoService;
    private long CompanyId { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        this._companyService = new CompanyService();
        this._logoService = new LogoService();
    }

    public void NavigatePage(Page page)
        => Navigate.Content = page;

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if (NormalButton.Visibility == Visibility.Visible)
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(MaxButton.Visibility == Visibility.Visible)
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
    }

    private async Task GetCompany()
    {
        try
        {
            var company = await _companyService.GetByIdAsync(this.CompanyId);

            if(company is not null)
            {
                var existsLogo = await _logoService.GetByIdAsync(company.LogoId);
                if (existsLogo is null)
                    return;

                AppState.CompanyId = company.Id;
                AppState.Name = company.Name;
                AppState.PhoneNumber = company.PhoneNumber;
                tbCompanyName.Text = company.Name;

                var logo = await _logoService.GetLogoByIdAsync(existsLogo.Id);

                if(logo is not null)
                {
                    string savedPath = await FileHelper.SaveLogoAsync(logo, existsLogo.FileName);

                    AppState.LogoFilePath = savedPath;
                }
            }
        }
        catch(Exception ex)
        {
            tbCompanyName.Text = "Edu Flow";
        }
    }

    private bool SetLogoToImage(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            byte[] imageBytes = File.ReadAllBytes(filePath);

            using (var ms = new MemoryStream(imageBytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze();
                imgLogo.Source = bitmap;
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        NormalButton.IsChecked = true;

        var role = IdentitySingelton.GetInstance().Role;
        this.CompanyId = IdentitySingelton.GetInstance().CompanyId;

        if (role is Domain.Enums.UserRole.Teacher)
        {
            TeacherNavigationPage page = new TeacherNavigationPage();
            MainMenuNavigation.Content = page;
        }
        else
        {
            ManagerNavigationPage page = new ManagerNavigationPage();
            MainMenuNavigation.Content = page;
        }

        await GetCompany();

        if(!string.IsNullOrWhiteSpace(AppState.LogoFilePath))
        {
            bool logoResult = SetLogoToImage(AppState.LogoFilePath);

            if(!logoResult)
                imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/logo.png"));
        }
        else
            imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/logo.png"));
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();
}