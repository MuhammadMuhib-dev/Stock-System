namespace ClothStockSystem
{
    partial class MainForm
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
            pnlMenu = new Panel();
            lblTitle = new Label();
            btnArticles = new Button();
            btnColors = new Button();
            btnStockIn = new Button();
            btnStockView = new Button();
            pnlContent = new Panel();
            pnlMenu.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlMenu.Controls.Add(btnStockView);
            pnlMenu.Controls.Add(btnStockIn);
            pnlMenu.Controls.Add(btnColors);
            pnlMenu.Controls.Add(btnArticles);
            pnlMenu.Controls.Add(lblTitle);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(220, 700);
            pnlMenu.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(220, 70);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Cloth Stock";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnArticles
            // 
            StyleMenuButton(btnArticles, "btnArticles", "Articles", 1);
            btnArticles.Click += btnArticles_Click;
            // 
            // btnColors
            // 
            StyleMenuButton(btnColors, "btnColors", "Colors", 2);
            btnColors.Click += btnColors_Click;
            // 
            // btnStockIn
            // 
            StyleMenuButton(btnStockIn, "btnStockIn", "Stock In", 3);
            btnStockIn.Click += btnStockIn_Click;
            // 
            // btnStockView
            // 
            StyleMenuButton(btnStockView, "btnStockView", "Stock View", 4);
            btnStockView.Click += btnStockView_Click;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = System.Drawing.Color.White;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Name = "pnlContent";
            pnlContent.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1300, 700);
            Controls.Add(pnlContent);
            Controls.Add(pnlMenu);
            MinimumSize = new Size(1000, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cloth Stock System";
            WindowState = FormWindowState.Maximized;
            pnlMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Sab menu buttons ka ek jaisa style
        private static void StyleMenuButton(Button b, string name, string text, int tabIndex)
        {
            b.Dock = DockStyle.Top;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            b.Font = new System.Drawing.Font("Segoe UI", 11F);
            b.ForeColor = System.Drawing.Color.White;
            b.Name = name;
            b.Size = new Size(220, 54);
            b.TabIndex = tabIndex;
            b.Text = text;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(20, 0, 0, 0);
            b.UseVisualStyleBackColor = false;
            b.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
        }

        #endregion

        private Panel pnlMenu;
        private Label lblTitle;
        private Button btnArticles;
        private Button btnColors;
        private Button btnStockIn;
        private Button btnStockView;
        private Panel pnlContent;
    }
}