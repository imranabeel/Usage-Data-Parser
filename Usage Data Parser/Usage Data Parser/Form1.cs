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
using System.Diagnostics;

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

        public ParsedJSONFile parseDodgyFile(string data)
        {
            ParsedJSONFile parsedFile = new ParsedJSONFile();
            int n = 0;
            int i = 0;
            int charCount = data.Length;

            bool key = false;
            bool newValueExpected = false;
            bool value = false;
            bool classValue = false;
            string keyValue = "";
            string valueValue = "";

            foreach (char c in data)
            {
                n++;

                if (i > 0)
                {
                    i--;
                    continue;
                }

                if (n == 1)
                {
                    if (c != '{')
                    {
                        Debug.WriteLine("First char not {");
                        continue;
                    }
                }

                if (c == ':')
                {
                    continue;
                }

                if (c == '{')
                {
                    if (!key && !value && newValueExpected)
                    {
                        classValue = true;
                        char d = data.ElementAt(n);
                        while (d != '}')
                        {
                        }
                    }
                }

                if (c == '"')
                {
                    if (!key && !value && !newValueExpected) //new key value
                    {
                        keyValue = "";
                        key = true;
                        continue;
                    }
                    if (key && !value && !newValueExpected) //key completed
                    {
                        key = false;
                        newValueExpected = true;
                        //KEY COMPLETE - do something with it.
                        continue;
                    }
                    if (!key && !value && newValueExpected)
                    {
                        valueValue = "";
                        value = true;
                        newValueExpected = false;
                        continue;
                    }
                    if (!key && value && !newValueExpected)
                    {
                        //end of value.
                        value = false;
                        //VALUE Complete - do something with it.
                        continue;
                    }
                }

                if (key && !value) //new key value, but not in value yet.
                {
                    keyValue.Append(c);
                    continue;
                }
                if (!key && value)
                {
                    valueValue.Append(c);
                    continue;
                }
            }
        }

        public class keyValuePair
        {
            private string key;
            private string value;
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
            private string nMotors;
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
            private string n;
            private string size;
            private GripChild[] grip;
        }

        public class GripChild
        {
            private string n;
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
            private string n;
            private string battV;
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
            private string n;
            private string tempC;
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
            private string max;
            private string duration;
        }

        public class MagFluxY
        {
            private string max;
            private string duration;
        }
    }
}