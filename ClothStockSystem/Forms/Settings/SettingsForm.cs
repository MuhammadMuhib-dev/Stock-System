using System.Text.RegularExpressions;
using ClothStock.Business.Services;
using ClothStock.Models.System;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Settings;

public partial class SettingsForm : Form
{
    private readonly SettingsService _settings;
    private List<Setting> _items = new();

    // Key types
    private static readonly string[] BoolKeys = { "TaxOnReturn", "AllowSaleBelowCost", "ReceiptPreview", "SellerSeesCost" };
    private static readonly string[] TextKeys = { "ShopName", "ShopAddress", "ShopPhone", "ReceiptFooter" };
    private static readonly string[] MultilineKeys = { "ShopAddress", "ReceiptFooter" };

    // English descriptions (jo key yahan nahi hogi, uski description DB se aayegi)
    private static readonly Dictionary<string, string> Descriptions = new()
    {
        ["ShopName"] = "Shop name printed on receipts and reports.",
        ["ShopAddress"] = "Shop address printed in the receipt header.",
        ["ShopPhone"] = "Contact phone number printed on the receipt.",
        ["TaxPercent"] = "Tax percentage applied to sales (0 to 100).",
        ["TaxOnReturn"] = "Also refund the tax when an item is returned.",
        ["AllowSaleBelowCost"] = "Allow selling an item below its cost price.",
        ["ReceiptFooter"] = "Message printed at the bottom of every receipt.",
        ["ReceiptPreview"] = "Show a preview before printing a receipt.",
        ["SellerSeesCost"] = "Allow sales staff to see the cost price of items.",
    };

    // Section grouping (jo key yahan nahi hogi wo "Other" mein jayegi)
    private static readonly Dictionary<string, string> KeyGroups = new()
    {
        ["ShopName"] = "Shop Information",
        ["ShopAddress"] = "Shop Information",
        ["ShopPhone"] = "Shop Information",
        ["TaxPercent"] = "Tax & Pricing",
        ["TaxOnReturn"] = "Tax & Pricing",
        ["AllowSaleBelowCost"] = "Tax & Pricing",
        ["ReceiptFooter"] = "Receipt",
        ["ReceiptPreview"] = "Receipt",
        ["SellerSeesCost"] = "Permissions",
    };

    private static readonly string[] SectionOrder =
        { "Shop Information", "Tax & Pricing", "Receipt", "Permissions", "Other" };

    // Theme
    private static readonly Color PageBack = Color.FromArgb(243, 244, 246);
    private static readonly Color CardBack = Color.White;
    private static readonly Color Border = Color.FromArgb(229, 231, 235);
    private static readonly Color Accent = Color.FromArgb(37, 99, 235);
    private static readonly Color TextDark = Color.FromArgb(17, 24, 39);
    private static readonly Color TextMuted = Color.FromArgb(107, 114, 128);
    private const int CardWidth = 860;

    private readonly Dictionary<string, Func<string>> _getters = new();
    private readonly Dictionary<string, Control> _editors = new();

    private Panel _header = null!;
    private Panel _footer = null!;
    private FlowLayoutPanel _body = null!;
    private Button _btnSave = null!;
    private Button _btnReload = null!;
    private Label _lblStatus = null!;

    public SettingsForm()
    {
        InitializeComponent();
        _settings = Program.Services.GetRequiredService<SettingsService>();

        Font = new Font("Segoe UI", 10F);
        BackColor = PageBack;
        BuildShell();

        Load += async (_, _) => await LoadAsync();
        Resize += (_, _) => CenterCards();
    }

    // ---------- Shell (header / body / footer) ----------
    private void BuildShell()
    {
        _header = new Panel { Dock = DockStyle.Top, Height = 78, BackColor = CardBack, Padding = new Padding(32, 0, 32, 0) };
        _header.Paint += (s, e) =>
            e.Graphics.DrawLine(new Pen(Border), 0, _header.Height - 1, _header.Width, _header.Height - 1);

        var title = new Label
        {
            Text = "Settings",
            Font = new Font("Segoe UI Semibold", 18F),
            ForeColor = TextDark,
            AutoSize = true,
            Location = new Point(32, 12)
        };
        var sub = new Label
        {
            Text = "Manage your shop, tax and receipt settings",
            ForeColor = TextMuted,
            AutoSize = true,
            Location = new Point(34, 46)
        };
        _header.Controls.AddRange(new Control[] { title, sub });

        _footer = new Panel { Dock = DockStyle.Bottom, Height = 68, BackColor = CardBack };
        _footer.Paint += (s, e) => e.Graphics.DrawLine(new Pen(Border), 0, 0, _footer.Width, 0);

        _btnSave = MakeButton("Save Changes", Accent, Color.White);
        _btnSave.Click += btnSave_Click;
        _btnReload = MakeButton("Reload", Color.White, TextDark);
        _btnReload.FlatAppearance.BorderColor = Border;
        _btnReload.FlatAppearance.BorderSize = 1;
        _btnReload.Click += async (_, _) => await LoadAsync();

        _lblStatus = new Label { AutoSize = true, ForeColor = Color.FromArgb(34, 139, 94), Font = new Font("Segoe UI Semibold", 10F) };

        _footer.Controls.AddRange(new Control[] { _btnSave, _btnReload, _lblStatus });
        _footer.Resize += (_, _) => PlaceFooter();
        PlaceFooter();

        _body = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            BackColor = PageBack,
            Padding = new Padding(0, 16, 0, 16)
        };

        // Add order: Fill pehle, phir Top/Bottom
        Controls.Add(_body);
        Controls.Add(_footer);
        Controls.Add(_header);
    }

    private void PlaceFooter()
    {
        _btnSave.Location = new Point(_footer.Width - _btnSave.Width - 32, 14);
        _btnReload.Location = new Point(_btnSave.Left - _btnReload.Width - 12, 14);
        _lblStatus.Location = new Point(32, 24);
    }

    private static Button MakeButton(string text, Color back, Color fore) => new()
    {
        Text = text,
        Size = new Size(140, 40),
        FlatStyle = FlatStyle.Flat,
        BackColor = back,
        ForeColor = fore,
        Font = new Font("Segoe UI Semibold", 10F),
        Cursor = Cursors.Hand,
        FlatAppearance = { BorderSize = 0 }
    };

    // ---------- Load + build cards ----------
    private async Task LoadAsync()
    {
        try
        {
            _items = (await _settings.GetAllAsync()).ToList();
            BuildCards();
            _lblStatus.Text = "";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BuildCards()
    {
        _body.SuspendLayout();
        foreach (Control c in _body.Controls) c.Dispose();
        _body.Controls.Clear();
        _getters.Clear();
        _editors.Clear();

        var groups = _items
            .GroupBy(s => KeyGroups.TryGetValue(s.SettingKey, out var g) ? g : "Other")
            .OrderBy(g => Array.IndexOf(SectionOrder, g.Key) is var i && i < 0 ? int.MaxValue : i);

        foreach (var group in groups)
            _body.Controls.Add(BuildCard(group.Key, group.ToList()));

        _body.ResumeLayout();
        CenterCards();
    }

    private Control BuildCard(string title, List<Setting> items)
    {
        var card = new TableLayoutPanel
        {
            ColumnCount = 1,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(CardWidth, 0),
            MaximumSize = new Size(CardWidth, 0),
            BackColor = CardBack,
            Margin = new Padding(0, 0, 0, 16)
        };
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        card.Paint += (s, e) =>
            e.Graphics.DrawRectangle(new Pen(Border), 0, 0, card.Width - 1, card.Height - 1);

        var head = new Label
        {
            Text = title,
            Font = new Font("Segoe UI Semibold", 12F),
            ForeColor = Accent,
            Dock = DockStyle.Fill,
            AutoSize = false,
            Height = 48,
            Padding = new Padding(20, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft
        };
        card.Controls.Add(head);

        foreach (var s in items)
            card.Controls.Add(BuildRow(s));

        return card;
    }

    private Control BuildRow(Setting s)
    {
        const int padX = 20, padY = 12;
        int leftWidth = (int)(CardWidth * 0.6) - padX;

        var nameFont = new Font("Segoe UI Semibold", 10.5F);
        var descFont = new Font("Segoe UI", 9.5F);

        string name = Prettify(s.SettingKey);
        string desc = Descriptions.TryGetValue(s.SettingKey, out var d) ? d : (s.Description ?? "");
        bool hasDesc = !string.IsNullOrWhiteSpace(desc);

        // Measure text so the row height is always correct
        var flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding;
        int nameH = TextRenderer.MeasureText(name, nameFont, new Size(leftWidth, 0), flags).Height;
        int descH = hasDesc ? TextRenderer.MeasureText(desc, descFont, new Size(leftWidth, 0), flags).Height : 0;

        // Editor
        Control editor;
        var value = (s.SettingValue ?? "").Trim();

        if (BoolKeys.Contains(s.SettingKey))
        {
            var t = new ToggleSwitch { Checked = value == "1", BackColor = CardBack };
            _getters[s.SettingKey] = () => t.Checked ? "1" : "0";
            editor = t;
        }
        else
        {
            bool multi = MultilineKeys.Contains(s.SettingKey);
            bool isText = TextKeys.Contains(s.SettingKey);
            var tb = new TextBox
            {
                Text = value,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(249, 250, 251),
                Multiline = multi,
                Height = multi ? 64 : 30,
                Width = isText ? 280 : 140,
                TextAlign = isText ? HorizontalAlignment.Left : HorizontalAlignment.Right
            };
            tb.Enter += (_, _) => tb.BackColor = Color.White;
            tb.Leave += (_, _) => tb.BackColor = Color.FromArgb(249, 250, 251);
            _getters[s.SettingKey] = () => tb.Text.Trim();
            editor = tb;
        }
        _editors[s.SettingKey] = editor;

        int textBlockH = nameH + (hasDesc ? 3 + descH : 0);
        int rowH = Math.Max(textBlockH, editor.Height) + padY * 2;

        var row = new Panel
        {
            Width = CardWidth,
            Height = rowH,
            Dock = DockStyle.Fill,
            Margin = new Padding(0),
            BackColor = CardBack
        };
        row.Paint += (_, e) =>
            e.Graphics.DrawLine(new Pen(Border), 0, 0, row.Width, 0);

        int textTop = (rowH - textBlockH) / 2;
        row.Controls.Add(new Label
        {
            Text = name,
            Font = nameFont,
            ForeColor = TextDark,
            AutoSize = false,
            Location = new Point(padX, textTop),
            Size = new Size(leftWidth, nameH)
        });

        if (hasDesc)
            row.Controls.Add(new Label
            {
                Text = desc,
                Font = descFont,
                ForeColor = TextMuted,
                AutoSize = false,
                Location = new Point(padX, textTop + nameH + 3),
                Size = new Size(leftWidth, descH)
            });

        editor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        editor.Location = new Point(CardWidth - padX - editor.Width, (rowH - editor.Height) / 2);
        row.Controls.Add(editor);

        return row;
    }

    private static string Prettify(string key) =>
        Regex.Replace(key, "(?<=[a-z])(?=[A-Z])", " ");

    private void CenterCards()
    {
        if (_body == null) return;
        int left = Math.Max(16, (_body.ClientSize.Width - CardWidth) / 2);
        foreach (Control c in _body.Controls)
            c.Margin = new Padding(left, 0, 0, 16);
    }

    // ---------- Save ----------
    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            // Pehle sab values validate karo
            foreach (var s in _items)
            {
                if (TextKeys.Contains(s.SettingKey)) continue;

                var v = _getters[s.SettingKey]();

                if (!decimal.TryParse(v, out var num))
                { Warn($"'{Prettify(s.SettingKey)}' must be a number.", s.SettingKey); return; }

                if (BoolKeys.Contains(s.SettingKey) && v != "0" && v != "1")
                { Warn($"'{Prettify(s.SettingKey)}' must be either 1 (Yes) or 0 (No).", s.SettingKey); return; }

                if (s.SettingKey == "TaxPercent" && (num < 0 || num > 100))
                { Warn("Tax % must be between 0 and 100.", s.SettingKey); return; }

                if (num < 0)
                { Warn($"'{Prettify(s.SettingKey)}' cannot be negative.", s.SettingKey); return; }
            }

            _btnSave.Enabled = false;
            _btnSave.Text = "Saving...";

            foreach (var s in _items)
                await _settings.SaveAsync(s.SettingKey, _getters[s.SettingKey]());

            await ShowStatusAsync("✔ Settings saved successfully");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _btnSave.Enabled = true;
            _btnSave.Text = "Save Changes";
        }
    }

    private void Warn(string message, string key)
    {
        MessageBox.Show(message, "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        if (_editors.TryGetValue(key, out var c)) c.Focus();
    }

    private async Task ShowStatusAsync(string text)
    {
        _lblStatus.Text = text;
        await Task.Delay(3000);
        if (!IsDisposed) _lblStatus.Text = "";
    }
}