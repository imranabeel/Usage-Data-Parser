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
        SqlConnection connection;


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
            //// TODO: This line of code loads data into the 'HeroUsageData.sessions' table. You can move, or remove it, as needed.
            //this.sessionsTableAdapter.Fill(thisHeroUsageData.sessions);
            this.Text += " v" + Version.getVersion();

            string lastOpenedUsageDataPath = Properties.Settings.Default.usageDataPath;

            if (lastOpenedUsageDataPath != null && lastOpenedUsageDataPath != "")
            {
                treeView1.BeginUpdate();
                ListDirectory(treeView1, lastOpenedUsageDataPath);
                treeView1.EndUpdate();
            }

            //Establish Connection to database
            connection = EstablishConnectionToSqlDatabase();

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
                List<String> touchPoints = new List<String>();
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


                    // Search to see if the record already exists
                    connection.Open();
                    SqlDataReader sessionIdReader;
                    string numberOfMatchingSessionsQuery = "SELECT COUNT(1) FROM sessions WHERE SessionID = @SessionID";
                    SqlCommand command = new SqlCommand(numberOfMatchingSessionsQuery, connection);
                    command.Parameters.AddWithValue("@SessionID", hashString);
                    sessionIdReader = command.ExecuteReader();
                    String numberOfMatchingSessionHashes = "";
                    while (sessionIdReader.Read())
                    {
                        numberOfMatchingSessionHashes += sessionIdReader.GetValue(0) + "\n";
                    }
                    int numberOfMatchingRecords = Int32.Parse(numberOfMatchingSessionHashes);
                    sessionIdReader.Close();
                    connection.Close();

                    // If there is a duplicate of a record in the selected files, it won't be in the database yet
                    // so each will pass the check. Therefore they list to add is also checked for duplicates
                    bool isAlreadyInUpload = newHashes.Contains(hashString);

                    // If it's not in the database or already set to be inserted
                    if (numberOfMatchingRecords == 0 && !isAlreadyInUpload)
                    {
                        newHashes.Add(hashString);
                        newHandNumbers.Add(handNumber);
                        newFiles.Add(selectedDataFiles[i]);

                        connection.Open();
                        SqlDataReader touchPointIdReader;
                        string numberOfMatchingTouchPointsQuery = "SELECT max(TouchPointIndex) as [Highest Index] FROM touchPoints WHERE HandNumber = @HandNumber ; " +
                            "SELECT TouchPointIndex AS 'Exists' FROM touchPoints WHERE HandNumber = @HandNumber AND Date = @Date";
                        SqlCommand sqlCommand = new SqlCommand(numberOfMatchingTouchPointsQuery, connection);
                        sqlCommand.Parameters.AddWithValue("@HandNumber", (object)handNumber ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Date", (object)dataCollectionDate.Date ?? DBNull.Value);
                        touchPointIdReader = sqlCommand.ExecuteReader();
                        int[] existingTouchPoints = new int[2] { -1, -1 }; // [0] = highest index of touch points for the hand in question (i.e count - 1). [1] = Does a touchpoint exist for this date?
                        int index = 0;
                        while (touchPointIdReader.HasRows)
                        {
                            Console.WriteLine("Result: {0}", touchPointIdReader.GetName(0));
                            while (touchPointIdReader.Read())
                            {
                                Console.WriteLine("value" + touchPointIdReader.GetValue(0).ToString());

                                existingTouchPoints[index] = touchPointIdReader.GetValue(0) != DBNull.Value ? (int)touchPointIdReader.GetValue(0) : -1;
                                index++;
                            }
                            touchPointIdReader.NextResult();
                        }

                        Console.WriteLine("latest touch point index for hand: " + existingTouchPoints[0].ToString());
                        Console.WriteLine("touch points for hand at date exists: " + existingTouchPoints[1].ToString());
                        touchPointIdReader.Close();
                        sqlCommand.Dispose();
                        connection.Close();
                        string touchPoint = "";
                        if (existingTouchPoints[1] == -1) // If there are no existing touch points for this session's hand and date, make one
                        {
                            connection.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            int newTouchPointIndex;
                            if (existingTouchPoints[0] == -1) // If there aren't any for the hand
                            {
                                newTouchPointIndex = 1; // Start on 1 as the send out date is 0
                            }
                            else
                            {
                                newTouchPointIndex = existingTouchPoints[0] + 1; // else use the latest one + 1
                            }
                            touchPoint = handNumber + "_" + newTouchPointIndex.ToString(); // make this this first touch point after the send out (i.e. touch point 1). 
                            string addTouchPoint = "insert into touchPoints (TouchPointID, HandNumber, TouchPointIndex, Date) values (@TouchPointID, @HandNumber, @TouchPointIndex, @Date)";
                            SqlCommand insertTouchPoint = new SqlCommand(addTouchPoint, connection);
                            insertTouchPoint.Parameters.AddWithValue("@TouchPointID", touchPoint); // Cannot be null
                            insertTouchPoint.Parameters.AddWithValue("@HandNumber", (object)handNumber ?? DBNull.Value);
                            insertTouchPoint.Parameters.AddWithValue("@TouchPointIndex", (object)newTouchPointIndex);
                            insertTouchPoint.Parameters.AddWithValue("@Date", (object)dataCollectionDate.Date ?? DBNull.Value);
                            adapter.InsertCommand = insertTouchPoint;
                            adapter.InsertCommand.ExecuteNonQuery();
                            adapter.Dispose();
                            insertTouchPoint.Dispose();
                            connection.Close();
                        }
                        else
                        {
                            touchPoint = handNumber + "_" + existingTouchPoints[1]; // Set it to the matching touchpoint
                        }
                        touchPoints.Add(touchPoint);
                    }


                }

                int numberOfNewFiles = newFiles.Count;
                ParsedJSONFile[] jsonParsedDataFiles = new ParsedJSONFile[numberOfNewFiles];
                for (int i = 0; i < numberOfNewFiles; i++)
                {
                    label1.Text = "Adding session " + i.ToString() + " of " + numberOfNewFiles.ToString() + ".";
                    TreeNode _node = newFiles[i];
                    string _fullpath = _node.Tag.ToString();
                    jsonParsedDataFiles[i] = ParseUsageDataFile(_node, _fullpath);
                }
                TimeSpan averageActiveTime = TimeSpan.Zero;
                TimeSpan averageOnTime = TimeSpan.Zero;
                TimeSpan totalActiveTime = TimeSpan.Zero;
                TimeSpan totalOnTime = TimeSpan.Zero;
                int count = 0;

                for (int i = 0; i < numberOfNewFiles; i++)
                {
                    ParsedJSONFile file = jsonParsedDataFiles[i];
                    if (file != null)
                    {
                        TimeSpan sessionActiveTime;
                        TimeSpan sessionOnTime;
                        if (file.time != null && TimeSpan.TryParse(file.time.activeTime, out sessionActiveTime) && TimeSpan.TryParse(file.time.onTime, out sessionOnTime))
                        {
                            totalActiveTime += sessionActiveTime;
                            totalOnTime += sessionOnTime;
                            count++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("File is null");
                    }

                    InsertSessionToSQLdatabase(newHashes[i], touchPoints[i], newHandNumbers[i], file);

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
            connectionString = @"Data Source=localhost\SQLEXPRESS; Database=HeroUsageData; Integrated Security=True;";
            connection = new SqlConnection(connectionString);
            return connection;
        }

        private void InsertSessionToSQLdatabase(string hashString, string touchPoint, string handNumber, ParsedJSONFile session)
        {
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "Insert into sessions (" +
                "SessionID, " +
                "CollectedInTouchPoint, " +
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
                "@CollectedInTouchPoint, " +
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
            command.Parameters.AddWithValue("@SessionID", hashString);
            command.Parameters.AddWithValue("@CollectedInTouchPoint", (object)touchPoint ?? DBNull.Value);
            command.Parameters.AddWithValue("@SessionNumber", session.sessionN); // Cannot be null
            command.Parameters.AddWithValue("@HandNumber", (object)handNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@serialNumber", session.handConfig != null ? (object)session.handConfig.serialNum : DBNull.Value);
            command.Parameters.AddWithValue("@firmwareVersion", session.handConfig != null ? (object)session.handConfig.fwVer : DBNull.Value);
            command.Parameters.AddWithValue("@chirality", session.handConfig != null ? (object)session.handConfig.chirality : DBNull.Value);
            command.Parameters.AddWithValue("@nMotors", session.handConfig != null? (object)session.handConfig.nMotors : DBNull.Value);
            command.Parameters.AddWithValue("@resetCause", (object)session.resetCause ?? DBNull.Value);
            command.Parameters.AddWithValue("@activeTime", session.time != null ? (object)TimeSpan.Parse(session.time.activeTime) : DBNull.Value);
            command.Parameters.AddWithValue("@onTime", session.time != null ? (object)TimeSpan.Parse(session.time.onTime) : DBNull.Value);
            command.Parameters.AddWithValue("@BatteryMinV", session.battery != null ? (object)Single.Parse(session.battery.min.battV) : DBNull.Value);
            command.Parameters.AddWithValue("@BatteryMaxV", session.battery != null ? (object)Single.Parse(session.battery.max.battV) : DBNull.Value);
            command.Parameters.AddWithValue("@TempMinC", session.temp != null ? (object)Single.Parse(session.temp.minTemp.tempC) : DBNull.Value);
            command.Parameters.AddWithValue("@TempMaxC", session.temp != null ? (object)Single.Parse(session.temp.maxTemp.tempC) : DBNull.Value);
            command.Parameters.AddWithValue("@MagMaxX", session.magFlux != null ? (object)Single.Parse(session.magFlux.X.max) : DBNull.Value);
            command.Parameters.AddWithValue("@MagMaxY", session.magFlux != null ? (object)Single.Parse(session.magFlux.Y.max) : DBNull.Value);
            command.Parameters.AddWithValue("@AccelMaxX", session.accel != null ? (object)Single.Parse(session.accel.X.max) : DBNull.Value);
            command.Parameters.AddWithValue("@AccelMaxY", session.accel != null ? (object)Single.Parse(session.accel.Y.max) : DBNull.Value);
            command.Parameters.AddWithValue("@AccelMaxZ", session.accel != null ? (object)Single.Parse(session.accel.Z.max) : DBNull.Value);
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