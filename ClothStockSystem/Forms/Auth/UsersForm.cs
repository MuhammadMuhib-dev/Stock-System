using ClothStock.Business.Services;
using ClothStock.Models.System;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Auth;

public class UsersForm : Form
{
    private readonly AuthService _auth;
    private readonly TextBox _user = new() { Width = 150 };
    private readonly TextBox _name = new() { Width = 180 };
    private readonly TextBox _pass = new() { Width = 150 };
    private readonly ComboBox _role = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
    private readonly CheckBox _active = new() { Text = "Active", Checked = true, AutoSize = true };
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = System.Drawing.Color.White
    };
    private int _id = 0;
    private bool _loading = false;

    public UsersForm()
    {
        _auth = Program.Services.GetRequiredService<AuthService>();
        Text = "Users";
        Font = new System.Drawing.Font("Segoe UI", 10.5F);
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1000, 600);
        _role.Items.AddRange(new object[] { Roles.Owner, Roles.Seller });
        _role.SelectedIndex = 1;

        var save = new Button { Text = "Save", Width = 90, Height = 32 };
        var nw = new Button { Text = "New", Width = 80, Height = 32 };
        var reset = new Button { Text = "Password Reset", Width = 140, Height = 32 };
        save.Click += async (s, e) => await SaveAsync();
        nw.Click += (s, e) => ClearForm();
        reset.Click += async (s, e) => await ResetAsync();

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 120, Padding = new Padding(8) };
        void Add(string label, Control c) { top.Controls.Add(new Label { Text = label, AutoSize = true, Margin = new Padding(8, 8, 3, 0) }); top.Controls.Add(c); }
        Add("Username", _user); Add("Name", _name); Add("Role", _role);
        top.Controls.Add(_active);
        Add("New Password", _pass);
        top.Controls.Add(save); top.Controls.Add(nw); top.Controls.Add(reset);
        top.Controls.Add(new Label
        {
            Text = "After creating new user or resetting password, user must change password on first login. Username cannot be changed.",
            AutoSize = true,
            ForeColor = System.Drawing.Color.DimGray,
            Margin = new Padding(8, 10, 3, 0)
        });

        Controls.Add(_grid);
        Controls.Add(top);
        _grid.SelectionChanged += (s, e) => PickRow();
        Load += async (s, e) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _loading = true;
        _grid.DataSource = (await _auth.GetUsersAsync()).ToList();
        foreach (var c in new[] { "UserId", "PasswordHash", "MustChangePassword" })
            if (_grid.Columns.Contains(c)) _grid.Columns[c]!.Visible = false;
        _grid.ClearSelection();
        _loading = false;
    }

    private void PickRow()
    {
        if (_loading) return;
        if (_grid.CurrentRow?.DataBoundItem is User u)
        {
            _id = u.UserId; _user.Text = u.Username; _user.Enabled = false;
            _name.Text = u.FullName; _role.SelectedItem = u.Role; _active.Checked = u.IsActive; _pass.Clear();
        }
    }

    private void ClearForm()
    {
        _id = 0; _user.Enabled = true; _user.Clear(); _name.Clear(); _pass.Clear();
        _role.SelectedIndex = 1; _active.Checked = true; _grid.ClearSelection(); _user.Focus();
    }

    private async Task SaveAsync()
    {
        try
        {
            if (_id == 0) await _auth.CreateUserAsync(_user.Text, _name.Text, _role.Text, _pass.Text);
            else
            {
                if (_id == SessionManager.UserId && !_active.Checked)
                    throw new InvalidOperationException("You cannot disable your own account.");
                await _auth.UpdateUserAsync(_id, _name.Text, _role.Text, _active.Checked);
            }
            MessageBox.Show("Saved successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAsync(); ClearForm();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Could not save", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private async Task ResetAsync()
    {
        try
        {
            if (_id == 0) throw new InvalidOperationException("Please select a user from the list first.");
            await _auth.ResetPasswordAsync(_id, _pass.Text);
            MessageBox.Show("Password reset ho gaya. User ko naye password se login karna hoga.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            _pass.Clear();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Could not reset", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}