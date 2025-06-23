using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Cashier.Desktop.Components.GroupForComponents;
using EduFlow.Cashier.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for IncomeForPaymentWindow.xaml
/// </summary>
public partial class IncomeForPaymentWindow : Window
{
    private readonly IPaymentService _paymentService;
    private readonly IGroupService _groupService;
    private readonly ITeacherService _teacherService;
    private readonly IStudentService _studentService;
    private TeacherForResultDto _teacher = new TeacherForResultDto();
    private long _studentId { get; set; }
    public IncomeForPaymentWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._groupService = new GroupService();
        this._teacherService = new TeacherService();
        this._studentService = new StudentService();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if(this.WindowState == WindowState.Normal)
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
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
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
    }

    private async Task GetAllTeacher()
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

            ShowTeachers(teachers);
        }
        catch(Exception ex)
        {
            notifier.ShowError("Xatolik yuz berdi!");
        }
    }

    private void ShowTeachers(List<TeacherForResultDto> teachers)
    {
        if (teachers.Any())
        {
            teacherComboBox.Items.Clear();

            teacherComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach (var teacher in teachers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = $"{teacher.User?.Firstname} {teacher.User?.Lastname}",
                    Tag = teacher.Id
                };

                teacherComboBox.Items.Add(item);
            }
        }
    }

    private void EmptyDataForLoaded()
    {
        //groups
        stGroups.Children.Clear();
        groupLoader.Visibility = Visibility.Collapsed;
        emptyDataForGroups.Visibility = Visibility.Visible;

        //students
        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Collapsed;
        emptyDataForStudents.Visibility = Visibility.Visible;
    }

    private async Task LoadWindow()
    {
        EmptyDataForLoaded();
        await GetAllTeacher();
    }

    public bool isWindowLoaded = false;
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (!isWindowLoaded)
        {
            isWindowLoaded = true;
            LoadWindow();
        }
    }

    private async Task GetAllGroupByTeacher(long teacherId)
    {
        try
        {
            groupLoader.Visibility = Visibility.Visible;
            emptyDataForGroups.Visibility = Visibility.Collapsed;

            var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(teacherId));

            ShowGroups(groups);
        }
        catch(Exception ex)
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroups.Visibility = Visibility.Visible;
            notifier.ShowError("Xatolik yuz berdi!");
        }
    }

    private async Task GetAllStudentByGroup(long groupId)
    {
        try
        {
            studentLoader.Visibility = Visibility.Visible;
            emptyDataForStudents.Visibility = Visibility.Collapsed;

            var groups = await Task.Run(async () => await _studentService.GetAllAsync());

            var thisGroupStudents = groups.Where(x => x.Groups.Any(x => x.Id == groupId)).ToList();

            ShowStudents(thisGroupStudents);
        }
        catch (Exception ex)
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudents.Visibility = Visibility.Visible;
            notifier.ShowError("Xatolik yuz berdi!");
        }
    }

    private StudentForComponent _selectedStudentComponent = null!;
    private void ShowStudents(List<StudentForResultDto> students)
    {
        stStudents.Children.Clear();

        if (students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudents.Visibility = Visibility.Collapsed;

            foreach (var student in students)
            {
                int paymentCount = student.Payments?.Count ?? 0;

                StudentForComponent component = new StudentForComponent();
                component.SetValues(
                    student.Id,
                    student.Fullname,
                    student.PhoneNumber,
                    paymentCount);

                component.isClicked += async () =>
                {
                    try
                    {
                        if (_selectedStudentComponent is not null)
                            _selectedStudentComponent.SelectedState(false);

                        _selectedStudentComponent = component;
                        _selectedStudentComponent.SelectedState(true);

                        this._studentId = component.GetId();
                    }
                    catch (Exception ex)
                    {
                        notifier.ShowError("Xatolik yuz berdi!");
                    }
                };

                stStudents.Children.Add(component);
            }
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudents.Visibility = Visibility.Visible;
        }
    }

    private GroupForComponent _selectedGroupComponent = null!;
    private void ShowGroups(List<GroupForResultDto> groups)
    {
        stGroups.Children.Clear();

        if (groups.Any())
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroups.Visibility = Visibility.Collapsed;

            foreach(var group in groups)
            {
                int lessonCount = group.Lessons?.Count ?? 0;

                GroupForComponent component = new GroupForComponent();
                component.SetValues(
                    group.Id,
                    group.Name,
                    lessonCount,
                    group.CreatedAt);

                component.isClicked += async () =>
                {
                    try
                    {
                        if(_selectedGroupComponent is not null)
                            _selectedGroupComponent.SelectedState(false);

                        _selectedGroupComponent = component;
                        _selectedGroupComponent.SelectedState(true);

                        await GetAllStudentByGroup(component.GetId());
                    }
                    catch (Exception ex)
                    {
                        notifier.ShowError("Xatolik yuz berdi!");
                    }
                };

                stGroups.Children.Add(component);
            }
        }
        else
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroups.Visibility = Visibility.Visible;
        }
    }

    private async Task GetTeacher(long id)
    {
        try
        {
            var teacher = await _teacherService.GetByIdAsync(id);

            if(teacher is not null)
            {
                this._teacher = teacher;

                await GetAllGroupByTeacher(teacher.Id);
                AmountTxt.Text = teacher.Course.Price.ToString("0");
            }
            else
            {
                notifier.ShowWarning("O'qituvchi ma'lumotlari topilmadi, qayta urinib ko'ring!");
            }
        }
        catch(Exception ex)
        {
            notifier.ShowError("Xatolik yuz berdi!");
        }
    }

    private async void teacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        stGroups.Children.Clear();
        stStudents.Children.Clear();

        if (!isWindowLoaded)
            return;

        if (this.teacherComboBox.SelectedItem is ComboBoxItem selectedItem &&
            selectedItem.Tag != null)
        {
            long id = (long)selectedItem.Tag;
            await GetTeacher(id);
        }
    }
}
