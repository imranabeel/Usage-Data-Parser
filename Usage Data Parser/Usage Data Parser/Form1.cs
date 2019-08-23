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
                        //ParsedJSONFile jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
                        ParsedJSONFile jsonParsedData = parseDodgyFile(jsonInputData);
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
            var keyValue = new StringBuilder();
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

                if (c == '"')
                {
                    if (!key) //new key value
                    {
                        keyValue = new StringBuilder();
                        key = true;
                        continue;
                    }
                    if (key) //key completed
                    {
                        var builtString = keyValue.ToString();
                        key = false;
                        newValueExpected = true;
                        //KEY COMPLETE - do something with it.

                        Debug.WriteLine(builtString);
                        switch (builtString)
                        {
                            case "$schema":
                                {
                                    KeyValue keyValue1 = getValue(n, data);
                                    Debug.WriteLine(keyValue1.value);
                                    i = keyValue1.charToSkip;
                                }
                                break;

                            case "sessionN":
                                {
                                    KeyValue keyValue2 = getValue(n, data);
                                    Debug.WriteLine(keyValue2.value);
                                    i = keyValue2.charToSkip;
                                }
                                break;

                            case "handConfig":
                                break;

                            case "resetCause":
                                break;

                            case "time":
                                break;

                            case "grip":
                                break;

                            case "battery":
                                break;

                            case "temp":
                                break;

                            case "magFlux":
                                break;

                            default: break;
                        }
                        continue;
                    }
                }

                if (key) //new key value, but not in value yet.
                {
                    keyValue.Append(c);
                    continue;
                }
            }
            return parsedFile;
        }

        public KeyValue getValue(int startingPos, string data)
        {
            KeyValue keyValue = new KeyValue();
            keyValue.value = "";

            bool valueStarted = false;

            var tempString = new StringBuilder();

            for (int i = startingPos; i < data.Length; i++)
            {
                char c = data.ElementAt(i);
                keyValue.charToSkip++;

                if (((c == ':') || c == (' ')) && (!valueStarted))
                {
                    continue;
                }

                if ((c == '"') && (!valueStarted))
                {
                    valueStarted = true;
                    continue;
                }

                if ((c == '"') && (data.ElementAt(i + 1) == ',') && (valueStarted))
                {
                    keyValue.charToSkip++; //skip comma as well.
                    keyValue.value = tempString.ToString();
                    break;
                }

                tempString.Append(c);
            }

            return keyValue;
        }

        public class KeyValue
        {
            public int charToSkip { get; set; }
            public string value { get; set; }
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