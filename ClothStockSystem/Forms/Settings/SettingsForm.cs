using System.Text.RegularExpressions;
using ClothStock.Business.Services;
using ClothStock.Models.System;
using ClothStockSystem.Forms.Common;
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

    // Theme (saare rang yahan se badal sakte hain)
    private static readonly Color PageBack = Color.FromArgb(243, 244, 246);
    private static readonly Color CardBack = Color.White;
    private static readonly Color CardHead = Color.FromArgb(249, 250, 251);
    private static readonly Color Border = Color.FromArgb(229, 231, 235);
    private static readonly Color InputBorder = Color.FromArgb(209, 213, 219);
    private static readonly Color InputBack = Color.FromArgb(249, 250, 251);
    private static readonly Color Accent = Color.FromArgb(37, 99, 235);
    private static readonly Color TextDark = Color.FromArgb(17, 24, 39);
    private static readonly Color TextMuted = Color.FromArgb(107, 114, 128);
    private static readonly Color SuccessGreen = Color.FromArgb(22, 163, 74);

    private readonly Dictionary<string, Func<string>> _getters = new();
    private readonly Dictionary<string, Control> _editors = new();

    private readonly float _k;
    private int S(int v) => (int)Math.Round(v * _k);

    private int _cardW;
    private readonly System.Windows.Forms.Timer _resizeTimer = new() { Interval = 200 };

    private TableLayoutPanel _header = null!;
    private TableLayoutPanel _footer = null!;
    private FlowLayoutPanel _body = null!;
    private Button _btnSave = null!;
    private Button _btnReload = null!;
    private Label _lblStatus = null!;

    public SettingsForm()
    {
        InitializeComponent();
        _settings = Program.Services.GetRequiredService<SettingsService>();

        _k = DeviceDpi / 96f;
        AutoScaleMode = AutoScaleMode.None;   // sizes hum khud DPI ke hisaab se scale karte hain
        Font = new Font("Segoe UI", 10F);
        BackColor = PageBack;
        BuildShell();

        Load += async (_, _) => await LoadAsync();

        // Window ka size badle to cards ki width adjust ho (edit kiye hue values bache rehte hain)
        _resizeTimer.Tick += (_, _) =>
        {
            _resizeTimer.Stop();
            if (_items.Count == 0) return;
            if (DesiredCardWidth() != _cardW)
                BuildCards(_getters.ToDictionary(k => k.Key, k => k.Value()));
            else
                CenterCards();
        };
        SizeChanged += (_, _) => { _resizeTimer.Stop(); _resizeTimer.Start(); };
        Disposed += (_, _) => _resizeTimer.Dispose();
    }

    // ---------------------------------------------------------------
    //  Shell: header / body / footer
    // ---------------------------------------------------------------
    private void BuildShell()
    {
        // ---- header ----
        _header = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = CardBack,
            Padding = new Padding(S(32), S(20), S(32), S(18))
        };
        _header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _header.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _header.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _header.Paint += (_, e) =>
        {
            using var pen = new Pen(Border);
            e.Graphics.DrawLine(pen, 0, _header.Height - 1, _header.Width, _header.Height - 1);
        };

        _header.Controls.Add(new Label
        {
            Text = "Settings",
            Font = new Font("Segoe UI Semibold", 18F),
            ForeColor = TextDark,
            AutoSize = true,
            Margin = Padding.Empty
        }, 0, 0);
        _header.Controls.Add(new Label
        {
            Text = "Manage your shop, tax and receipt settings",
            ForeColor = TextMuted,
            AutoSize = true,
            Margin = new Padding(S(2), S(2), 0, 0)
        }, 0, 1);

        // ---- footer ----
        _footer = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = S(72),
            ColumnCount = 3,
            RowCount = 1,
            BackColor = CardBack,
            Padding = new Padding(S(32), 0, S(32), 0)
        };
        _footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _footer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _footer.Paint += (_, e) =>
        {
            using var pen = new Pen(Border);
            e.Graphics.DrawLine(pen, 0, 0, _footer.Width, 0);
        };

        _lblStatus = new Label
        {
            AutoSize = true,
            ForeColor = SuccessGreen,
            Font = new Font("Segoe UI Semibold", 10F),
            Anchor = AnchorStyles.Left,
            Margin = Padding.Empty
        };

        _btnReload = MakeButton("Reload", Color.White, TextDark, primary: false);
        _btnReload.Click += async (_, _) => await LoadAsync();

        _btnSave = MakeButton("Save Changes", Accent, Color.White, primary: true);
        _btnSave.Click += btnSave_Click;

        _footer.Controls.Add(_lblStatus, 0, 0);
        _footer.Controls.Add(_btnReload, 1, 0);
        _footer.Controls.Add(_btnSave, 2, 0);

        // ---- body ----
        _body = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            BackColor = PageBack,
            Padding = new Padding(0, S(24), 0, S(8))
        };

        // Add order: Fill pehle, phir Top/Bottom
        Controls.Add(_body);
        Controls.Add(_footer);
        Controls.Add(_header);
    }

    private Button MakeButton(string text, Color back, Color fore, bool primary)
    {
        var b = new Button
        {
            Text = text,
            Size = new Size(S(150), S(42)),
            FlatStyle = FlatStyle.Flat,
            BackColor = back,
            ForeColor = fore,
            Font = new Font("Segoe UI Semibold", 10F),
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false,
            Anchor = AnchorStyles.Right,
            Margin = new Padding(S(12), 0, 0, 0)
        };

        if (primary)
        {
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 64, 175);
        }
        else
        {
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = InputBorder;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(229, 231, 235);
        }
        return b;
    }

    // ---------------------------------------------------------------
    //  Load + build cards
    // ---------------------------------------------------------------
    private async Task LoadAsync()
    {
        try
        {
            _items = (await _settings.GetAllAsync()).ToList();
            BuildCards(null);
            _lblStatus.Text = "";
        }
        catch (Exception ex)
        {
            AppDialog.Error(this, ex.Message);
        }
    }

    private int DesiredCardWidth()
    {
        int available = _body.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - S(48);
        return Math.Clamp(available, S(560), S(920));
    }

    private void BuildCards(Dictionary<string, string>? keep)
    {
        _body.SuspendLayout();

        foreach (var c in _body.Controls.Cast<Control>().ToList())
        {
            _body.Controls.Remove(c);
            c.Dispose();
        }
        _getters.Clear();
        _editors.Clear();

        _cardW = DesiredCardWidth();

        static int Order(string g)
        {
            int i = Array.IndexOf(SectionOrder, g);
            return i < 0 ? int.MaxValue : i;
        }

        var groups = _items
            .GroupBy(s => KeyGroups.TryGetValue(s.SettingKey, out var g) ? g : "Other")
            .OrderBy(g => Order(g.Key));

        foreach (var group in groups)
            _body.Controls.Add(BuildCard(group.Key, group.ToList(), keep));

        _body.ResumeLayout(true);
        CenterCards();
    }

    private Control BuildCard(string title, List<Setting> items, Dictionary<string, string>? keep)
    {
        var card = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(_cardW, 0),
            MaximumSize = new Size(_cardW, 0),
            BackColor = CardBack,
            Padding = new Padding(1),
            Margin = new Padding(0, 0, 0, S(20))
        };
        card.Paint += (_, e) =>
        {
            using var pen = new Pen(Border);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        // Card header: left par rang ki patti + section ka naam
        var head = new Panel
        {
            Size = new Size(_cardW - 2, S(52)),
            BackColor = CardHead,
            Margin = Padding.Empty
        };
        head.Controls.Add(new Label
        {
            Text = title,
            Font = new Font("Segoe UI Semibold", 11.5F),
            ForeColor = TextDark,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(S(22), 0, 0, 0),
            UseMnemonic = false
        });
        head.Controls.Add(new Panel { Dock = DockStyle.Left, Width = S(4), BackColor = Accent });
        card.Controls.Add(head);

        foreach (var s in items)
        {
            string value = keep != null && keep.TryGetValue(s.SettingKey, out var v)
                ? v
                : (s.SettingValue ?? "").Trim();
            card.Controls.Add(BuildRow(s, value));
        }

        return card;
    }

    private Control BuildRow(Setting s, string value)
    {
        int padX = S(24), padY = S(16), gap = S(32);
        int innerW = _cardW - 2;

        // ---- editor ----
        Control editor;
        Control focusTarget;

        if (BoolKeys.Contains(s.SettingKey))
        {
            var t = new ToggleSwitch { Checked = value.Trim() == "1", BackColor = CardBack };
            _getters[s.SettingKey] = () => t.Checked ? "1" : "0";
            editor = focusTarget = t;
        }
        else
        {
            bool multi = MultilineKeys.Contains(s.SettingKey);
            bool isText = TextKeys.Contains(s.SettingKey);
            var (host, tb) = MakeInput(value.Trim(), S(isText ? 300 : 140), multi,
                isText ? HorizontalAlignment.Left : HorizontalAlignment.Right);
            _getters[s.SettingKey] = () => tb.Text.Trim();
            editor = host;
            focusTarget = tb;
        }
        _editors[s.SettingKey] = focusTarget;

        // ---- text block (naam + description) ----
        int textW = Math.Max(S(160), innerW - padX * 2 - editor.Width - gap);
        string desc = Descriptions.TryGetValue(s.SettingKey, out var d) ? d : (s.Description ?? "");

        var textBlock = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Anchor = AnchorStyles.Left,
            Margin = Padding.Empty
        };
        textBlock.Controls.Add(new Label
        {
            Text = Prettify(s.SettingKey),
            Font = new Font("Segoe UI Semibold", 10.5F),
            ForeColor = TextDark,
            AutoSize = true,
            UseMnemonic = false,
            MaximumSize = new Size(textW, 0),
            Margin = Padding.Empty
        });
        if (!string.IsNullOrWhiteSpace(desc))
        {
            textBlock.Controls.Add(new Label
            {
                Text = desc,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextMuted,
                AutoSize = true,
                UseMnemonic = false,
                MaximumSize = new Size(textW, 0),
                Margin = new Padding(0, S(3), 0, 0)
            });
        }

        // ---- row ----
        var row = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 1,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(innerW, 0),
            MaximumSize = new Size(innerW, 0),
            Padding = new Padding(padX, padY, padX, padY),
            Margin = Padding.Empty,
            BackColor = CardBack
        };
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        row.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        row.Paint += (_, e) =>
        {
            using var pen = new Pen(Border);
            e.Graphics.DrawLine(pen, 0, 0, row.Width, 0);   // upar patli line
        };

        editor.Anchor = AnchorStyles.Right;
        editor.Margin = new Padding(gap, 0, 0, 0);

        row.Controls.Add(textBlock, 0, 0);
        row.Controls.Add(editor, 1, 0);
        return row;
    }

    // Modern input: border wala box, focus par nila border aur safed background
    private (Control Host, TextBox Tb) MakeInput(string value, int width, bool multi, HorizontalAlignment align)
    {
        var tb = new TextBox
        {
            Text = value,
            BorderStyle = BorderStyle.None,
            BackColor = InputBack,
            ForeColor = TextDark,
            Multiline = multi,
            ScrollBars = multi ? ScrollBars.Vertical : ScrollBars.None,
            TextAlign = align,
            Dock = DockStyle.Fill
        };

        int padH = S(12), padV = S(9);
        int height = multi ? S(84) : tb.PreferredHeight + padV * 2;

        var host = new Panel
        {
            Size = new Size(width, height),
            Padding = new Padding(padH, padV, padH, padV),
            BackColor = InputBack
        };
        host.Controls.Add(tb);

        bool focused = false;
        host.Paint += (_, e) =>
        {
            using var pen = new Pen(focused ? Accent : InputBorder);
            e.Graphics.DrawRectangle(pen, 0, 0, host.Width - 1, host.Height - 1);
        };
        tb.Enter += (_, _) =>
        {
            focused = true;
            host.BackColor = tb.BackColor = Color.White;
            host.Invalidate();
        };
        tb.Leave += (_, _) =>
        {
            focused = false;
            host.BackColor = tb.BackColor = InputBack;
            host.Invalidate();
        };
        host.Click += (_, _) => tb.Focus();

        return (host, tb);
    }

    private static string Prettify(string key) =>
        Regex.Replace(key, "(?<=[a-z])(?=[A-Z])", " ");

    private void CenterCards()
    {
        if (_body == null) return;
        int usable = _body.ClientSize.Width;
        int left = Math.Max(S(16), (usable - _cardW) / 2);
        foreach (Control c in _body.Controls)
            c.Margin = new Padding(left, 0, 0, S(20));
    }

    // ---------------------------------------------------------------
    //  Save
    // ---------------------------------------------------------------
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
            _btnReload.Enabled = false;
            _btnSave.Text = "Saving...";

            foreach (var s in _items)
                await _settings.SaveAsync(s.SettingKey, _getters[s.SettingKey]());

            await ShowStatusAsync("\u2714  Settings saved successfully");
        }
        catch (Exception ex)
        {
            AppDialog.Error(this, ex.Message);
        }
        finally
        {
            if (!IsDisposed)
            {
                _btnSave.Enabled = true;
                _btnReload.Enabled = true;
                _btnSave.Text = "Save Changes";
            }
        }
    }

    private void Warn(string message, string key)
    {
        AppDialog.Warning(this, message, "Check");
        if (_editors.TryGetValue(key, out var c)) c.Focus();
    }

    private async Task ShowStatusAsync(string text)
    {
        _lblStatus.Text = text;
        await Task.Delay(3000);
        if (!IsDisposed) _lblStatus.Text = "";
    }
}