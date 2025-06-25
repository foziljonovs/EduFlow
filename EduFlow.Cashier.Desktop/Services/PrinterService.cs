using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Integrated.Security;
using ESC_POS_USB_NET.Printer;
using System.ComponentModel;
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

    public void Print(PaymentForResultDto paymentDto, double coursePrice, string paymentType)
    {
        var cashier = IdentitySingelton.GetInstance().Name;

        printer = new Printer(PRINTER_NAME, "UTF-8");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        printer.AlignCenter();
        printer.DoubleWidth2();
        printer.Append("Edu Flow");
        printer.Separator();
        printer.AlignLeft();
        printer.Append($"Kurs to'lovi:      {coursePrice} so'm");
        printer.Append($"To'lov turi:       {paymentType}");
        printer.Append($"Chegirma:          {paymentDto.Discount} so'm");
        printer.Append($"Yo'nalish:         {paymentDto.Group.Course.Name}");
        printer.Append($"O'qituvchi:\n{paymentDto.Teacher.User.Firstname} {paymentDto.Teacher.User.Lastname}");
        printer.Append($"O'quvchi:\n{paymentDto.Student.Fullname}");
        printer.Append($"Jami summa:        {paymentDto.Amount + paymentDto.Discount} so'm");
        printer.Append($"To'langan summa:   {paymentDto.Amount} so'm");
        printer.Append("\n");
        printer.AlignLeft();
        printer.Append($"Kassir: {cashier}");
        printer.Append($"To'lov sanasi:     {paymentDto.PaymentDate:dd/MM/yyyy}");
        printer.Append($"To'lov raqami:\n{paymentDto.ReceiptNumber}");
        printer.Append("\n");

        printer.AlignCenter();
        printer.BoldMode("Tashakkur!");

        printer.Append("\n");

        printer.FullPaperCut();
        printer.PrintDocument();
    }

    public string GetPrinterName()
    {
        if (string.IsNullOrEmpty(printerName))
        {
            printerName = GetSavedPrinterName();
        }

        if (string.IsNullOrEmpty(printerName))
        {
            printerName = GetUsbPrinterName();
        }
        
        return printerName;
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

    public string GetSavedPrinterName()
    {
        try
        {
            StreamReader streamReader = new StreamReader(PRINTER_NAME);
            string res = streamReader.ReadToEnd();
            streamReader.Close();
            return res;
        }
        catch(Exception ex)
        {
            return string.Empty;
        } 
    }

    public void SavePrinter(string name)
    {
        StreamWriter writer = new StreamWriter(PRINTER_NAME);
        writer.Write(name);
        writer.Flush();
        writer.Close();
    }
    public List<string> ConnectedPrinters()
    {
        List<string> printers = new List<string>();
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
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer WHERE PortName LIKE 'USB%'");
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

    public void Dispose()
         => GC.SuppressFinalize(this);
}

