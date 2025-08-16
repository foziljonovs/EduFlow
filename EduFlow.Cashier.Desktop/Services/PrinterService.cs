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

        printer.AlignCenter();

        if (!string.IsNullOrEmpty(AppState.LogoFilePath) && File.Exists(AppState.LogoFilePath))
        {
            try
            {
                using (var imgTemp = Image.FromFile(AppState.LogoFilePath))
                using (var bitmap = new Bitmap(imgTemp))
                {
                    printer.Image(bitmap);
                    printer.Append("\n\n");
                }
            }
            catch
            {
                printer.AlignCenter();
                printer.DoubleWidth2();
                printer.Append(AppState.Name);
                printer.Append("\n\n");
            }
        }
        else
        {
            printer.AlignCenter();
            printer.DoubleWidth2();
            printer.Append(AppState.Name);
            printer.Append("\n\n");
        }

        printer.Separator();
        printer.AlignLeft();

        printer.Append($"{Pad("Kurs to'lovi:", 15)} {coursePrice:N0} so'm");
        printer.Append($"{Pad("To'lov turi:", 15)} {paymentType}");
        if (paymentDto.Discount > 0)
            printer.Append($"{Pad("Chegirma:", 15)} {paymentDto.Discount:N0} so'm");
        printer.Append($"{Pad("Yo'nalish:", 15)} {course}");
        printer.Append($"{Pad("O'qituvchi:", 15)} {teacher}");
        printer.Append($"{Pad("O'quvchi:", 15)} {paymentDto.Student.Fullname}");
        printer.Append($"{Pad("Jami summa:", 15)} {(paymentDto.Amount + paymentDto.Discount):N0} so'm");
        printer.Append($"{Pad("To'langan summa:", 15)} {paymentDto.Amount:N0} so'm");
        printer.Append("\n");
        printer.Append($"{Pad("Kassir:", 15)} {cashier}");
        printer.Append($"{Pad("To'lov sanasi:", 15)} {paymentDto.PaymentDate:dd/MM/yyyy HH:mm}");
        string receipt = paymentDto.ReceiptNumber;
        if (receipt.Length > 16)
            receipt = receipt.Substring(0, 13) + "...";
        printer.Append($"{Pad("To'lov raqami:", 15)} {receipt}");
        printer.Separator();

        printer.AlignCenter();
        printer.BoldMode("Tashakkur!");
        printer.Append("\n");
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