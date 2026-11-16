using ClothStock.Models.Stock;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ClothStockSystem.Helpers;

public class PurchaseVoucherPdfLine
{
    public string Article { get; set; } = "";
    public string Color { get; set; } = "";
    public int Bunches { get; set; }
    public int SuitsPerBunch { get; set; }
    public decimal MetersPerSuit { get; set; }
    public decimal CostPerSuit { get; set; }
    public int Suits => Bunches * SuitsPerBunch;
    public decimal Meters => Suits * MetersPerSuit;
    public decimal LineTotal => Suits * CostPerSuit;
}

public class PurchaseVoucherPdfData
{
    public string ShopName { get; set; } = "";
    public string PurchaseHead { get; set; } = "Purchases";
    public string VoucherNo { get; set; } = "";
    public DateTime VoucherDate { get; set; }
    public string Supplier { get; set; } = "";
    public string InvoiceNo { get; set; } = "";
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Notes { get; set; }
    public string? User { get; set; }
    public decimal Total { get; set; }
    public decimal Paid { get; set; }
    public string? PaidFrom { get; set; }
    public decimal Balance => Total - Paid;
    public List<PurchaseVoucherPdfLine> Lines { get; set; } = new();
}

public static class PurchaseVoucherPdf
{
    private static readonly string Dark = Colors.Teal.Darken3;
    private static readonly string Soft = Colors.Grey.Lighten3;

    public static void Create(string filePath, PurchaseVoucherPdfData d)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text(d.ShopName).FontSize(18).Bold().FontColor(Dark);
                    col.Item().Text("PURCHASE VOUCHER").FontSize(12).SemiBold();
                    col.Item().PaddingTop(6).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                });

                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Spacing(12);

                    // ---- voucher / bill ki details
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(t => { t.Span("Voucher no: ").SemiBold(); t.Span(d.VoucherNo); });
                            c.Item().Text(t => { t.Span("Voucher date: ").SemiBold(); t.Span($"{d.VoucherDate:dd-MMM-yyyy}"); });
                            c.Item().Text(t => { t.Span("Supplier: ").SemiBold(); t.Span(d.Supplier); });
                        });
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(t => { t.Span("Bill no: ").SemiBold(); t.Span(d.InvoiceNo); });
                            c.Item().Text(t => { t.Span("Bill date: ").SemiBold(); t.Span($"{d.InvoiceDate:dd-MMM-yyyy}"); });
                            if (d.DueDate.HasValue)
                                c.Item().Text(t => { t.Span("Due date: ").SemiBold(); t.Span($"{d.DueDate:dd-MMM-yyyy}"); });
                        });
                    });

                    // ---- maal ki table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3);   // article
                            c.RelativeColumn(3);   // color
                            c.RelativeColumn(1.2f);// bunches
                            c.RelativeColumn(1.4f);// suits/bunch
                            c.RelativeColumn(1.4f);// kul suits
                            c.RelativeColumn(1.6f);// kul meter
                            c.RelativeColumn(1.8f);// cost/suit
                            c.RelativeColumn(2.2f);// total
                        });

                        table.Header(h =>
                        {
                            void H(string s, bool right = false)
                            {
                                var cell = h.Cell().Element(HeadCell);
                                if (right) cell.AlignRight().Text(s).SemiBold().FontColor(Colors.White);
                                else cell.Text(s).SemiBold().FontColor(Colors.White);
                            }
                            H("Article"); H("Color"); H("Bunches", true); H("Suits/bunch", true);
                            H("Kul suits", true); H("Kul meter", true); H("Cost/suit", true); H("Line total", true);
                        });

                        foreach (var l in d.Lines)
                        {
                            table.Cell().Element(BodyCell).Text(l.Article);
                            table.Cell().Element(BodyCell).Text(l.Color);
                            table.Cell().Element(BodyCell).AlignRight().Text($"{l.Bunches:N0}");
                            table.Cell().Element(BodyCell).AlignRight().Text($"{l.SuitsPerBunch:N0}");
                            table.Cell().Element(BodyCell).AlignRight().Text($"{l.Suits:N0}");
                            table.Cell().Element(BodyCell).AlignRight().Text($"{l.Meters:0.###}");
                            table.Cell().Element(BodyCell).AlignRight().Text($"{l.CostPerSuit:N0}");
                            table.Cell().Element(BodyCell).AlignRight().Text($"{l.LineTotal:N0}");
                        }
                    });

                    // ---- total / ada / baqi
                    col.Item().AlignRight().Width(230).Column(c =>
                    {
                        c.Item().Row(r => { r.RelativeItem().Text("Bill total").SemiBold(); r.ConstantItem(100).AlignRight().Text($"{d.Total:N0}").Bold(); });
                        c.Item().Row(r => { r.RelativeItem().Text("Abhi ada kiya"); r.ConstantItem(100).AlignRight().Text($"{d.Paid:N0}"); });
                        c.Item().PaddingTop(2).LineHorizontal(0.5f);
                        c.Item().Row(r => { r.RelativeItem().Text("Baqi udhaar").SemiBold(); r.ConstantItem(100).AlignRight().Text($"{d.Balance:N0}").Bold(); });
                    });

                    // ---- ledger entry (double entry)
                    col.Item().Column(c =>
                    {
                        c.Item().Text("Ledger entry").SemiBold().FontColor(Dark);
                        c.Item().PaddingTop(4).Table(t =>
                        {
                            t.ColumnsDefinition(cd => { cd.RelativeColumn(5); cd.RelativeColumn(2); cd.RelativeColumn(2); });
                            t.Header(h =>
                            {
                                h.Cell().Element(HeadCell).Text("Account").SemiBold().FontColor(Colors.White);
                                h.Cell().Element(HeadCell).AlignRight().Text("Debit").SemiBold().FontColor(Colors.White);
                                h.Cell().Element(HeadCell).AlignRight().Text("Credit").SemiBold().FontColor(Colors.White);
                            });

                            t.Cell().Element(BodyCell).Text(d.PurchaseHead);
                            t.Cell().Element(BodyCell).AlignRight().Text($"{d.Total:N0}");
                            t.Cell().Element(BodyCell).AlignRight().Text("");

                            if (d.Balance > 0)
                            {
                                t.Cell().Element(BodyCell).Text($"{d.Supplier} (udhaar)");
                                t.Cell().Element(BodyCell).AlignRight().Text("");
                                t.Cell().Element(BodyCell).AlignRight().Text($"{d.Balance:N0}");
                            }
                            if (d.Paid > 0)
                            {
                                t.Cell().Element(BodyCell).Text(d.PaidFrom ?? "Payment");
                                t.Cell().Element(BodyCell).AlignRight().Text("");
                                t.Cell().Element(BodyCell).AlignRight().Text($"{d.Paid:N0}");
                            }
                        });
                    });

                    if (!string.IsNullOrWhiteSpace(d.Notes))
                        col.Item().Text(t => { t.Span("Notes: ").SemiBold(); t.Span(d.Notes!); });

                    // ---- dastakhat
                    col.Item().PaddingTop(30).Row(r =>
                    {
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().LineHorizontal(0.5f);
                            c.Item().Text($"Tayyar karne wala{(string.IsNullOrWhiteSpace(d.User) ? "" : ": " + d.User)}").FontSize(9);
                        });
                        r.ConstantItem(40);
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().LineHorizontal(0.5f);
                            c.Item().Text("Supplier ke dastakhat").FontSize(9);
                        });
                    });
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span($"{d.VoucherNo}   |   Page ").FontSize(8).FontColor(Colors.Grey.Darken1);
                    t.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Darken1);
                    t.Span(" / ").FontSize(8).FontColor(Colors.Grey.Darken1);
                    t.TotalPages().FontSize(8).FontColor(Colors.Grey.Darken1);
                });
            });
        }).GeneratePdf(filePath);
    }

    private static IContainer HeadCell(IContainer c) =>
        c.Background(Dark).PaddingVertical(4).PaddingHorizontal(5);

    private static IContainer BodyCell(IContainer c) =>
        c.BorderBottom(0.5f).BorderColor(Soft).PaddingVertical(4).PaddingHorizontal(5);
}