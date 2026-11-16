using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using ClothStock.Business.Services;

namespace ClothStockSystem.Helpers;

public class ReceiptLine
{
    public string Name { get; set; } = "";
    public int Qty { get; set; }
    public decimal Price { get; set; }
    public decimal ListPrice { get; set; }          // article ka asal rate (0 = maloom nahi)
    public string Note { get; set; } = "";          // item ke neeche chhota detail (bunch / batch)
    public decimal Total => Qty * Price;
}

public class ReceiptData
{
    public string ShopName { get; set; } = "";
    public string ShopNameUrdu { get; set; } = "";
    public System.Drawing.Image? Logo { get; set; }
    public string Address { get; set; } = "";
    public string ShopPhone { get; set; } = "";
    public string Footer { get; set; } = "";
    public string ReceiptNo { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
    public string CashierName { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string CustomerPhone { get; set; } = "";
    public List<ReceiptLine> Lines { get; set; } = new();
    public decimal Paid { get; set; }
    public string PaymentMethod { get; set; } = "";
    public string PaymentAccount { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public bool Duplicate { get; set; }
    public bool Preview { get; set; } = true;
    public int WidthMm { get; set; } = 80;

    public decimal Total => Lines.Sum(l => l.Total);
    public int Suits => Lines.Sum(l => l.Qty);
    public decimal Change => Paid - Total;
    public decimal Saving => Lines.Where(l => l.ListPrice > l.Price).Sum(l => (l.ListPrice - l.Price) * l.Qty);
}

public static class ReceiptPrinter
{
    // Shop ki details aur paper ki settings Settings table se
    public static async Task<ReceiptData> CreateAsync(SettingsService s)
    {
        var width = await s.GetIntAsync("ReceiptWidthMm", 80);
        return new ReceiptData
        {
            ShopName = await s.GetTextAsync("ShopName", "Shop"),
            ShopNameUrdu = await s.GetTextAsync("ShopNameUrdu", ""),
            Address = await s.GetTextAsync("ShopAddress", ""),
            ShopPhone = await s.GetTextAsync("ShopPhone", ""),
            Footer = await s.GetTextAsync("ReceiptFooter", ""),
            WidthMm = width > 0 ? width : 80,
            Preview = await s.GetBoolAsync("ReceiptPreview", true),
            Logo = LoadLogo()
        };
    }

    // receipt_logo.png program ke folder mein ho to logo chhapta hai, warna shop ka naam
    private static System.Drawing.Image? LoadLogo()
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "receipt_logo.png");
            if (!File.Exists(path)) return null;
            using var fs = File.OpenRead(path);
            using var img = System.Drawing.Image.FromStream(fs);
            return new Bitmap(img);                      // copy, taake file lock na ho
        }
        catch { return null; }
    }

    public static void Print(ReceiptData d, IWin32Window? owner = null, bool forcePreview = false)
    {
        var wPts = d.WidthMm / 25.4f * 72f;

        // Pehle khali bitmap par naap kar lein ke receipt kitni lambi hai (paper zaya na ho)
        float hPts;
        using (var bmp = new Bitmap(1, 1))
        using (var mg = Graphics.FromImage(bmp))
        {
            mg.PageUnit = GraphicsUnit.Point;
            hPts = Draw(d, mg, wPts) + 14;
        }

        var doc = new PrintDocument { DocumentName = d.ReceiptNo };
        doc.DefaultPageSettings.PaperSize =
            new PaperSize("Receipt", (int)(wPts / 72f * 100f), (int)(hPts / 72f * 100f));
        doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
        doc.PrintPage += (s, e) =>
        {
            var g = e.Graphics!;
            g.PageUnit = GraphicsUnit.Point;
            Draw(d, g, wPts);
            e.HasMorePages = false;
        };

        if (d.Preview || forcePreview)
        {
            using var dlg = new PrintPreviewDialog { Document = doc, Width = 600, Height = 850 };
            dlg.ShowDialog(owner);
        }
        else
        {
            doc.Print();
        }
    }

    internal static string Fmt(decimal v) => v % 1 == 0 ? v.ToString("N0") : v.ToString("N2");

    // Jo font computer par maujood ho wohi use hoga
    internal static readonly string Family = PickFamily("Segoe UI", "Calibri", "Arial");
    internal static readonly string FamilyBold = PickFamily("Segoe UI Semibold", "Segoe UI", "Calibri", "Arial");
    private static readonly string FamilyUrdu = PickFamily("Jameel Noori Nastaleeq", "Noto Nastaliq Urdu", "Segoe UI", "Arial");

    private static string PickFamily(params string[] names)
    {
        var installed = System.Drawing.FontFamily.Families.Select(f => f.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return names.FirstOrDefault(installed.Contains) ?? "Arial";
    }

    // ---------------------------------------------------------------- barcode (Code 128-B)

    private static readonly string[] C128 =
    {
        "212222","222122","222221","121223","121322","131222","122213","122312","132212","221213",
        "221312","231212","112232","122132","122231","113222","123122","123221","223211","221132",
        "221231","213212","223112","312131","311222","321122","321221","312212","322112","322211",
        "212123","212321","232121","111323","131123","131321","112313","132113","132311","211313",
        "231113","231311","112133","112331","132131","113123","113321","133121","313121","211331",
        "231131","213113","213311","213131","311123","311321","331121","312113","312311","332111",
        "314111","221411","431111","111224","111422","121124","121421","141122","141221","112214",
        "112412","122114","122411","142112","142211","241211","221114","413111","241112","134111",
        "111242","121142","121241","114212","124112","124211","411212","421112","421211","212141",
        "214121","412121","111143","111341","131141","114113","114311","411113","411311","113141",
        "114131","311141","411131","211412","211214","211232","2331112"
    };

    // Text ko bar / space ki chaurai (modules) ki list mein badalta hai
    private static List<int> Code128Widths(string text)
    {
        var values = new List<int> { 104 };               // Start B
        foreach (var ch in text) values.Add(ch is >= ' ' and <= '~' ? ch - 32 : '?' - 32);
        var sum = values[0];
        for (var i = 1; i < values.Count; i++) sum += i * values[i];
        values.Add(sum % 103);                            // checksum
        values.Add(106);                                  // Stop

        var widths = new List<int>();
        foreach (var v in values) widths.AddRange(C128[v].Select(c => c - '0'));
        return widths;
    }

    // ---------------------------------------------------------------- receipt

    // Receipt banata hai, aur aakhri y (lambai) wapas deta hai
    private static float Draw(ReceiptData d, Graphics g, float pageW)
    {
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

        var k = pageW < 190 ? 0.88f : 1f;                 // 58mm paper par font thora chhota
        float pad = pageW < 190 ? 5 : 8;
        float x = pad, w = pageW - pad * 2, y = 8;

        using var fSmall = new System.Drawing.Font(Family, 7.5f * k);
        using var fReg = new System.Drawing.Font(Family, 8.5f * k);
        using var fBold = new System.Drawing.Font(FamilyBold, 8.5f * k, System.Drawing.FontStyle.Bold);
        using var fHead = new System.Drawing.Font(FamilyBold, 7.5f * k, System.Drawing.FontStyle.Bold);
        using var fShop = new System.Drawing.Font(FamilyBold, 14f * k, System.Drawing.FontStyle.Bold);
        using var fUrdu = new System.Drawing.Font(FamilyUrdu, 14f * k);
        using var fBig = new System.Drawing.Font(FamilyBold, 11.5f * k, System.Drawing.FontStyle.Bold);
        using var wrap = new StringFormat { Trimming = StringTrimming.Word };
        using var center = new StringFormat { Alignment = StringAlignment.Center, Trimming = StringTrimming.Word };
        using var centerRtl = new StringFormat { Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft };
        using var right = new StringFormat { Alignment = StringAlignment.Far, FormatFlags = StringFormatFlags.NoWrap };
        using var dash = new Pen(System.Drawing.Color.Black, 0.75f) { DashStyle = DashStyle.Dash };
        using var thick = new Pen(System.Drawing.Color.Black, 1.8f);
        using var thin = new Pen(System.Drawing.Color.Black, 0.75f);

        float TextH(string t, System.Drawing.Font f, float width, StringFormat sf) =>
            g.MeasureString(t, f, new SizeF(width, 0), sf).Height;

        void CenterText(string t, System.Drawing.Font f, float gap = 0, StringFormat? sf = null)
        {
            if (string.IsNullOrWhiteSpace(t)) return;
            sf ??= center;
            var h = TextH(t, f, w, sf);
            g.DrawString(t, f, Brushes.Black, new RectangleF(x, y, w, h), sf);
            y += h + gap;
        }

        // Left label + right value; lamba left text wrap ho jata hai
        void Row(string l, string r, System.Drawing.Font lf, System.Drawing.Font? rf = null, float gap = 2)
        {
            rf ??= lf;
            var rw = g.MeasureString(r, rf).Width + 2;
            var lw = Math.Max(20, w - rw - 4);
            var lh = TextH(l, lf, lw, wrap);
            var rh = rf.GetHeight(g);
            var h = Math.Max(lh, rh);
            g.DrawString(l, lf, Brushes.Black, new RectangleF(x, y, lw, h), wrap);
            g.DrawString(r, rf, Brushes.Black, new RectangleF(x + w - rw, y, rw, h), right);
            y += h + gap;
        }

        // Boxed row (outline ya kala), important raqam ke liye
        void BoxRow(string l, string r, System.Drawing.Font f, bool filled)
        {
            var bh = f.GetHeight(g) + 8;
            if (filled) g.FillRectangle(Brushes.Black, x, y, w, bh); else g.DrawRectangle(thick, x, y, w, bh);
            var br = filled ? Brushes.White : Brushes.Black;
            g.DrawString(l, f, br, new RectangleF(x + 5, y + 4, w, bh), wrap);
            g.DrawString(r, f, br, new RectangleF(x, y + 4, w - 5, bh), right);
            y += bh + 5;
        }

        void Dashed() { y += 3; g.DrawLine(dash, x, y, x + w, y); y += 5; }
        void Double()
        {
            y += 2; g.DrawLine(thick, x, y, x + w, y);
            y += 3; g.DrawLine(thin, x, y, x + w, y); y += 5;
        }

        // ---- Duplicate banner
        if (d.Duplicate)
        {
            var bh = fBold.GetHeight(g) + 6;
            g.DrawRectangle(thin, x, y, w, bh);
            g.DrawString("DUPLICATE COPY", fBold, Brushes.Black, new RectangleF(x, y + 3, w, bh), center);
            y += bh + 6;
        }

        // ---- Header: logo ho to logo, warna English + Urdu naam
        if (d.Logo != null)
        {
            var lw = Math.Min(w * 0.72f, 170f);
            var lh = lw * d.Logo.Height / d.Logo.Width;
            var oldMode = g.InterpolationMode;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(d.Logo, x + (w - lw) / 2, y, lw, lh);
            g.InterpolationMode = oldMode;
            y += lh + 5;
        }
        else
        {
            CenterText(d.ShopName.ToUpperInvariant(), fShop, 2);
            CenterText(d.ShopNameUrdu, fUrdu, 2, centerRtl);
        }
        CenterText(d.Address, fSmall, 1);
        if (!string.IsNullOrWhiteSpace(d.ShopPhone)) CenterText("Tel: " + d.ShopPhone, fSmall, 1);
        Double();
        CenterText("S A L E   R E C E I P T", fHead, 2);
        Dashed();

        // ---- Receipt info
        Row("Receipt No", d.ReceiptNo, fReg, fBold);
        Row("Date", d.Date.ToString("dd-MMM-yyyy"), fReg);
        Row("Time", d.Date.ToString("hh:mm tt"), fReg);
        if (!string.IsNullOrWhiteSpace(d.CashierName)) Row("Cashier", d.CashierName, fReg);
        if (!string.IsNullOrWhiteSpace(d.CustomerName)) Row("Customer", d.CustomerName, fReg, fBold);
        if (!string.IsNullOrWhiteSpace(d.CustomerPhone)) Row("Phone", d.CustomerPhone, fReg);
        Dashed();

        // ---- Items table
        float qtyR = x + w * 0.52f, rateR = x + w * 0.76f, amtR = x + w;
        void Cols(string q, string r, string a, System.Drawing.Font f)
        {
            var h = f.GetHeight(g);
            g.DrawString(q, f, Brushes.Black, new RectangleF(qtyR - 60, y, 60, h), right);
            g.DrawString(r, f, Brushes.Black, new RectangleF(rateR - 60, y, 60, h), right);
            g.DrawString(a, f, Brushes.Black, new RectangleF(amtR - 70, y, 70, h), right);
            y += h + 2;
        }

        g.DrawString("ITEM", fHead, Brushes.Black, x, y);
        Cols("QTY", "RATE", "AMOUNT", fHead);
        g.DrawLine(thin, x, y, x + w, y);
        y += 4;

        var n = 1;
        var oneRow = w >= 180;                              // 80mm: naam aur raqam ek hi line mein
        var nameW = qtyR - x - 26;
        foreach (var l in d.Lines)
        {
            var name = $"{n++}. {l.Name}";
            if (oneRow)
            {
                var nh = TextH(name, fBold, nameW, wrap);
                var rowTop = y;
                g.DrawString(name, fBold, Brushes.Black, new RectangleF(x, y, nameW, nh), wrap);
                Cols(l.Qty.ToString("N0"), Fmt(l.Price), Fmt(l.Total), fBold);
                y = rowTop + Math.Max(nh, fBold.GetHeight(g));
            }
            else
            {
                var nh = TextH(name, fBold, w, wrap);
                g.DrawString(name, fBold, Brushes.Black, new RectangleF(x, y, w, nh), wrap);
                y += nh;
                Cols(l.Qty.ToString("N0"), Fmt(l.Price), Fmt(l.Total), fReg);
            }

            if (!string.IsNullOrWhiteSpace(l.Note))
            {
                var th = TextH(l.Note, fSmall, w - 14, wrap);
                g.DrawString(l.Note, fSmall, Brushes.Black, new RectangleF(x + 14, y, w - 14, th), wrap);
                y += th;
            }
            y += 5;
        }

        Dashed();

        // ---- Totals
        Row("Total Suits", d.Suits.ToString("N0"), fReg, fBold);
        y += 3;
        BoxRow("GRAND TOTAL", "Rs. " + Fmt(d.Total), fBig, filled: true);

        Row("Paid (customer ne diye)", Fmt(d.Paid), fReg, fBold);
        if (d.Paid > 0 && !string.IsNullOrWhiteSpace(d.PaymentMethod))
        {
            var pay = d.PaymentMethod;
            if (!string.IsNullOrWhiteSpace(d.PaymentAccount) &&
                !d.PaymentAccount.Equals(d.PaymentMethod, StringComparison.OrdinalIgnoreCase))
                pay += " - " + d.PaymentAccount;
            Row("Payment", pay, fReg);
        }

        var saleDue = Math.Max(0, d.Total - d.Paid);
        if (d.Paid >= d.Total)
        {
            Row("Change (wapas diye)", Fmt(d.Change), fReg, fBold);
        }
        else
        {
            y += 2;
            BoxRow("BAQI UDHAAR", "Rs. " + Fmt(saleDue), fBold, filled: false);
            if (d.DueDate.HasValue) Row("Due date", d.DueDate.Value.ToString("dd-MMM-yyyy"), fReg, fBold);
        }

        // ---- Bachat
        if (d.Saving > 0)
        {
            y += 2;
            CenterText($"Aap ne Rs. {Fmt(d.Saving)} bachaye!", fBold, 2);
        }

        // ---- Footer
        Dashed();
        CenterText(d.Footer, fSmall, 3);
        CenterText("Shukriya! Dobara tashreef layein.", fSmall, 6);

        // ---- Barcode (receipt number), return par scan karne ke liye
        if (!string.IsNullOrWhiteSpace(d.ReceiptNo))
        {
            var widths = Code128Widths(d.ReceiptNo);
            var total = widths.Sum();
            var mw = Math.Min(1.6f, w * 0.94f / total);
            var bx = x + (w - total * mw) / 2;
            const float bh = 30f;

            var oldSmooth = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;
            var cx = bx;
            var isBar = true;
            foreach (var wd in widths)
            {
                if (isBar) g.FillRectangle(Brushes.Black, cx, y, wd * mw, bh);
                cx += wd * mw;
                isBar = !isBar;
            }
            g.SmoothingMode = oldSmooth;
            y += bh + 2;
            CenterText(d.ReceiptNo, fSmall);
        }

        return y;
    }
}