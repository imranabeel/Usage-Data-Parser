using MicroJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Usage_Data_Parser
{
    public partial class Form1 : Form
    {
        private List<ParsedJSONFile> parsedFiles = new List<ParsedJSONFile>();
        private string rootNodePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void folderSelectButton(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    rootNodePath = fbd.SelectedPath;
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

        private void FileFolderSelected(object sender, TreeViewEventArgs e)
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

                dataGridViewHandConfig.Columns.Clear();
                dataGridViewSummary.Columns.Clear();
                dataGridViewGrips.Columns.Clear();
                dataGridViewBattSummary.Columns.Clear();
                dataGridViewBattSamples.Columns.Clear();
                dataGridViewTempSummary.Columns.Clear();
                dataGridViewTempSamples.Columns.Clear();
                dataGridViewMagFlux.Columns.Clear();
                dataGridViewAccel.Columns.Clear();

                DataTable dt = new DataTable();

                if (jsonParsedData != null)
                {
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
                        DataColumn[] columns2 = { new DataColumn("Session Num"), new DataColumn("Reset Cause"), new DataColumn("On Time"), new DataColumn("Active Time") };
                        dt2.Columns.AddRange(columns2);
                        Object[] row2 = { jsonParsedData.sessionN, jsonParsedData.resetCause, jsonParsedData.time.onTime, jsonParsedData.time.activeTime };
                        dt2.Rows.Add(row2);
                        dataGridViewSummary.DataSource = dt2;
                    }

                    if (jsonParsedData.grip != null)
                    {
                        DataTable dt3 = new DataTable();
                        DataColumn[] columns3 = { new DataColumn("Grip Group"), new DataColumn("Grip"), new DataColumn("Duration") };
                        dt3.Columns.AddRange(columns3);
                        foreach (Group group in jsonParsedData.grip.gripGroups)
                        {
                            foreach (GripChild grip in group.grips)
                            {
                                Object[] row3 = { group.n, grip.name, grip.duration };
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
                        foreach (BattSample batt in jsonParsedData.battery.battSamples)
                        {
                            Object[] row6 = { batt.n, batt.battV, batt.duration };
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
                        foreach (TempSample temp in jsonParsedData.temp.tempSamples)
                        {
                            Object[] row9 = { temp.n, temp.tempC, temp.duration };
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
                    workbook.Sheets.Add(Count: 4);

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
                            worksheet = (Excel.Worksheet)workbook.Sheets[6].Select();
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
    }
}