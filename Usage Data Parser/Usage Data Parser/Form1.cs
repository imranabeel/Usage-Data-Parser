using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MicroJson;
using System.Data;

namespace Usage_Data_Parser
{
    public partial class Form1 : Form
    {
        private List<ParsedJSONFile> parsedFiles = new List<ParsedJSONFile>();
        private string rootNodePath = "";

        public Form1()
        {
            InitializeComponent();
            //dataGridView1.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string path = @"C:\Users\Chris\Downloads\Usage data-20190821T145657Z-001\Usage data\H151\19-07-2019\DATA";

            List<String> files;

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // files = DirSearch(fbd.SelectedPath);// Directory.GetFiles(fbd.SelectedPath, "*.txt");
                    rootNodePath = fbd.SelectedPath;
                    treeView1.BeginUpdate();
                    ListDirectory(treeView1, rootNodePath);
                    treeView1.EndUpdate();

                    //string[] folders = Directory.GetDirectories(fbd.SelectedPath);

                    //files = Directory.GetFiles(path, "*.txt");

                    parsedFiles = new List<ParsedJSONFile>();

                    string jsonInputData;

                    DodgyJsonParser dodgyJsonParser = new DodgyJsonParser();
                    //ParsedJSONFile jsonParsedData;
                    //foreach (string file in files)
                    //{
                    //    jsonInputData = File.ReadAllText(file);
                    //    try
                    //    {
                    //        jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
                    //    }
                    //    catch (ParserException ex)
                    //    {
                    //        jsonParsedData = dodgyJsonParser.parseDodgyFile(jsonInputData);
                    //    }

                    //    parsedFiles.Add(jsonParsedData);
                    //}
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

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = treeView1.SelectedNode;
            var tag = (string)node.Tag;
            string fullPath = tag.ToString();

            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(fullPath);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                //MessageBox.Show("Its a directory");
            }
            else
            {
                //MessageBox.Show("Its a file");

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

                dataGridView1.Columns.Clear();
                dataGridView2.Columns.Clear();
                dataGridView3.Columns.Clear();
                dataGridView4.Columns.Clear();
                dataGridView5.Columns.Clear();
                dataGridView6.Columns.Clear();
                dataGridView7.Columns.Clear();
                dataGridView8.Columns.Clear();
                dataGridView9.Columns.Clear();

                DataTable dt = new DataTable();

                DataColumn[] columns = { new DataColumn("Serial Num"), new DataColumn("Firmware Version"), new DataColumn("Chirality"), new DataColumn("nMotors") };
                dt.Columns.AddRange(columns);
                Object[] row = { jsonParsedData.handConfig.serialNum, jsonParsedData.handConfig.fwVer, jsonParsedData.handConfig.chirality, jsonParsedData.handConfig.nMotors };
                dt.Rows.Add(row);
                dataGridView1.DataSource = dt;

                DataTable dt2 = new DataTable();
                DataColumn[] columns2 = { new DataColumn("Session Num"), new DataColumn("Reset Cause"), new DataColumn("On Time"), new DataColumn("Active Time") };
                dt2.Columns.AddRange(columns2);
                Object[] row2 = { jsonParsedData.sessionN, jsonParsedData.resetCause, jsonParsedData.time.onTime, jsonParsedData.time.activeTime };
                dt2.Rows.Add(row2);
                dataGridView2.DataSource = dt2;

                if (jsonParsedData.grip != null)
                {
                    DataTable dt3 = new DataTable();
                    DataColumn[] columns3 = { new DataColumn("Grip Group"), new DataColumn("Grip"), new DataColumn("Duration") };
                    dt3.Columns.AddRange(columns3);
                    foreach (Group group in jsonParsedData.grip.group)
                    {
                        foreach (GripChild grip in group.grip)
                        {
                            Object[] row3 = { group.n, grip.name, grip.duration };
                            dt3.Rows.Add(row3);
                        }
                    }
                    dataGridView3.DataSource = dt3;
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
                    dataGridView4.DataSource = dt4;

                    DataTable dt5 = new DataTable();
                    DataColumn[] columns5 = { new DataColumn("n"), new DataColumn("Batt Voltage"), new DataColumn("Duration") };
                    dt5.Columns.AddRange(columns5);
                    foreach (BattSample batt in jsonParsedData.battery.battSample)
                    {
                        Object[] row6 = { batt.n, batt.battV, batt.duration };
                        dt5.Rows.Add(row6);
                    }
                    dataGridView5.DataSource = dt5;
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
                    dataGridView6.DataSource = dt6;

                    DataTable dt7 = new DataTable();
                    DataColumn[] columns7 = { new DataColumn("n"), new DataColumn("Temp (Celcius)"), new DataColumn("Duration") };
                    dt7.Columns.AddRange(columns7);
                    foreach (TempSample temp in jsonParsedData.temp.tempSample)
                    {
                        Object[] row9 = { temp.n, temp.tempC, temp.duration };
                        dt7.Rows.Add(row9);
                    }
                    dataGridView7.DataSource = dt7;
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
                    dataGridView8.DataSource = dt8;
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
                    dataGridView9.DataSource = dt9;
                };
            }
        }
    }
}