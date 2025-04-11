using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Pages.GroupForPages;

/// <summary>
/// Interaction logic for GroupPage.xaml
/// </summary>
public partial class GroupPage : Page
{
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    public GroupPage()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
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
        }
        else
        {

        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPage();
    }
}
