namespace MyGeneration.UI.Plugins.EntityManager
{
    partial class TableBrowser
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIdentify = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colAllowDBNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colNativeType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colName,
            this.colPK,
            this.colIdentify,
            this.colAllowDBNull,
            this.colNativeType,
            this.colLength,
            this.colPrecision,
            this.colScale,
            this.colDefaultValue,
            this.colDescription});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(848, 614);
            this.dataGridView1.TabIndex = 1;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.MinimumWidth = 50;
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 50;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "字段名";
            this.colName.Name = "colName";
            this.colName.Width = 160;
            // 
            // colPK
            // 
            this.colPK.DataPropertyName = "PrimaryKey";
            this.colPK.HeaderText = "P";
            this.colPK.Name = "colPK";
            this.colPK.ReadOnly = true;
            this.colPK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colPK.ToolTipText = "是否为主键";
            this.colPK.Width = 25;
            // 
            // colIdentify
            // 
            this.colIdentify.DataPropertyName = "Identify";
            this.colIdentify.HeaderText = "I";
            this.colIdentify.Name = "colIdentify";
            this.colIdentify.ReadOnly = true;
            this.colIdentify.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIdentify.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colIdentify.ToolTipText = "是否为标识列";
            this.colIdentify.Width = 25;
            // 
            // colAllowDBNull
            // 
            this.colAllowDBNull.DataPropertyName = "Nullable";
            this.colAllowDBNull.HeaderText = "N";
            this.colAllowDBNull.Name = "colAllowDBNull";
            this.colAllowDBNull.ReadOnly = true;
            this.colAllowDBNull.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAllowDBNull.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colAllowDBNull.ToolTipText = "是否允许为空";
            this.colAllowDBNull.Width = 25;
            // 
            // colNativeType
            // 
            this.colNativeType.DataPropertyName = "NativeType";
            this.colNativeType.HeaderText = "数据类型";
            this.colNativeType.Name = "colNativeType";
            this.colNativeType.ToolTipText = "数据库定义的数据类型";
            this.colNativeType.Width = 120;
            // 
            // colLength
            // 
            this.colLength.DataPropertyName = "Length";
            this.colLength.HeaderText = "长度";
            this.colLength.Name = "colLength";
            this.colLength.Width = 60;
            // 
            // colPrecision
            // 
            this.colPrecision.DataPropertyName = "Precision";
            this.colPrecision.HeaderText = "精度";
            this.colPrecision.Name = "colPrecision";
            this.colPrecision.Width = 60;
            // 
            // colScale
            // 
            this.colScale.DataPropertyName = "Scale";
            this.colScale.HeaderText = "小数";
            this.colScale.Name = "colScale";
            this.colScale.ToolTipText = "小数位数";
            this.colScale.Width = 60;
            // 
            // colDefaultValue
            // 
            this.colDefaultValue.DataPropertyName = "DefaultValue";
            this.colDefaultValue.HeaderText = "默认值";
            this.colDefaultValue.Name = "colDefaultValue";
            this.colDefaultValue.Width = 80;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.DataPropertyName = "Comment";
            this.colDescription.HeaderText = "注释";
            this.colDescription.Name = "colDescription";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "打开";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(848, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TableBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 639);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "TableBrowser";
            this.Text = "TableBrowser";
            this.Load += new System.EventHandler(this.TableBrowser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPK;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIdentify;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colAllowDBNull;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNativeType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefaultValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}