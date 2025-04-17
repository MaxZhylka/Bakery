using System.Globalization;
using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Infrastructure.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace backend.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<byte[]> GenerateLoanApplicationsByMonthPdfAsync(DateTime from, DateTime to)
        {
            var reportData = await _reportRepository.GetApplicationsByMonthAsync(from, to);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(column =>
                    {
                        column.Item().Text("Звіт: Заявки на займи")
                            .FontSize(18)
                            .Bold()
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Text($"Період: {from:dd.MM.yyyy} - {to:dd.MM.yyyy}")
                            .FontSize(12)
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Місяць/Рік").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Всього заявок").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Відхилено").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Відсоток відхилень").Bold().FontFamily("Microsoft Sans Serif");
                            });

                            foreach (var item in reportData)
                            {
                                var monthYear = $"{item.Month:D2}/{item.Year}";
                                table.Cell().Text(monthYear).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(item.Total.ToString()).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(item.Rejected.ToString()).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(item.RejectedPercent.ToString("F2") + "%").FontFamily("Microsoft Sans Serif");
                            }
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateMoneyFlowReportPdfAsync(DateTime from, DateTime to)
        {
            var reportData = await _reportRepository.GetMoneyFlowAsync(from, to);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(column =>
                    {
                        column.Item().Text("Звіт: Грошовий обіг")
                            .FontSize(18)
                            .Bold()
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Text($"Період: {from:dd.MM.yyyy} - {to:dd.MM.yyyy}")
                            .FontSize(12)
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Text($"Видано: {reportData.TotalIssued.ToString("C", new CultureInfo("uk-UA"))}")
                            .FontSize(14)
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Text($"Повернуто: {reportData.TotalReturned.ToString("C", new CultureInfo("uk-UA"))}")
                            .FontSize(14)
                            .FontFamily("Microsoft Sans Serif");
                    });
                });
            });
            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateUserApplicationsReportPdfAsync(DateTime from, DateTime to)
        {
            var reportData = await _reportRepository.GetApplicationsPerUserAsync(from, to);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(column =>
                    {
                        column.Item().Text("Звіт: Заявки за клієнтами")
                            .FontSize(18)
                            .Bold()
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Text($"Період: {from:dd.MM.yyyy} - {to:dd.MM.yyyy}")
                            .FontSize(12)
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Електронна пошта").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Кількість заявок").Bold().FontFamily("Microsoft Sans Serif");
                            });

                            foreach (var item in reportData)
                            {
                                table.Cell().Text(item.Email).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(item.ApplicationsCount.ToString()).FontFamily("Microsoft Sans Serif");
                            }
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateMonthlyPaymentsReportPdfAsync(DateTime from, DateTime to)
        {
            var reportData = await _reportRepository.GetMonthlyPaymentsAsync(from, to);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(column =>
                    {
                        column.Item().Text("Звіт: Платежі по місяцях")
                            .FontSize(18)
                            .Bold()
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Text($"Період: {from:dd.MM.yyyy} - {to:dd.MM.yyyy}")
                            .FontSize(12)
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Місяць/Рік").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Кількість платежів").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Середня сума").Bold().FontFamily("Microsoft Sans Serif");
                            });

                            foreach (var item in reportData)
                            {
                                var monthYear = $"{item.Month:D2}/{item.Year}";
                                table.Cell().Text(monthYear).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(item.PaymentsCount.ToString()).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(item.AverageAmount.ToString("C", new CultureInfo("uk-UA")))
                                    .FontFamily("Microsoft Sans Serif");
                            }
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateCompletedLoansReportPdfAsync()
        {
            var loans = await _reportRepository.GetCompletedLoansAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(column =>
                    {
                        column.Item().Text("Звіт: Завершені позики")
                            .FontSize(18)
                            .Bold()
                            .FontFamily("Microsoft Sans Serif");

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID позики").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Сума позики").Bold().FontFamily("Microsoft Sans Serif");
                                header.Cell().Text("Дата створення").Bold().FontFamily("Microsoft Sans Serif");
                            });

                            foreach (var loan in loans)
                            {
                                table.Cell().Text(loan.Id.ToString()).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(loan.ValueToPay.ToString("C", new CultureInfo("uk-UA"))).FontFamily("Microsoft Sans Serif");
                                table.Cell().Text(loan.CreatedAt.ToString("dd.MM.yyyy")).FontFamily("Microsoft Sans Serif");
                            }
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }
    }
}
