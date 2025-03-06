using System.Globalization;
using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Infrastructure.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace backend.Core.Services
{
  public class ReportService(IReportRepository reportRepository) : IReportService
  {
    private readonly IReportRepository _reportRepository = reportRepository;

    public async Task<byte[]> GenerateProductReportAsync()
    {
      var reportData = await _reportRepository.GetReportByProductAsync();

      var document = Document.Create(container =>
      {
        container.Page(page =>
              {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Content().Column(column =>
                      {
                        column.Item().Text("Product Report")
                              .FontSize(18).Bold().FontFamily("Microsoft Sans Serif");


                        column.Item().Table(table =>
                          {
                            table.ColumnsDefinition(c =>
                              {
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                              });
                            table.Header(header =>
                              {
                                header.Cell().Text("Назва продукту").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Всього продано").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Повний прибуток").Bold().FontFamily("Microsoft Sans Serif");
                              });

                            foreach (var item in reportData)
                            {
                              table.Cell().Text(item.ProductName).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.TotalSold.ToString()).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.TotalRevenue.ToString("C", new CultureInfo("uk-UA"))).FontFamily("Microsoft Sans Serif");
                            }
                          });
                      });
              });
      });

      return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateCustomerReportAsync()
    {
      var reportData = await _reportRepository.GetReportByCustomerAsync();

      var document = Document.Create(container =>
      {
        container.Page(page =>
              {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Content().Column(column =>
                      {
                        column.Item().Text("Customer Report").FontSize(18).Bold().FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                          {
                            table.ColumnsDefinition(c =>
                              {
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                              });

                            table.Header(header =>
                              {
                                header.Cell().Text("Ім\'я користувача").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Всього замовлень").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Повний прибуток").Bold().FontFamily("Microsoft Sans Serif");
                              });

                            foreach (var item in reportData)
                            {
                              table.Cell().Text(item.CustomerName).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.TotalOrders.ToString()).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.TotalSpent.ToString("C", new CultureInfo("uk-UA"))).FontFamily("Microsoft Sans Serif");
                            }
                          });
                      });
              });
      });

      return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateAllOrdersReportAsync()
    {
      var reportData = await _reportRepository.GetAllOrdersReportAsync();

      var document = Document.Create(container =>
      {
        container.Page(page =>
              {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Content().Column(column =>
                      {
                        column.Item().Text("All Orders Report")
                              .FontSize(18).Bold().FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                          {
                            table.ColumnsDefinition(c =>
                              {
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                              });

                            table.Header(header =>
                              {
                                header.Cell().Text("Ім\'я користувача").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Назва продукту").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Кількість продукту").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Вартість замовлення").Bold().FontFamily("Microsoft Sans Serif");
                              });

                            foreach (var item in reportData)
                            {
                              table.Cell().Text(item.CustomerName).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.ProductName).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.ProductCount.ToString()).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.Price.ToString("C", new CultureInfo("uk-UA"))).FontFamily("Microsoft Sans Serif");
                            }
                          });
                      });
              });
      });

      return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateOrderTrendsByCustomerReportAsync()
    {
      var reportData = await _reportRepository.GetOrderTrendsByCustomerAsync();

      var document = Document.Create(container =>
      {
        container.Page(page =>
              {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Content().Column(column =>
                      {
                        column.Item().Text("Order Trends by Customer Report")
                              .FontSize(18).Bold().FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                          {
                            table.ColumnsDefinition(c =>
                              {
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                              });

                            table.Header(header =>
                              {
                                header.Cell().Text("Ім\'я користувача").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Дата замовлення").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Всього замовлень").Bold().FontFamily("Microsoft Sans Serif");
                              });

                            foreach (var item in reportData)
                            {
                              table.Cell().Text(item.CustomerName).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.OrderMonth).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.TotalOrders.ToString()).FontFamily("Microsoft Sans Serif");
                            }
                          });
                      });
              });
      });

      return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateOrderTrendsByProductReportAsync()
    {
      var reportData = await _reportRepository.GetOrderTrendsByProductAsync();

      var document = Document.Create(container =>
      {
        container.Page(page =>
              {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Content().Column(column =>
                      {
                        column.Item().Text("Order Trends by Product Report")
                              .FontSize(18).Bold().FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                          {
                            table.ColumnsDefinition(c =>
                              {
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                              });

                            table.Header(header =>
                              {
                                header.Cell().Text("Назва продукту").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Дата замовлення").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Усього продано").Bold().FontFamily("Microsoft Sans Serif");
                              });

                            foreach (var item in reportData)
                            {
                              table.Cell().Text(item.ProductName).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.OrderMonth).FontFamily("Microsoft Sans Serif");
                              table.Cell().Text(item.TotalSold.ToString()).FontFamily("Microsoft Sans Serif");
                            }
                          });
                      });
              });
      });

      return document.GeneratePdf();
    }

    private byte[] GenerateChart(IEnumerable<ProductReportDto> data)
    {
      var items = data.ToList();
      var maxValue = items.Any() ? items.Max(d => d.TotalSold) : 0;
      if (maxValue == 0) maxValue = 1;

      int width = 600;
      int height = 400;
      int margin = 40;
      int marginBottom = 60;
      int barSpacing = 10;

      using var surface = SKSurface.Create(new SKImageInfo(width, height));
      var canvas = surface.Canvas;

      canvas.Clear(SKColors.White);

      var barPaint = new SKPaint
      {
        Color = SKColors.DodgerBlue,
        IsAntialias = true
      };

      var textPaint = new SKPaint
      {
        Color = SKColors.Black,
        TextSize = 14,
        IsAntialias = true
      };

      int count = items.Count;
      float availableWidth = (width - margin * 2f - (count - 1) * barSpacing);
      float barWidth = (count > 0) ? (availableWidth / count) : 0;

      float chartHeight = height - marginBottom;

      for (int i = 0; i < count; i++)
      {
        var item = items[i];
        var productName = item.ProductName;
        var sold = item.TotalSold;

        float barHeight = (sold / (float)maxValue) * (chartHeight - margin);

        float x = margin + i * (barWidth + barSpacing);
        float y = chartHeight - barHeight;

        var rect = new SKRect(x, y, x + barWidth, chartHeight);
        canvas.DrawRect(rect, barPaint);

        var valueText = sold.ToString();
        var textWidth = textPaint.MeasureText(valueText);
        float textX = x + barWidth / 2 - textWidth / 2;
        float textY = y - 5;
        canvas.DrawText(valueText, textX, textY, textPaint);

        canvas.Save();
        float labelX = x + barWidth / 2;
        float labelY = height - 20;
        canvas.Translate(labelX, labelY);
        canvas.RotateDegrees(-45);
        canvas.DrawText(productName, 0, 0, textPaint);
        canvas.Restore();
      }

      using var image = surface.Snapshot();
      using var dataPng = image.Encode(SKEncodedImageFormat.Png, 100);
      using var ms = new MemoryStream();
      dataPng.SaveTo(ms);
      return ms.ToArray();
    }
  }
}
