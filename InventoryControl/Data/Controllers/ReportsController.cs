using InventoryControl.Data.Interfaces;
using InventoryControl.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

[Route("api/reports")]
[ApiController]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IDataService _dataService;

    public ReportsController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("sales/pdf")]
    public async Task<IActionResult> GetSalesReportPdf(DateTime? fromDate, DateTime? toDate)
    {
        var sales = await _dataService.GetAllInventoryOperationsAsync();

        var filteredSales = sales
            .Where(o => o.OperationType == OperationType.Sale)
            .Where(o => !fromDate.HasValue || o.Date.Date >= fromDate.Value.Date)
            .Where(o => !toDate.HasValue || o.Date.Date <= toDate.Value.Date);

        var report = filteredSales
            .GroupBy(o => new { o.ProductId, ProductName = o.Product.Name, Price = o.Product.Price })
            .Select(g => new SalesReportRow
            {
                ProductName = g.Key.ProductName,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalSum = g.Sum(x => x.Quantity * g.Key.Price)
            })
            .ToList();

        var doc = new SalesReportDocument(report);
        var pdfBytes = doc.GeneratePdf();

        return File(pdfBytes, "application/pdf", $"SalesReport_{DateTime.Now:yyyyMMdd_HHmm}.pdf");
    }
}