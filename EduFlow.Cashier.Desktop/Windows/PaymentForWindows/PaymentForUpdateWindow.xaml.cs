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
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.Threading.Tasks;
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
    private readonly IRegistryService _registryService;
    private readonly ITeacherService _teacherService;
    private readonly IGroupService _groupService;
    private readonly IStudentService _studentService;
    PrinterService _printerService = new PrinterService();
    private long Id { get; set; }
    private long TeacherId { get; set; }
    private long StudentId { get; set; }
    private TeacherForResultDto teacher = new TeacherForResultDto();
    private PaymentForResultDto payment = new PaymentForResultDto();
    public PaymentForUpdateWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._teacherService = new TeacherService();
        this._groupService = new GroupService();
        this._studentService = new StudentService();
        this._registryService = new RegistryService();
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

    private async Task GetTeacher(long id)
    {
        try
        {
            var teacher = await _teacherService.GetByIdAsync(id);

            if (teacher is not null)
                this.teacher = teacher;
            else
            {
                this.teacher = null;
                notifierThis.ShowWarning("Iltimos o'qituvchini qayta tanlang!");
            }
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

    private async Task GetAllGroupByTeacher(long teacherId)
    {
        try
        {
            groupLoader.Visibility = Visibility.Visible;
            emptyDataForGroups.Visibility = Visibility.Collapsed;

            var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(teacherId));

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
        _selectedStudentComponent = null;

        stGroups.Children.Clear();
        emptyDataForGroups.Visibility = Visibility.Visible;
        _selectedGroupComponent = null;
    }

    private async void teacherComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (isInitialLoad)
            return;

        if(teacherComboBox.SelectedItem is ComboBoxItem selectedItem &&
            selectedItem.Tag is not null)
        {
            var id = (long)selectedItem.Tag;

            Clean();
            await GetTeacher(id);
            await GetAllGroupByTeacher(id);
        }
    }

    private bool isInitialLoad = true;
    private async Task Load()
    {
        await GetAllTeacher();
        await GetPayment();
        await GetTeacher(this.TeacherId);
        await GetAllGroupByTeacher(this.TeacherId);
        await GetAllStudentBySelectedGroup(this.payment.GroupId);

        ShowActivites();

        isInitialLoad = false;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Load();
    }

    private async Task SavedAsync()
    {
        try
        {
            string printerName = _printerService.GetPrinterName();

            if (string.IsNullOrEmpty(printerName))
            {
                notifierThis.ShowWarning("Iltimos, printerni sozlang!");
                SaveBtn.IsEnabled = true;
                return;
            }

            RegistryForCreateDto registryDto = new RegistryForCreateDto();
            PaymentForUpdateDto paymentDto = new PaymentForUpdateDto();


            if (teacherComboBox.SelectedItem is ComboBoxItem selectedTeacherItem &&
                selectedTeacherItem.Tag != null)
                paymentDto.TeacherId = (long)selectedTeacherItem.Tag;
            else
            {
                notifierThis.ShowWarning("Iltimos, o'qituvchini tanlang!");
                teacherComboBox.Focus();
                SaveBtn.IsEnabled = true;
                return;
            }

            if (!double.TryParse(AmountTxt.Text, out double amount) || amount <= 0)
            {
                notifierThis.ShowWarning("Iltimos, to'g'ri to'lov summasini kiriting!");
                AmountTxt.Focus();
                SaveBtn.IsEnabled = true;
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
                SaveBtn.IsEnabled = true;
                return;
            }

            if (!string.IsNullOrEmpty(NotesTxt.Text))
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
                SaveBtn.IsEnabled = true;
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
                SaveBtn.IsEnabled = true;
                return;
            }

            paymentDto.ReceiptNumber = string.Empty; //Receipt number backendda generatsiya qilinadi
            paymentDto.PaymentDate = DateTime.UtcNow.AddHours(5);
            paymentDto.Status = PaymentStatus.Pending;
            registryDto.IsConfirmed = false;

            if(this.payment.Amount != paymentDto.Amount &&
                this.payment.Type != paymentDto.Type)
            {
                var deletedOldRegistry = await _registryService.DeleteAsync(payment.RegistryId);

                if(!deletedOldRegistry)
                {
                    notifierThis.ShowWarning("Malumotlarni yangilashda xatolik yuz berdi, Iltimos qayta urining!");
                    SaveBtn.IsEnabled = true;
                    return;
                }

                var addNewRegisry = await _registryService.IncomeAsync(registryDto);

                if (addNewRegisry > 0)
                    paymentDto.RegistryId = addNewRegisry;
                else
                {
                    notifierThis.ShowWarning("To'lovni saqlashda xatolik yuz berdi, Iltimos qayta urining!");
                    SaveBtn.IsEnabled = true;
                    return;
                }
            }

            var updatedPayment = await _paymentService.UpdateToPayAsync(this.Id, paymentDto);

            if (updatedPayment)
            {
                WriteToPrinter(this.payment.Id, $"{this.teacher.Course.Name ?? "Nomalum"}", $"{this.teacher.User?.Firstname + this.teacher.User?.Lastname ?? "Nomalum"}");

                this.Close();
                notifier.ShowSuccess("To'lov muvaffaqiyatli saqlandi!");
            }
            else
            {
                bool deletedRegistry = await _registryService.DeleteAsync(payment.RegistryId);

                if (deletedRegistry)
                    notifierThis.ShowWarning("To'lov saqlanmadi, qayta urinib ko'ring!");
                else
                    notifierThis.ShowWarning("Xatolik sabab joriy to'lov summasini tushumdan chiqarib yuboring!");

                SaveBtn.IsEnabled = true;
                return;
            }
        }
        catch(Exception ex)
        {
            notifierThis.ShowError("Malumotlarni saqlashda xatolik yuz berdi, Iltimos tekshiring!");
            SaveBtn.IsEnabled = true;   
        }
    }

    private async void WriteToPrinter(long paymentId, string courseName, string teacherName)
    {
        try
        {
            if (paymentId > 0)
            {
                var payment = await Task.Run(async () => await _paymentService.GetByIdAsync(paymentId));

                if (payment is not null)
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
        catch (Exception ex)
        {
            notifierThis.ShowError("Chekni chop etishda xatolik yuz berdi! Iltimos printerni tekshiring.");
        }
    }

    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        if(!SaveBtn.IsEnabled)
        {
            notifierThis.ShowWarning("Iltimos, kuting!");
            return;
        }

        SaveBtn.IsEnabled = false;

        try
        {
            await SavedAsync();
        }
        catch(Exception ex)
        {
            SaveBtn.IsEnabled = true;
        }
    }
}
