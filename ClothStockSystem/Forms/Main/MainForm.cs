using ClothStockSystem.Forms.Stock;

namespace ClothStockSystem;

public partial class MainForm : Form
{
    private Form? _current;
    private Button? _activeButton;

    public MainForm()
    {
        InitializeComponent();
    }

    // Koi bhi form right side ke panel mein kholne ka common tareeqa
    private void OpenForm(Form form, Button button)
    {
        // Purani form band (dispose) karein, taake us ka timer wagera bhi ruk jaye
        if (_current != null)
        {
            pnlContent.Controls.Remove(_current);
            _current.Dispose();
        }

        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(form);
        form.Show();
        _current = form;

        // Active button highlight
        if (_activeButton != null)
            _activeButton.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
        button.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
        _activeButton = button;
    }

    private void btnArticles_Click(object? sender, EventArgs e)
        => OpenForm(new ArticleForm(), btnArticles);

    private void btnColors_Click(object? sender, EventArgs e)
        => OpenForm(new ColorForm(), btnColors);

    private void btnStockIn_Click(object? sender, EventArgs e)
        => OpenForm(new StockInForm(), btnStockIn);

    private void btnStockView_Click(object? sender, EventArgs e)
        => OpenForm(new StockViewForm(), btnStockView);
}