using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MicroJson;

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

                BindingSource bs = new BindingSource();
                bs.DataSource = jsonParsedData;
                dataGridView1.DataSource = bs;
                //dataGridView1.Auto
                // Resize the DataGridView columns to fit the newly loaded content.
                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                dataGridView1.Refresh();
            }
        }
    }
}