using ClothStock.Business.Services;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Auth;

public class LoginForm : Form
{
    private readonly AuthService _auth;
    private readonly SettingsService _settings;
    private readonly TextBox _user = new() { Width = 260 };
    private readonly TextBox _pass = new() { Width = 260, UseSystemPasswordChar = true };
    private readonly Label _err = new() { AutoSize = true, ForeColor = System.Drawing.Color.Firebrick };
    private readonly Button _ok = new() { Text = "Login", Width = 260, Height = 40 };

    public LoginForm()
    {
        _auth = Program.Services.GetRequiredService<AuthService>();
        _settings = Program.Services.GetRequiredService<SettingsService>();

        Text = "Login";
        Font = new System.Drawing.Font("Segoe UI", 11F);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        AutoSize = true; AutoSizeMode = AutoSizeMode.GrowAndShrink;

        _ok.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
        _ok.ForeColor = System.Drawing.Color.White;
        _ok.FlatStyle = FlatStyle.Flat;
        _ok.Click += async (s, e) => await DoLoginAsync();

        var t = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Padding = new Padding(30), WrapContents = false };
        t.Controls.Add(new Label { Text = "Cloth Stock System", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold) });
        t.Controls.Add(new Label { Text = "Username", AutoSize = true, Margin = new Padding(3, 16, 3, 2) });
        t.Controls.Add(_user);
        t.Controls.Add(new Label { Text = "Password", AutoSize = true, Margin = new Padding(3, 10, 3, 2) });
        t.Controls.Add(_pass);
        t.Controls.Add(_err);
        t.Controls.Add(_ok);
        _ok.Margin = new Padding(3, 14, 3, 3);
        Controls.Add(t);
        AcceptButton = _ok;
    }

    private async Task DoLoginAsync()
    {
        try
        {
            _err.Text = "";
            _ok.Enabled = false;
            var u = await _auth.LoginAsync(_user.Text, _pass.Text);
            if (u == null) { _err.Text = "Username or password is incorrect."; _pass.Clear(); _pass.Focus(); return; }

            if (u.MustChangePassword)
            {
                MessageBox.Show("This is first login. Please set your new password.", "Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                using var dlg = new ChangePasswordDialog(u.UserId, forced: true, oldKnown: _pass.Text);
                if (dlg.ShowDialog(this) != DialogResult.OK) { _pass.Clear(); return; }
            }

            SessionManager.Set(u);
            SessionManager.SellerSeesCost = await _settings.GetBoolAsync("SellerSeesCost", false);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { _ok.Enabled = true; }
    }
}