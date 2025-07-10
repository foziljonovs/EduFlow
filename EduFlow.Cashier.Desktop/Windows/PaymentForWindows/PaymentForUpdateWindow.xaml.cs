using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Cashier.Desktop.Components.GroupForComponents;
using EduFlow.Cashier.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for PaymentForUpdateWindow.xaml
/// </summary>
public partial class PaymentForUpdateWindow : Window
{
    private readonly IPaymentService _paymentService;
    private readonly ITeacherService _teacherService;
    private readonly IGroupService _groupService;
    private readonly IStudentService _studentService;
    private long Id { get; set; }
    private long TeacherId { get; set; }
    private long StudentId { get; set; }
    private PaymentForResultDto payment = new PaymentForResultDto();
    public PaymentForUpdateWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._teacherService = new TeacherService();
        this._groupService = new GroupService();
        this._studentService = new StudentService();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

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
            NormalButton.Visibility = Visibility.Visible;
            MaxButton.Visibility = Visibility.Collapsed;
        }
    }

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async Task GetAllTeacher()
    {
        try
        {
            var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

            ShowTeachers(teachers);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'qituvchi malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async Task GetPayment()
    {
        try
        {
            var payment = await Task.Run(async () => await _paymentService.GetByIdAsync(this.Id));

            ShowPayment(payment);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Ma'lumotlarni yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async Task GetAllGroupByTeacher()
    {
        try
        {
            groupLoader.Visibility = Visibility.Visible;
            emptyDataForGroups.Visibility = Visibility.Collapsed;

            var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(this.TeacherId));

            ShowTeacherGroups(groups);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("O'qituvchining guruhlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private async Task GetAllStudentBySelectedGroup(long groupId)
    {
        try
        {
            studentLoader.Visibility = Visibility.Visible;
            emptyDataForStudents.Visibility = Visibility.Collapsed;

            var students = await Task.Run(async () => await _studentService.GetAllByTeacherIdAsync(this.TeacherId));

            var thisGroupStudents = students.Where(x => x.Groups.Any(g => g.Id == groupId)).ToList();

            ShowStudents(thisGroupStudents);
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Guruh o'quchilarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private GroupForComponent _selectedGroupComponent = null!;
    private void ShowTeacherGroups(List<GroupForResultDto> groups)
    {
        stGroups.Children.Clear();

        if (groups.Any())
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroups.Visibility = Visibility.Collapsed;

            foreach(var group in groups)
            {
                GroupForComponent component = new GroupForComponent();
                component.SetValues( 
                    group.Id,
                    group.Name,
                    group.CreatedAt);

                component.isClicked += async () =>
                {
                    try
                    {
                        if (_selectedGroupComponent is not null)
                            _selectedGroupComponent.SelectedState(false);

                        _selectedGroupComponent = component;
                        _selectedGroupComponent.SelectedState(true);

                        await GetAllStudentBySelectedGroup(component.GetId());
                    }
                    catch (Exception ex)
                    {
                        notifierThis.ShowError("Xatolik yuz berdi!");
                    }
                };

                stGroups.Children.Add(component);
            }
        }
        else
        {
            notifierThis.ShowWarning("Guruhlarning malumotlarini yuklab bo'lmadi, Iltimos qayta urining!");
            groupLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroups.Visibility = Visibility.Visible;
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

            foreach(var student in students)
            {
                StudentForComponent component = new StudentForComponent();
                component.SetValues(
                    student.Id,
                    student.Fullname,
                    student.PhoneNumber,
                    student.Payments.Count);

                component.isClicked += async () =>
                {
                    try
                    {
                        if (_selectedStudentComponent is not null)
                            _selectedStudentComponent.SelectedState(false);

                        _selectedStudentComponent = component;
                        _selectedStudentComponent.SelectedState(true);

                        this.StudentId = component.GetId();
                    }
                    catch (Exception ex)
                    {
                        notifierThis.ShowError("Xatolik yuz berdi!");
                    }
                };

                stStudents.Children.Add(component);
            }
        }
        else
        {
            notifierThis.ShowError("O'quvchilarning malumotlarini yuklab bo'lmadi, Iltimos qayta urining!");
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudents.Visibility = Visibility.Visible;
        }
    }

    private void ShowTeachers(List<TeacherForResultDto> teachers)
    {
        teacherComboBox.Items.Clear();

        if (teachers.Any())
        {
            foreach(var teacher in teachers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = $"{teacher.User.Firstname} {teacher.User.Lastname}",
                    Tag = teacher.Id,
                };

                teacherComboBox.Items.Add(item);
            }
        }
        else
        {
            notifierThis.ShowError("O'qituvchi malumotlari yuklab bo'lmadi, Iltimos qayta urining!");
        }
    }

    public void SetId(long id)
        => this.Id = id;

    private void ShowPayment(PaymentForResultDto payment)
    {
        if(payment is not null)
        {
            this.payment = payment;
            this.TeacherId = payment.TeacherId;

            AmountTxt.Text = payment.Amount.ToString();
            DiscountTxt.Text = payment.Discount.ToString();
            NotesTxt.Text = payment.Notes?.ToString() ?? "...";
        }
        else
        {
            notifierThis.ShowError("To'lov malumotlarini yuklashda xatolik yuz berdi, Iltimos qayta urining!");
        }
    }

    private void ShowActivites()
    {
        if(payment is not null)
        {
            paymentTypeComboBox.SelectedItem = payment.Type switch
            {
                Domain.Enums.PaymentType.Cash => paymentTypeComboBox.Items[0],
                Domain.Enums.PaymentType.Card => paymentTypeComboBox.Items[1],
                Domain.Enums.PaymentType.Transfer => paymentTypeComboBox.Items[2],
                Domain.Enums.PaymentType.Credit => paymentTypeComboBox.Items[3],
                Domain.Enums.PaymentType.Other => paymentTypeComboBox.Items[4],
                _ => paymentTypeComboBox.Items[0]
            };

            foreach (var item in teacherComboBox.Items)
            {
                if(item is ComboBoxItem comboboxItem && 
                    comboboxItem.Tag?.ToString() == payment.TeacherId.ToString())
                {
                    teacherComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach(var item in stGroups.Children)
            {
                if(item is GroupForComponent component &&
                    component.GetId() == this.payment.GroupId)
                {
                    _selectedGroupComponent = component;
                    component.SelectedState(true);
                    break;
                }
            }

            foreach(var item in stStudents.Children)
            {
                if(item is StudentForComponent component &&
                    component.GetId() == this.payment.StudentId)
                {
                    _selectedStudentComponent = component;
                    component.SelectedState(true);
                    break;
                }
            }
        }
    }

    private void Clean()
    {
        stStudents.Children.Clear();
        emptyDataForStudents.Visibility = Visibility.Visible;

        stGroups.Children.Clear();
        emptyDataForGroups.Visibility = Visibility.Visible;
    }

    private async void teacherComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (isInitialLoad)
            return;

        await GetAllGroupByTeacher();
        Clean();
    }

    private bool isInitialLoad = true;
    private async Task Load()
    {
        await GetPayment();
        await GetAllTeacher();
        await GetAllGroupByTeacher();
        await GetAllStudentBySelectedGroup(this.payment.GroupId);

        ShowActivites();

        isInitialLoad = false;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Load();
    }
}
