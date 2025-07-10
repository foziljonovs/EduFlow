using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Cashier.Desktop.Components.GroupForComponents;
using EduFlow.Cashier.Desktop.Components.StudentForComponents;
using EduFlow.Cashier.Desktop.Services;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Payments.Registry;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using EduFlow.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
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
    private readonly IRegistryService _registryService;
    private readonly IGroupService _groupService;
    private readonly ITeacherService _teacherService;
    private readonly IStudentService _studentService;
    private TeacherForResultDto _teacher = new TeacherForResultDto();
    PrinterService _printerService = new PrinterService();
    private long _studentId { get; set; }
    public IncomeForPaymentWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._registryService = new RegistryService();
        this._groupService = new GroupService();
        this._teacherService = new TeacherService();
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
            notifierThis.ShowError("Xatolik yuz berdi!");
            groupLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroups.Visibility = Visibility.Visible;
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
            notifierThis.ShowError("Xatolik yuz berdi!");
        }
    }

    private async Task  GetAllStudentByGroup(long groupId)
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
            notifierThis.ShowError("Xatolik yuz berdi!");
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
                        notifierThis.ShowError("Xatolik yuz berdi!");
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
                GroupForComponent component = new GroupForComponent();
                component.SetValues(
                    group.Id,
                    group.Name,
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
                        notifierThis.ShowError("Xatolik yuz berdi!");
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
                notifierThis.ShowWarning("O'qituvchi ma'lumotlari topilmadi, qayta urinib ko'ring!");
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi!");
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

    private async Task SavedAsync()
    {
        try
        {
            string printerName = _printerService.GetPrinterName();

            if(string.IsNullOrEmpty(printerName))
            {
                notifierThis.ShowWarning("Iltimos, printerni sozlang!");
                saveBtn.IsEnabled = true;
                return;
            }

            RegistryForCreateDto registryDto = new RegistryForCreateDto();
            PaymentForCreateDto paymentDto = new PaymentForCreateDto();

            if(teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem &&
                selectedTeacherItem.Tag != null)
                paymentDto.TeacherId = (long)selectedTeacherItem.Tag;
            else
            {
                notifierThis.ShowWarning("Iltimos, o'qituvchini tanlang!");
                teacherComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if (!double.TryParse(AmountTxt.Text, out double amount) || amount <= 0)
            {
                notifierThis.ShowWarning("Iltimos, to'g'ri to'lov summasini kiriting!");
                AmountTxt.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            registryDto.Debit = amount;
            registryDto.Credit = 0;
            paymentDto.Amount = amount;
            paymentDto.Discount = 0;

            if (!string.IsNullOrWhiteSpace(DiscountTxt.Text))
            {
                if (double.TryParse(DiscountTxt.Text, out double discount) && discount >= 0)
                {
                    if (discount > amount)
                    {
                        notifierThis.ShowWarning("Chegirma summasi to'lovdan katta bo'lmasligi kerak!");
                        DiscountTxt.Focus();
                        return;
                    }

                    double totalAmount = amount - discount;

                    paymentDto.Discount = discount;
                    paymentDto.Amount = totalAmount;
                    registryDto.Debit = totalAmount;
                }
                else
                {
                    notifierThis.ShowWarning("Iltimos, to'g'ri chegirma qiymatini kiriting!");
                    DiscountTxt.Focus();
                    return;
                }
            }

            if (paymentTypeComboBox.SelectedItem is ComboBoxItem selectedPaymentTypeItem &&
            selectedPaymentTypeItem.Tag != null)
            {
                PaymentType type = selectedPaymentTypeItem.Tag.ToString() switch
                {
                    "0" => PaymentType.Cash,
                    "1" => PaymentType.Card,
                    "2" => PaymentType.Transfer,
                    "3" => PaymentType.Credit,
                    "4" => PaymentType.Other,
                    _ => PaymentType.Other
                };

                registryDto.Type = type;
                paymentDto.Type = type;
            }
            else
            {
                notifierThis.ShowWarning("Iltimos, to'lov turini tanlang!");
                paymentTypeComboBox.Focus();
                saveBtn.IsEnabled = true;
                return;
            }

            if(!string.IsNullOrEmpty(NotesTxt.Text))
            {
                registryDto.Description = NotesTxt.Text;
                paymentDto.Notes = NotesTxt.Text;
            }

            if (_selectedGroupComponent is not null)
            {
                long groupId = _selectedGroupComponent.GetId();
                paymentDto.GroupId = groupId;
            }
            else
            {
                notifierThis.ShowWarning("Iltimos, guruhni tanlang!");
                saveBtn.IsEnabled = true;
                return;
            }

            if (_selectedStudentComponent is not null)
            {
                long studentId = _selectedStudentComponent.GetId();
                paymentDto.StudentId = studentId;
            }
            else
            {
                notifierThis.ShowWarning("Iltimos, o'quvchini tanlang!");
                saveBtn.IsEnabled = true;
                return;
            }

            paymentDto.ReceiptNumber = string.Empty; //Receipt number backendda generatsiya qilinadi
            paymentDto.PaymentDate = DateTime.UtcNow.AddHours(5);
            paymentDto.Status = PaymentStatus.Pending;
            registryDto.IsConfirmed = false;

            long registryId = await _registryService.IncomeAsync(registryDto);

            if(registryId > 0)
            {
                paymentDto.RegistryId = registryId;

                long paymentResult = await _paymentService.AddToPayAsync(paymentDto);

                if (paymentResult > 0)
                {
                    WriteToPrinter(paymentResult, $"{_teacher.Course.Name ?? "Nomalum"}", $"{_teacher.User?.Firstname + _teacher.User?.Lastname ?? "Nomalum"}");

                    this.Close();
                    notifier.ShowSuccess("To'lov muvaffaqiyatli saqlandi!");
                }
                else
                {
                    bool deletedRegistry = await _registryService.DeleteAsync(registryId);

                    if (deletedRegistry)
                        notifierThis.ShowWarning("To'lov saqlanmadi, qayta urinib ko'ring!");
                    else
                        notifierThis.ShowWarning("Xatolik sabab joriy to'lov summasini tushumdan chiqarib yuboring!");

                    saveBtn.IsEnabled = true;
                    return;
                }
            }
            else
            {
                notifierThis.ShowWarning("To'lovni saqlashda xatolik yuz berdi, qayta urinib ko'ring!");
                saveBtn.IsEnabled = true;
                return;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Xatolik yuz berdi!");
            saveBtn.IsEnabled = true;
        }
    }

    private async void WriteToPrinter(long paymentId, string courseName, string teacherName)
    {
        try
        {
            if(paymentId > 0)
            {
                var payment = await Task.Run(async () => await _paymentService.GetByIdAsync(paymentId));
                
                if(payment is not null)
                {
                    double coursePrice = double.Parse(AmountTxt.Text.ToString());
                    string paymentType = payment.Type switch
                    {
                        PaymentType.Cash => "Naqd",
                        PaymentType.Card => "Karta",
                        PaymentType.Transfer => "O'tkazma",
                        PaymentType.Credit => "Nasiya",
                        PaymentType.Other => "Aniq emas",
                        _ => "Aniq emas"
                    };

                    _printerService.Print(payment, coursePrice, paymentType, teacherName, courseName);
                }
                else
                {
                    notifierThis.ShowWarning("To'lov ma'lumotlari topilmadi, chekni qayta chiqarishga urinib ko'ring!");
                }
            }
            else
            {
                notifierThis.ShowWarning("To'lov ma'lumotlari noto'g'ri yuklandi!");
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Chekni chop etishda xatolik yuz berdi! Iltimos printerni tekshiring.");
        }
    }

    private async void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!saveBtn.IsEnabled)
        {
            notifierThis.ShowWarning("iltimos, kuting!");
            return;
        }

        saveBtn.IsEnabled = false;

        try 
        { 
            await SavedAsync();
        }
        catch(Exception ex)
        {
            saveBtn.IsEnabled = true;
        }
    }
}
