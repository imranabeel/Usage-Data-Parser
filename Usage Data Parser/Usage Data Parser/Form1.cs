using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MicroJson;

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
            string[] files;
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    files = Directory.GetFiles(fbd.SelectedPath, "*.txt");
                    parsedFiles = new List<ParsedJSONFile>();

                    string jsonInputData;

                    foreach (string file in files)
                    {
                        jsonInputData = File.ReadAllText(file);
                        ParsedJSONFile jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);

                        string sessionN = jsonParsedData.ToString();
                    }
                }
            }
        }

        public class ParsedJSONFile
        {
            private int sessionN;
            private HandConfig handConfig;
            private string resetCause;
            private Time time;
            private GripParent grip;
            private Battery battery;
            private Temp temp;
            private MagFlux magFlux;
        }

        public class HandConfig
        {
            private string serialNum;
            private string fwVer;
            private string chirality;
            private int nMotors;
        }

        public class Time
        {
            private string onTime;
            private string activeTime;
        }

        public class GripParent
        {
            private Group[] group;
        }

        public class Group
        {
            private int n;
            private int size;
            private GripChild[] grip;
        }

        public class GripChild
        {
            private int n;
            private string name;
            private string gripStr;
            private string duration;
        }

        public class Battery
        {
            private MinBattery min;
            private MaxBattery max;
            private BattSample[] battSample;
        }

        public class MinBattery
        {
            private BattSample battSample;
        }

        public class MaxBattery
        {
            private BattSample battSample;
        }

        public class BattSample
        {
            private int n;
            private float battV;
            private string duration;
        }

        public class Temp
        {
            private MinTemp minTemp;
            private MaxTemp maxTemp;
            private TempSample[] tempSample;
        }

        public class MaxTemp
        {
            private TempSample tempSample;
        }

        public class MinTemp
        {
            private TempSample tempSample;
        }

        public class TempSample
        {
            private int n;
            private float tempC;
            private string duration;
        }

        public class MagFlux
        {
            private string unit;
            private MagFluxX X;
            private MagFluxY Y;
        }

        public class MagFluxX
        {
            private float max;
            private string duration;
        }

        public class MagFluxY
        {
            private float max;
            private string duration;
        }
    }
}