using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace ClothStockSystem.Forms.Settings;

public class ToggleSwitch : Control
{
    private bool _checked;

    public event EventHandler? CheckedChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color OnColor { get; set; } = Color.FromArgb(34, 139, 94);

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color OffColor { get; set; } = Color.FromArgb(156, 163, 175);

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value) return;
            _checked = value;
            Invalidate();
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public ToggleSwitch()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Size = new Size(48, 26);
        Cursor = Cursors.Hand;
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        Checked = !Checked;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? BackColor);

        var track = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var path = new GraphicsPath())
        {
            int d = track.Height;
            path.AddArc(track.X, track.Y, d, d, 90, 180);
            path.AddArc(track.Right - d, track.Y, d, d, 270, 180);
            path.CloseFigure();
            using var brush = new SolidBrush(_checked ? OnColor : OffColor);
            g.FillPath(brush, path);
        }

        int knob = Height - 7;
        int x = _checked ? Width - knob - 4 : 3;
        using var knobBrush = new SolidBrush(Color.White);
        g.FillEllipse(knobBrush, x, 3, knob, knob);
    }
}