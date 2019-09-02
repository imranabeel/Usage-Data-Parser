namespace Usage_Data_Parser
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridViewHandConfig = new System.Windows.Forms.DataGridView();
            this.dataGridViewSummary = new System.Windows.Forms.DataGridView();
            this.dataGridViewGrips = new System.Windows.Forms.DataGridView();
            this.dataGridViewBattSummary = new System.Windows.Forms.DataGridView();
            this.dataGridViewBattSamples = new System.Windows.Forms.DataGridView();
            this.dataGridViewTempSamples = new System.Windows.Forms.DataGridView();
            this.dataGridViewTempSummary = new System.Windows.Forms.DataGridView();
            this.dataGridViewMagFlux = new System.Windows.Forms.DataGridView();
            this.dataGridViewAccel = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHandConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMagFlux)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccel)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select Usage Data Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.folderSelectButton);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 42);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(203, 275);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileFolderSelected);
            // 
            // dataGridViewHandConfig
            // 
            this.dataGridViewHandConfig.AllowUserToAddRows = false;
            this.dataGridViewHandConfig.AllowUserToDeleteRows = false;
            this.dataGridViewHandConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHandConfig.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewHandConfig.Location = new System.Drawing.Point(221, 73);
            this.dataGridViewHandConfig.Name = "dataGridViewHandConfig";
            this.dataGridViewHandConfig.ReadOnly = true;
            this.dataGridViewHandConfig.Size = new System.Drawing.Size(464, 57);
            this.dataGridViewHandConfig.TabIndex = 2;
            // 
            // dataGridViewSummary
            // 
            this.dataGridViewSummary.AllowUserToAddRows = false;
            this.dataGridViewSummary.AllowUserToDeleteRows = false;
            this.dataGridViewSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSummary.Location = new System.Drawing.Point(221, 12);
            this.dataGridViewSummary.Name = "dataGridViewSummary";
            this.dataGridViewSummary.ReadOnly = true;
            this.dataGridViewSummary.Size = new System.Drawing.Size(464, 55);
            this.dataGridViewSummary.TabIndex = 3;
            // 
            // dataGridViewGrips
            // 
            this.dataGridViewGrips.AllowUserToAddRows = false;
            this.dataGridViewGrips.AllowUserToDeleteRows = false;
            this.dataGridViewGrips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGrips.Location = new System.Drawing.Point(222, 137);
            this.dataGridViewGrips.Name = "dataGridViewGrips";
            this.dataGridViewGrips.ReadOnly = true;
            this.dataGridViewGrips.Size = new System.Drawing.Size(463, 180);
            this.dataGridViewGrips.TabIndex = 4;
            // 
            // dataGridViewBattSummary
            // 
            this.dataGridViewBattSummary.AllowUserToAddRows = false;
            this.dataGridViewBattSummary.AllowUserToDeleteRows = false;
            this.dataGridViewBattSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBattSummary.Location = new System.Drawing.Point(691, 12);
            this.dataGridViewBattSummary.Name = "dataGridViewBattSummary";
            this.dataGridViewBattSummary.ReadOnly = true;
            this.dataGridViewBattSummary.Size = new System.Drawing.Size(464, 92);
            this.dataGridViewBattSummary.TabIndex = 5;
            // 
            // dataGridViewBattSamples
            // 
            this.dataGridViewBattSamples.AllowUserToAddRows = false;
            this.dataGridViewBattSamples.AllowUserToDeleteRows = false;
            this.dataGridViewBattSamples.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBattSamples.Location = new System.Drawing.Point(691, 110);
            this.dataGridViewBattSamples.Name = "dataGridViewBattSamples";
            this.dataGridViewBattSamples.ReadOnly = true;
            this.dataGridViewBattSamples.Size = new System.Drawing.Size(464, 373);
            this.dataGridViewBattSamples.TabIndex = 6;
            // 
            // dataGridViewTempSamples
            // 
            this.dataGridViewTempSamples.AllowUserToAddRows = false;
            this.dataGridViewTempSamples.AllowUserToDeleteRows = false;
            this.dataGridViewTempSamples.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTempSamples.Location = new System.Drawing.Point(221, 415);
            this.dataGridViewTempSamples.Name = "dataGridViewTempSamples";
            this.dataGridViewTempSamples.ReadOnly = true;
            this.dataGridViewTempSamples.Size = new System.Drawing.Size(464, 213);
            this.dataGridViewTempSamples.TabIndex = 8;
            // 
            // dataGridViewTempSummary
            // 
            this.dataGridViewTempSummary.AllowUserToAddRows = false;
            this.dataGridViewTempSummary.AllowUserToDeleteRows = false;
            this.dataGridViewTempSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTempSummary.Location = new System.Drawing.Point(221, 327);
            this.dataGridViewTempSummary.Name = "dataGridViewTempSummary";
            this.dataGridViewTempSummary.ReadOnly = true;
            this.dataGridViewTempSummary.Size = new System.Drawing.Size(464, 82);
            this.dataGridViewTempSummary.TabIndex = 7;
            // 
            // dataGridViewMagFlux
            // 
            this.dataGridViewMagFlux.AllowUserToAddRows = false;
            this.dataGridViewMagFlux.AllowUserToDeleteRows = false;
            this.dataGridViewMagFlux.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMagFlux.Location = new System.Drawing.Point(691, 490);
            this.dataGridViewMagFlux.Name = "dataGridViewMagFlux";
            this.dataGridViewMagFlux.ReadOnly = true;
            this.dataGridViewMagFlux.Size = new System.Drawing.Size(464, 138);
            this.dataGridViewMagFlux.TabIndex = 9;
            // 
            // dataGridViewAccel
            // 
            this.dataGridViewAccel.AllowUserToAddRows = false;
            this.dataGridViewAccel.AllowUserToDeleteRows = false;
            this.dataGridViewAccel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAccel.Location = new System.Drawing.Point(221, 634);
            this.dataGridViewAccel.Name = "dataGridViewAccel";
            this.dataGridViewAccel.ReadOnly = true;
            this.dataGridViewAccel.Size = new System.Drawing.Size(464, 126);
            this.dataGridViewAccel.TabIndex = 10;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 327);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(202, 33);
            this.button2.TabIndex = 11;
            this.button2.Text = "Export to Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ExportToExcel);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 955);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridViewAccel);
            this.Controls.Add(this.dataGridViewMagFlux);
            this.Controls.Add(this.dataGridViewTempSamples);
            this.Controls.Add(this.dataGridViewTempSummary);
            this.Controls.Add(this.dataGridViewBattSamples);
            this.Controls.Add(this.dataGridViewBattSummary);
            this.Controls.Add(this.dataGridViewGrips);
            this.Controls.Add(this.dataGridViewSummary);
            this.Controls.Add(this.dataGridViewHandConfig);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHandConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMagFlux)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dataGridViewHandConfig;
        private System.Windows.Forms.DataGridView dataGridViewSummary;
        private System.Windows.Forms.DataGridView dataGridViewGrips;
        private System.Windows.Forms.DataGridView dataGridViewBattSummary;
        private System.Windows.Forms.DataGridView dataGridViewBattSamples;
        private System.Windows.Forms.DataGridView dataGridViewTempSamples;
        private System.Windows.Forms.DataGridView dataGridViewTempSummary;
        private System.Windows.Forms.DataGridView dataGridViewMagFlux;
        private System.Windows.Forms.DataGridView dataGridViewAccel;
        private System.Windows.Forms.Button button2;
    }
}

