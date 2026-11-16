using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace ClothStockSystem.Helpers;

public enum PaperMode { A4Portrait, A4Landscape, Thermal }

/// <summary>Ek table (grid) ka print-ready snapshot.</summary>
public class PrintSection
{
    public string Title { get; set; } = "";
    public string[] Headers { get; set; } = Array.Empty<string>();
    public bool[] RightAlign { get; set; } = Array.Empty<bool>();
    public List<string[]> Rows { get; set; } = new();
    public bool LastRowIsTotal { get; set; }
}

/// <summary>Summary ka ek card (label + value).</summary>
public record PrintItem(string Label, string Value);

/// <summary>Summary ka ek group (title + cards).</summary>
public class PrintSummary
{
    public string Title { get; set; } = "";
    public List<PrintItem> Items { get; set; } = new();
}

public class GridReportData
{
    public ReceiptData Shop { get; set; } = new();      // shop ka naam, logo, paper ki chaurai
    public string Title { get; set; } = "REPORT";
    public string Period { get; set; } = "";
    public List<PrintSection> Sections { get; set; } = new();
    public List<PrintSummary> Summaries { get; set; } = new();
    public string PrintedBy { get; set; } = "";
    public DateTime PrintedAt { get; set; } = DateTime.Now;
}

public static class GridReportPrinter
{
    // Report ke aakhir mein ye naam (ya logo) chhapta hai
    private const string BrandName = "VASL";

    // ------------------------------------------------ A4 theme (Sky Blue, print-friendly)
    private static readonly System.Drawing.Color ClrInk    = System.Drawing.Color.FromArgb(12, 74, 110);    // headings
    private static readonly System.Drawing.Color ClrAccent = System.Drawing.Color.FromArgb(2, 132, 199);    // lines / bars
    private static readonly System.Drawing.Color ClrHead   = System.Drawing.Color.FromArgb(224, 242, 254);  // table header fill
    private static readonly System.Drawing.Color ClrSoft   = System.Drawing.Color.FromArgb(240, 249, 255);  // cards / total row
    private static readonly System.Drawing.Color ClrZebra  = System.Drawing.Color.FromArgb(246, 249, 252);  // alternate rows
    private static readonly System.Drawing.Color ClrLine   = System.Drawing.Color.FromArgb(215, 223, 233);  // hairlines
    private static readonly System.Drawing.Color ClrText   = System.Drawing.Color.FromArgb(30, 41, 59);     // body text
    private static readonly System.Drawing.Color ClrMuted  = System.Drawing.Color.FromArgb(100, 116, 139);  // secondary text

    // Summary cards ke accent colours (sky, indigo, emerald, amber)
    private static readonly System.Drawing.Color[] CardAccents =
    {
        System.Drawing.Color.FromArgb(2, 132, 199),
        System.Drawing.Color.FromArgb(79, 70, 229),
        System.Drawing.Color.FromArgb(5, 150, 105),
        System.Drawing.Color.FromArgb(217, 119, 6)
    };

    private class State
    {
        public int Sec;
        public int Row;
        public int Sum;
        public bool Signed;                              // approval section chhap chuka?
        public Dictionary<int, float[]> Widths = new();
        public void Reset() { Sec = 0; Row = 0; Sum = 0; Signed = false; Widths.Clear(); }
    }

    public static void Print(GridReportData r, PaperMode mode, IWin32Window? owner = null, bool preview = true)
    {
        var doc = new PrintDocument { DocumentName = r.Title };

        if (mode != PaperMode.Thermal)
        {
            try
            {
                var size = doc.PrinterSettings.PaperSizes.Cast<PaperSize>().FirstOrDefault(p => p.Kind == PaperKind.A4);
                if (size != null) doc.DefaultPageSettings.PaperSize = size;
            }
            catch { /* printer na ho to default paper */ }
            doc.DefaultPageSettings.Landscape = mode == PaperMode.A4Landscape;
            doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            var st = new State();
            var page = 0;
            doc.BeginPrint += (s, e) => { st.Reset(); page = 0; };
            doc.PrintPage += (s, e) =>
            {
                var g = e.Graphics!;
                g.PageUnit = GraphicsUnit.Point;
                e.HasMorePages = DrawA4Page(r, g, e.PageBounds.Width * 0.72f, e.PageBounds.Height * 0.72f, st, ++page);
            };
        }
        else
        {
            var mm = r.Shop.WidthMm is > 0 and < 150 ? r.Shop.WidthMm : 80;
            var wPts = mm / 25.4f * 72f;
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
            using var dlg = new PrintPreviewDialog
            {
                Document = doc,
                Width = mode == PaperMode.Thermal ? 600 : (mode == PaperMode.A4Landscape ? 1100 : 900),
                Height = 850
            };
            dlg.ShowDialog(owner);
        }
        else doc.Print();
    }

    // ------------------------------------------------------------ drawing helpers

    private static GraphicsPath RoundPath(RectangleF r, float radius)
    {
        var d = radius * 2;
        var gp = new GraphicsPath();
        gp.AddArc(r.X, r.Y, d, d, 180, 90);
        gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        gp.CloseFigure();
        return gp;
    }

    // ------------------------------------------------------------ A4 (multi-page tables)

    private static float[] ColWidths(Graphics g, PrintSection s, System.Drawing.Font reg,
                                     System.Drawing.Font bold, float w)
    {
        var n = s.Headers.Length;
        var nat = new float[n];
        for (var i = 0; i < n; i++) nat[i] = g.MeasureString(s.Headers[i], bold).Width;
        foreach (var row in s.Rows)
            for (var i = 0; i < n && i < row.Length; i++)
                if (!string.IsNullOrEmpty(row[i]))
                    nat[i] = Math.Max(nat[i], g.MeasureString(row[i], reg).Width);
        for (var i = 0; i < n; i++) nat[i] += 12;
        var k = w / Math.Max(1f, nat.Sum());
        return nat.Select(v => v * k).ToArray();
    }

    // true wapas karta hai agar agla page bhi hai
    private static bool DrawA4Page(GridReportData r, Graphics g, float pw, float ph, State st, int page)
    {
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        const float m = 30f;
        float x = m, w = pw - m * 2, y = m;

        var maxCols = r.Sections.Count == 0 ? 0 : r.Sections.Max(s => s.Headers.Length);
        var fs = maxCols > 9 ? 7f : maxCols > 7 ? 8f : 9f;

        using var fSmall = new System.Drawing.Font(ReceiptPrinter.Family, Math.Max(7f, fs - 1));
        using var fReg = new System.Drawing.Font(ReceiptPrinter.Family, fs);
        using var fBold = new System.Drawing.Font(ReceiptPrinter.FamilyBold, fs, System.Drawing.FontStyle.Bold);
        using var fSec = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 11f, System.Drawing.FontStyle.Bold);
        using var fTitle = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 18f, System.Drawing.FontStyle.Bold);
        using var fStat = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 17f, System.Drawing.FontStyle.Bold);
        using var fLabel = new System.Drawing.Font(ReceiptPrinter.FamilyBold, Math.Max(7f, fs - 0.5f), System.Drawing.FontStyle.Bold);
        using var fBrand = new System.Drawing.Font(ReceiptPrinter.FamilyBold, 14f, System.Drawing.FontStyle.Bold);
        using var left = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        using var right = new StringFormat { Alignment = StringAlignment.Far, FormatFlags = StringFormatFlags.NoWrap };
        using var centerEll = new StringFormat
        {
            Alignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };
        using var hair = new Pen(ClrLine, 0.75f);
        using var thin = new Pen(ClrMuted, 0.75f);
        using var thick = new Pen(ClrAccent, 1.8f);
        using var bText = new SolidBrush(ClrText);
        using var bInk = new SolidBrush(ClrInk);
        using var bMuted = new SolidBrush(ClrMuted);
        using var bAccent = new SolidBrush(ClrAccent);
        using var bHead = new SolidBrush(ClrHead);
        using var bSoft = new SolidBrush(ClrSoft);
        using var zebra = new SolidBrush(ClrZebra);

        // ---- header
        if (page == 1)
        {
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
                g.DrawString(shop.ShopName.ToUpperInvariant(), fTitle, bInk, x, y);
                y += fTitle.GetHeight(g);
            }
            if (!string.IsNullOrWhiteSpace(shop.Address)) { g.DrawString(shop.Address, fSmall, bMuted, x, y); y += fSmall.GetHeight(g); }
            if (!string.IsNullOrWhiteSpace(shop.ShopPhone)) { g.DrawString("Tel: " + shop.ShopPhone, fSmall, bMuted, x, y); y += fSmall.GetHeight(g); }

            g.DrawString(r.Title.ToUpperInvariant(), fTitle, bAccent, new RectangleF(x, m, w, 30), right);
            g.DrawString(r.Period, fBold, bText, new RectangleF(x, m + 28, w, 18), right);
            var pr = $"Printed: {r.PrintedAt:dd-MMM-yyyy hh:mm tt}" +
                     (string.IsNullOrWhiteSpace(r.PrintedBy) ? "" : $"  |  By: {r.PrintedBy}");
            g.DrawString(pr, fSmall, bMuted, new RectangleF(x, m + 46, w, 14), right);

            y = Math.Max(y, m + 62) + 4;
            g.DrawLine(thick, x, y, x + w, y);
            y += 12;
        }
        else
        {
            g.DrawString($"{r.Shop.ShopName}  -  {r.Title}  ({r.Period})", fSmall, bMuted, x, y);
            y += 16;
            g.DrawLine(hair, x, y, x + w, y);
            y += 4;
        }

        var limit = ph - m - 20;
        var rowH = fReg.GetHeight(g) + 5;
        var drawnOnPage = 0;

        // ---- sections
        while (st.Sec < r.Sections.Count)
        {
            var sec = r.Sections[st.Sec];
            if (sec.Headers.Length == 0) { st.Sec++; st.Row = 0; continue; }

            if (!st.Widths.TryGetValue(st.Sec, out var cw))
                st.Widths[st.Sec] = cw = ColWidths(g, sec, fReg, fBold, w);

            if (st.Row == 0)
            {
                if (drawnOnPage > 0 && y + fSec.GetHeight(g) + rowH * 4 > limit) { DrawPageNo(g, fSmall, right, x, ph, m, w, page); return true; }
                var secH = fSec.GetHeight(g);
                g.FillRectangle(bAccent, x, y + 2, 3.5f, secH - 4);
                g.DrawString(sec.Title, fSec, bInk, x + 9, y);
                y += secH + 3;
            }

            // table header
            g.FillRectangle(bInk, x, y, w, rowH + 2);
            var cx = x;
            for (var i = 0; i < cw.Length; i++)
            {
                var ra = i < sec.RightAlign.Length && sec.RightAlign[i];
                g.DrawString(sec.Headers[i], fBold, Brushes.White,
                    new RectangleF(cx + 4, y + 3, cw[i] - 8, rowH), ra ? right : left);
                cx += cw[i];
            }
            g.DrawLine(thick, x, y + rowH + 2, x + w, y + rowH + 2);
            y += rowH + 2;
            drawnOnPage++;

            if (sec.Rows.Count == 0)
            {
                g.DrawString("Koi data nahi.", fReg, bMuted, x + 4, y + 3);
                y += rowH + 14;
                st.Sec++; st.Row = 0;
                continue;
            }

            var rowsThisPage = 0;
            while (st.Row < sec.Rows.Count && (rowsThisPage == 0 || y + rowH <= limit))
            {
                var row = sec.Rows[st.Row];
                var isTotal = sec.LastRowIsTotal && st.Row == sec.Rows.Count - 1;
                if (isTotal)
                {
                    g.FillRectangle(bSoft, x, y + 1, w, rowH - 1);
                    g.DrawLine(thick, x, y + 1, x + w, y + 1);
                }
                else
                {
                    if (st.Row % 2 == 1) g.FillRectangle(zebra, x, y, w, rowH);
                    g.DrawLine(hair, x, y + rowH, x + w, y + rowH);
                }

                cx = x;
                for (var i = 0; i < cw.Length; i++)
                {
                    var ra = i < sec.RightAlign.Length && sec.RightAlign[i];
                    var val = i < row.Length ? row[i] : "";
                    g.DrawString(val, isTotal ? fBold : fReg, isTotal ? bInk : bText,
                        new RectangleF(cx + 4, y + 3, cw[i] - 8, rowH), ra ? right : left);
                    cx += cw[i];
                }
                y += rowH;
                st.Row++;
                rowsThisPage++;
            }

            if (st.Row < sec.Rows.Count) { DrawPageNo(g, fSmall, right, x, ph, m, w, page); return true; }

            st.Sec++; st.Row = 0;
            y += 14;
        }

        // ---- summary (cards)
        while (st.Sum < r.Summaries.Count)
        {
            var sm = r.Summaries[st.Sum];
            if (sm.Items.Count == 0) { st.Sum++; continue; }

            const float cardH = 80f, gap = 14f, titleH = 24f;
            var cols = sm.Items.Count <= 4 ? sm.Items.Count : (sm.Items.Count <= 6 ? 3 : 4);
            var rows = (int)Math.Ceiling(sm.Items.Count / (double)cols);
            var need = titleH + 6 + rows * (cardH + gap) + (st.Sum == 0 ? 28 : 0);

            if (drawnOnPage > 0 && y + need > limit) { DrawPageNo(g, fSmall, right, x, ph, m, w, page); return true; }

            if (st.Sum == 0)
            {
                g.DrawLine(thick, x, y, x + w, y);
                y += 6;
                var sh = fSec.GetHeight(g);
                g.FillRectangle(bAccent, x, y + 2, 3.5f, sh - 4);
                g.DrawString("SUMMARY", fSec, bInk, x + 9, y);
                y += sh + 6;
            }

            g.FillRectangle(bHead, x, y, w, titleH);
            g.FillRectangle(bAccent, x, y, 3f, titleH);
            g.DrawString(sm.Title, fBold, bInk, new RectangleF(x + 12, y + 4, w - 18, titleH), left);
            y += titleH + 6;

            var cardW = (w - gap * (cols - 1)) / cols;
            for (var i = 0; i < sm.Items.Count; i++)
            {
                var bx = x + (i % cols) * (cardW + gap);
                var by = y + (i / cols) * (cardH + gap);
                var accent = CardAccents[i % CardAccents.Length];
                const float bandH = 26f;
                using (var path = RoundPath(new RectangleF(bx, by, cardW, cardH), 8f))
                {
                    g.FillPath(Brushes.White, path);
                    g.SetClip(path, CombineMode.Intersect);
                    using (var band = new SolidBrush(accent))
                        g.FillRectangle(band, bx, by, cardW, bandH);
                    g.ResetClip();
                    using var edge = new Pen(accent, 1.2f);
                    g.DrawPath(edge, path);
                }
                g.DrawString(sm.Items[i].Label.ToUpperInvariant(), fLabel, Brushes.White,
                    new RectangleF(bx + 8, by + 7, cardW - 16, 14), centerEll);
                g.DrawString(sm.Items[i].Value, fStat, bInk,
                    new RectangleF(bx + 8, by + bandH + 10, cardW - 16, 30), centerEll);
            }
            y += rows * (cardH + gap) + 4;
            st.Sum++;
            drawnOnPage++;
        }

        // ---- approval (owner signature) + VASL, document ke bilkul aakhir mein
        if (!st.Signed)
        {
            const float need = 160f;
            if (drawnOnPage > 0 && y + need > limit) { DrawPageNo(g, fSmall, right, x, ph, m, w, page); return true; }

            y += 18;
            g.DrawLine(thick, x, y, x + w, y);
            y += 6;
            var ah = fSec.GetHeight(g);
            g.FillRectangle(bAccent, x, y + 2, 3.5f, ah - 4);
            g.DrawString("APPROVAL", fSec, bInk, x + 9, y);
            y += ah + 42;                                      // dastakhat karne ki jagah

            var sigW = Math.Min(210f, w * 0.4f);
            var dateW = Math.Min(140f, w * 0.25f);
            g.DrawLine(thin, x, y, x + dateW, y);
            g.DrawString("Date", fSmall, bMuted, new RectangleF(x, y + 3, dateW, 14), left);
            g.DrawLine(thin, x + w - sigW, y, x + w, y);
            g.DrawString("Owner Signature", fSmall, bMuted, new RectangleF(x + w - sigW, y + 3, sigW, 14), centerEll);
            y += 36;

            // VASL: logo ho to logo, warna naam
            if (r.Shop.Logo != null)
            {
                var lw = 70f;
                var lh = lw * r.Shop.Logo.Height / r.Shop.Logo.Width;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(r.Shop.Logo, x + (w - lw) / 2, y, lw, lh);
            }
            else
            {
                var spaced = string.Join(" ", BrandName.ToCharArray());
                g.DrawString(spaced, fBrand, bAccent, new RectangleF(x, y, w, fBrand.GetHeight(g)), centerEll);
            }
            st.Signed = true;
        }

        DrawPageNo(g, fSmall, right, x, ph, m, w, page);
        return false;
    }

    private static void DrawPageNo(Graphics g, System.Drawing.Font f, StringFormat right,
                                   float x, float ph, float m, float w, int page)
    {
        using var hair = new Pen(ClrLine, 0.75f);
        using var muted = new SolidBrush(ClrMuted);
        g.DrawLine(hair, x, ph - m - 4, x + w, ph - m - 4);
        g.DrawString($"Page {page}", f, muted, new RectangleF(x, ph - m, w, 14), right);
    }

    // ------------------------------------------------------------ thermal (80mm / 58mm)

    private static float DrawThermal(GridReportData r, Graphics g, float pageW)
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
        using var wrap = new StringFormat { Trimming = StringTrimming.Word };
        using var center = new StringFormat { Alignment = StringAlignment.Center, Trimming = StringTrimming.Word };
        using var right = new StringFormat { Alignment = StringAlignment.Far, FormatFlags = StringFormatFlags.NoWrap };
        using var dash = new Pen(System.Drawing.Color.Black, 0.75f) { DashStyle = DashStyle.Dash };
        using var dot = new Pen(System.Drawing.Color.Black, 0.5f) { DashStyle = DashStyle.Dot };
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
            var rw = string.IsNullOrEmpty(rv) ? 0 : g.MeasureString(rv, rf).Width + 2;
            var lw = Math.Max(20, w - rw - 4);
            var lh = TextH(l, lf, lw, wrap);
            var h = Math.Max(lh, rf.GetHeight(g));
            g.DrawString(l, lf, Brushes.Black, new RectangleF(x, y, lw, h), wrap);
            if (rw > 0) g.DrawString(rv, rf, Brushes.Black, new RectangleF(x + w - rw, y, rw, h), right);
            y += h + gap;
        }

        void Dashed() { y += 3; g.DrawLine(dash, x, y, x + w, y); y += 5; }
        void Double()
        {
            y += 2; g.DrawLine(thick, x, y, x + w, y);
            y += 3; g.DrawLine(thin, x, y, x + w, y); y += 5;
        }

        // Section title: black bar, white text (thermal par saaf nazar aata hai)
        void Bar(string t)
        {
            var h = TextH(t, fHead, w - 6, center) + 4;
            g.FillRectangle(Brushes.Black, x, y, w, h);
            g.DrawString(t, fHead, Brushes.White, new RectangleF(x + 3, y + 2, w - 6, h), center);
            y += h + 4;
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
        CenterText(r.Title.ToUpperInvariant(), fHead, 2);
        CenterText(r.Period, fBold, 2);
        Dashed();
        Row("Printed", r.PrintedAt.ToString("dd-MMM-yyyy hh:mm tt"), fSmall);
        if (!string.IsNullOrWhiteSpace(r.PrintedBy)) Row("By", r.PrintedBy, fSmall);

        // Sections
        foreach (var sec in r.Sections)
        {
            y += 5;
            Bar(sec.Title.ToUpperInvariant());

            if (sec.Rows.Count == 0) { CenterText("Koi data nahi.", fSmall, 4); continue; }

            for (var ri = 0; ri < sec.Rows.Count; ri++)
            {
                var row = sec.Rows[ri];
                var isTotal = sec.LastRowIsTotal && ri == sec.Rows.Count - 1;
                if (isTotal) { g.DrawLine(thick, x, y, x + w, y); y += 3; }

                if (sec.Headers.Length == 2)
                {
                    Row(row.ElementAtOrDefault(0) ?? "", row.ElementAtOrDefault(1) ?? "", isTotal ? fBold : fReg, isTotal ? fBold : fReg);
                    continue;
                }

                Row(row.ElementAtOrDefault(0) ?? "", "", fBold);
                for (var j = 1; j < sec.Headers.Length; j++)
                {
                    var val = row.ElementAtOrDefault(j);
                    if (string.IsNullOrWhiteSpace(val)) continue;
                    Row("  " + sec.Headers[j], val, fSmall, isTotal ? fBold : fSmall, 1);
                }
                y += 3;
                if (!isTotal && ri < sec.Rows.Count - 1)
                {
                    g.DrawLine(dot, x, y, x + w, y);
                    y += 3;
                }
            }
        }

        // Summary
        var sums = r.Summaries.Where(s => s.Items.Count > 0).ToList();
        if (sums.Count > 0)
        {
            Double();
            Bar("SUMMARY");
            foreach (var sm in sums)
            {
                CenterText(sm.Title, fBold, 3);
                foreach (var it in sm.Items) Row(it.Label, it.Value, fReg, fBold);
                Dashed();
            }
        }

        // Approval (owner signature) + VASL
        Double();
        CenterText("APPROVAL", fHead);
        y += 34;                                               // dastakhat karne ki jagah
        g.DrawLine(thin, x + w * 0.12f, y, x + w * 0.88f, y);
        y += 3;
        CenterText("Owner Signature", fSmall, 8);
        Row("Date", "____ / ____ / ________", fSmall);
        y += 6;

        if (shop.Logo != null)
        {
            var lw = Math.Min(w * 0.4f, 80f);
            var lh = lw * shop.Logo.Height / shop.Logo.Width;
            g.DrawImage(shop.Logo, x + (w - lw) / 2, y, lw, lh);
            y += lh + 4;
        }
        else CenterText(string.Join(" ", BrandName.ToCharArray()), fHead, 4);

        CenterText("*** End of report ***", fSmall);
        return y;
    }
}