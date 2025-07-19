using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.GroupForComponents;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Desktop.Windows.GroupForWindows;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Pages.GroupForPages;

/// <summary>
/// Interaction logic for GroupPage.xaml
/// </summary>
public partial class GroupPage : Page
{
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private TeacherForResultDto _teacher;
    private int pageSize = 10;
    private int pageNumber = 1;
    private bool hasNext = false;
    private bool hasPrevious = false;
    private bool isFiltered = false;
    private GroupForFilterDto isFilteredDto = new GroupForFilterDto();
    public GroupPage()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();

        var window = GetActiveWindow();

        if (window.WindowState == WindowState.Maximized)
            pageSize = 15;
        else if (window.WindowState == WindowState.Normal)
            pageSize = 10;
        else
            pageSize = 10;
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

    private Window? GetActiveWindow()
        => Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());
        if (courses.Any())
        {
            foreach (var item in courses)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.Name;
                comboBoxItem.Tag = item.Id;
                courseComboBox.Items.Add(comboBoxItem);
            }
        }
        else
        {
            notifier.ShowWarning("Kurslar topilmadi!");
        }
    }

    private async Task GetAllTeacher()
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());
            if (teachers.Any())
            {
                foreach (var item in teachers)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = item.User.Firstname + " " + item.User.Lastname;
                    comboBoxItem.Tag = item.Id;
                    teacherComboBox.Items.Add(comboBoxItem);
                }
            }
            else
            {
                notifier.ShowWarning("O'qituvchilar topilmadi!");
            }
        }
        catch(Exception ex)
        {
            notifier.ShowError("O'qituvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private void Pagination(PagedResponse<GroupForResultDto> pagedResponse)
    {
        this.pageNumber = pagedResponse.CurrentPage;
        this.pageSize = (pagedResponse.PageSize > 0 ? pagedResponse.PageSize : 10);
        this.hasNext = pagedResponse.HasNext;
        this.hasPrevious = pagedResponse.HasPrevious;

        btnPrevious.Visibility = pagedResponse.HasPrevious switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed
        };

        btnNext.Visibility = pagedResponse.HasNext switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed
        };

        tbCurrentPageNumber.Text = pagedResponse.CurrentPage.ToString();
        btnPrevious.IsChecked = false;
        btnNext.IsChecked = false;
    }

    private async Task GetAllGroup()
    {
        try
        {
            stGroups.Children.Clear();
            groupLoader.Visibility = Visibility.Visible;
            var groups = await Task.Run(async () => await _groupService.GetAllPaginationAsync(pageSize, pageNumber));

            ShowGroups(groups.Data);
            Pagination(groups);
        }
        catch(Exception ex)
        {
            notifier.ShowError("Guruh malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async Task GetAllGroupByTeacherId()
    {
        stGroups.Children.Clear();
        groupLoader.Visibility = Visibility.Visible;
        var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(_teacher.Id));

        ShowGroups(groups);
    }

    private void ShowGroups(List<GroupForResultDto> groups)
    {
        int count = 1;
        stGroups.Children.Clear();

        if (groups.Any())
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Collapsed;

            foreach (var item in groups)
            {
                GroupForComponent component = new GroupForComponent();
                component.Tag = item;
                component.setValues(
                    item.Id,
                    count,
                    item.Name,
                    item.Course.Name,
                    item.Students.Count,
                    item.IsStatus,
                    item.CreatedAt);

                if (_teacher is not null)
                    component.OnGroupView += GetAllGroupByTeacherId;
                else
                    component.OnGroupView += GetAllGroup;

                stGroups.Children.Add(component);
                count++;
            }
        }
        else
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Visible;
        }
    }

    private async Task Filter()
    {
        stGroups.Children.Clear();
        emptyData.Visibility = Visibility.Collapsed;
        groupLoader.Visibility = Visibility.Visible;

        GroupForFilterDto dto = new GroupForFilterDto();

        if(courseComboBox.SelectedItem is ComboBoxItem selectedCourseItem
            && selectedCourseItem.Tag != null)
            dto.CourseId = (long)selectedCourseItem.Tag;

        if (teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem
            && selectedTeacherItem.Tag != null)
            dto.TeacherId = (long)selectedTeacherItem.Tag;

        if(IdentitySingelton.GetInstance().Role == Domain.Enums.UserRole.Teacher)
            dto.TeacherId = _teacher.Id;

        if (activeComboBox.SelectedItem is ComboBoxItem selectedActiveItem)
        {
            string status = selectedActiveItem.Content.ToString();

            switch (status)
            {
                case "Faol":
                    dto.IsStatus = Domain.Enums.Status.Active;
                    break;
                case "Arxivlangan":
                    dto.IsStatus = Domain.Enums.Status.Archived;
                    break;
                case "Yakunlangan":
                    dto.IsStatus = Domain.Enums.Status.Graduated;
                    break;
                default:
                    dto.IsStatus = null;
                    break;
            }
        }

        this.isFiltered = true;
        this.isFilteredDto = dto;
        this.pageNumber = 1;

        var groups = await Task.Run(async () => await _groupService.FilterAsync(dto, pageSize, pageNumber));

        if (groups.Data.Any())
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Collapsed;

            ShowGroups(groups.Data);
            Pagination(groups);
        }
        else
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Visible;
        }
    }

    private async Task<long> GetTeacher(long userId)
    {
        var teacher = await Task.Run(async () => await _teacherService.GetByUserIdAsync(userId));

        if (teacher is null)
        {
            notifier.ShowInformation("Ustoz topilmadi!");
            return 0;
        }

        _teacher = teacher;
        return teacher.Id;
    }

    private async Task LoadPage()
    {
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;

        if(role == Domain.Enums.UserRole.Teacher)
        {
            courseComboBox.Visibility = Visibility.Collapsed;
            teacherComboBox.Visibility = Visibility.Collapsed;
            createGroupBtn.Visibility = Visibility.Collapsed;

            var teacherId = await GetTeacher(id);
            if (teacherId == 0)
            {
                stGroups.Children.Clear();
                emptyData.Visibility = Visibility.Visible;
                return;
            }

            await GetAllGroupByTeacherId();
        }
        else
        {
            await GetAllGroup();
            await GetAllCourse();
            await GetAllTeacher();
        }
    }

    private bool isLoadPage = false;
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if(!isLoadPage)
        {
            isLoadPage = true;
            LoadPage();
        }
    }

    private void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isLoadPage)
            Filter();
    }

    private void teacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(isLoadPage)
            Filter();
    }

    private void activeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isLoadPage)
            Filter();
    }

    private async void createGroupBtn_Click(object sender, RoutedEventArgs e)
    {
        GroupForCreateWindow window = new GroupForCreateWindow();
        window.ShowDialog();
        await GetAllGroup();
    }

    private async void btnPrevious_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber -= 1;

        if (this.isFiltered && this.isFilteredDto is not null)
        {
            var groups = await Task.Run(async () => await _groupService.FilterAsync(isFilteredDto, pageSize, pageNumber));

            ShowGroups(groups.Data);
            Pagination(groups);
        }
        else
            await GetAllGroup();
    }

    private async void btnNext_Click(object sender, RoutedEventArgs e)
    {
        this.pageNumber += 1;

        if (this.isFiltered && this.isFilteredDto is not null)
        {
            var groups = await Task.Run(async () => await _groupService.FilterAsync(isFilteredDto, pageSize, pageNumber));

            ShowGroups(groups.Data);
            Pagination(groups);
        }
        else
            await GetAllGroup();
    }
}
