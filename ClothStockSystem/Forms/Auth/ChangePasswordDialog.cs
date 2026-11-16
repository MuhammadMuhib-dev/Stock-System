using ClothStock.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Auth;

public class ChangePasswordDialog : Form
{
    private readonly AuthService _auth;
    private readonly int _userId;
    private readonly TextBox _old = new() { Width = 260, UseSystemPasswordChar = true };
    private readonly TextBox _new = new() { Width = 260, UseSystemPasswordChar = true };
    private readonly TextBox _confirm = new() { Width = 260, UseSystemPasswordChar = true };

    public ChangePasswordDialog(int userId, bool forced, string? oldKnown = null)
    {
        _auth = Program.Services.GetRequiredService<AuthService>();
        _userId = userId;
        if (oldKnown != null) _old.Text = oldKnown;

        Text = "Password badlein";
        Font = new System.Drawing.Font("Segoe UI", 11F);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false; ControlBox = !forced;
        AutoSize = true; AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var ok = new Button { Text = "Save", Width = 120, Height = 36 };
        ok.Click += async (s, e) =>
        {
            try
            {
                if (_new.Text != _confirm.Text) throw new InvalidOperationException("Dono naye password ek jaise likhein.");
                await _auth.ChangePasswordAsync(_userId, _old.Text, _new.Text);
                MessageBox.Show("Password badal gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Password", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        };

        var t = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Padding = new Padding(24), WrapContents = false };
        t.Controls.Add(new Label { Text = "Purana password", AutoSize = true }); t.Controls.Add(_old);
        t.Controls.Add(new Label { Text = "Naya password (kam az kam 6 harf)", AutoSize = true, Margin = new Padding(3, 10, 3, 2) }); t.Controls.Add(_new);
        t.Controls.Add(new Label { Text = "Naya password dobara", AutoSize = true, Margin = new Padding(3, 10, 3, 2) }); t.Controls.Add(_confirm);
        ok.Margin = new Padding(3, 14, 3, 3);
        t.Controls.Add(ok);
        Controls.Add(t);
        AcceptButton = ok;
    }
}