using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using ClothStock.Models.Orders;

namespace ClothStockSystem.Helpers;

public class SalesReportData
{
    public ReceiptData Shop { get; set; } = new();      // shop ka naam, logo, paper ki chaurai
    public string Period { get; set; } = "";
    public List<OrderSummary> Orders { get; set; } = new();
    public bool ShowProfit { get; set; }                // sirf Owner ke liye
    public string PrintedBy { get; set; } = "";
    public DateTime PrintedAt { get; set; } = DateTime.Now;
}

public static class ReportPrinter
{
    private static decimal D(object? v) => v == null ? 0 : Convert.ToDecimal(v);
    private static string Fmt(decimal v) => ReceiptPrinter.Fmt(v);

    public static void Print(SalesReportData r, IWin32Window? owner = null, bool preview = true)
    {
        var a4 = r.Shop.WidthMm >= 150;                 // 150mm se chauda paper = A4
        var doc = new PrintDocument { DocumentName = "Sales Report" };

        if (a4)
        {
            try
            {
                var size = doc.PrinterSettings.PaperSizes.Cast<PaperSize>().FirstOrDefault(p => p.Kind == PaperKind.A4);
                if (size != null) doc.DefaultPageSettings.PaperSize = size;
            }
            catch { /* printer na ho to default paper */ }
            doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            var idx = 0;
            var page = 0;
            doc.PrintPage += (s, e) =>
            {
                var g = e.Graphics!;
                g.PageUnit = GraphicsUnit.Point;
                e.HasMorePages = DrawA4Page(r, g, e.PageBounds.Width * 0.72f, e.PageBounds.Height * 0.72f, ref idx, ++page);
            };
        }
        else
        {
            var wPts = r.Shop.WidthMm / 25.4f * 72f;
            float hPts;
            using (var bmp = new Bitmap(1, 1))
            using (var mg = Graphics.FromImage(bmp))
            {
                mg.PageUnit = GraphicsUnit.Point;
                hPts = DrawThermal(r, mg, wPts) + 14;
            }
            doc.DefaultPageSettings.PaperSize =
                new PaperSize("Report", (int)(wPts / 72f * 100f), (int)(hPts / 72f * 100f));
            doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            doc.PrintPage += (s, e) =>
            {
                var g = e.Graphics!;
                g.PageUnit = GraphicsUnit.Point;
                DrawThermal(r, g, wPts);
                e.HasMorePages = false;
            };
        }

        if (preview)
        {
            using var dlg = new PrintPreviewDialog { Document = doc, Width = a4 ? 900 : 600, Height = 850 };
            dlg.ShowDialog(owner);
        }
        else doc.Print();
    }

    // ------------------------------------------------------------ thermal (80mm / 58mm)

    private static float DrawThermal(SalesReportData r, Graphics g, float pageW)
    {
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        var k = pageW < 190 ? 0.88f : 1f;
        float pad = pageW < 190 ? 5 : 8;
        float x = pad, w = pageW - pad * 2, y = 8;

        using var fSmall = new System.Drawing.Font(ReceiptPrinter.Family, 7.5f * k);
        using var fReg = new System.Drawing.Font(ReceiptPrinter.Family, 8.5f * k);
        using var fBold = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 8.5f * k, System.Drawing.FontStyle.Bold);
        using var fHead = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 9f * k, System.Drawing.FontStyle.Bold);
        using var fShop = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 14f * k, System.Drawing.FontStyle.Bold);
        using var fBig = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 11.5f * k, System.Drawing.FontStyle.Bold);
        using var wrap = new StringFormat { Trimming = StringTrimming.Word };
        using var center = new StringFormat { Alignment = StringAlignment.Center, Trimming = StringTrimming.Word };
        using var right = new StringFormat { Alignment = StringAlignment.Far, FormatFlags = StringFormatFlags.NoWrap };
        using var dash = new Pen(System.Drawing.Color.Black, 0.75f) { DashStyle = DashStyle.Dash };
        using var thick = new Pen(System.Drawing.Color.Black, 1.8f);
        using var thin = new Pen(System.Drawing.Color.Black, 0.75f);

        float TextH(string t, System.Drawing.Font f, float width, StringFormat sf) =>
            g.MeasureString(t, f, new SizeF(width, 0), sf).Height;

        void CenterText(string t, System.Drawing.Font f, float gap = 0)
        {
            if (string.IsNullOrWhiteSpace(t)) return;
            var h = TextH(t, f, w, center);
            g.DrawString(t, f, Brushes.Black, new RectangleF(x, y, w, h), center);
            y += h + gap;
        }

        void Row(string l, string rv, System.Drawing.Font lf, System.Drawing.Font? rf = null, float gap = 2)
        {
            rf ??= lf;
            var rw = g.MeasureString(rv, rf).Width + 2;
            var lw = Math.Max(20, w - rw - 4);
            var lh = TextH(l, lf, lw, wrap);
            var h = Math.Max(lh, rf.GetHeight(g));
            g.DrawString(l, lf, Brushes.Black, new RectangleF(x, y, lw, h), wrap);
            g.DrawString(rv, rf, Brushes.Black, new RectangleF(x + w - rw, y, rw, h), right);
            y += h + gap;
        }

        void Dashed() { y += 3; g.DrawLine(dash, x, y, x + w, y); y += 5; }
        void Double()
        {
            y += 2; g.DrawLine(thick, x, y, x + w, y);
            y += 3; g.DrawLine(thin, x, y, x + w, y); y += 5;
        }

        // Header
        var shop = r.Shop;
        if (shop.Logo != null)
        {
            var lw = Math.Min(w * 0.72f, 170f);
            var lh = lw * shop.Logo.Height / shop.Logo.Width;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(shop.Logo, x + (w - lw) / 2, y, lw, lh);
            y += lh + 5;
        }
        else CenterText(shop.ShopName.ToUpperInvariant(), fShop, 2);
        Double();
        CenterText("S A L E S   R E P O R T", fHead, 2);
        CenterText(r.Period, fBold, 2);
        Dashed();
        Row("Printed", r.PrintedAt.ToString("dd-MMM-yyyy hh:mm tt"), fSmall);
        if (!string.IsNullOrWhiteSpace(r.PrintedBy)) Row("By", r.PrintedBy, fSmall);
        Dashed();

        // Totals
        var sold = r.Orders.Where(o => o.Status != "Cancelled").ToList();
        var cancelled = r.Orders.Count - sold.Count;
        var amount = sold.Sum(o => D(o.NetSale));
        var profit = sold.Sum(o => D(o.Profit));
        var suits = sold.Sum(o => D(o.Suits));

        Row("Total Sales", sold.Count.ToString("N0"), fReg, fBold);
        Row("Total Suits", suits.ToString("N0"), fReg, fBold);
        y += 3;
        var bh = fBig.GetHeight(g) + 8;
        g.FillRectangle(Brushes.Black, x, y, w, bh);
        g.DrawString("TOTAL AMOUNT", fBig, Brushes.White, new RectangleF(x + 5, y + 4, w, bh), wrap);
        g.DrawString("Rs. " + Fmt(amount), fBig, Brushes.White, new RectangleF(x, y + 4, w - 5, bh), right);
        y += bh + 5;
        if (r.ShowProfit) Row("Profit", Fmt(profit), fReg, fBold);
        if (cancelled > 0) Row("Cancelled (ginti mein nahi)", cancelled.ToString("N0"), fSmall);

        // Sale-wise list
        Dashed();
        CenterText("SALE-WISE", fHead, 3);
        foreach (var o in sold)
        {
            Row(o.OrderNo ?? "", Fmt(D(o.NetSale)), fBold);
            var cust = string.IsNullOrWhiteSpace(o.CustomerName) ? "Walk-in" : o.CustomerName;
            var line = $"{o.OrderDate:dd-MMM} | {cust} | {D(o.Suits):N0} suits";
            var h = TextH(line, fSmall, w - 8, wrap);
            g.DrawString(line, fSmall, Brushes.Black, new RectangleF(x + 8, y, w - 8, h), wrap);
            y += h + 4;
        }
        if (sold.Count == 0) CenterText("Is period mein koi sale nahi.", fSmall, 4);

        Dashed();
        CenterText("*** End of report ***", fSmall);
        return y;
    }

    // ------------------------------------------------------------ A4 (multi-page table)

    // true wapas karta hai agar agla page bhi hai
    private static bool DrawA4Page(SalesReportData r, Graphics g, float pw, float ph, ref int idx, int page)
    {
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        const float m = 40f;
        float x = m, w = pw - m * 2, y = m;

        using var fSmall = new System.Drawing.Font(ReceiptPrinter.Family, 8f);
        using var fReg = new System.Drawing.Font(ReceiptPrinter.Family, 9f);
        using var fBold = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 9f, System.Drawing.FontStyle.Bold);
        using var fHead = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 9f, System.Drawing.FontStyle.Bold);
        using var fTitle = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 18f, System.Drawing.FontStyle.Bold);
        using var fStat = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 15f, System.Drawing.FontStyle.Bold);
        using var left = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        using var right = new StringFormat { Alignment = StringAlignment.Far, FormatFlags = StringFormatFlags.NoWrap };
        using var center = new StringFormat { Alignment = StringAlignment.Center };
        using var thin = new Pen(System.Drawing.Color.Black, 0.75f);
        using var thick = new Pen(System.Drawing.Color.Black, 1.8f);
        using var zebra = new SolidBrush(System.Drawing.Color.FromArgb(240, 240, 240));

        var sold = r.Orders.Where(o => o.Status != "Cancelled").ToList();
        var cancelled = r.Orders.Count - sold.Count;
        var amount = sold.Sum(o => D(o.NetSale));
        var profit = sold.Sum(o => D(o.Profit));
        var suits = sold.Sum(o => D(o.Suits));

        // Columns
        float cNo = 28, cSale = 100, cDate = 72, cSuits = 48, cAmt = 84, cProf = r.ShowProfit ? 84 : 0;
        var cCust = w - (cNo + cSale + cDate + cSuits + cAmt + cProf);
        var xNo = x; var xSale = xNo + cNo; var xDate = xSale + cSale; var xCust = xDate + cDate;
        var xSuits = xCust + cCust; var xAmt = xSuits + cSuits; var xProf = xAmt + cAmt;

        if (page == 1)
        {
            // Header
            var shop = r.Shop;
            if (shop.Logo != null)
            {
                var lw = 130f;
                var lh = lw * shop.Logo.Height / shop.Logo.Width;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(shop.Logo, x, y, lw, lh);
                y += lh;
            }
            else
            {
                g.DrawString(shop.ShopName.ToUpperInvariant(), fTitle, Brushes.Black, x, y);
                y += fTitle.GetHeight(g);
            }
            if (!string.IsNullOrWhiteSpace(shop.Address)) { g.DrawString(shop.Address, fSmall, Brushes.Black, x, y); y += fSmall.GetHeight(g); }
            if (!string.IsNullOrWhiteSpace(shop.ShopPhone)) { g.DrawString("Tel: " + shop.ShopPhone, fSmall, Brushes.Black, x, y); y += fSmall.GetHeight(g); }

            // Right side title
            g.DrawString("SALES REPORT", fTitle, Brushes.Black, new RectangleF(x, m, w, 30), right);
            g.DrawString(r.Period, fBold, Brushes.Black, new RectangleF(x, m + 28, w, 18), right);
            var pr = $"Printed: {r.PrintedAt:dd-MMM-yyyy hh:mm tt}" + (string.IsNullOrWhiteSpace(r.PrintedBy) ? "" : $"  |  By: {r.PrintedBy}");
            g.DrawString(pr, fSmall, Brushes.Black, new RectangleF(x, m + 46, w, 14), right);

            y += 8;
            g.DrawLine(thick, x, y, x + w, y);
            y += 12;

            // Stat boxes
            var labels = new List<(string, string)>
            {
                ("SALES", sold.Count.ToString("N0")),
                ("SUITS", suits.ToString("N0")),
                ("AMOUNT", "Rs. " + Fmt(amount)),
                r.ShowProfit ? ("PROFIT", "Rs. " + Fmt(profit)) : ("CANCELLED", cancelled.ToString("N0"))
            };
            const float gap = 8f;
            var bw = (w - gap * 3) / 4;
            for (var i = 0; i < 4; i++)
            {
                var bx = x + i * (bw + gap);
                g.DrawRectangle(thin, bx, y, bw, 48);
                g.DrawString(labels[i].Item1, fSmall, Brushes.Black, new RectangleF(bx, y + 5, bw, 12), center);
                g.DrawString(labels[i].Item2, fStat, Brushes.Black, new RectangleF(bx, y + 19, bw, 26), center);
            }
            y += 48 + 14;
            if (r.ShowProfit && cancelled > 0)
            {
                g.DrawString($"Cancelled sales (ginti mein nahi): {cancelled:N0}", fSmall, Brushes.Black, x, y - 8);
                y += 8;
            }
        }
        else
        {
            g.DrawString($"{r.Shop.ShopName}  -  Sales Report  ({r.Period})", fSmall, Brushes.Black, x, y);
            y += 20;
        }

        // Table header
        const float rowH = 19f;
        g.FillRectangle(Brushes.Black, x, y, w, rowH + 2);
        float ty = y + 3;
        g.DrawString("#", fHead, Brushes.White, new RectangleF(xNo + 4, ty, cNo, rowH), left);
        g.DrawString("SALE NO", fHead, Brushes.White, new RectangleF(xSale, ty, cSale, rowH), left);
        g.DrawString("DATE", fHead, Brushes.White, new RectangleF(xDate, ty, cDate, rowH), left);
        g.DrawString("CUSTOMER", fHead, Brushes.White, new RectangleF(xCust, ty, cCust, rowH), left);
        g.DrawString("SUITS", fHead, Brushes.White, new RectangleF(xSuits, ty, cSuits - 6, rowH), right);
        g.DrawString("AMOUNT", fHead, Brushes.White, new RectangleF(xAmt, ty, cAmt - 6, rowH), right);
        if (r.ShowProfit) g.DrawString("PROFIT", fHead, Brushes.White, new RectangleF(xProf, ty, cProf - 6, rowH), right);
        y += rowH + 2;

        // Rows (neeche totals ke liye 2 row ki jagah chhodte hain)
        var limit = ph - m - 30;
        while (idx < sold.Count && y + rowH * 2 < limit)
        {
            var o = sold[idx];
            if (idx % 2 == 1) g.FillRectangle(zebra, x, y, w, rowH);
            var ry = y + 3;
            var cust = string.IsNullOrWhiteSpace(o.CustomerName) ? "Walk-in" : o.CustomerName;
            g.DrawString((idx + 1).ToString(), fReg, Brushes.Black, new RectangleF(xNo + 4, ry, cNo, rowH), left);
            g.DrawString(o.OrderNo ?? "", fReg, Brushes.Black, new RectangleF(xSale, ry, cSale - 4, rowH), left);
            g.DrawString($"{o.OrderDate:dd-MMM-yy}", fReg, Brushes.Black, new RectangleF(xDate, ry, cDate, rowH), left);
            g.DrawString(cust, fReg, Brushes.Black, new RectangleF(xCust, ry, cCust - 6, rowH), left);
            g.DrawString(D(o.Suits).ToString("N0"), fReg, Brushes.Black, new RectangleF(xSuits, ry, cSuits - 6, rowH), right);
            g.DrawString(Fmt(D(o.NetSale)), fReg, Brushes.Black, new RectangleF(xAmt, ry, cAmt - 6, rowH), right);
            if (r.ShowProfit) g.DrawString(Fmt(D(o.Profit)), fReg, Brushes.Black, new RectangleF(xProf, ry, cProf - 6, rowH), right);
            y += rowH;
            idx++;
        }

        var more = idx < sold.Count;
        if (!more)
        {
            if (sold.Count == 0)
                g.DrawString("Is period mein koi sale nahi.", fReg, Brushes.Black, new RectangleF(x, y + 6, w, rowH), center);
            g.DrawLine(thick, x, y + 2, x + w, y + 2);
            y += 4;
            g.DrawString("TOTAL", fBold, Brushes.Black, new RectangleF(xCust, y + 3, cCust, rowH), left);
            g.DrawString(suits.ToString("N0"), fBold, Brushes.Black, new RectangleF(xSuits, y + 3, cSuits - 6, rowH), right);
            g.DrawString(Fmt(amount), fBold, Brushes.Black, new RectangleF(xAmt, y + 3, cAmt - 6, rowH), right);
            if (r.ShowProfit) g.DrawString(Fmt(profit), fBold, Brushes.Black, new RectangleF(xProf, y + 3, cProf - 6, rowH), right);
        }

        // Page number
        g.DrawString($"Page {page}", fSmall, Brushes.Black, new RectangleF(x, ph - m, w, 14), right);
        return more;
    }
}