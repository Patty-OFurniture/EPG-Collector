﻿namespace EPGCentre
{
    /// <summary>
    /// Change Program Content.
    /// </summary>
    partial class ChangeProgramContentControl
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

        #region Component Designer generated code

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
            this.dgContents = new System.Windows.Forms.DataGridView();
            this.contentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.categoryTagColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xmltvDescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wmcDescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvblogicDescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgContents
            // 
            this.dgContents.AutoGenerateColumns = false;
            this.dgContents.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgContents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgContents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgContents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.categoryTagColumn,
            this.xmltvDescriptionColumn,
            this.wmcDescriptionColumn,
            this.dvblogicDescriptionColumn});
            this.dgContents.DataSource = this.contentBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgContents.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgContents.GridColor = System.Drawing.SystemColors.Control;
            this.dgContents.Location = new System.Drawing.Point(0, 0);
            this.dgContents.Name = "dgContents";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgContents.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgContents.RowHeadersVisible = false;
            this.dgContents.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgContents.RowTemplate.Height = 18;
            this.dgContents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgContents.Size = new System.Drawing.Size(950, 672);
            this.dgContents.TabIndex = 3;
            this.dgContents.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgContentsColumnHeaderMouseClick);
            this.dgContents.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgContentsDefaultValuesNeeded);
            this.dgContents.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgContents_EditingControlShowing);
            this.dgContents.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgContentsRowValidating);
            // 
            // contentBindingSource
            // 
            this.contentBindingSource.AllowNew = true;
            this.contentBindingSource.DataSource = typeof(DVBServices.EITProgramCategory);
            this.contentBindingSource.Sort = "";
            // 
            // categoryTagColumn
            // 
            this.categoryTagColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.categoryTagColumn.DataPropertyName = "CategoryTag";
            this.categoryTagColumn.HeaderText = "Category Tag";
            this.categoryTagColumn.MaxInputLength = 5;
            this.categoryTagColumn.Name = "categoryTagColumn";
            this.categoryTagColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.categoryTagColumn.Width = 96;
            // 
            // xmltvDescriptionColumn
            // 
            this.xmltvDescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.xmltvDescriptionColumn.DataPropertyName = "XmltvDescription";
            this.xmltvDescriptionColumn.HeaderText = "XMLTV Description";
            this.xmltvDescriptionColumn.MaxInputLength = 256;
            this.xmltvDescriptionColumn.Name = "xmltvDescriptionColumn";
            this.xmltvDescriptionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.xmltvDescriptionColumn.Width = 114;
            // 
            // wmcDescriptionColumn
            // 
            this.wmcDescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.wmcDescriptionColumn.DataPropertyName = "WMCDescription";
            this.wmcDescriptionColumn.HeaderText = "Media Centre Description";
            this.wmcDescriptionColumn.Name = "wmcDescriptionColumn";
            this.wmcDescriptionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.wmcDescriptionColumn.Width = 138;
            // 
            // dvblogicDescriptionColumn
            // 
            this.dvblogicDescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dvblogicDescriptionColumn.DataPropertyName = "DVBLogicDescription";
            this.dvblogicDescriptionColumn.HeaderText = "DVBLogic Description";
            this.dvblogicDescriptionColumn.Name = "dvblogicDescriptionColumn";
            this.dvblogicDescriptionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ChangeProgramContentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgContents);
            this.Name = "ChangeProgramContentControl";
            this.Size = new System.Drawing.Size(950, 672);
            ((System.ComponentModel.ISupportInitialize)(this.dgContents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgContents;
        private System.Windows.Forms.BindingSource contentBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryTagColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn xmltvDescriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wmcDescriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvblogicDescriptionColumn;
    }
}
