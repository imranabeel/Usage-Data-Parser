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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridViewHandConfig = new System.Windows.Forms.DataGridView();
            this.sessionIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.collectedInTouchPointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serialNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firmwareVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chiralityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nMotorsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resetCauseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.onTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.batteryMinVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.batteryMaxVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tempMinCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tempMaxCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.magMaxXDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.magMaxYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accelMaxXDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accelMaxYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accelMaxZDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.heroUsageDataDataSet = new Usage_Data_Parser.HeroUsageDataDataSet();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.touchPointIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handNumberDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.touchPointIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.touchPointsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.summariseSelectedDataButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sessionsTableAdapter = new Usage_Data_Parser.HeroUsageDataDataSetTableAdapters.sessionsTableAdapter();
            this.tableAdapterManager = new Usage_Data_Parser.HeroUsageDataDataSetTableAdapters.TableAdapterManager();
            this.touchPointsTableAdapter = new Usage_Data_Parser.HeroUsageDataDataSetTableAdapters.touchPointsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHandConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heroUsageDataDataSet)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.touchPointsBindingSource)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
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
            this.button1.Text = "Select Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.folderSelectButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Location = new System.Drawing.Point(6, 50);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(261, 373);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNode_Selected);
            // 
            // dataGridViewHandConfig
            // 
            this.dataGridViewHandConfig.AllowUserToAddRows = false;
            this.dataGridViewHandConfig.AllowUserToDeleteRows = false;
            this.dataGridViewHandConfig.AllowUserToOrderColumns = true;
            this.dataGridViewHandConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewHandConfig.AutoGenerateColumns = false;
            this.dataGridViewHandConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHandConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sessionIDDataGridViewTextBoxColumn,
            this.collectedInTouchPointDataGridViewTextBoxColumn,
            this.sessionNumberDataGridViewTextBoxColumn,
            this.handNumberDataGridViewTextBoxColumn,
            this.serialNumberDataGridViewTextBoxColumn,
            this.firmwareVersionDataGridViewTextBoxColumn,
            this.chiralityDataGridViewTextBoxColumn,
            this.nMotorsDataGridViewTextBoxColumn,
            this.resetCauseDataGridViewTextBoxColumn,
            this.activeTimeDataGridViewTextBoxColumn,
            this.onTimeDataGridViewTextBoxColumn,
            this.batteryMinVDataGridViewTextBoxColumn,
            this.batteryMaxVDataGridViewTextBoxColumn,
            this.tempMinCDataGridViewTextBoxColumn,
            this.tempMaxCDataGridViewTextBoxColumn,
            this.magMaxXDataGridViewTextBoxColumn,
            this.magMaxYDataGridViewTextBoxColumn,
            this.accelMaxXDataGridViewTextBoxColumn,
            this.accelMaxYDataGridViewTextBoxColumn,
            this.accelMaxZDataGridViewTextBoxColumn});
            this.dataGridViewHandConfig.DataSource = this.sessionsBindingSource;
            this.dataGridViewHandConfig.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewHandConfig.Location = new System.Drawing.Point(5, 6);
            this.dataGridViewHandConfig.Name = "dataGridViewHandConfig";
            this.dataGridViewHandConfig.ReadOnly = true;
            this.dataGridViewHandConfig.Size = new System.Drawing.Size(937, 471);
            this.dataGridViewHandConfig.TabIndex = 2;
            // 
            // sessionIDDataGridViewTextBoxColumn
            // 
            this.sessionIDDataGridViewTextBoxColumn.DataPropertyName = "SessionID";
            this.sessionIDDataGridViewTextBoxColumn.HeaderText = "SessionID";
            this.sessionIDDataGridViewTextBoxColumn.Name = "sessionIDDataGridViewTextBoxColumn";
            this.sessionIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // collectedInTouchPointDataGridViewTextBoxColumn
            // 
            this.collectedInTouchPointDataGridViewTextBoxColumn.DataPropertyName = "CollectedInTouchPoint";
            this.collectedInTouchPointDataGridViewTextBoxColumn.HeaderText = "CollectedInTouchPoint";
            this.collectedInTouchPointDataGridViewTextBoxColumn.Name = "collectedInTouchPointDataGridViewTextBoxColumn";
            this.collectedInTouchPointDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sessionNumberDataGridViewTextBoxColumn
            // 
            this.sessionNumberDataGridViewTextBoxColumn.DataPropertyName = "SessionNumber";
            this.sessionNumberDataGridViewTextBoxColumn.HeaderText = "SessionNumber";
            this.sessionNumberDataGridViewTextBoxColumn.Name = "sessionNumberDataGridViewTextBoxColumn";
            this.sessionNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // handNumberDataGridViewTextBoxColumn
            // 
            this.handNumberDataGridViewTextBoxColumn.DataPropertyName = "HandNumber";
            this.handNumberDataGridViewTextBoxColumn.HeaderText = "HandNumber";
            this.handNumberDataGridViewTextBoxColumn.Name = "handNumberDataGridViewTextBoxColumn";
            this.handNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // serialNumberDataGridViewTextBoxColumn
            // 
            this.serialNumberDataGridViewTextBoxColumn.DataPropertyName = "serialNumber";
            this.serialNumberDataGridViewTextBoxColumn.HeaderText = "serialNumber";
            this.serialNumberDataGridViewTextBoxColumn.Name = "serialNumberDataGridViewTextBoxColumn";
            this.serialNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // firmwareVersionDataGridViewTextBoxColumn
            // 
            this.firmwareVersionDataGridViewTextBoxColumn.DataPropertyName = "firmwareVersion";
            this.firmwareVersionDataGridViewTextBoxColumn.HeaderText = "firmwareVersion";
            this.firmwareVersionDataGridViewTextBoxColumn.Name = "firmwareVersionDataGridViewTextBoxColumn";
            this.firmwareVersionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // chiralityDataGridViewTextBoxColumn
            // 
            this.chiralityDataGridViewTextBoxColumn.DataPropertyName = "chirality";
            this.chiralityDataGridViewTextBoxColumn.HeaderText = "chirality";
            this.chiralityDataGridViewTextBoxColumn.Name = "chiralityDataGridViewTextBoxColumn";
            this.chiralityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nMotorsDataGridViewTextBoxColumn
            // 
            this.nMotorsDataGridViewTextBoxColumn.DataPropertyName = "nMotors";
            this.nMotorsDataGridViewTextBoxColumn.HeaderText = "nMotors";
            this.nMotorsDataGridViewTextBoxColumn.Name = "nMotorsDataGridViewTextBoxColumn";
            this.nMotorsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // resetCauseDataGridViewTextBoxColumn
            // 
            this.resetCauseDataGridViewTextBoxColumn.DataPropertyName = "resetCause";
            this.resetCauseDataGridViewTextBoxColumn.HeaderText = "resetCause";
            this.resetCauseDataGridViewTextBoxColumn.Name = "resetCauseDataGridViewTextBoxColumn";
            this.resetCauseDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // activeTimeDataGridViewTextBoxColumn
            // 
            this.activeTimeDataGridViewTextBoxColumn.DataPropertyName = "activeTime";
            this.activeTimeDataGridViewTextBoxColumn.HeaderText = "activeTime";
            this.activeTimeDataGridViewTextBoxColumn.Name = "activeTimeDataGridViewTextBoxColumn";
            this.activeTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // onTimeDataGridViewTextBoxColumn
            // 
            this.onTimeDataGridViewTextBoxColumn.DataPropertyName = "onTime";
            this.onTimeDataGridViewTextBoxColumn.HeaderText = "onTime";
            this.onTimeDataGridViewTextBoxColumn.Name = "onTimeDataGridViewTextBoxColumn";
            this.onTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // batteryMinVDataGridViewTextBoxColumn
            // 
            this.batteryMinVDataGridViewTextBoxColumn.DataPropertyName = "BatteryMinV";
            this.batteryMinVDataGridViewTextBoxColumn.HeaderText = "BatteryMinV";
            this.batteryMinVDataGridViewTextBoxColumn.Name = "batteryMinVDataGridViewTextBoxColumn";
            this.batteryMinVDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // batteryMaxVDataGridViewTextBoxColumn
            // 
            this.batteryMaxVDataGridViewTextBoxColumn.DataPropertyName = "BatteryMaxV";
            this.batteryMaxVDataGridViewTextBoxColumn.HeaderText = "BatteryMaxV";
            this.batteryMaxVDataGridViewTextBoxColumn.Name = "batteryMaxVDataGridViewTextBoxColumn";
            this.batteryMaxVDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tempMinCDataGridViewTextBoxColumn
            // 
            this.tempMinCDataGridViewTextBoxColumn.DataPropertyName = "TempMinC";
            this.tempMinCDataGridViewTextBoxColumn.HeaderText = "TempMinC";
            this.tempMinCDataGridViewTextBoxColumn.Name = "tempMinCDataGridViewTextBoxColumn";
            this.tempMinCDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tempMaxCDataGridViewTextBoxColumn
            // 
            this.tempMaxCDataGridViewTextBoxColumn.DataPropertyName = "TempMaxC";
            this.tempMaxCDataGridViewTextBoxColumn.HeaderText = "TempMaxC";
            this.tempMaxCDataGridViewTextBoxColumn.Name = "tempMaxCDataGridViewTextBoxColumn";
            this.tempMaxCDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // magMaxXDataGridViewTextBoxColumn
            // 
            this.magMaxXDataGridViewTextBoxColumn.DataPropertyName = "MagMaxX";
            this.magMaxXDataGridViewTextBoxColumn.HeaderText = "MagMaxX";
            this.magMaxXDataGridViewTextBoxColumn.Name = "magMaxXDataGridViewTextBoxColumn";
            this.magMaxXDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // magMaxYDataGridViewTextBoxColumn
            // 
            this.magMaxYDataGridViewTextBoxColumn.DataPropertyName = "MagMaxY";
            this.magMaxYDataGridViewTextBoxColumn.HeaderText = "MagMaxY";
            this.magMaxYDataGridViewTextBoxColumn.Name = "magMaxYDataGridViewTextBoxColumn";
            this.magMaxYDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // accelMaxXDataGridViewTextBoxColumn
            // 
            this.accelMaxXDataGridViewTextBoxColumn.DataPropertyName = "AccelMaxX";
            this.accelMaxXDataGridViewTextBoxColumn.HeaderText = "AccelMaxX";
            this.accelMaxXDataGridViewTextBoxColumn.Name = "accelMaxXDataGridViewTextBoxColumn";
            this.accelMaxXDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // accelMaxYDataGridViewTextBoxColumn
            // 
            this.accelMaxYDataGridViewTextBoxColumn.DataPropertyName = "AccelMaxY";
            this.accelMaxYDataGridViewTextBoxColumn.HeaderText = "AccelMaxY";
            this.accelMaxYDataGridViewTextBoxColumn.Name = "accelMaxYDataGridViewTextBoxColumn";
            this.accelMaxYDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // accelMaxZDataGridViewTextBoxColumn
            // 
            this.accelMaxZDataGridViewTextBoxColumn.DataPropertyName = "AccelMaxZ";
            this.accelMaxZDataGridViewTextBoxColumn.HeaderText = "AccelMaxZ";
            this.accelMaxZDataGridViewTextBoxColumn.Name = "accelMaxZDataGridViewTextBoxColumn";
            this.accelMaxZDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sessionsBindingSource
            // 
            this.sessionsBindingSource.DataMember = "sessions";
            this.sessionsBindingSource.DataSource = this.heroUsageDataDataSet;
            // 
            // heroUsageDataDataSet
            // 
            this.heroUsageDataDataSet.DataSetName = "HeroUsageDataDataSet";
            this.heroUsageDataDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(261, 28);
            this.button2.TabIndex = 11;
            this.button2.Text = "Export to Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ExportToExcel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(294, 94);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(957, 509);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridViewHandConfig);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(949, 483);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sessions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(945, 448);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Touch Points";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.touchPointIDDataGridViewTextBoxColumn,
            this.handNumberDataGridViewTextBoxColumn1,
            this.touchPointIndexDataGridViewTextBoxColumn,
            this.dateDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.touchPointsBindingSource;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(933, 474);
            this.dataGridView1.TabIndex = 3;
            // 
            // touchPointIDDataGridViewTextBoxColumn
            // 
            this.touchPointIDDataGridViewTextBoxColumn.DataPropertyName = "TouchPointID";
            this.touchPointIDDataGridViewTextBoxColumn.HeaderText = "TouchPointID";
            this.touchPointIDDataGridViewTextBoxColumn.Name = "touchPointIDDataGridViewTextBoxColumn";
            this.touchPointIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // handNumberDataGridViewTextBoxColumn1
            // 
            this.handNumberDataGridViewTextBoxColumn1.DataPropertyName = "HandNumber";
            this.handNumberDataGridViewTextBoxColumn1.HeaderText = "HandNumber";
            this.handNumberDataGridViewTextBoxColumn1.Name = "handNumberDataGridViewTextBoxColumn1";
            this.handNumberDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // touchPointIndexDataGridViewTextBoxColumn
            // 
            this.touchPointIndexDataGridViewTextBoxColumn.DataPropertyName = "TouchPointIndex";
            this.touchPointIndexDataGridViewTextBoxColumn.HeaderText = "TouchPointIndex";
            this.touchPointIndexDataGridViewTextBoxColumn.Name = "touchPointIndexDataGridViewTextBoxColumn";
            this.touchPointIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dateDataGridViewTextBoxColumn
            // 
            this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
            this.dateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            this.dateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // touchPointsBindingSource
            // 
            this.touchPointsBindingSource.DataMember = "touchPoints";
            this.touchPointsBindingSource.DataSource = this.heroUsageDataDataSet;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(949, 483);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Hands";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView2.Location = new System.Drawing.Point(6, 6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(937, 471);
            this.dataGridView2.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.summariseSelectedDataButton);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 460);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import";
            // 
            // summariseSelectedDataButton
            // 
            this.summariseSelectedDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.summariseSelectedDataButton.Location = new System.Drawing.Point(6, 429);
            this.summariseSelectedDataButton.Name = "summariseSelectedDataButton";
            this.summariseSelectedDataButton.Size = new System.Drawing.Size(261, 25);
            this.summariseSelectedDataButton.TabIndex = 18;
            this.summariseSelectedDataButton.Text = "Import Selected";
            this.summariseSelectedDataButton.UseVisualStyleBackColor = true;
            this.summariseSelectedDataButton.Click += new System.EventHandler(this.importSelected_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(15, 548);
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
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.label1);
            this.groupBox9.Location = new System.Drawing.Point(294, 4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(957, 80);
            this.groupBox9.TabIndex = 16;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Selection Details";
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
            // sessionsTableAdapter
            // 
            this.sessionsTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.sessionsTableAdapter = this.sessionsTableAdapter;
            this.tableAdapterManager.touchPointsTableAdapter = this.touchPointsTableAdapter;
            this.tableAdapterManager.UpdateOrder = Usage_Data_Parser.HeroUsageDataDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // touchPointsTableAdapter
            // 
            this.touchPointsTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 615);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Usage Data Parser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHandConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heroUsageDataDataSet)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.touchPointsBindingSource)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dataGridViewHandConfig;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button summariseSelectedDataButton;
        private HeroUsageDataDataSet heroUsageDataDataSet;
        private System.Windows.Forms.BindingSource sessionsBindingSource;
        private HeroUsageDataDataSetTableAdapters.sessionsTableAdapter sessionsTableAdapter;
        private HeroUsageDataDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectedInTouchPointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firmwareVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chiralityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nMotorsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resetCauseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn activeTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn onTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn batteryMinVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn batteryMaxVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tempMinCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tempMaxCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn magMaxXDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn magMaxYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accelMaxXDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accelMaxYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accelMaxZDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private HeroUsageDataDataSetTableAdapters.touchPointsTableAdapter touchPointsTableAdapter;
        private System.Windows.Forms.BindingSource touchPointsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn touchPointIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handNumberDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn touchPointIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}

