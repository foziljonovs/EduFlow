using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Services.Companies.Company;
using EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;
using EduFlow.Management.Pages;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Management.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ICompanyService _companyService;
    private readonly ICompanySubscriptionService _companySubscriptionService;
    public MainWindow()
    {
        InitializeComponent();
        this._companyService = new CompanyService();
        this._companySubscriptionService = new CompanySubscriptionService();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private async Task GetAllCompanyCount()
    {
        try
        {
            var companies = await Task.Run(async () => await _companyService.GetAllAsync());

            ShowCompanyCount(companies);
        }
        catch(Exception ex)
        {
            tbCompanyCount.Text = "0";
        }
    }

    private async Task GetAllSubscription()
    {
        try
        {
            var subscriptions = await Task.Run(async () => await _companySubscriptionService.GetAllAsync());

            ShowActiveSubscriptionCount(subscriptions);
        }
        catch
        {
            tbActiveSubscriptionCount.Text = "0";
            tbNoActiveSubscriptionCount.Text = "0";
        }
    }

    private void ShowCompanyCount(List<CompanyForResultDto> companies)
    {
        if (companies.Any())
            tbCompanyCount.Text = companies.Count().ToString();
        else
            tbCompanyCount.Text = "0";
    }

    private void ShowActiveSubscriptionCount(List<CompanySubscriptionForResultDto> subscriptions)
    {
        if (subscriptions.Any())
        {
            int activeSubscriptionCount = subscriptions
                .Count(x => x.IsActive == true);

            int noActiveSubscriptionCount = subscriptions
                .Count(x => x.IsActive == false);

            tbActiveSubscriptionCount.Text = activeSubscriptionCount.ToString();
            tbNoActiveSubscriptionCount.Text = noActiveSubscriptionCount.ToString();
        }
        else
        {
            tbActiveSubscriptionCount.Text = "0";
            tbNoActiveSubscriptionCount.Text = "0";
        }
    }

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if(this.WindowState == WindowState.Normal)
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(this.WindowState == WindowState.Maximized)
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            NormalButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    public void NavigatePage(Page page)
        => Navigate.Content = page;
    private void HomeBtn_Click(object sender, RoutedEventArgs e)
    {
        HomeBtn.IsChecked = true;

        MainPage page = new MainPage();
        NavigatePage(page);
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        HomeBtn.IsChecked = true;

        MainPage page = new MainPage();
        NavigatePage(page);

        await GetAllCompanyCount();
        await GetAllSubscription();
    }
}