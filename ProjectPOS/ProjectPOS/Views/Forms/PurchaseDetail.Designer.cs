namespace ProjectPOS.Views.Forms
{
    partial class PurchaseDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dgvCartDetail = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbID = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btn = new Guna.UI2.WinForms.Guna2Button();
            this.lbTotalAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.ptbExit = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCartDetail)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbExit)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 15;
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(377, 14);
            this.guna2HtmlLabel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(262, 52);
            this.guna2HtmlLabel1.TabIndex = 0;
            this.guna2HtmlLabel1.Text = "PurchaseDetail";
            // 
            // dgvCartDetail
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvCartDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCartDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCartDetail.ColumnHeadersHeight = 4;
            this.dgvCartDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCartDetail.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCartDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCartDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCartDetail.Location = new System.Drawing.Point(0, 0);
            this.dgvCartDetail.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dgvCartDetail.Name = "dgvCartDetail";
            this.dgvCartDetail.ReadOnly = true;
            this.dgvCartDetail.RowHeadersVisible = false;
            this.dgvCartDetail.Size = new System.Drawing.Size(1004, 331);
            this.dgvCartDetail.TabIndex = 1;
            this.dgvCartDetail.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvCartDetail.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvCartDetail.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvCartDetail.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvCartDetail.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvCartDetail.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvCartDetail.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCartDetail.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvCartDetail.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCartDetail.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCartDetail.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCartDetail.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvCartDetail.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvCartDetail.ThemeStyle.ReadOnly = true;
            this.dgvCartDetail.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvCartDetail.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCartDetail.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCartDetail.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvCartDetail.ThemeStyle.RowsStyle.Height = 22;
            this.dgvCartDetail.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCartDetail.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.dgvCartDetail);
            this.guna2Panel1.Location = new System.Drawing.Point(15, 219);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1004, 331);
            this.guna2Panel1.TabIndex = 2;
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(38, 181);
            this.guna2HtmlLabel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(128, 27);
            this.guna2HtmlLabel2.TabIndex = 3;
            this.guna2HtmlLabel2.Text = "Tên nhân viên:";
            // 
            // lbName
            // 
            this.lbName.BackColor = System.Drawing.Color.Transparent;
            this.lbName.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(189, 181);
            this.lbName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(128, 27);
            this.lbName.TabIndex = 4;
            this.lbName.Text = "Tên nhân viên:";
            // 
            // guna2HtmlLabel4
            // 
            this.guna2HtmlLabel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel4.Location = new System.Drawing.Point(38, 101);
            this.guna2HtmlLabel4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            this.guna2HtmlLabel4.Size = new System.Drawing.Size(113, 27);
            this.guna2HtmlLabel4.TabIndex = 5;
            this.guna2HtmlLabel4.Text = "Mã hóa đơn:";
            // 
            // lbID
            // 
            this.lbID.BackColor = System.Drawing.Color.Transparent;
            this.lbID.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbID.Location = new System.Drawing.Point(189, 101);
            this.lbID.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(113, 27);
            this.lbID.TabIndex = 6;
            this.lbID.Text = "Mã hóa đơn:";
            // 
            // guna2HtmlLabel6
            // 
            this.guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel6.Location = new System.Drawing.Point(421, 101);
            this.guna2HtmlLabel6.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.guna2HtmlLabel6.Name = "guna2HtmlLabel6";
            this.guna2HtmlLabel6.Size = new System.Drawing.Size(150, 27);
            this.guna2HtmlLabel6.TabIndex = 7;
            this.guna2HtmlLabel6.Text = "Ngày nhập hàng:";
            // 
            // guna2HtmlLabel7
            // 
            this.guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel7.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel7.Location = new System.Drawing.Point(421, 181);
            this.guna2HtmlLabel7.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            this.guna2HtmlLabel7.Size = new System.Drawing.Size(89, 27);
            this.guna2HtmlLabel7.TabIndex = 8;
            this.guna2HtmlLabel7.Text = "Tổng tiền:";
            // 
            // lbDate
            // 
            this.lbDate.BackColor = System.Drawing.Color.Transparent;
            this.lbDate.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDate.Location = new System.Drawing.Point(592, 101);
            this.lbDate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(89, 27);
            this.lbDate.TabIndex = 9;
            this.lbDate.Text = "Tổng tiền:";
            // 
            // btn
            // 
            this.btn.BorderColor = System.Drawing.Color.LightGray;
            this.btn.BorderRadius = 10;
            this.btn.BorderThickness = 1;
            this.btn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn.FillColor = System.Drawing.SystemColors.Control;
            this.btn.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn.ForeColor = System.Drawing.Color.Gray;
            this.btn.Image = global::ProjectPOS.Properties.Resources.export;
            this.btn.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btn.Location = new System.Drawing.Point(911, 161);
            this.btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(108, 47);
            this.btn.TabIndex = 10;
            this.btn.Text = "Export";
            this.btn.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lbTotalAmount
            // 
            this.lbTotalAmount.BackColor = System.Drawing.SystemColors.Control;
            this.lbTotalAmount.BorderThickness = 0;
            this.lbTotalAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lbTotalAmount.DefaultText = "";
            this.lbTotalAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.lbTotalAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lbTotalAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.lbTotalAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.lbTotalAmount.FillColor = System.Drawing.SystemColors.Control;
            this.lbTotalAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.lbTotalAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalAmount.ForeColor = System.Drawing.Color.Black;
            this.lbTotalAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.lbTotalAmount.Location = new System.Drawing.Point(592, 170);
            this.lbTotalAmount.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.lbTotalAmount.Name = "lbTotalAmount";
            this.lbTotalAmount.PlaceholderText = "";
            this.lbTotalAmount.SelectedText = "";
            this.lbTotalAmount.Size = new System.Drawing.Size(308, 47);
            this.lbTotalAmount.TabIndex = 11;
            // 
            // ptbExit
            // 
            this.ptbExit.FillColor = System.Drawing.SystemColors.Control;
            this.ptbExit.Image = global::ProjectPOS.Properties.Resources.cancellation;
            this.ptbExit.ImageRotate = 0F;
            this.ptbExit.Location = new System.Drawing.Point(946, 14);
            this.ptbExit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ptbExit.Name = "ptbExit";
            this.ptbExit.Size = new System.Drawing.Size(69, 78);
            this.ptbExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ptbExit.TabIndex = 12;
            this.ptbExit.TabStop = false;
            this.ptbExit.Click += new System.EventHandler(this.ptbExit_Click);
            // 
            // PurchaseDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 582);
            this.Controls.Add(this.ptbExit);
            this.Controls.Add(this.lbTotalAmount);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.lbDate);
            this.Controls.Add(this.guna2HtmlLabel7);
            this.Controls.Add(this.guna2HtmlLabel6);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.guna2HtmlLabel4);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.guna2HtmlLabel2);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2HtmlLabel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "PurchaseDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PurchaseDetail";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCartDetail)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbExit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvCartDetail;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbDate;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel7;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel6;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbID;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel4;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbName;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2Button btn;
        private Guna.UI2.WinForms.Guna2TextBox lbTotalAmount;
        private Guna.UI2.WinForms.Guna2PictureBox ptbExit;
    }
}