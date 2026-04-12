using InventoryControl.Data.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class SalesReportDocument : IDocument
{
    private readonly List<SalesReportRow> _report;

    public SalesReportDocument(List<SalesReportRow> report)
    {
        _report = report;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);

            page.Content()
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(100);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Товар");
                        header.Cell().Element(CellStyle).Text("Кол-во");
                        header.Cell().Element(CellStyle).Text("Сумма");
                    });

                    foreach (var row in _report)
                    {
                        table.Cell().Element(CellStyle).Text(row.ProductName);
                        table.Cell().Element(CellStyle).Text(row.TotalQuantity.ToString());
                        table.Cell().Element(CellStyle).Text($"{row.TotalSum:C}");
                    }

                    static IContainer CellStyle(IContainer container) =>
                        container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                });
        });
    }
}