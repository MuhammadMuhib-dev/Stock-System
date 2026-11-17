using System.Diagnostics;
using System.Drawing;
using ClothStock.Business.Services;
using ClothStock.Models.Finance;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Finance;

// Purchase Bills ki list: ab tak ke saray bills, bill ki lines, aur voucher PDF dekhne ka button.
public class PurchaseHistoryForm : Form
{
    // ---- Rang (ek jagah, taake poora form ek jaisa rahe) ----
    private static readonly Color Navy = Color.FromArgb(30, 41, 59);
    private static readonly Color Blue = Color.FromArgb(37, 99, 235);
    private static readonly Color Teal = Color.FromArgb(13, 148, 136);
    private static readonly Color PageBg = Color.FromArgb(241, 245, 249);
    private static readonly Color Border = Color.FromArgb(226, 232, 240);
    private static readonly Color Muted = Color.FromArgb(100, 116, 139);
    private static readonly Color Udhaar = Color.FromArgb(255, 244, 225);

    private readonly FinanceService _fin;
    private readonly SettingsService _settings;

    private readonly ComboBox _range = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120 };
    private readonly DateTimePicker _from = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 130 };
    private readonly DateTimePicker _to = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 130 };
    private readonly TextBox _search = new() { Width = 240, PlaceholderText = "Voucher / bill no / supplier" };
    private readonly Button _btnSearch = new() { Text = "Search", Width = 100, Height = 32 };
    private readonly Button _btnPdf = new() { Text = "Voucher PDF Dekhein", Width = 200, Height = 32 };

    private readonly Label _vBills = new(), _vSuits = new(), _vTotal = new(), _vPaid = new(), _vBalance = new();
    private readonly Label _badge = new()
    {
        Text = "SEARCH KA NATIJA",
        AutoSize = true,
        Visible = false,
        ForeColor = Color.White,
        BackColor = Blue,
        Padding = new Padding(8, 3, 8, 3),
        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
        Margin = new Padding(14, 14, 0, 0)
    };

    private readonly DataGridView _grid = NewGrid();
    private readonly DataGridView _lines = NewGrid();

    private bool _ready, _loading;

    // ---------------- UI helpers ----------------

    private static DataGridView NewGrid()
    {
        var g = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            GridColor = Border,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersHeight = 38
        };
        g.RowTemplate.Height = 30;
        g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Navy,
            ForeColor = Color.White,
            SelectionBackColor = Navy,
            SelectionForeColor = Color.White,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Padding = new Padding(6, 0, 6, 0)
        };
        g.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            SelectionBackColor = Color.FromArgb(191, 219, 254),
            SelectionForeColor = Color.Black,
            Padding = new Padding(6, 0, 6, 0)
        };
        g.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };

        // Flicker kam karne ke liye
        typeof(DataGridView).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(g, true);
        return g;
    }

    private static void StyleButton(Button b, Color back)
    {
        b.BackColor = back;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        b.Cursor = Cursors.Hand;
        b.Margin = new Padding(8, 3, 0, 3);
    }

    // Chhota stat card: upar title, neeche bara number
    private static Panel Card(string title, Label value, Color accent)
    {
        var p = new Panel
        {
            Width = 190,
            Height = 62,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(12, 6, 6, 4)
        };
        p.Paint += (s, e) =>
        {
            using var br = new SolidBrush(accent);
            e.Graphics.FillRectangle(br, 0, 0, 4, p.Height);   // rang ki patti
            using var pen = new Pen(Border);
            e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
        };
        var t = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 20,
            ForeColor = Muted,
            Font = new Font("Segoe UI", 9F)
        };
        value.Dock = DockStyle.Fill;
        value.Font = new Font("Segoe UI Semibold", 15F);
        value.ForeColor = accent == Color.FromArgb(220, 38, 38) ? accent : Navy;
        value.Text = "0";
        p.Controls.Add(value);
        p.Controls.Add(t);
        return p;
    }

    private static void Hdr(DataGridView g, string col, string text)
    {
        if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text;
    }

    // Column ki chorai (weight) aur number columns right-align
    private static void Layout(DataGridView g, params (string col, float weight, bool number)[] cols)
    {
        foreach (var (col, weight, number) in cols)
        {
            if (!g.Columns.Contains(col)) continue;
            var c = g.Columns[col]!;
            c.FillWeight = weight;
            c.MinimumWidth = 60;
            if (number)
            {
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
    }

    // ---------------- Constructor ----------------

    public PurchaseHistoryForm()
    {
        _fin = Program.Services.GetRequiredService<FinanceService>();
        _settings = Program.Services.GetRequiredService<SettingsService>();

        AutoScaleMode = AutoScaleMode.Dpi;
        Text = "Purchase Bills List";
        Font = new Font("Segoe UI", 10F);
        BackColor = PageBg;
        Size = new Size(1280, 820);
        MinimumSize = new Size(1000, 600);
        StartPosition = FormStartPosition.CenterScreen;

        _range.Items.AddRange(new object[] { "Aaj", "Is mahine", "Custom", "Sab" });
        _range.SelectedIndex = 3;   // Sab: ab tak ke bills
        _from.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        _to.Value = DateTime.Today;
        _from.Enabled = _to.Enabled = false;

        StyleButton(_btnSearch, Blue);
        StyleButton(_btnPdf, Teal);

        // ----- Filter bar (apni height khud adjust karti hai, buttons kategi nahi) -----
        Label L(string t) => new()
        {
            Text = t,
            AutoSize = true,
            ForeColor = Muted,
            Margin = new Padding(12, 9, 4, 0)
        };
        foreach (Control c in new Control[] { _range, _from, _to, _search })
            c.Margin = new Padding(0, 5, 0, 3);

        var filters = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = true,
            Padding = new Padding(8, 6, 8, 6),
            BackColor = Color.White
        };
        filters.Controls.Add(L("Muddat")); filters.Controls.Add(_range);
        filters.Controls.Add(L("Se")); filters.Controls.Add(_from);
        filters.Controls.Add(L("Tak")); filters.Controls.Add(_to);
        filters.Controls.Add(L("Search")); filters.Controls.Add(_search);
        filters.Controls.Add(_btnSearch);
        filters.Controls.Add(_btnPdf);

        // ----- Stat cards -----
        var stats = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 80,
            Padding = new Padding(12, 9, 8, 6),
            WrapContents = false,
            BackColor = PageBg
        };
        stats.Controls.Add(Card("Bills", _vBills, Blue));
        stats.Controls.Add(Card("Kul suits", _vSuits, Blue));
        stats.Controls.Add(Card("Total", _vTotal, Navy));
        stats.Controls.Add(Card("Ada kiya", _vPaid, Teal));
        stats.Controls.Add(Card("Baqi udhaar", _vBalance, Color.FromArgb(220, 38, 38)));
        stats.Controls.Add(_badge);

        // ----- Upar bills, neeche bill ka maal (splitter se size badal sakte hain) -----
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            Panel1MinSize = 150,
            Panel2MinSize = 120,
            SplitterWidth = 6,
            BackColor = PageBg
        };

        var gridHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 12, 0) };
        gridHost.Controls.Add(_grid);
        split.Panel1.Controls.Add(gridHost);

        var legend = new Label
        {
            Text = "■ Narangi rang = udhaar wale bills",
            Dock = DockStyle.Bottom,
            Height = 22,
            ForeColor = Color.FromArgb(217, 119, 6),
            Font = new Font("Segoe UI", 8.5F),
            TextAlign = ContentAlignment.MiddleLeft
        };
        gridHost.Controls.Add(legend);

        var detailTitle = new Label
        {
            Text = "Is bill ka maal",
            Dock = DockStyle.Top,
            Height = 30,
            Padding = new Padding(2, 6, 0, 0),
            ForeColor = Navy,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold)
        };
        var detailHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 12, 12) };
        detailHost.Controls.Add(_lines);
        detailHost.Controls.Add(detailTitle);
        split.Panel2.Controls.Add(detailHost);

        // Dock ka order: pehle Fill, phir Top wale (aakhri add hone wala sab se upar)
        Controls.Add(split);
        Controls.Add(stats);
        Controls.Add(filters);

        // ----- Events -----
        _range.SelectedIndexChanged += async (s, e) =>
        {
            _from.Enabled = _to.Enabled = _range.SelectedIndex == 2;
            if (_ready) await LoadAsync();
        };
        _btnSearch.Click += async (s, e) => await LoadAsync();
        _search.KeyDown += async (s, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            await LoadAsync();
        };
        _btnPdf.Click += async (s, e) => await OpenPdfAsync();
        _grid.CellDoubleClick += async (s, e) => { if (e.RowIndex >= 0) await OpenPdfAsync(); };
        _grid.SelectionChanged += async (s, e) => { if (!_loading) await LoadLinesAsync(); };
        _grid.DataBindingComplete += (s, e) => ColorRows();

        Load += async (s, e) =>
        {
            // Splitter ka distance yahan set karein, constructor mein size abhi final nahi hota
            try { split.SplitterDistance = Math.Max(split.Panel1MinSize, split.Height - 260); } catch { }
            _ready = true;
            await LoadAsync();
        };
    }

    private PurchaseVoucherRow? Selected => _grid.CurrentRow?.DataBoundItem as PurchaseVoucherRow;

    // ---------------- Data ----------------

    private async Task LoadAsync()
    {
        try
        {
            DateTime? from = null, to = null;
            var s = _search.Text.Trim();
            var searching = s.Length > 0;
            if (!searching)   // search mein sab tareekhon mein dhoondhein
            {
                var today = DateTime.Today;
                switch (_range.SelectedIndex)
                {
                    case 0: from = today; to = today; break;
                    case 1: from = new DateTime(today.Year, today.Month, 1); to = today; break;
                    case 2: from = _from.Value.Date; to = _to.Value.Date; break;
                }
            }

            var list = (await _fin.GetPurchaseVouchersAsync(from, to, searching ? s : null)).ToList();

            _loading = true;
            _grid.DataSource = list;
            string[] keep = { "VoucherNo", "VoucherDate", "InvoiceNo", "SupplierName", "Suits",
                              "TotalAmount", "PaidAmount", "Balance", "DueDate", "PaymentText", "CreatedBy" };
            foreach (DataGridViewColumn c in _grid.Columns) c.Visible = keep.Contains(c.Name);
            Hdr(_grid, "VoucherNo", "Voucher no"); Hdr(_grid, "VoucherDate", "Date");
            Hdr(_grid, "InvoiceNo", "Bill no"); Hdr(_grid, "SupplierName", "Supplier");
            Hdr(_grid, "Suits", "Suits");
            Hdr(_grid, "TotalAmount", "Total"); Hdr(_grid, "PaidAmount", "Ada kiya");
            Hdr(_grid, "Balance", "Baqi udhaar"); Hdr(_grid, "DueDate", "Due date");
            Hdr(_grid, "PaymentText", "Kaise diya"); Hdr(_grid, "CreatedBy", "User");
            foreach (var c in new[] { "VoucherDate", "DueDate" })
                if (_grid.Columns.Contains(c)) _grid.Columns[c]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            foreach (var c in new[] { "TotalAmount", "PaidAmount", "Balance", "Suits" })
                if (_grid.Columns.Contains(c)) _grid.Columns[c]!.DefaultCellStyle.Format = "N0";

            Layout(_grid,
                ("VoucherNo", 95, false), ("VoucherDate", 90, false), ("InvoiceNo", 85, false),
                ("SupplierName", 170, false), ("Suits", 65, true), ("TotalAmount", 95, true),
                ("PaidAmount", 95, true), ("Balance", 100, true), ("DueDate", 90, false),
                ("PaymentText", 110, false), ("CreatedBy", 80, false));
            _loading = false;

            _badge.Visible = searching;
            _vBills.Text = list.Count.ToString("N0");
            _vSuits.Text = list.Sum(x => x.Suits).ToString("N0");
            _vTotal.Text = list.Sum(x => x.TotalAmount).ToString("N0");
            _vPaid.Text = list.Sum(x => x.PaidAmount).ToString("N0");
            _vBalance.Text = list.Sum(x => x.Balance).ToString("N0");

            await LoadLinesAsync();
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Bills load nahi hue. 02_Voucher_Records.sql chalayi thi?", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ColorRows()
    {
        foreach (DataGridViewRow r in _grid.Rows)
            if (r.DataBoundItem is PurchaseVoucherRow v && v.Balance > 0)
                r.DefaultCellStyle.BackColor = Udhaar;   // udhaar wale bills
    }

    private async Task LoadLinesAsync()
    {
        try
        {
            var v = Selected;
            if (v == null) { _lines.DataSource = null; return; }

            _lines.DataSource = (await _fin.GetPurchaseVoucherItemsAsync(v.VoucherId)).ToList();
            string[] keep = { "ArticleName", "ColorName", "Bunches", "SuitsPerBunch", "Suits", "Meters", "CostPerSuit", "LineTotal" };
            foreach (DataGridViewColumn c in _lines.Columns) c.Visible = keep.Contains(c.Name);
            Hdr(_lines, "ArticleName", "Article"); Hdr(_lines, "ColorName", "Color");
            Hdr(_lines, "Bunches", "Bunches"); Hdr(_lines, "SuitsPerBunch", "Suits/bunch");
            Hdr(_lines, "Suits", "Kul suits"); Hdr(_lines, "Meters", "Kul meter");
            Hdr(_lines, "CostPerSuit", "Cost/suit"); Hdr(_lines, "LineTotal", "Line total");
            foreach (var c in new[] { "CostPerSuit", "LineTotal" })
                if (_lines.Columns.Contains(c)) _lines.Columns[c]!.DefaultCellStyle.Format = "N0";
            if (_lines.Columns.Contains("Meters")) _lines.Columns["Meters"]!.DefaultCellStyle.Format = "0.###";

            Layout(_lines,
                ("ArticleName", 160, false), ("ColorName", 110, false), ("Bunches", 70, true),
                ("SuitsPerBunch", 85, true), ("Suits", 80, true), ("Meters", 80, true),
                ("CostPerSuit", 90, true), ("LineTotal", 100, true));
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    // Selected bill ki PDF database ke record se dobara banati hai aur kholti hai.
    private async Task OpenPdfAsync()
    {
        var v = Selected;
        if (v == null) { MessageBox.Show("Pehle list mein se bill chunen.", "Check"); return; }

        try
        {
            _btnPdf.Enabled = false;
            var items = (await _fin.GetPurchaseVoucherItemsAsync(v.VoucherId)).ToList();
            var folder = await PurchaseVoucherPdf.GetFolderAsync(_settings);

            var data = new PurchaseVoucherPdfData
            {
                VoucherNo = v.VoucherNo,
                VoucherDate = v.VoucherDate,
                Supplier = v.SupplierName,
                InvoiceNo = v.InvoiceNo,
                InvoiceDate = v.VoucherDate,
                DueDate = v.DueDate,
                Notes = v.Notes,
                User = v.CreatedBy,
                Total = v.TotalAmount,
                Paid = v.PaidAmount,
                PaidFrom = v.PaidAmount > 0 ? v.PaymentText : null,
                Lines = items.Select(i => new PurchaseVoucherPdfLine
                {
                    Article = i.ArticleName,
                    Color = i.ColorName,
                    Bunches = i.Bunches,
                    SuitsPerBunch = i.SuitsPerBunch,
                    MetersPerSuit = i.MetersPerSuit,
                    CostPerSuit = i.CostPerSuit
                }).ToList()
            };

            await PurchaseVoucherPdf.LoadShopInfoAsync(data, _settings);

            var path = Path.Combine(folder, v.VoucherNo + ".pdf");
            try { PurchaseVoucherPdf.Create(path, data); }
            catch (IOException)   // purani PDF kisi viewer mein khuli ho to nayi file ke naam se banayen
            {
                path = Path.Combine(folder, $"{v.VoucherNo}_{DateTime.Now:HHmmss}.pdf");
                PurchaseVoucherPdf.Create(path, data);
            }
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "PDF nahi ban saki", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        finally { _btnPdf.Enabled = true; }
    }
}