using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Integrated.Security;
using ESC_POS_USB_NET.Printer;
using System.ComponentModel;
using System.Text;

namespace EduFlow.Cashier.Desktop.Services;

public class PrinterService : IDisposable
{
    private readonly string PRINTER_NAME = Properties.Settings.Default.PrinterName;
    public string printerName { get; set; } = string.Empty;
    Printer? printer;
    public PrinterService()
    {
        
    }

    public void Print(PaymentForResultDto paymentDto, double coursePrice)
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
        printer.Append($"To'lov turi:       {paymentDto.Type}");
        printer.Append($"Chegirma:          {paymentDto.Discount} so'm");
        printer.Append($"Jami summa:        {paymentDto.Amount + paymentDto.Discount} so'm");
        printer.Append($"To'langan summa:   {paymentDto.Amount}");
        printer.Append("\n");
        printer.AlignLeft();
        printer.Append($"Kassir: {cashier}");
        printer.Append($"To'lov sanasi:     {paymentDto.PaymentDate:dd/MM/yyyy}");
        printer.Append($"ID: {paymentDto.ReceiptNumber}");
        printer.Append("\n");

        printer.AlignCenter();
        printer.BoldMode("Tashakkur!");

        printer.Append("\n");

        printer.FullPaperCut();
        printer.PrintDocument();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

