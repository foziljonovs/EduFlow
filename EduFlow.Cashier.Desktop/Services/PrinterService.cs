using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Cashier.Desktop.State;
using EduFlow.Desktop.Integrated.Security;
using ESC_POS_USB_NET.Printer;
using System.Drawing;
using System.IO;
using System.Management;
using System.Text;

namespace EduFlow.Cashier.Desktop.Services;

public class PrinterService : IDisposable
{
    private readonly string PRINTER_NAME = Properties.Settings.Default.PrinterName;
    public string printerName { get; set; } = string.Empty;
    Printer? printer;

    public PrinterService()
    {
        printerName = GetPrinterName();
    }

    public void Print(PaymentForResultDto paymentDto, double coursePrice, string paymentType, string teacher, string course)
    {
        if (string.IsNullOrEmpty(printerName))
            throw new ArgumentException("Printer aniqlanmadi. Printerni sozlang!");

        var cashier = IdentitySingelton.GetInstance().Name;
        printer = new Printer(PRINTER_NAME, "UTF-8");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // ✅ Sarlavha (Markaz nomi)
        printer.AlignCenter();
        
        if(AppState.Name.Length < 10)
        {
            printer.DoubleWidth3();
            printer.Append($"{AppState.Name}\n");
        }
        else
        {
            printer.DoubleWidth2();
            printer.Append($"{AppState.Name}\n");
        }

        if (!string.IsNullOrEmpty(AppState.PhoneNumber))
        {
            printer.NormalWidth();
            printer.Append($"Tel: {AppState.PhoneNumber}\n");
        }

        printer.Separator();

        // ✅ Asosiy ma’lumotlar
        printer.AlignLeft();
        printer.Append($"O'quvchi:\n{paymentDto.Student.Fullname}\n");
        printer.Append($"Yo'nalish:     {course}\n");
        printer.Append($"O'qituvchi:    {teacher}\n");
        printer.Append($"To'lov turi:   {paymentType}\n");
        printer.Append($"Kurs narxi:    {coursePrice:N0} so'm\n");

        printer.Append($"To'langan:     {paymentDto.Amount:N0} so'm\n");
        
        if (paymentDto.Discount > 0)
        {
            printer.Append($"Chegirma:      {paymentDto.Discount:N0} so'm\n");
            printer.Append($"Jami summa:    {(paymentDto.Amount + paymentDto.Discount):N0} so'm\n");
        }

        printer.Separator();

        // ✅ Kassir va sana
        printer.Append($"Kassir:\n{cashier}\n");
        printer.Append($"Sana:          {paymentDto.PaymentDate:dd/MM/yyyy}\n");

        string receipt = paymentDto.ReceiptNumber;
        if (receipt.Length > 16)
            receipt = receipt.Substring(0, 13) + "...";

        printer.Append($"To'lov raqami: {receipt}\n");

        printer.Separator();

        // ✅ Pastki qism
        printer.AlignCenter();
        printer.BoldMode("To'lovingiz uchun tashakkur!\n\n");

        printer.FullPaperCut();
        printer.PrintDocument();
    }


    private string Pad(string text, int length)
    {
        return text.PadRight(length, ' ');
    }

    public void Test()
    {
        printer = new Printer(PRINTER_NAME, "UTF-8");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        printer.Separator();
        printer.AlignCenter();
        printer.Append("\n");
        printer.BoldMode("Printer bilan aloqa qilindi!");
        printer.Append("\n");
        printer.Separator();
        printer.FullPaperCut();
        printer.PrintDocument();
    }

    public string GetPrinterName()
    {
        if (string.IsNullOrEmpty(printerName))
            printerName = GetSavedPrinterName();

        if (string.IsNullOrEmpty(printerName))
            printerName = GetUsbPrinterName();

        return printerName;
    }

    public string GetSavedPrinterName()
    {
        try
        {
            using var reader = new StreamReader(PRINTER_NAME);
            return reader.ReadToEnd();
        }
        catch
        {
            return string.Empty;
        }
    }

    public void SavePrinter(string name)
    {
        using var writer = new StreamWriter(PRINTER_NAME);
        writer.Write(name);
    }

    public List<string> ConnectedPrinters()
    {
        List<string> printers = new();
        foreach (var print in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
        {
            printers.Add(print.ToString()!);
        }
        return printers;
    }

    public string GetUsbPrinterName()
    {
        try
        {
            var usbPrinters = new List<string>();
            ManagementObjectSearcher searcher = new("SELECT * FROM Win32_Printer WHERE PortName LIKE 'USB%'");
            foreach (ManagementObject printer in searcher.Get())
            {
                usbPrinters.Add(printer["Name"].ToString()!);
            }

            if (usbPrinters.Count > 0)
            {
                SavePrinter(usbPrinters[0]);
                return usbPrinters[0];
            }

            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public void Dispose() => GC.SuppressFinalize(this);
}