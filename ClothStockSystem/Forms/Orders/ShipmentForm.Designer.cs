namespace ClothStockSystem.Forms.Orders
{
    partial class ShipmentForm
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
            pnlFilter = new Panel();
            lblFilter = new Label(); cmbFilter = new ComboBox();
            lblSearch = new Label(); txtSearch = new TextBox(); btnRefresh = new Button();
            gridShip = new DataGridView();
            grpDetails = new GroupBox();
            lblCourier = new Label(); cmbCourier = new ComboBox();
            lblTracking = new Label(); txtTracking = new TextBox();
            lblWeight = new Label(); numWeight = new NumericUpDown();
            lblCharge = new Label(); numCharge = new NumericUpDown();
            lblReturn = new Label(); numReturn = new NumericUpDown();
            lblNotes = new Label(); txtNotes = new TextBox();
            btnSave = new Button(); btnNew = new Button();
            pnlActions = new Panel();
            btnDispatch = new Button(); btnDeliver = new Button();
            btnReturn = new Button(); btnCancelShip = new Button();
            lblInfo = new Label();
            split = new SplitContainer();
            gridOrders = new DataGridView(); btnRemove = new Button();
            gridUnshipped = new DataGridView(); btnAdd = new Button();
            pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridShip).BeginInit();
            grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numWeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numReturn).BeginInit();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridUnshipped).BeginInit();
            SuspendLayout();
            // 
            // pnlFilter
            // 
            pnlFilter.Controls.Add(lblFilter); pnlFilter.Controls.Add(cmbFilter);
            pnlFilter.Controls.Add(lblSearch); pnlFilter.Controls.Add(txtSearch); pnlFilter.Controls.Add(btnRefresh);
            pnlFilter.Dock = DockStyle.Top; pnlFilter.Name = "pnlFilter"; pnlFilter.Size = new Size(1200, 52); pnlFilter.TabIndex = 0;
            lblFilter.AutoSize = true; lblFilter.Location = new Point(12, 15); lblFilter.Name = "lblFilter"; lblFilter.Text = "Status";
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList; cmbFilter.Location = new Point(80, 11); cmbFilter.Name = "cmbFilter"; cmbFilter.Size = new Size(160, 33); cmbFilter.TabIndex = 1;
            cmbFilter.SelectedIndexChanged += Filter_Changed;
            lblSearch.AutoSize = true; lblSearch.Location = new Point(265, 15); lblSearch.Name = "lblSearch"; lblSearch.Text = "Search";
            txtSearch.Location = new Point(340, 11); txtSearch.Name = "txtSearch"; txtSearch.Size = new Size(280, 31); txtSearch.TabIndex = 2;
            txtSearch.KeyDown += txtSearch_KeyDown;
            btnRefresh.Location = new Point(640, 8); btnRefresh.Name = "btnRefresh"; btnRefresh.Size = new Size(120, 36); btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh"; btnRefresh.UseVisualStyleBackColor = true; btnRefresh.Click += btnRefresh_Click;
            // 
            // gridShip
            // 
            gridShip.AllowUserToAddRows = false; gridShip.AllowUserToDeleteRows = false; gridShip.AllowUserToResizeRows = false;
            gridShip.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridShip.BackgroundColor = System.Drawing.Color.White; gridShip.BorderStyle = BorderStyle.None;
            gridShip.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridShip.Dock = DockStyle.Top; gridShip.MultiSelect = false; gridShip.Name = "gridShip"; gridShip.ReadOnly = true;
            gridShip.RowHeadersVisible = false; gridShip.RowHeadersWidth = 62; gridShip.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridShip.Size = new Size(1200, 190); gridShip.TabIndex = 4;
            gridShip.SelectionChanged += gridShip_SelectionChanged;
            // 
            // grpDetails
            // 
            grpDetails.Controls.Add(lblCourier); grpDetails.Controls.Add(cmbCourier);
            grpDetails.Controls.Add(lblTracking); grpDetails.Controls.Add(txtTracking);
            grpDetails.Controls.Add(lblWeight); grpDetails.Controls.Add(numWeight);
            grpDetails.Controls.Add(lblCharge); grpDetails.Controls.Add(numCharge);
            grpDetails.Controls.Add(lblReturn); grpDetails.Controls.Add(numReturn);
            grpDetails.Controls.Add(lblNotes); grpDetails.Controls.Add(txtNotes);
            grpDetails.Controls.Add(btnSave); grpDetails.Controls.Add(btnNew);
            grpDetails.Dock = DockStyle.Top; grpDetails.Name = "grpDetails"; grpDetails.Size = new Size(1200, 140);
            grpDetails.TabIndex = 5; grpDetails.TabStop = false; grpDetails.Text = "New Parcel (Add / Edit)";
            lblCourier.AutoSize = true; lblCourier.Location = new Point(12, 36); lblCourier.Name = "lblCourier"; lblCourier.Text = "Courier";
            cmbCourier.DropDownStyle = ComboBoxStyle.DropDownList; cmbCourier.Location = new Point(95, 32); cmbCourier.Name = "cmbCourier"; cmbCourier.Size = new Size(190, 33); cmbCourier.TabIndex = 6;
            lblTracking.AutoSize = true; lblTracking.Location = new Point(305, 36); lblTracking.Name = "lblTracking"; lblTracking.Text = "Tracking ID";
            txtTracking.Location = new Point(420, 32); txtTracking.MaxLength = 50; txtTracking.Name = "txtTracking"; txtTracking.Size = new Size(200, 31); txtTracking.TabIndex = 7;
            lblWeight.AutoSize = true; lblWeight.Location = new Point(640, 36); lblWeight.Name = "lblWeight"; lblWeight.Text = "Weight (kg)";
            numWeight.DecimalPlaces = 3; numWeight.Increment = new decimal(new int[] { 5, 0, 0, 65536 }); numWeight.Location = new Point(760, 32);
            numWeight.Maximum = new decimal(new int[] { 10000, 0, 0, 0 }); numWeight.Name = "numWeight"; numWeight.Size = new Size(120, 31); numWeight.TabIndex = 8; numWeight.TextAlign = HorizontalAlignment.Right;
            lblCharge.AutoSize = true; lblCharge.Location = new Point(900, 36); lblCharge.Name = "lblCharge"; lblCharge.Text = "Courier Charge";
            numCharge.DecimalPlaces = 2; numCharge.Increment = new decimal(new int[] { 10, 0, 0, 0 }); numCharge.Location = new Point(1040, 32);
            numCharge.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 }); numCharge.Name = "numCharge"; numCharge.Size = new Size(140, 31); numCharge.TabIndex = 9; numCharge.TextAlign = HorizontalAlignment.Right; numCharge.ThousandsSeparator = true;
            lblReturn.AutoSize = true; lblReturn.Location = new Point(12, 90); lblReturn.Name = "lblReturn"; lblReturn.Text = "Return Charge";
            numReturn.DecimalPlaces = 2; numReturn.Increment = new decimal(new int[] { 10, 0, 0, 0 }); numReturn.Location = new Point(150, 86);
            numReturn.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 }); numReturn.Name = "numReturn"; numReturn.Size = new Size(135, 31); numReturn.TabIndex = 10; numReturn.TextAlign = HorizontalAlignment.Right; numReturn.ThousandsSeparator = true;
            lblNotes.AutoSize = true; lblNotes.Location = new Point(305, 90); lblNotes.Name = "lblNotes"; lblNotes.Text = "Notes";
            txtNotes.Location = new Point(380, 86); txtNotes.MaxLength = 250; txtNotes.Name = "txtNotes"; txtNotes.Size = new Size(380, 31); txtNotes.TabIndex = 11;
            btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215); btnSave.FlatAppearance.BorderSize = 0; btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = System.Drawing.Color.White; btnSave.Location = new Point(790, 82); btnSave.Name = "btnSave"; btnSave.Size = new Size(190, 40); btnSave.TabIndex = 12;
            btnSave.Text = "Save Parcel"; btnSave.UseVisualStyleBackColor = false; btnSave.Click += btnSave_Click;
            btnNew.Location = new Point(990, 82); btnNew.Name = "btnNew"; btnNew.Size = new Size(190, 40); btnNew.TabIndex = 13;
            btnNew.Text = "New"; btnNew.UseVisualStyleBackColor = true; btnNew.Click += btnNew_Click;
            // 
            // pnlActions
            // 
            pnlActions.BackColor = System.Drawing.Color.FromArgb(240, 246, 255);
            pnlActions.Controls.Add(btnDispatch); pnlActions.Controls.Add(btnDeliver);
            pnlActions.Controls.Add(btnReturn); pnlActions.Controls.Add(btnCancelShip); pnlActions.Controls.Add(lblInfo);
            pnlActions.Dock = DockStyle.Top; pnlActions.Name = "pnlActions"; pnlActions.Size = new Size(1200, 90); pnlActions.TabIndex = 14;
            btnDispatch.Location = new Point(12, 8); btnDispatch.Name = "btnDispatch"; btnDispatch.Size = new Size(170, 38); btnDispatch.TabIndex = 15;
            btnDispatch.Text = "Dispatch"; btnDispatch.UseVisualStyleBackColor = true; btnDispatch.Click += btnDispatch_Click;
            btnDeliver.Location = new Point(192, 8); btnDeliver.Name = "btnDeliver"; btnDeliver.Size = new Size(170, 38); btnDeliver.TabIndex = 16;
            btnDeliver.Text = "Delivered"; btnDeliver.UseVisualStyleBackColor = true; btnDeliver.Click += btnDeliver_Click;
            btnReturn.Location = new Point(372, 8); btnReturn.Name = "btnReturn"; btnReturn.Size = new Size(170, 38); btnReturn.TabIndex = 17;
            btnReturn.Text = "Return (wapas aaya)"; btnReturn.UseVisualStyleBackColor = true; btnReturn.Click += btnReturn_Click;
            btnCancelShip.BackColor = System.Drawing.Color.FromArgb(200, 40, 40); btnCancelShip.FlatAppearance.BorderSize = 0; btnCancelShip.FlatStyle = FlatStyle.Flat;
            btnCancelShip.ForeColor = System.Drawing.Color.White; btnCancelShip.Location = new Point(552, 8); btnCancelShip.Name = "btnCancelShip"; btnCancelShip.Size = new Size(190, 38);
            btnCancelShip.TabIndex = 18; btnCancelShip.Text = "Cancel Parcel"; btnCancelShip.UseVisualStyleBackColor = false; btnCancelShip.Click += btnCancelShip_Click;
            lblInfo.AutoSize = true; lblInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblInfo.Location = new Point(12, 56); lblInfo.Name = "lblInfo"; lblInfo.Text = "";
            // 
            // split
            // 
            split.Dock = DockStyle.Fill; split.Name = "split"; split.Orientation = Orientation.Vertical; split.SplitterDistance = 600; split.TabIndex = 19;
            split.Panel1.Controls.Add(gridOrders); split.Panel1.Controls.Add(btnRemove);
            split.Panel2.Controls.Add(gridUnshipped); split.Panel2.Controls.Add(btnAdd);
            // 
            btnRemove.Dock = DockStyle.Bottom; btnRemove.Name = "btnRemove"; btnRemove.Size = new Size(600, 42); btnRemove.TabIndex = 21;
            btnRemove.Text = "Remove Order from Parcel"; btnRemove.UseVisualStyleBackColor = true; btnRemove.Click += btnRemove_Click;
            gridOrders.AllowUserToAddRows = false; gridOrders.AllowUserToDeleteRows = false; gridOrders.AllowUserToResizeRows = false;
            gridOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; gridOrders.BackgroundColor = System.Drawing.Color.White; gridOrders.BorderStyle = BorderStyle.None;
            gridOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; gridOrders.Dock = DockStyle.Fill; gridOrders.MultiSelect = false;
            gridOrders.Name = "gridOrders"; gridOrders.ReadOnly = true; gridOrders.RowHeadersVisible = false; gridOrders.RowHeadersWidth = 62;
            gridOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridOrders.TabIndex = 20;
            // 
            btnAdd.Dock = DockStyle.Bottom; btnAdd.Name = "btnAdd"; btnAdd.Size = new Size(590, 42); btnAdd.TabIndex = 23;
            btnAdd.Text = "Add Orders to Parcel"; btnAdd.UseVisualStyleBackColor = true; btnAdd.Click += btnAdd_Click;
            gridUnshipped.AllowUserToAddRows = false; gridUnshipped.AllowUserToDeleteRows = false; gridUnshipped.AllowUserToResizeRows = false;
            gridUnshipped.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; gridUnshipped.BackgroundColor = System.Drawing.Color.White; gridUnshipped.BorderStyle = BorderStyle.None;
            gridUnshipped.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; gridUnshipped.Dock = DockStyle.Fill; gridUnshipped.MultiSelect = true;
            gridUnshipped.Name = "gridUnshipped"; gridUnshipped.ReadOnly = true; gridUnshipped.RowHeadersVisible = false; gridUnshipped.RowHeadersWidth = 62;
            gridUnshipped.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridUnshipped.TabIndex = 22;
            // 
            // ShipmentForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 800);
            Controls.Add(split);
            Controls.Add(pnlActions);
            Controls.Add(grpDetails);
            Controls.Add(gridShip);
            Controls.Add(pnlFilter);
            Name = "ShipmentForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Shipments (Parcels)";
            Load += ShipmentForm_Load;
            pnlFilter.ResumeLayout(false); pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridShip).EndInit();
            grpDetails.ResumeLayout(false); grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numWeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numReturn).EndInit();
            pnlActions.ResumeLayout(false); pnlActions.PerformLayout();
            split.Panel1.ResumeLayout(false); split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridUnshipped).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlFilter; private Label lblFilter; private ComboBox cmbFilter;
        private Label lblSearch; private TextBox txtSearch; private Button btnRefresh;
        private DataGridView gridShip;
        private GroupBox grpDetails;
        private Label lblCourier; private ComboBox cmbCourier;
        private Label lblTracking; private TextBox txtTracking;
        private Label lblWeight; private NumericUpDown numWeight;
        private Label lblCharge; private NumericUpDown numCharge;
        private Label lblReturn; private NumericUpDown numReturn;
        private Label lblNotes; private TextBox txtNotes;
        private Button btnSave; private Button btnNew;
        private Panel pnlActions;
        private Button btnDispatch; private Button btnDeliver; private Button btnReturn; private Button btnCancelShip;
        private Label lblInfo;
        private SplitContainer split;
        private DataGridView gridOrders; private Button btnRemove;
        private DataGridView gridUnshipped; private Button btnAdd;
    }
}