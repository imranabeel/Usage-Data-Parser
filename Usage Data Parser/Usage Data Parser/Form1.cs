using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Usage_Data_Parser
{
    public partial class Form1 : Form
    {
        private List<ParsedJSONFile> parsedFiles = new List<ParsedJSONFile>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Chris\Downloads\Usage data-20190821T145657Z-001\Usage data\H151\19-07-2019\DATA";

            string[] files;
            //using (var fbd = new FolderBrowserDialog())
            //{
            //    DialogResult result = fbd.ShowDialog();

            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //files = Directory.GetFiles(fbd.SelectedPath, "*.txt");
            files = Directory.GetFiles(path, "*.txt");
            parsedFiles = new List<ParsedJSONFile>();

            string jsonInputData;

            DodgyJsonParser dodgyJsonParser = new DodgyJsonParser();

            foreach (string file in files)
            {
                jsonInputData = File.ReadAllText(file);
                //ParsedJSONFile jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
                ParsedJSONFile jsonParsedData = dodgyJsonParser.parseDodgyFile(jsonInputData);
                string sessionN = jsonParsedData.ToString();
            }
            //    }
            //}
        }
    }
}