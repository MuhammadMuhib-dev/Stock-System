using System.Runtime.InteropServices;

namespace ClothStockSystem.Forms.Common;

public enum DialogKind { Info, Success, Warning, Error, Question }

/// <summary>
/// MessageBox ki jagah use hone wala professional dialog.
/// AppDialog.Success(this, "..."), AppDialog.Warning(...), AppDialog.Error(...),
/// AppDialog.Info(...), AppDialog.Confirm(...)
/// </summary>
public static class AppDialog
{
    // Saare rang ek hi jagah, yahan se badal sakte hain
    private static readonly Color InfoColor = Color.FromArgb(37, 99, 235);
    private static readonly Color SuccessColor = Color.FromArgb(22, 163, 74);
    private static readonly Color WarningColor = Color.FromArgb(217, 119, 6);
    private static readonly Color ErrorColor = Color.FromArgb(220, 38, 38);

    public static void Info(IWin32Window? owner, string message, string title = "Information")
        => Show(owner, DialogKind.Info, title, message, null, "OK", null);

    public static void Success(IWin32Window? owner, string message, string title = "Done")
        => Show(owner, DialogKind.Success, title, message, null, "OK", null);

    public static void Warning(IWin32Window? owner, string message, string title = "Check karein")
        => Show(owner, DialogKind.Warning, title, message, null, "OK", null);

    public static void Error(IWin32Window? owner, string message, string title = "Error")
        => Show(owner, DialogKind.Error, title, message, null, "OK", null);

    /// <summary>
    /// Yes/No confirmation. danger = true ho to Yes button laal hota hai
    /// aur Enter dabane par "No" chalta hai (galti se delete na ho).
    /// </summary>
    public static bool Confirm(
        IWin32Window? owner,
        string title,
        string message,
        IEnumerable<(string Label, string Value)>? details = null,
        string yesText = "Yes",
        string noText = "No",
        bool danger = false)
    {
        var kind = danger ? DialogKind.Error : DialogKind.Question;
        return Show(owner, kind, title, message, details, yesText, noText, danger) == DialogResult.Yes;
    }

    private static DialogResult Show(
        IWin32Window? owner, DialogKind kind, string title, string message,
        IEnumerable<(string Label, string Value)>? details,
        string primaryText, string? secondaryText, bool danger = false)
    {
        using var dlg = new DialogForm(kind, title, message, details,
            primaryText, secondaryText, danger, ColorFor(kind));
        return owner == null ? dlg.ShowDialog() : dlg.ShowDialog(owner);
    }

    private static Color ColorFor(DialogKind kind) => kind switch
    {
        DialogKind.Success => SuccessColor,
        DialogKind.Warning => WarningColor,
        DialogKind.Error => ErrorColor,
        _ => InfoColor
    };

    // ---------------------------------------------------------------
    //  Icon badge: halka rang ka bahar ka circle + andar solid circle
    // ---------------------------------------------------------------
    private sealed class Badge : Control
    {
        private readonly Color _color;
        private readonly DialogKind _kind;

        public Badge(DialogKind kind, Color color, int size)
        {
            _kind = kind;
            _color = color;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.White;
            Size = new Size(size, size);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            float k = Width / 52f;

            // bahar ka halka circle
            using (var soft = new SolidBrush(Color.FromArgb(32, _color)))
                g.FillEllipse(soft, 0, 0, Width - 1, Height - 1);

            // andar ka solid circle
            float inner = Width * 0.68f;
            float off = (Width - inner) / 2f;
            using (var solid = new SolidBrush(_color))
                g.FillEllipse(solid, off, off, inner, inner);

            float cx = Width / 2f, cy = Height / 2f;
            using var pen = new Pen(Color.White, 3.2f * k)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            switch (_kind)
            {
                case DialogKind.Success:
                    g.DrawLines(pen, new[]
                    {
                        new PointF(cx - 7 * k, cy + 0.5f * k),
                        new PointF(cx - 2 * k, cy + 5.5f * k),
                        new PointF(cx + 7 * k, cy - 5 * k)
                    });
                    break;

                case DialogKind.Error:
                    g.DrawLine(pen, cx - 6 * k, cy - 6 * k, cx + 6 * k, cy + 6 * k);
                    g.DrawLine(pen, cx + 6 * k, cy - 6 * k, cx - 6 * k, cy + 6 * k);
                    break;

                default:
                    var symbol = _kind switch
                    {
                        DialogKind.Warning => "!",
                        DialogKind.Question => "?",
                        _ => "i"
                    };
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    using (var font = new Font("Segoe UI", 16F, FontStyle.Bold))
                        TextRenderer.DrawText(g, symbol, font,
                            new Rectangle(0, 0, Width, Height), Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                    break;
            }
        }
    }

    // ---------------------------------------------------------------

    private sealed class DialogForm : Form
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 2;

        private static readonly Color TextDark = Color.FromArgb(17, 24, 39);
        private static readonly Color TextMuted = Color.FromArgb(75, 85, 99);
        private static readonly Color TextSoft = Color.FromArgb(107, 114, 128);
        private static readonly Color LineColor = Color.FromArgb(229, 231, 235);

        private readonly float _k;
        private int S(int v) => (int)Math.Round(v * _k);

        public DialogForm(
            DialogKind kind, string title, string message,
            IEnumerable<(string Label, string Value)>? details,
            string primaryText, string? secondaryText, bool danger, Color accent)
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
                _k = g.DpiX / 96f;

            AutoScaleMode = AutoScaleMode.None;   // sizes hum khud DPI ke hisaab se scale karte hain
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            KeyPreview = true;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(1);
            Text = title;
            MinimumSize = new Size(S(380), 0);

            var titleFont = new Font("Segoe UI Semibold", 13.5F, FontStyle.Bold);
            var msgFont = Font;
            const TextFormatFlags measureFlags = TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak;

            // ---- content width: text ke mutabiq khud adjust ----
            int minW = S(300), maxW = S(520);
            int msgNatural = string.IsNullOrWhiteSpace(message) ? 0
                : TextRenderer.MeasureText(message, msgFont, new Size(int.MaxValue, 0), TextFormatFlags.NoPrefix).Width;
            int titleNatural = TextRenderer.MeasureText(title, titleFont, new Size(int.MaxValue, 0), TextFormatFlags.NoPrefix).Width;
            int contentWidth = Math.Clamp(Math.Max(msgNatural, titleNatural) + S(4), minW, maxW);

            // ---- root ----
            var root = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var strip = new Panel { BackColor = accent, Height = S(4), Dock = DockStyle.Top, Margin = Padding.Empty };

            // ---- body ----
            var body = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(S(28), S(26), S(28), S(24)),
                Margin = Padding.Empty
            };

            var content = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = Padding.Empty
            };

            var lblTitle = new Label
            {
                AutoSize = true,
                UseMnemonic = false,
                MaximumSize = new Size(contentWidth, 0),
                Font = titleFont,
                ForeColor = TextDark,
                Text = title,
                Margin = new Padding(0, S(2), 0, S(6))
            };
            content.Controls.Add(lblTitle);

            if (!string.IsNullOrWhiteSpace(message))
            {
                // Bohat lamba message ho to scroll wala box, warna simple label
                int maxMsgH = (int)(Screen.PrimaryScreen!.WorkingArea.Height * 0.45);
                int msgH = TextRenderer.MeasureText(message, msgFont, new Size(contentWidth, 0), measureFlags).Height;

                if (msgH > maxMsgH)
                {
                    content.Controls.Add(new TextBox
                    {
                        Multiline = true,
                        ReadOnly = true,
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.White,
                        ForeColor = TextMuted,
                        ScrollBars = ScrollBars.Vertical,
                        TabStop = false,
                        Text = message,
                        Size = new Size(contentWidth, maxMsgH),
                        Margin = new Padding(0, 0, 0, S(4))
                    });
                }
                else
                {
                    content.Controls.Add(new Label
                    {
                        AutoSize = true,
                        UseMnemonic = false,
                        MaximumSize = new Size(contentWidth, 0),
                        ForeColor = TextMuted,
                        Text = message,
                        Margin = new Padding(0, 0, 0, S(4))
                    });
                }
            }

            var detailList = details?.ToList();
            if (detailList != null && detailList.Count > 0)
            {
                var card = new TableLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = Color.FromArgb(249, 250, 251),
                    ColumnCount = 2,
                    Padding = new Padding(S(14), S(8), S(14), S(8)),
                    Margin = new Padding(0, S(12), 0, 0),
                    MinimumSize = new Size(contentWidth, 0),
                    MaximumSize = new Size(contentWidth, 0)
                };
                card.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                card.Paint += (_, e) =>
                {
                    using var pen = new Pen(LineColor);
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                };

                var valueFont = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
                foreach (var (label, value) in detailList)
                {
                    card.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    var row = card.RowCount++;
                    card.Controls.Add(new Label
                    {
                        AutoSize = true,
                        UseMnemonic = false,
                        ForeColor = TextSoft,
                        Text = label,
                        Anchor = AnchorStyles.Left,
                        Margin = new Padding(0, S(4), S(16), S(4))
                    }, 0, row);
                    card.Controls.Add(new Label
                    {
                        AutoSize = true,
                        UseMnemonic = false,
                        MaximumSize = new Size((int)(contentWidth * 0.62), 0),
                        Font = valueFont,
                        ForeColor = TextDark,
                        Text = value,
                        TextAlign = ContentAlignment.MiddleRight,
                        Anchor = AnchorStyles.Right,
                        Margin = new Padding(0, S(4), 0, S(4))
                    }, 1, row);
                }
                content.Controls.Add(card);
            }

            var badge = new Badge(kind, accent, S(52)) { Margin = new Padding(0, 0, S(18), 0) };
            body.Controls.Add(badge);
            body.Controls.Add(content);

            // ---- buttons ----
            var buttons = new FlowLayoutPanel
            {
                BackColor = Color.FromArgb(249, 250, 251),
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(S(20), S(14), S(20), S(14)),
                Margin = Padding.Empty
            };
            buttons.Paint += (_, e) =>
            {
                using var pen = new Pen(LineColor);
                e.Graphics.DrawLine(pen, 0, 0, buttons.Width, 0);   // upar patli line
            };

            var btnPrimary = MakeButton(primaryText, accent, Color.White, true);
            btnPrimary.DialogResult = secondaryText == null ? DialogResult.OK : DialogResult.Yes;
            buttons.Controls.Add(btnPrimary);

            Button? btnSecondary = null;
            if (secondaryText != null)
            {
                btnSecondary = MakeButton(secondaryText, Color.White, Color.FromArgb(55, 65, 81), false);
                btnSecondary.DialogResult = DialogResult.No;
                buttons.Controls.Add(btnSecondary);
            }

            // Enter / Esc behaviour
            if (btnSecondary == null)
            {
                AcceptButton = btnPrimary;
                CancelButton = btnPrimary;
            }
            else
            {
                AcceptButton = danger ? btnSecondary : btnPrimary;
                CancelButton = btnSecondary;
            }

            root.Controls.Add(strip, 0, 0);
            root.Controls.Add(body, 0, 1);
            root.Controls.Add(buttons, 0, 2);
            Controls.Add(root);

            // Dialog ko upar se pakad kar drag kar sakte hain
            foreach (Control c in new Control[] { strip, body, content, badge, lblTitle })
                c.MouseDown += DragWindow;
        }

        private Button MakeButton(string text, Color back, Color fore, bool primary)
        {
            var font = primary ? new Font("Segoe UI Semibold", 10F, FontStyle.Bold) : new Font("Segoe UI", 10F);
            int w = Math.Max(S(104), TextRenderer.MeasureText(text, font).Width + S(44));

            var b = new Button
            {
                Text = text,
                AutoSize = false,
                Size = new Size(w, S(38)),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Cursor = Cursors.Hand,
                Font = font,
                Margin = new Padding(S(10), 0, 0, 0),
                UseVisualStyleBackColor = false
            };

            if (primary)
            {
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = Darken(back, 0.10f);
                b.FlatAppearance.MouseDownBackColor = Darken(back, 0.20f);
            }
            else
            {
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(229, 231, 235);
            }
            return b;
        }

        private static Color Darken(Color c, float amount) => Color.FromArgb(
            (int)(c.R * (1 - amount)), (int)(c.G * (1 - amount)), (int)(c.B * (1 - amount)));

        private void DragWindow(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        // Soft shadow (borderless form par)
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ClassStyle |= 0x00020000; // CS_DROPSHADOW
                return cp;
            }
        }

        // Windows 11 par rounded corners (Windows 10 par khud ignore ho jata hai)
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                int round = 2; // DWMWCP_ROUND
                DwmSetWindowAttribute(Handle, 33, ref round, sizeof(int));
            }
            catch { /* purane Windows par ignore */ }
        }

        // Focus dialog dikhne ke baad set hota hai, jab saare controls ready hote hain
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (AcceptButton is Control c && c.CanFocus)
                c.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(Color.FromArgb(209, 213, 219));
            e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }
    }
}