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
                col.Item().Text($"{_teacher.User.Firstname} {_teacher.User.Lastname}")
                    .FontSize(20)
                    .Bold()
                    .FontColor("#003049")
                    .AlignCenter();

                col.Item().Text($"Sanadan: {_startedDate:dd.MM.yyyy} sanagacha: {_finishedDate:dd.MM.yyyy} bo'lgan tushumlar")
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
                        cols.RelativeColumn(3); // Student
                        cols.RelativeColumn(3); // Group
                        cols.RelativeColumn(3); // Payment Date
                        cols.RelativeColumn(2); // Amount
                        cols.RelativeColumn(2); // Discount
                        cols.RelativeColumn(3); // Type
                        cols.RelativeColumn(3); // Status
                    });

                    // header row
                    table.Header(header =>
                    {
                        header.Cell().Text("O'quvchi").Bold();
                        header.Cell().Text("Guruh").Bold();
                        header.Cell().Text("Sana").Bold();
                        header.Cell().Text("Summa").Bold();
                        header.Cell().Text("Chegirma").Bold();
                        header.Cell().Text("To'lov turi").Bold();
                        header.Cell().Text("Holati").Bold();
                    });

                    // data rows
                    foreach (var p in _payments)
                    {
                        table.Cell().Text($"{p.Student.Fullname}");
                        table.Cell().Text(p.Group.Name);
                        table.Cell().Text(p.PaymentDate.ToString("dd.MM.yyyy"));
                        table.Cell().Text($"{p.Amount:C}");
                        table.Cell().Text($"{p.Discount:C}");
                        table.Cell().Text(p.Type.ToString());
                        table.Cell().Text(p.Status.ToString());
                    }

                    // summary row
                    var totalCount = _payments.Count;
                    var totalAmount = _payments.Sum(x => x.Amount - x.Discount);

                    table.Cell().ColumnSpan(7).PaddingTop(10).BorderTop(1).AlignRight().Text(
                        $"Umumiy To'lovlar soni: {totalCount},   Umumiy tushum: {totalAmount:C}"
                    ).Bold();
                });
            });

            // Footer
            page.Footer()
                .AlignRight()
                .Text($"Chop etildi: {DateTime.Now:dd.MM.yyyy HH:mm}");
        });
    }
}
