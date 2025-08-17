using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Teacher;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EduFlow.Desktop.Helpers;

public class PDFGenerator : IDocument
{
    private readonly TeacherForResultDto _teacher;
    private readonly List<PaymentForResultDto> _payments;
    private readonly DateTime _startedDate;
    private readonly DateTime _finishedDate;

    public PDFGenerator(
        TeacherForResultDto teacher,
        List<PaymentForResultDto> payments,
        DateTime startedDate,
        DateTime finishedDate)
    {
        _teacher = teacher;
        _payments = payments;
        _startedDate = startedDate;
        _finishedDate = finishedDate;
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);

            // Header
            page.Header().Column(col =>
            {
                col.Item().Text("📄 To'lov Hisoboti")
                    .FontSize(20)
                    .Bold()
                    .FontColor("#003049")
                    .AlignCenter();

                col.Item().Text($"{_teacher.User.Firstname} {_teacher.User.Lastname}")
                    .FontSize(14)
                    .Bold()
                    .AlignCenter();

                col.Item().Text($"{_startedDate:dd.MM.yyyy} - {_finishedDate:dd.MM.yyyy} oralig'idagi tushumlar")
                    .FontSize(12)
                    .FontColor("#555")
                    .AlignCenter();
            });

            // Content
            page.Content().PaddingVertical(10).Column(column =>
            {
                column.Item().Table(table =>
                {
                    // define columns
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(4); // Student
                        cols.RelativeColumn(3); // Date
                        cols.RelativeColumn(3); // Amount
                        cols.RelativeColumn(3); // Discount
                        cols.RelativeColumn(3); // Status
                    });

                    // header row
                    table.Header(header =>
                    {
                        header.Cell().Background("#003049").Padding(5).Text("O'quvchi").FontColor("#fff").Bold();
                        header.Cell().Background("#003049").Padding(5).Text("Sana").FontColor("#fff").Bold();
                        header.Cell().Background("#003049").Padding(5).Text("Summa").FontColor("#fff").Bold();
                        header.Cell().Background("#003049").Padding(5).Text("Chegirma").FontColor("#fff").Bold();
                        header.Cell().Background("#003049").Padding(5).Text("Holati").FontColor("#fff").Bold();
                    });

                    // data rows
                    foreach (var p in _payments)
                    {
                        table.Cell()
                            .PaddingVertical(7)   // yuqoridan va pastdan 7px bo‘sh joy
                            .PaddingHorizontal(5) // chap-o‘ngdan 5px
                            .BorderBottom(1)
                            .BorderColor("#cccccc")
                            .Text(p.Student.Fullname);

                        table.Cell()
                            .PaddingVertical(7)
                            .PaddingHorizontal(5)
                            .BorderBottom(1)
                            .BorderColor("#cccccc")
                            .Text(p.PaymentDate.ToString("dd.MM.yyyy"));

                        table.Cell()
                            .PaddingVertical(7)
                            .PaddingHorizontal(5)
                            .BorderBottom(1)
                            .BorderColor("#cccccc")
                            .Text($"{p.Amount}");

                        table.Cell()
                            .PaddingVertical(7)
                            .PaddingHorizontal(5)
                            .BorderBottom(1)
                            .BorderColor("#cccccc")
                            .Text($"{p.Discount}");

                        table.Cell()
                            .PaddingVertical(7)
                            .PaddingHorizontal(5)
                            .BorderBottom(1)
                            .BorderColor("#cccccc")
                            .Text(p.Status switch
                            {
                                Domain.Enums.PaymentStatus.Pending => "Kutilmoqda",
                                Domain.Enums.PaymentStatus.Completed => "Yakunlangan",
                                Domain.Enums.PaymentStatus.Refunded => "Qaytarilgan",
                                Domain.Enums.PaymentStatus.Failed => "Muvaffaqiyatsiz"
                            });
                    }


                    // summary row
                    var totalCount = _payments.Count;
                    var totalAmount = _payments.Sum(x => x.Amount);
                    var totalDiscount = _payments.Sum(x => x.Discount);
                    var netAmount = totalAmount - totalDiscount;

                    table.Cell().ColumnSpan(5).Padding(8).Background("#e0e0e0").Text(
                        $"Umumiy to'lovlar soni: {totalCount}   |   Umumiy summa: {totalAmount}   |   Umumiy chegirma: {totalDiscount}   |   Sof tushum: {netAmount}"
                    ).Bold().AlignCenter();
                });
            });

            // Footer
            page.Footer()
                .AlignRight()
                .Text($"Chop etildi: {DateTime.Now:dd.MM.yyyy HH:mm}");
        });
    }
}