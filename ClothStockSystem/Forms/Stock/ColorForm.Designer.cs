using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ClothStockSystem.Forms.Stock
{
    partial class ColorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            grpDetails = new GroupBox();
            tblInput = new TableLayoutPanel();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            lblColor = new Label();
            txtColor = new TextBox();
            chkActive = new CheckBox();
            flowButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnNew = new Button();
            btnDelete = new Button();
            grid = new DataGridView();
            grpDetails.SuspendLayout();
            tblInput.SuspendLayout();
            flowButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            // grpDetails
            // 
            grpDetails.Controls.Add(tblInput);
            grpDetails.Controls.Add(flowButtons);
            grpDetails.Dock = DockStyle.Top;
            grpDetails.Font = new System.Drawing.Font("Segoe UI", 10F);
            grpDetails.Location = new Point(0, 0);
            grpDetails.Name = "grpDetails";
            grpDetails.Padding = new Padding(12, 8, 12, 8);
            grpDetails.Size = new Size(900, 190);
            grpDetails.TabIndex = 0;
            grpDetails.TabStop = false;
            grpDetails.Text = "Color Details";
            // 
            // tblInput
            // 
            tblInput.ColumnCount = 4;
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tblInput.Controls.Add(lblArticle, 0, 0);
            tblInput.Controls.Add(cmbArticle, 1, 0);
            tblInput.Controls.Add(lblColor, 0, 1);
            tblInput.Controls.Add(txtColor, 1, 1);
            tblInput.Controls.Add(chkActive, 3, 1);
            tblInput.Dock = DockStyle.Fill;
            tblInput.Location = new Point(12, 35);
            tblInput.Name = "tblInput";
            tblInput.RowCount = 2;
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblInput.Size = new Size(876, 92);
            tblInput.TabIndex = 0;
            // 
            // lblArticle
            // 
            lblArticle.Anchor = AnchorStyles.Left;
            lblArticle.AutoSize = true;
            lblArticle.Location = new Point(3, 9);
            lblArticle.Name = "lblArticle";
            lblArticle.Size = new Size(68, 28);
            lblArticle.TabIndex = 0;
            lblArticle.Text = "Article";
            // 
            // cmbArticle
            // 
            cmbArticle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.Location = new Point(143, 6);
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(291, 36);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            // 
            // lblColor
            // 
            lblColor.Anchor = AnchorStyles.Left;
            lblColor.AutoSize = true;
            lblColor.Location = new Point(3, 55);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(117, 28);
            lblColor.TabIndex = 2;
            lblColor.Text = "Color Name";
            // 
            // txtColor
            // 
            txtColor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtColor.Location = new Point(143, 52);
            txtColor.MaxLength = 50;
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(291, 34);
            txtColor.TabIndex = 2;
            // 
            // chkActive
            // 
            chkActive.Anchor = AnchorStyles.Left;
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Location = new Point(580, 53);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(92, 32);
            chkActive.TabIndex = 3;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // flowButtons
            // 
            flowButtons.Controls.Add(btnSave);
            flowButtons.Controls.Add(btnNew);
            flowButtons.Controls.Add(btnDelete);
            flowButtons.Dock = DockStyle.Bottom;
            flowButtons.FlowDirection = FlowDirection.RightToLeft;
            flowButtons.Location = new Point(12, 127);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new Size(876, 55);
            flowButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(750, 8);
            btnSave.Margin = new Padding(6, 8, 6, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 38);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(618, 8);
            btnNew.Margin = new Padding(6, 8, 6, 3);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(120, 38);
            btnNew.TabIndex = 5;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(200, 40, 40);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(486, 8);
            btnDelete.Margin = new Padding(6, 8, 6, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 38);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(0, 190);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(900, 410);
            grid.TabIndex = 7;
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // ColorForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(grid);
            Controls.Add(grpDetails);
            MinimumSize = new Size(780, 520);
            Name = "ColorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Colors";
            Load += ColorForm_Load;
            grpDetails.ResumeLayout(false);
            tblInput.ResumeLayout(false);
            tblInput.PerformLayout();
            flowButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpDetails;
        private TableLayoutPanel tblInput;
        private FlowLayoutPanel flowButtons;
        private Label lblArticle;
        private ComboBox cmbArticle;
        private Label lblColor;
        private TextBox txtColor;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnNew;
        private Button btnDelete;
        private DataGridView grid;
    }
}