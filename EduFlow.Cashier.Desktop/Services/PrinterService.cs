using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Cashier.Desktop.State;
using EduFlow.Desktop.Integrated.Security;
using ESC_POS_USB_NET.Printer;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Media;

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

        int maxWidth = 384;

        if(!string.IsNullOrEmpty(AppState.LogoFilePath) && File.Exists(AppState.LogoFilePath))
        {
            try
            {
                using (var imgTemp = Image.FromFile(AppState.LogoFilePath))
                {
                    using (var img = new Bitmap(imgTemp))
                    {
                        printer.AlignCenter();
                        printer.Image(img);
                        printer.Append("\n");
                    }
                }

            }
            catch (Exception ex)
            {
                printer.AlignCenter();
                printer.DoubleWidth2();
                printer.Append(AppState.Name);
                printer.Separator();
            }
        }

        printer.AlignLeft();
        printer.Append($"Kurs to'lovi:      {coursePrice} so'm");
        printer.Append($"To'lov turi:       {paymentType}");
        printer.Append($"Chegirma:          {paymentDto.Discount} so'm");
        printer.Append($"Yo'nalish:         {course}");
        printer.Append($"O'qituvchi:\n{teacher}");
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
    private Bitmap ResizeAndConvertToMonochrome(Image original, int maxWidth)
    {
        int newWidth = Math.Min(maxWidth, original.Width);
        int newHeight = (int)(original.Height * ((float)newWidth / original.Width));

        // 24-bit RGB format (safer for Get/SetPixel)
        Bitmap resized = new Bitmap(original, new Size(newWidth, newHeight));
        Bitmap bwBitmap = new Bitmap(resized.Width, resized.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        for (int y = 0; y < resized.Height; y++)
        {
            for (int x = 0; x < resized.Width; x++)
            {
                var pixelColor = resized.GetPixel(x, y);
                int grayscale = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                var bwColor = grayscale < 128 ? System.Drawing.Color.Black : System.Drawing.Color.White;
                bwBitmap.SetPixel(x, y, bwColor);
            }
        }

        resized.Dispose();
        return bwBitmap;
    }

    public void Dispose()
         => GC.SuppressFinalize(this);
}

