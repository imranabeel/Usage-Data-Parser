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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHandConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMagFlux)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccel)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(261, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select Usage Data Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.folderSelectButton);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(6, 48);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(261, 636);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileFolderSelected);
            // 
            // dataGridViewHandConfig
            // 
            this.dataGridViewHandConfig.AllowUserToAddRows = false;
            this.dataGridViewHandConfig.AllowUserToDeleteRows = false;
            this.dataGridViewHandConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHandConfig.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewHandConfig.Location = new System.Drawing.Point(5, 19);
            this.dataGridViewHandConfig.Name = "dataGridViewHandConfig";
            this.dataGridViewHandConfig.ReadOnly = true;
            this.dataGridViewHandConfig.Size = new System.Drawing.Size(453, 57);
            this.dataGridViewHandConfig.TabIndex = 2;
            // 
            // dataGridViewSummary
            // 
            this.dataGridViewSummary.AllowUserToAddRows = false;
            this.dataGridViewSummary.AllowUserToDeleteRows = false;
            this.dataGridViewSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSummary.Location = new System.Drawing.Point(4, 19);
            this.dataGridViewSummary.Name = "dataGridViewSummary";
            this.dataGridViewSummary.ReadOnly = true;
            this.dataGridViewSummary.Size = new System.Drawing.Size(452, 91);
            this.dataGridViewSummary.TabIndex = 3;
            // 
            // dataGridViewGrips
            // 
            this.dataGridViewGrips.AllowUserToAddRows = false;
            this.dataGridViewGrips.AllowUserToDeleteRows = false;
            this.dataGridViewGrips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGrips.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewGrips.Name = "dataGridViewGrips";
            this.dataGridViewGrips.ReadOnly = true;
            this.dataGridViewGrips.Size = new System.Drawing.Size(471, 740);
            this.dataGridViewGrips.TabIndex = 4;
            // 
            // dataGridViewBattSummary
            // 
            this.dataGridViewBattSummary.AllowUserToAddRows = false;
            this.dataGridViewBattSummary.AllowUserToDeleteRows = false;
            this.dataGridViewBattSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBattSummary.Location = new System.Drawing.Point(5, 19);
            this.dataGridViewBattSummary.Name = "dataGridViewBattSummary";
            this.dataGridViewBattSummary.ReadOnly = true;
            this.dataGridViewBattSummary.Size = new System.Drawing.Size(451, 100);
            this.dataGridViewBattSummary.TabIndex = 5;
            // 
            // dataGridViewBattSamples
            // 
            this.dataGridViewBattSamples.AllowUserToAddRows = false;
            this.dataGridViewBattSamples.AllowUserToDeleteRows = false;
            this.dataGridViewBattSamples.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBattSamples.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewBattSamples.Name = "dataGridViewBattSamples";
            this.dataGridViewBattSamples.ReadOnly = true;
            this.dataGridViewBattSamples.Size = new System.Drawing.Size(468, 740);
            this.dataGridViewBattSamples.TabIndex = 6;
            // 
            // dataGridViewTempSamples
            // 
            this.dataGridViewTempSamples.AllowUserToAddRows = false;
            this.dataGridViewTempSamples.AllowUserToDeleteRows = false;
            this.dataGridViewTempSamples.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTempSamples.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewTempSamples.Name = "dataGridViewTempSamples";
            this.dataGridViewTempSamples.ReadOnly = true;
            this.dataGridViewTempSamples.Size = new System.Drawing.Size(468, 741);
            this.dataGridViewTempSamples.TabIndex = 8;
            // 
            // dataGridViewTempSummary
            // 
            this.dataGridViewTempSummary.AllowUserToAddRows = false;
            this.dataGridViewTempSummary.AllowUserToDeleteRows = false;
            this.dataGridViewTempSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTempSummary.Location = new System.Drawing.Point(5, 19);
            this.dataGridViewTempSummary.Name = "dataGridViewTempSummary";
            this.dataGridViewTempSummary.ReadOnly = true;
            this.dataGridViewTempSummary.Size = new System.Drawing.Size(451, 100);
            this.dataGridViewTempSummary.TabIndex = 7;
            // 
            // dataGridViewMagFlux
            // 
            this.dataGridViewMagFlux.AllowUserToAddRows = false;
            this.dataGridViewMagFlux.AllowUserToDeleteRows = false;
            this.dataGridViewMagFlux.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMagFlux.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewMagFlux.Name = "dataGridViewMagFlux";
            this.dataGridViewMagFlux.ReadOnly = true;
            this.dataGridViewMagFlux.Size = new System.Drawing.Size(452, 103);
            this.dataGridViewMagFlux.TabIndex = 9;
            // 
            // dataGridViewAccel
            // 
            this.dataGridViewAccel.AllowUserToAddRows = false;
            this.dataGridViewAccel.AllowUserToDeleteRows = false;
            this.dataGridViewAccel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAccel.Location = new System.Drawing.Point(6, 19);
            this.dataGridViewAccel.Name = "dataGridViewAccel";
            this.dataGridViewAccel.ReadOnly = true;
            this.dataGridViewAccel.Size = new System.Drawing.Size(452, 100);
            this.dataGridViewAccel.TabIndex = 10;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(261, 28);
            this.button2.TabIndex = 11;
            this.button2.Text = "Export to Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ExportToExcel);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(294, 94);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(485, 773);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox8);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(477, 747);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Summary";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.dataGridViewAccel);
            this.groupBox8.Location = new System.Drawing.Point(7, 615);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(464, 125);
            this.groupBox8.TabIndex = 16;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Acceleration";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.dataGridViewMagFlux);
            this.groupBox7.Location = new System.Drawing.Point(7, 484);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(464, 125);
            this.groupBox7.TabIndex = 15;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Mag Flux";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dataGridViewTempSummary);
            this.groupBox6.Location = new System.Drawing.Point(7, 350);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(464, 125);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Temperature Summary";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dataGridViewBattSummary);
            this.groupBox5.Location = new System.Drawing.Point(7, 219);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(464, 125);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Battery Summary";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dataGridViewSummary);
            this.groupBox4.Location = new System.Drawing.Point(6, 97);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(464, 116);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Session Summary";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridViewHandConfig);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(464, 85);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Hand Info";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewGrips);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(477, 747);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Grips";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridViewBattSamples);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(477, 747);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Battery";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridViewTempSamples);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(477, 747);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Temperature";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 690);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Selection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(12, 812);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 55);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Export";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 15;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.label1);
            this.groupBox9.Location = new System.Drawing.Point(294, 4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(485, 80);
            this.groupBox9.TabIndex = 16;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "File Details";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 16;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(273, 74);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Version";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 879);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Usage Data Parser";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHandConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBattSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTempSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMagFlux)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccel)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
    }
}

