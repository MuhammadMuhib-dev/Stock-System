using ClothStock.Business.Services;
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
    public string ShopAddress { get; set; } = "";
    public string ShopPhone { get; set; } = "";
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
    // Shop name, address, phone and purchase head come from Settings
    public static async Task LoadShopInfoAsync(PurchaseVoucherPdfData d, SettingsService s)
    {
        d.ShopName = await s.GetTextAsync("ShopName", "Shop");
        d.ShopAddress = await s.GetTextAsync("ShopAddress", "");
        d.ShopPhone = await s.GetTextAsync("ShopPhone", "");
        d.PurchaseHead = await s.GetTextAsync("PurchaseHeadName", "Purchases");
    }

    // PDF folder: Settings "VoucherFolder"; if empty, Documents\ClothStock Vouchers
    public static async Task<string> GetFolderAsync(SettingsService s)
    {
        var folder = await s.GetTextAsync("VoucherFolder", "");
        if (string.IsNullOrWhiteSpace(folder))
            folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ClothStock Vouchers");
        Directory.CreateDirectory(folder);
        return folder;
    }

    // ---- Palette ----
    private const string Primary = "#0F766E";   // deep teal
    private const string Accent = "#14B8A6";
    private const string Ink = "#0F172A";
    private const string Muted = "#64748B";
    private const string Line = "#E2E8F0";
    private const string Light = "#F1F5F9";
    private const string Green = "#15803D";
    private const string Amber = "#B45309";
    private const string Red = "#B91C1C";

    public static void Create(string filePath, PurchaseVoucherPdfData d)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var status = d.Balance <= 0 ? "PAID" : d.Paid > 0 ? "PARTIALLY PAID" : "UNPAID";
        var statusColor = d.Balance <= 0 ? Green : d.Paid > 0 ? Amber : Red;

        Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(36);
                page.DefaultTextStyle(x => x.FontSize(9.5f).FontColor(Ink));

                // ================= HEADER =================
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(left =>
                        {
                            left.Item().Text(d.ShopName).FontSize(20).Bold().FontColor(Primary);
                            if (!string.IsNullOrWhiteSpace(d.ShopAddress))
                                left.Item().PaddingTop(2).Text(d.ShopAddress.Trim()).FontSize(9).FontColor(Muted);
                            if (!string.IsNullOrWhiteSpace(d.ShopPhone))
                                left.Item().Text("Tel: " + d.ShopPhone.Trim()).FontSize(9).FontColor(Muted);
                        });

                        row.ConstantItem(210).Column(right =>
                        {
                            right.Item().AlignRight().Text("PURCHASE VOUCHER").FontSize(15).Bold().FontColor(Ink);
                            right.Item().AlignRight().PaddingTop(2).Text(d.VoucherNo).FontSize(11).SemiBold().FontColor(Muted);
                            right.Item().AlignRight().PaddingTop(5)
                                .Background(statusColor).PaddingHorizontal(9).PaddingVertical(2)
                                .Text(status).FontSize(8).Bold().FontColor(Colors.White);
                        });
                    });
                    col.Item().PaddingTop(10).LineHorizontal(2.5f).LineColor(Accent);
                    col.Item().PaddingBottom(10);
                });

                // ================= CONTENT =================
                page.Content().Column(col =>
                {
                    col.Spacing(16);

                    // ---- Supplier + voucher info cards
                    col.Item().ShowEntire().Row(row =>
                    {
                        row.RelativeItem().Element(Card).Column(c =>
                        {
                            c.Spacing(4);
                            c.Item().Text("SUPPLIER").FontSize(8).Bold().FontColor(Primary);
                            c.Item().Text(d.Supplier).FontSize(13).Bold();
                            KV(c, "Bill No", d.InvoiceNo);
                            KV(c, "Bill Date", $"{d.InvoiceDate:dd MMM yyyy}");
                        });

                        row.ConstantItem(14);

                        row.RelativeItem().Element(Card).Column(c =>
                        {
                            c.Spacing(4);
                            c.Item().Text("VOUCHER DETAILS").FontSize(8).Bold().FontColor(Primary);
                            KV(c, "Voucher No", d.VoucherNo);
                            KV(c, "Voucher Date", $"{d.VoucherDate:dd MMM yyyy}");
                            if (d.DueDate.HasValue) KV(c, "Due Date", $"{d.DueDate:dd MMM yyyy}");
                            KV(c, "Prepared By", d.User);
                        });
                    });

                    // ---- Items table
                    col.Item().Column(items =>
                    {
                        items.Item().PaddingBottom(5).Text("ITEMS RECEIVED").FontSize(8).Bold().FontColor(Primary);

                        items.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(24);    // #
                                c.RelativeColumn(3);     // article
                                c.RelativeColumn(2.4f);  // color
                                c.RelativeColumn(1.2f);  // bunches
                                c.RelativeColumn(1.4f);  // suits per bunch
                                c.RelativeColumn(1.3f);  // total suits
                                c.RelativeColumn(1.5f);  // total meters
                                c.RelativeColumn(1.5f);  // cost per suit
                                c.RelativeColumn(2f);    // amount
                            });

                            table.Header(h =>
                            {
                                void H(string s, bool right = false)
                                {
                                    var cell = h.Cell().Element(HeadCell);
                                    if (right) cell.AlignRight().Text(s); else cell.Text(s);
                                }
                                H("#"); H("Article"); H("Color");
                                H("Bunches", true); H("Suits / Bunch", true); H("Total Suits", true);
                                H("Total Meters", true); H("Cost / Suit", true); H("Amount", true);
                            });

                            var i = 0;
                            foreach (var l in d.Lines)
                            {
                                var alt = i % 2 == 1;
                                i++;
                                IContainer B(IContainer c) => Body(c, alt);

                                table.Cell().Element(B).Text(i.ToString()).FontColor(Muted);
                                table.Cell().Element(B).Text(l.Article).SemiBold();
                                table.Cell().Element(B).Text(l.Color);
                                table.Cell().Element(B).AlignRight().Text($"{l.Bunches:N0}");
                                table.Cell().Element(B).AlignRight().Text($"{l.SuitsPerBunch:N0}");
                                table.Cell().Element(B).AlignRight().Text($"{l.Suits:N0}");
                                table.Cell().Element(B).AlignRight().Text($"{l.Meters:0.###}");
                                table.Cell().Element(B).AlignRight().Text($"{l.CostPerSuit:N0}");
                                table.Cell().Element(B).AlignRight().Text($"{l.LineTotal:N0}").SemiBold();
                            }

                            // ---- Totals row
                            table.Cell().ColumnSpan(3).Element(TotalCell).Text("TOTAL");
                            table.Cell().Element(TotalCell).AlignRight().Text($"{d.Lines.Sum(x => x.Bunches):N0}");
                            table.Cell().Element(TotalCell).Text("");
                            table.Cell().Element(TotalCell).AlignRight().Text($"{d.Lines.Sum(x => x.Suits):N0}");
                            table.Cell().Element(TotalCell).AlignRight().Text($"{d.Lines.Sum(x => x.Meters):0.###}");
                            table.Cell().Element(TotalCell).Text("");
                            table.Cell().Element(TotalCell).AlignRight().Text($"{d.Lines.Sum(x => x.LineTotal):N0}");
                        });
                    });

                    // ---- Notes (left) + payment summary (right)
                    col.Item().ShowEntire().Row(row =>
                    {
                        row.RelativeItem().PaddingRight(20).Column(c =>
                        {
                            if (!string.IsNullOrWhiteSpace(d.Notes))
                            {
                                c.Item().Text("NOTES").FontSize(8).Bold().FontColor(Primary);
                                c.Item().PaddingTop(3).Text(d.Notes!.Trim()).FontColor(Muted);
                            }
                        });

                        row.ConstantItem(250).Background(Light).Padding(12).Column(c =>
                        {
                            c.Spacing(5);
                            SumRow(c, "Bill Total", $"{d.Total:N0}", bold: true);
                            SumRow(c, "Paid Now", $"{d.Paid:N0}");
                            if (d.Paid > 0 && !string.IsNullOrWhiteSpace(d.PaidFrom))
                                c.Item().AlignRight().Text($"via {d.PaidFrom}").FontSize(8).FontColor(Muted);
                            c.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Primary);
                            c.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Balance Due").FontSize(11).Bold();
                                r.ConstantItem(110).AlignRight().Text($"{d.Balance:N0}").FontSize(12).Bold()
                                    .FontColor(d.Balance > 0 ? Red : Green);
                            });
                        });
                    });

                    // ---- Accounting entry
                    col.Item().ShowEntire().Column(c =>
                    {
                        c.Item().PaddingBottom(5).Text("ACCOUNTING ENTRY").FontSize(8).Bold().FontColor(Primary);
                        c.Item().Table(t =>
                        {
                            t.ColumnsDefinition(cd => { cd.RelativeColumn(5); cd.RelativeColumn(2); cd.RelativeColumn(2); });
                            t.Header(h =>
                            {
                                h.Cell().Element(HeadCell).Text("Account");
                                h.Cell().Element(HeadCell).AlignRight().Text("Debit");
                                h.Cell().Element(HeadCell).AlignRight().Text("Credit");
                            });

                            var n = 0;
                            void Row(string acct, decimal? debit, decimal? credit)
                            {
                                var alt = n++ % 2 == 1;
                                IContainer B(IContainer x) => Body(x, alt);
                                t.Cell().Element(B).Text(acct);
                                t.Cell().Element(B).AlignRight().Text(debit.HasValue ? $"{debit:N0}" : "");
                                t.Cell().Element(B).AlignRight().Text(credit.HasValue ? $"{credit:N0}" : "");
                            }

                            Row(d.PurchaseHead, d.Total, null);
                            if (d.Balance > 0) Row($"{d.Supplier} (payable)", null, d.Balance);
                            if (d.Paid > 0) Row(string.IsNullOrWhiteSpace(d.PaidFrom) ? "Payment" : d.PaidFrom!, null, d.Paid);

                            t.Cell().Element(TotalCell).Text("Total");
                            t.Cell().Element(TotalCell).AlignRight().Text($"{d.Total:N0}");
                            t.Cell().Element(TotalCell).AlignRight().Text($"{d.Balance + d.Paid:N0}");
                        });
                    });

                    // ---- Signatures
                    col.Item().ShowEntire().PaddingTop(34).Row(r =>
                    {
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().LineHorizontal(0.75f).LineColor(Muted);
                            c.Item().PaddingTop(3).Text("Prepared by" + (string.IsNullOrWhiteSpace(d.User) ? "" : $": {d.User}"))
                                .FontSize(8.5f).FontColor(Muted);
                        });
                        r.ConstantItem(40);
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().LineHorizontal(0.75f).LineColor(Muted);
                            c.Item().PaddingTop(3).Text("Authorised signature").FontSize(8.5f).FontColor(Muted);
                        });
                        r.ConstantItem(40);
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().LineHorizontal(0.75f).LineColor(Muted);
                            c.Item().PaddingTop(3).Text("Supplier signature").FontSize(8.5f).FontColor(Muted);
                        });
                    });
                });

                // ================= FOOTER =================
                page.Footer().BorderTop(0.75f).BorderColor(Line).PaddingTop(6)
                    .DefaultTextStyle(x => x.FontSize(8).FontColor(Muted))
                    .Row(r =>
                    {
                        r.RelativeItem().Text($"Generated on {DateTime.Now:dd MMM yyyy, HH:mm}");
                        r.RelativeItem().AlignCenter().Text(d.VoucherNo);
                        r.RelativeItem().AlignRight().Text(t =>
                        {
                            t.Span("Page ");
                            t.CurrentPageNumber();
                            t.Span(" of ");
                            t.TotalPages();
                        });
                    });
            });
        }).GeneratePdf(filePath);
    }

    // ---------------- Helpers ----------------

    private static IContainer Card(IContainer c) =>
        c.Border(1).BorderColor(Line).Padding(12);

    private static void KV(ColumnDescriptor c, string label, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        c.Item().Row(r =>
        {
            r.ConstantItem(85).Text(label).FontColor(Muted);
            r.RelativeItem().Text(value).SemiBold();
        });
    }

    private static void SumRow(ColumnDescriptor c, string label, string value, bool bold = false)
    {
        c.Item().Row(r =>
        {
            var l = r.RelativeItem().Text(label);
            var v = r.ConstantItem(110).AlignRight().Text(value);
            if (bold) { l.SemiBold(); v.Bold(); }
        });
    }

    private static IContainer HeadCell(IContainer c) =>
        c.Background(Primary).PaddingVertical(6).PaddingHorizontal(6)
         .DefaultTextStyle(x => x.FontColor(Colors.White).SemiBold().FontSize(8.5f));

    private static IContainer Body(IContainer c, bool alt) =>
        c.Background(alt ? Light : Colors.White)
         .BorderBottom(0.5f).BorderColor(Line)
         .PaddingVertical(5).PaddingHorizontal(6);

    private static IContainer TotalCell(IContainer c) =>
        c.Background(Light).BorderTop(1.25f).BorderColor(Primary)
         .PaddingVertical(6).PaddingHorizontal(6)
         .DefaultTextStyle(x => x.Bold());
}