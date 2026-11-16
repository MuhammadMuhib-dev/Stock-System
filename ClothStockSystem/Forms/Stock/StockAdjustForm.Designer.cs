using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ClothStockSystem.Forms.Stock
{
    partial class StockAdjustForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            lblColor = new Label();
            cmbColor = new ComboBox();
            grpBatches = new GroupBox();
            grid = new DataGridView();
            pnlBottom = new Panel();
            lblType = new Label();
            cmbType = new ComboBox();
            lblMeters = new Label();
            numMeters = new NumericUpDown();
            lblNote = new Label();
            txtNote = new TextBox();
            lblInfo = new Label();
            btnSave = new Button();
            pnlTop.SuspendLayout();
            grpBatches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMeters).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(lblArticle);
            pnlTop.Controls.Add(cmbArticle);
            pnlTop.Controls.Add(lblColor);
            pnlTop.Controls.Add(cmbColor);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1100, 60);
            pnlTop.TabIndex = 0;
            // 
            // lblArticle
            // 
            lblArticle.AutoSize = true;
            lblArticle.Location = new Point(15, 18);
            lblArticle.Name = "lblArticle";
            lblArticle.Text = "Article:";
            // 
            // cmbArticle
            // 
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.Location = new Point(100, 14);
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(260, 33);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            // 
            // lblColor
            // 
            lblColor.AutoSize = true;
            lblColor.Location = new Point(400, 18);
            lblColor.Name = "lblColor";
            lblColor.Text = "Color:";
            // 
            // cmbColor
            // 
            cmbColor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbColor.Location = new Point(470, 14);
            cmbColor.Name = "cmbColor";
            cmbColor.Size = new Size(260, 33);
            cmbColor.TabIndex = 2;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            // 
            // grpBatches
            // 
            grpBatches.Controls.Add(grid);
            grpBatches.Dock = DockStyle.Fill;
            grpBatches.Name = "grpBatches";
            grpBatches.Padding = new Padding(8);
            grpBatches.TabIndex = 1;
            grpBatches.TabStop = false;
            grpBatches.Text = "Select a batch";
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.Dock = DockStyle.Fill;
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.TabIndex = 3;
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(240, 246, 255);
            pnlBottom.Controls.Add(lblType);
            pnlBottom.Controls.Add(cmbType);
            pnlBottom.Controls.Add(lblMeters);
            pnlBottom.Controls.Add(numMeters);
            pnlBottom.Controls.Add(lblNote);
            pnlBottom.Controls.Add(txtNote);
            pnlBottom.Controls.Add(lblInfo);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1100, 190);
            pnlBottom.TabIndex = 2;
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new Point(15, 18);
            lblType.Name = "lblType";
            lblType.Text = "Kya hua:";
            // 
            // cmbType
            // 
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Location = new Point(110, 14);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(330, 33);
            cmbType.TabIndex = 4;
            cmbType.SelectedIndexChanged += Preview_Changed;
            // 
            // lblMeters
            // 
            lblMeters.AutoSize = true;
            lblMeters.Location = new Point(480, 18);
            lblMeters.Name = "lblMeters";
            lblMeters.Text = "Meter:";
            // 
            // numMeters
            // 
            numMeters.DecimalPlaces = 3;
            numMeters.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numMeters.Location = new Point(550, 14);
            numMeters.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numMeters.Name = "numMeters";
            numMeters.Size = new Size(160, 31);
            numMeters.TabIndex = 5;
            numMeters.TextAlign = HorizontalAlignment.Right;
            numMeters.ValueChanged += Preview_Changed;
            // 
            // lblNote
            // 
            lblNote.AutoSize = true;
            lblNote.Location = new Point(15, 64);
            lblNote.Name = "lblNote";
            lblNote.Text = "Wajah:";
            // 
            // txtNote
            // 
            txtNote.Location = new Point(110, 60);
            txtNote.MaxLength = 250;
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(600, 31);
            txtNote.TabIndex = 6;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblInfo.Location = new Point(15, 108);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Select a batch";
            // 
            // btnSave
            // 
            btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new Point(110, 140);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(200, 38);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save Adjustment";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // StockAdjustForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(grpBatches);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            MinimumSize = new Size(900, 600);
            Name = "StockAdjustForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock Adjust";
            Load += StockAdjustForm_Load;
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            grpBatches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMeters).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private Label lblArticle;
        private ComboBox cmbArticle;
        private Label lblColor;
        private ComboBox cmbColor;
        private GroupBox grpBatches;
        private DataGridView grid;
        private Panel pnlBottom;
        private Label lblType;
        private ComboBox cmbType;
        private Label lblMeters;
        private NumericUpDown numMeters;
        private Label lblNote;
        private TextBox txtNote;
        private Label lblInfo;
        private Button btnSave;
    }
}