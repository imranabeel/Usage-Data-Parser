using MicroJson;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Usage_Data_Parser
{


    public partial class Form1 : Form
    {
        private List<ParsedJSONFile> parsedFiles = new List<ParsedJSONFile>();
        List<TreeNode> selectedDataFiles = new List<TreeNode>();
        private string rootNodePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void folderSelectButton(object sender, EventArgs e)
        {
            using (var directoryDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Folder"
            })
            {
                CommonFileDialogResult result = directoryDialog.ShowDialog();

                if (result == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(directoryDialog.FileName))
                {
                    rootNodePath = directoryDialog.FileName;
                    treeView1.BeginUpdate();
                    ListDirectory(treeView1, rootNodePath);
                    treeView1.EndUpdate();
                }
            }

            //using (var fbd = new FolderBrowserDialog())
            //{
            //    fbd.ShowNewFolderButton = true;
            //    fbd.Description = "Please select the folder which contains the usage data you want to view:";
            //    DialogResult result = fbd.ShowDialog();

            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //        rootNodePath = fbd.SelectedPath;
            //        treeView1.BeginUpdate();
            //        ListDirectory(treeView1, rootNodePath);
            //        treeView1.EndUpdate();
            //    }
            //}
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name) { Tag = directoryInfo.FullName };
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name) { Tag = file.FullName });
            return directoryNode;
        }

        private TreeNode oldNode;

        private void FileFolderSelected(object sender, TreeViewEventArgs e)
        {
            ClearDataGrids();
            label1.Text = "";
            label2.Text = "";
            if (oldNode != null)
            {
                Font normalFont = new Font(treeView1.Font, FontStyle.Regular);
                oldNode.NodeFont = normalFont;
            }
            Font boldFont = new Font(treeView1.Font, FontStyle.Bold);

            var node = treeView1.SelectedNode;
            node.NodeFont = boldFont;
            oldNode = node;

            var tag = (string)node.Tag;
            string fullPath = tag.ToString();

            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(fullPath);

            // If the user selects a file
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                // Attempt to parse the file.
                ParsedJSONFile jsonParsedData = ParseUsageDataFile(node, fullPath);
                ClearDataGrids();
                // If the file was parsed, display it.
                if (jsonParsedData != null)
                {
                    DisplayParsedJSON(jsonParsedData);
                }
            }
            else // If the user selects a folder
            {
                label1.Text = "Folder selected";
                List<TreeNode> childNodes = node.GetAllTreeNodes();

                selectedDataFiles.Clear();
                foreach (TreeNode childNode in childNodes)
                {
                    bool isDataFile = childNode.Tag.ToString().Contains("_DAT");
                    if (isDataFile)
                    {
                        selectedDataFiles.Add(childNode);
                    }
                }

                int numberOfSelectedDataFiles = selectedDataFiles.Count;
                label1.Text += " contains " + numberOfSelectedDataFiles.ToString() + " data file";
                if (numberOfSelectedDataFiles > 1)
                {
                    label1.Text += "s";
                }



            }
        }

        private void DisplaySummarisedData(int numberOfSessions, string averageOnTime, string averageActiveTime, string totalOnTime, string totalActiveTime, string mTraveled)
        {
            DataTable dt2 = new DataTable();
            DataColumn[] columns2 = { new DataColumn("Number of Sessions"), new DataColumn("Average On Time"), new DataColumn("Average Active Time"), new DataColumn("Total On Time"), new DataColumn("Total Active Time"), new DataColumn("Total m Traveled") };
            dt2.Columns.AddRange(columns2);
            Object[] row2 = { numberOfSessions, averageOnTime, averageActiveTime, totalOnTime, totalActiveTime, mTraveled };
            dt2.Rows.Add(row2);
            dataGridViewSummary.DataSource = dt2;
        }

        private void DisplayParsedJSON(ParsedJSONFile jsonParsedData)
        {
            DataTable dt = new DataTable();
            if (jsonParsedData.handConfig != null)
            {
                DataColumn[] columns = { new DataColumn("Serial Num"), new DataColumn("Firmware Version"), new DataColumn("Chirality"), new DataColumn("nMotors") };
                dt.Columns.AddRange(columns);
                Object[] row = { jsonParsedData.handConfig.serialNum, jsonParsedData.handConfig.fwVer, jsonParsedData.handConfig.chirality, jsonParsedData.handConfig.nMotors };
                dt.Rows.Add(row);
                dataGridViewHandConfig.DataSource = dt;
            }

            if ((jsonParsedData.time != null) && (jsonParsedData.resetCause != null))
            {
                DataTable dt2 = new DataTable();
                DataColumn[] columns2 = { new DataColumn("Session Num"), new DataColumn("Reset Cause"), new DataColumn("On Time"), new DataColumn("Active Time"), new DataColumn("Error Num"), new DataColumn("Error Severity"), new DataColumn("Error Description") };
                dt2.Columns.AddRange(columns2);
                if (jsonParsedData.error != null)
                {
                    Object[] row2 = { jsonParsedData.sessionN, jsonParsedData.resetCause, jsonParsedData.time.onTime, jsonParsedData.time.activeTime, jsonParsedData.error.num, jsonParsedData.error.severity, jsonParsedData.error.description };
                    dt2.Rows.Add(row2);
                }
                else
                {
                    Object[] row2 = { jsonParsedData.sessionN, jsonParsedData.resetCause, jsonParsedData.time.onTime, jsonParsedData.time.activeTime, "no data", "no data", "no data" };
                    dt2.Rows.Add(row2);
                }

                dataGridViewSummary.DataSource = dt2;
            }

            if (jsonParsedData.grip != null)
            {
                DataTable dt3 = new DataTable();
                DataColumn[] columns3 = { new DataColumn("Grip Group"), new DataColumn("Grip"), new DataColumn("Duration") };
                dt3.Columns.AddRange(columns3);
                dt3.Columns[0].DataType = typeof(int);
                foreach (Group group in jsonParsedData.grip.gripGroups)
                {
                    foreach (GripChild grip in group.grips)
                    {
                        Object[] row3 = { int.Parse(group.n), grip.name, grip.duration };
                        dt3.Rows.Add(row3);
                    }
                }
                dataGridViewGrips.DataSource = dt3;
            }

            if (jsonParsedData.battery != null)
            {
                DataTable dt4 = new DataTable();
                DataColumn[] columns4 = { new DataColumn("Type"), new DataColumn("n"), new DataColumn("Batt Voltage"), new DataColumn("Duration") };
                dt4.Columns.AddRange(columns4);
                Object[] row4 = { "Min", jsonParsedData.battery.min.n, jsonParsedData.battery.min.battV, jsonParsedData.battery.min.duration };
                dt4.Rows.Add(row4);
                Object[] row5 = { "Max", jsonParsedData.battery.max.n, jsonParsedData.battery.max.battV, jsonParsedData.battery.max.duration };
                dt4.Rows.Add(row5);
                dataGridViewBattSummary.DataSource = dt4;

                DataTable dt5 = new DataTable();
                DataColumn[] columns5 = { new DataColumn("n"), new DataColumn("Batt Voltage"), new DataColumn("Duration") };
                dt5.Columns.AddRange(columns5);
                dt5.Columns[0].DataType = typeof(int);
                foreach (BattSample batt in jsonParsedData.battery.battSamples)
                {
                    Object[] row6 = { int.Parse(batt.n), batt.battV, batt.duration };
                    dt5.Rows.Add(row6);
                }
                dataGridViewBattSamples.DataSource = dt5;
            }

            if (jsonParsedData.temp != null)
            {
                DataTable dt6 = new DataTable();
                DataColumn[] columns6 = { new DataColumn("Type"), new DataColumn("n"), new DataColumn("Temp (Celcius)"), new DataColumn("Duration") };
                dt6.Columns.AddRange(columns6);
                Object[] row7 = { "Min", jsonParsedData.temp.minTemp.n, jsonParsedData.temp.minTemp.tempC, jsonParsedData.temp.minTemp.duration };
                dt6.Rows.Add(row7);
                Object[] row8 = { "Max", jsonParsedData.temp.maxTemp.n, jsonParsedData.temp.maxTemp.tempC, jsonParsedData.temp.maxTemp.tempC };
                dt6.Rows.Add(row8);
                dataGridViewTempSummary.DataSource = dt6;

                DataTable dt7 = new DataTable();
                DataColumn[] columns7 = { new DataColumn("n"), new DataColumn("Temp (Celcius)"), new DataColumn("Duration") };
                dt7.Columns.AddRange(columns7);
                dt7.Columns[0].DataType = typeof(int);
                foreach (TempSample temp in jsonParsedData.temp.tempSamples)
                {
                    Object[] row9 = { int.Parse(temp.n), temp.tempC, temp.duration };
                    dt7.Rows.Add(row9);
                }
                dataGridViewTempSamples.DataSource = dt7;
            }

            if (jsonParsedData.magFlux != null)
            {
                DataTable dt8 = new DataTable();
                DataColumn[] columns8 = { new DataColumn("Axis"), new DataColumn("Max"), new DataColumn("Units"), new DataColumn("Duration") };
                dt8.Columns.AddRange(columns8);
                Object[] row10 = { "X", jsonParsedData.magFlux.X.max, jsonParsedData.magFlux.unit, jsonParsedData.magFlux.X.duration };
                Object[] row11 = { "Y", jsonParsedData.magFlux.Y.max, jsonParsedData.magFlux.unit, jsonParsedData.magFlux.Y.duration };
                dt8.Rows.Add(row10);
                dt8.Rows.Add(row11);
                dataGridViewMagFlux.DataSource = dt8;
            }

            if (jsonParsedData.accel != null)
            {
                DataTable dt9 = new DataTable();
                DataColumn[] columns9 = { new DataColumn("Axis"), new DataColumn("Max"), new DataColumn("Units"), new DataColumn("Duration") };
                dt9.Columns.AddRange(columns9);
                Object[] row12 = { "X", jsonParsedData.accel.X.max, jsonParsedData.accel.unit, jsonParsedData.accel.X.duration };
                Object[] row13 = { "Y", jsonParsedData.accel.Y.max, jsonParsedData.accel.unit, jsonParsedData.accel.Y.duration };
                Object[] row14 = { "Z", jsonParsedData.accel.Z.max, jsonParsedData.accel.unit, jsonParsedData.accel.Z.duration };
                dt9.Rows.Add(row12);
                dt9.Rows.Add(row13);
                dt9.Rows.Add(row14);
                dataGridViewAccel.DataSource = dt9;
            }
        }

        private void ClearDataGrids()
        {
            dataGridViewHandConfig.Columns.Clear();
            dataGridViewSummary.Columns.Clear();
            dataGridViewGrips.Columns.Clear();
            dataGridViewBattSummary.Columns.Clear();
            dataGridViewBattSamples.Columns.Clear();
            dataGridViewTempSummary.Columns.Clear();
            dataGridViewTempSamples.Columns.Clear();
            dataGridViewMagFlux.Columns.Clear();
            dataGridViewAccel.Columns.Clear();
        }

        private ParsedJSONFile ParseUsageDataFile(TreeNode node, string fullPath)
        {
            ParsedJSONFile _jsonParsedData;

            string jsonInputData = File.ReadAllText(fullPath);
            try
            {
                _jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
            }
            catch (ParserException ex)
            {
                DodgyJsonParser dodgyJsonParser = new DodgyJsonParser();
                _jsonParsedData = dodgyJsonParser.parseDodgyFile(jsonInputData);
            }

            label1.Text = node.FullPath;
            if (_jsonParsedData.sessions > 1)
            {
                label2.Text = "Multiple Sessions detected in this file. Please check file manually";
                label2.Font = new System.Drawing.Font(label2.Font, System.Drawing.FontStyle.Bold);
            }
            else if (_jsonParsedData.dodgyFile)
            {
                label2.Text = "Null Character Detected in File. This file may need checking manually as well.";
                label2.Font = new System.Drawing.Font(label2.Font, System.Drawing.FontStyle.Regular);
            }
            else
            {
                label2.Text = "";
            }

            return _jsonParsedData;

        }

        private void dataGridViewGrips_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //Suppose your interested column has index 1
            if (e.Column.Index == 0)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;//pass by the default sorting
            }
        }

        private void dataGridViewTemp_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //Suppose your interested column has index 1
            if (e.Column.Index == 0)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;//pass by the default sorting
            }
        }

        private void dataGridViewBattery_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //Suppose your interested column has index 1
            if (e.Column.Index == 0)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;//pass by the default sorting
            }
        }

        private void ExportToExcel(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            var tag = (string)node.Tag;
            string fullPath = tag.ToString();

            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(fullPath);

            if (!attr.HasFlag(FileAttributes.Directory)) // is a file
            {
                ParsedJSONFile jsonParsedData;

                string jsonInputData = File.ReadAllText(fullPath);
                try
                {
                    jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
                }
                catch (ParserException ex)
                {
                    DodgyJsonParser dodgyJsonParser = new DodgyJsonParser();
                    jsonParsedData = dodgyJsonParser.parseDodgyFile(jsonInputData);
                }

                Excel._Application excel = new Excel.Application();
                Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                Excel._Worksheet worksheet = null;

                try
                {
                    worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                    workbook.Sheets.Add(Count: 5);

                    worksheet = (Excel.Worksheet)workbook.Sheets[1];

                    worksheet.Name = node.Text;

                    worksheet.Cells[1, 1] = "Serial Number";
                    worksheet.Cells[1, 2] = "Firmware Version";
                    worksheet.Cells[1, 3] = "Chirality";
                    worksheet.Cells[1, 4] = "nMotors";
                    worksheet.Cells[1, 5] = "Session Number";
                    worksheet.Cells[1, 6] = "Reset Cause";
                    worksheet.Cells[1, 7] = "ON time";
                    worksheet.Cells[1, 8] = "Active Time";

                    if (jsonParsedData != null)
                    {
                        if (jsonParsedData.handConfig != null)
                        {
                            worksheet.Cells[2, 1] = jsonParsedData.handConfig.serialNum;
                            worksheet.Cells[2, 2] = jsonParsedData.handConfig.fwVer;
                            worksheet.Cells[2, 3] = jsonParsedData.handConfig.chirality;
                            worksheet.Cells[2, 4] = jsonParsedData.handConfig.nMotors;
                        }

                        if ((jsonParsedData.time != null) && (jsonParsedData.resetCause != null))
                        {
                            worksheet.Cells[2, 5] = jsonParsedData.sessionN;
                            worksheet.Cells[2, 6] = jsonParsedData.resetCause;
                            worksheet.Cells[2, 7] = jsonParsedData.time.onTime;
                            worksheet.Cells[2, 8] = jsonParsedData.time.activeTime;
                        }

                        if (jsonParsedData.grip != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[2];//.Select();
                            worksheet.Name = "Grip";

                            worksheet.Cells[1, 1] = "Grip Group";
                            worksheet.Cells[1, 2] = "Grip Type";
                            worksheet.Cells[1, 3] = "Duration";

                            int row = 1;

                            foreach (Group group in jsonParsedData.grip.gripGroups)
                            {
                                foreach (GripChild grip in group.grips)
                                {
                                    row++;
                                    worksheet.Cells[row, 1] = group.n;
                                    worksheet.Cells[row, 2] = grip.name;
                                    worksheet.Cells[row, 3] = grip.duration;
                                }
                            }
                        }

                        if (jsonParsedData.battery != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[3];//.Select();
                            worksheet.Name = "Battery";

                            worksheet.Cells[1, 1] = "Type";
                            worksheet.Cells[1, 2] = "N";
                            worksheet.Cells[1, 3] = "Battery Voltage";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "Min";
                            worksheet.Cells[2, 2] = jsonParsedData.battery.min.n;
                            worksheet.Cells[2, 3] = jsonParsedData.battery.min.battV;
                            worksheet.Cells[2, 4] = jsonParsedData.battery.min.duration;

                            worksheet.Cells[3, 1] = "Max";
                            worksheet.Cells[3, 2] = jsonParsedData.battery.max.n;
                            worksheet.Cells[3, 3] = jsonParsedData.battery.max.battV;
                            worksheet.Cells[3, 4] = jsonParsedData.battery.max.duration;

                            worksheet.Cells[5, 2] = "n";
                            worksheet.Cells[5, 3] = "Battery Voltage";
                            worksheet.Cells[5, 4] = "Duration";

                            int row = 5;

                            foreach (BattSample batt in jsonParsedData.battery.battSamples)
                            {
                                row++;

                                worksheet.Cells[row, 2] = batt.n;
                                worksheet.Cells[row, 3] = batt.battV;
                                worksheet.Cells[row, 4] = batt.duration;
                            }
                        }

                        if (jsonParsedData.temp != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[4];//.Select();
                            worksheet.Name = "Temp";

                            worksheet.Cells[1, 1] = "Type";
                            worksheet.Cells[1, 2] = "N";
                            worksheet.Cells[1, 3] = "Temperature (celcius)";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "Min";
                            worksheet.Cells[2, 2] = jsonParsedData.temp.minTemp.n;
                            worksheet.Cells[2, 3] = jsonParsedData.temp.minTemp.tempC;
                            worksheet.Cells[2, 4] = jsonParsedData.temp.minTemp.duration;

                            worksheet.Cells[3, 1] = "Max";
                            worksheet.Cells[3, 2] = jsonParsedData.temp.maxTemp.n;
                            worksheet.Cells[3, 3] = jsonParsedData.temp.maxTemp.tempC;
                            worksheet.Cells[3, 4] = jsonParsedData.temp.maxTemp.duration;

                            worksheet.Cells[5, 2] = "n";
                            worksheet.Cells[5, 3] = "Temperature (celcius)";
                            worksheet.Cells[5, 4] = "Duration";

                            int row = 5;

                            foreach (TempSample temp in jsonParsedData.temp.tempSamples)
                            {
                                row++;

                                worksheet.Cells[row, 2] = temp.n;
                                worksheet.Cells[row, 3] = temp.tempC;
                                worksheet.Cells[row, 4] = temp.duration;
                            }
                        }

                        if (jsonParsedData.magFlux != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[5];//.Select();
                            worksheet.Name = "Magnetic Flux";

                            worksheet.Cells[1, 1] = "Axis";
                            worksheet.Cells[1, 2] = "Max";
                            worksheet.Cells[1, 3] = "Units";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "X";
                            worksheet.Cells[2, 2] = jsonParsedData.magFlux.X.max;
                            worksheet.Cells[2, 3] = jsonParsedData.magFlux.unit;
                            worksheet.Cells[2, 4] = jsonParsedData.magFlux.X.duration;

                            worksheet.Cells[3, 1] = "Y";
                            worksheet.Cells[3, 2] = jsonParsedData.magFlux.Y.max;
                            worksheet.Cells[3, 3] = jsonParsedData.magFlux.unit;
                            worksheet.Cells[3, 4] = jsonParsedData.magFlux.Y.duration;
                        }

                        if (jsonParsedData.accel != null)
                        {
                            worksheet = (Excel.Worksheet)workbook.Sheets[6];//.Select();
                            worksheet.Name = "Acceleration";

                            worksheet.Cells[1, 1] = "Axis";
                            worksheet.Cells[1, 2] = "Max";
                            worksheet.Cells[1, 3] = "Units";
                            worksheet.Cells[1, 4] = "Duration";

                            worksheet.Cells[2, 1] = "X";
                            worksheet.Cells[2, 2] = jsonParsedData.accel.X.max;
                            worksheet.Cells[2, 3] = jsonParsedData.accel.unit;
                            worksheet.Cells[2, 4] = jsonParsedData.accel.X.duration;

                            worksheet.Cells[3, 1] = "Y";
                            worksheet.Cells[3, 2] = jsonParsedData.accel.Y.max;
                            worksheet.Cells[3, 3] = jsonParsedData.accel.unit;
                            worksheet.Cells[3, 4] = jsonParsedData.accel.Y.duration;

                            worksheet.Cells[4, 1] = "Z";
                            worksheet.Cells[4, 2] = jsonParsedData.accel.Z.max;
                            worksheet.Cells[4, 3] = jsonParsedData.accel.unit;
                            worksheet.Cells[4, 4] = jsonParsedData.accel.Z.duration;
                        }
                    }

                    //Getting the location and file name of the excel to save from user.
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveDialog.FilterIndex = 2;

                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Export Successful");
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                }
                finally
                {
                    excel.Quit();
                    workbook = null;
                    excel = null;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //// TODO: This line of code loads data into the 'myFirstDatabaseDataSet.test' table. You can move, or remove it, as needed.
            //this.testTableAdapter.Fill(this.myFirstDatabaseDataSet.test);
            this.Text += " v" + Version.getVersion();

            string lastOpenedUsageDataPath = Properties.Settings.Default.usageDataPath;

            if (lastOpenedUsageDataPath != null && lastOpenedUsageDataPath != "")
            {
                treeView1.BeginUpdate();
                ListDirectory(treeView1, lastOpenedUsageDataPath);
                treeView1.EndUpdate();
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rootNodePath != "" & rootNodePath != null)
            {
                Properties.Settings.Default.usageDataPath = rootNodePath;
                Properties.Settings.Default.Save();
            }
        }

        private void summariseSelectedDataButton_Click(object sender, EventArgs e)
        {
            int numberOfSelectedDataFiles = selectedDataFiles.Count;
            if (numberOfSelectedDataFiles > 0)
            {
                List<TreeNode> newFiles = new List<TreeNode>();
                List<String> newHashes = new List<String>();
                List<DateTime> newDataCollectionDates = new List<DateTime>();
                List<String> newHandNumbers = new List<String>();

                
                for (int i = 0; i < numberOfSelectedDataFiles; i++)
                {
                    TreeNode _node = selectedDataFiles[i];
                    DateTime dataCollectionDate;
                    string handNumber;
                    try
                    {
                        dataCollectionDate = GetDataCollectionDate(_node);
                    }
                    catch (System.NullReferenceException)
                    {
                        dataCollectionDate = DateTime.MinValue;
                    }
                    try
                    {
                        handNumber = GetHandNumber(_node);
                    }
                    catch (System.NullReferenceException)
                    {
                        handNumber = null;
                    }

                    string _fullpath = _node.Tag.ToString();
                    byte[] hash;
                    using (FileStream stream = File.OpenRead(_fullpath))
                    {
                        var sha265 = SHA256.Create();
                        hash = sha265.ComputeHash(stream);
                    }

                    // Convert hash to string for human readability
                    string hashString = "";
                    foreach (byte b in hash)
                    {
                        hashString += b.ToString("x2");
                    }

                    //Establish Connection to database
                    SqlConnection connection = EstablishConnectionToSqlDatabase();

                    // Search to see if the record already exists
                    SqlDataReader dataReader;
                    string sql = "SELECT COUNT(1) FROM test WHERE SessionID = '" + hashString + "';";
                    SqlCommand command = new SqlCommand(sql, connection);
                    dataReader = command.ExecuteReader();
                    String sqlReadOutput = "";
                    while (dataReader.Read())
                    {
                        sqlReadOutput += dataReader.GetValue(0) + "\n";
                    }
                    dataReader.Close();
                    int numberOfMatchingRecords = Int32.Parse(sqlReadOutput);

                    // If there is a duplicate of a record in the selected files, it won't be in the database yet
                    // so each will pass the check. Therefore they list to add is also checked for duplicates
                    bool isAlreadyInUpload = newHashes.Contains(hashString);

                    // If it's not in the database or already set to be inserted
                    if (numberOfMatchingRecords == 0 && !isAlreadyInUpload)
                    {
                        newHashes.Add(hashString);
                        newDataCollectionDates.Add(dataCollectionDate);
                        newHandNumbers.Add(handNumber);
                        newFiles.Add(selectedDataFiles[i]);
                    }


                }

                int numberOfNewFiles = newFiles.Count;
                ParsedJSONFile[] jsonParsedDataFiles = new ParsedJSONFile[numberOfNewFiles];
                for (int i = 0; i < numberOfNewFiles; i++)
                {
                    TreeNode _node = newFiles[i];
                    string _fullpath = _node.Tag.ToString();
                    jsonParsedDataFiles[i] = ParseUsageDataFile(_node, _fullpath);
                }
                TimeSpan averageActiveTime = TimeSpan.Zero;
                TimeSpan averageOnTime = TimeSpan.Zero;
                TimeSpan totalActiveTime = TimeSpan.Zero;
                TimeSpan totalOnTime = TimeSpan.Zero;
                int count = 0;
                //foreach (ParsedJSONFile file in jsonParsedDataFiles)
                for (int i = 0; i < numberOfNewFiles; i++)
                {
                    ParsedJSONFile file = jsonParsedDataFiles[i];
                    if (file != null)
                    {
                        TimeSpan sessionActiveTime = TimeSpan.Parse(file.time.activeTime);
                        TimeSpan sesssionOnTime = TimeSpan.Parse(file.time.onTime);
                        if (sessionActiveTime >= TimeSpan.Zero)
                        {
                            totalActiveTime += sessionActiveTime;
                            totalOnTime += sesssionOnTime;
                            count++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("File is null");
                    }

                    InsertSessionToSQLdatabase(newHashes[i], newDataCollectionDates[i], newHandNumbers[i], file);

                }
                if (count > 0)
                {
                    averageActiveTime = TimeSpan.FromTicks(totalActiveTime.Ticks / count);
                    averageOnTime = TimeSpan.FromTicks(totalOnTime.Ticks / count);
                }

                string averageActiveTimeString = averageActiveTime.ToString("g");
                string averageOnTimeString = averageOnTime.ToString("g");
                string totalActiveTimeString = totalActiveTime.ToString("g");
                string totalOnTimeString = totalOnTime.ToString("g");
                string total_m_traveled = ((float)totalActiveTime.Seconds * 23f / 1000f).ToString("g"); //23 mm per second at no load.

                DisplaySummarisedData(count, averageOnTimeString, averageActiveTimeString, totalOnTimeString, totalActiveTimeString, total_m_traveled);

                if (numberOfNewFiles > 0)
                {
                    label1.Text = numberOfNewFiles.ToString() + " sessions added to the database.";
                }
                else
                {
                    label1.Text = "No new sessions in selection. No sessions added to the database.";
                }
            }
        }

        private string GetHandNumber(TreeNode node)
        {
            TreeNode parent;
            try
            {
                parent = node.Parent;
            }
            catch (System.NullReferenceException)
            {
                throw;
            }

            string text = parent.Text;
            string pattern = @"^H[0-9]{1,5}$";
            Regex rx = new Regex(pattern);
            if (rx.IsMatch(text))
            {
                return text;
            }
            else
            {
                return GetHandNumber(parent);
            }
        }

        private DateTime GetDataCollectionDate(TreeNode node)
        {
            TreeNode parent;
            try
            {
                parent = node.Parent;
            }
            catch (System.NullReferenceException)
            {
                throw;
            }
            
            try
            {
                return DateTime.Parse(parent.Text);
            }
            catch (System.FormatException)
            {
                return GetDataCollectionDate(parent);
            }
        }

        private static SqlConnection EstablishConnectionToSqlDatabase()
        {
            string connectionString;
            SqlConnection connection;
            connectionString = @"Data Source=localhost\SQLEXPRESS; Database=myFirstDatabase; Integrated Security=True;";
            connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private void InsertSessionToSQLdatabase(string hashString, DateTime dataCollectionDate, string handNumber, ParsedJSONFile session)
        {
            SqlConnection connection = EstablishConnectionToSqlDatabase();
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "Insert into test (" +
                "SessionID, " +
                "DataCollectionDate, " +
                "SessionNumber, " +
                "HandNumber, " + 
                "serialNumber, " +
                "firmwareVersion, " +
                "chirality, " +
                "nMotors, " +
                "resetCause, " +
                "activeTime, " +
                "onTime, " +
                "BatteryMinV, " +
                "BatteryMaxV, " +
                "TempMinC, " +
                "TempMaxC," +
                "MagMaxX, " +
                "MagMaxY, " +
                "AccelMaxX, " +
                "AccelMaxY," +
                "AccelMaxz) " +
                " values (" +
                "@SessionID, " +
                "@DataCollectionDate, " +
                "@SessionNumber, " + 
                "@HandNumber, " + 
                "@serialNumber, " +
                "@firmwareVersion, " +
                "@chirality, " +
                "@nMotors, " +
                "@resetCause, " +
                "@activeTime, " +
                "@onTime, " +
                "@BatteryMinV, " +
                "@BatteryMaxV, " +
                "@TempMinC, " +
                "@TempMaxC ," +
                "@MagMaxX, " +
                "@MagMaxY, " +
                "@AccelMaxX, " +
                "@AccelMaxY, " +
                "@AccelMaxZ)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter sessionIDParam = new SqlParameter();
            command.Parameters.AddWithValue("@SessionID", hashString);
            if (dataCollectionDate != DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@DataCollectionDate", dataCollectionDate);
            }
            else
            {
                command.Parameters.AddWithValue("@dataCollectionDate", DBNull.Value);
            }
            command.Parameters.AddWithValue("@SessionNumber", session.sessionN);
            if (handNumber != null)
            {
                command.Parameters.AddWithValue("@HandNumber", handNumber);
            }
            else
            {
                command.Parameters.AddWithValue("@HandNumber", DBNull.Value);
            }
            command.Parameters.AddWithValue("@serialNumber", session.handConfig.serialNum);
            command.Parameters.AddWithValue("@firmwareVersion", session.handConfig.fwVer);
            command.Parameters.AddWithValue("@chirality", session.handConfig.chirality);
            command.Parameters.AddWithValue("@nMotors", session.handConfig.nMotors);
            command.Parameters.AddWithValue("@resetCause", session.resetCause);
            command.Parameters.AddWithValue("@activeTime", TimeSpan.Parse(session.time.activeTime));
            command.Parameters.AddWithValue("@onTime", TimeSpan.Parse(session.time.onTime));
            if (session.battery != null)
            {
                command.Parameters.AddWithValue("@BatteryMinV", Single.Parse(session.battery.min.battV));
                command.Parameters.AddWithValue("@BatteryMaxV", Single.Parse(session.battery.max.battV));
            }
            else
            {
                command.Parameters.AddWithValue("@BatteryMinV", DBNull.Value);
                command.Parameters.AddWithValue("@BatteryMaxV", DBNull.Value);
            }
            if (session.temp != null)
            {
                command.Parameters.AddWithValue("@TempMinC", Single.Parse(session.temp.minTemp.tempC));
                command.Parameters.AddWithValue("@TempMaxC", Single.Parse(session.temp.maxTemp.tempC));
            }
            else
            {
                command.Parameters.AddWithValue("@TempMinC", DBNull.Value);
                command.Parameters.AddWithValue("@TempMaxC", DBNull.Value);
            }
            if (session.magFlux != null)
            {
                command.Parameters.AddWithValue("@MagMaxX", Single.Parse(session.magFlux.X.max));
                command.Parameters.AddWithValue("@MagMaxY", Single.Parse(session.magFlux.Y.max));
            }
            else
            {
                command.Parameters.AddWithValue("@MagMaxX", DBNull.Value);
                command.Parameters.AddWithValue("@MagMaxY", DBNull.Value);
            }
            if (session.accel != null)
            {
                command.Parameters.AddWithValue("@AccelMaxX", Single.Parse(session.accel.X.max));
                command.Parameters.AddWithValue("@AccelMaxY", Single.Parse(session.accel.Y.max));
                command.Parameters.AddWithValue("@AccelMaxZ", Single.Parse(session.accel.Z.max));
            }
            else
            {
                command.Parameters.AddWithValue("@AccelMaxX", DBNull.Value);
                command.Parameters.AddWithValue("@AccelMaxY", DBNull.Value);
                command.Parameters.AddWithValue("@AccelMaxZ", DBNull.Value);
            }
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static class TreeViewExtensions
    {
        public static List<TreeNode> GetAllTreeNodes(this TreeNode _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in _self.Nodes)
            {
                result.Add(child);
                result.AddRange(child.GetAllTreeNodes());
            }
            return result;
        }
    }
}