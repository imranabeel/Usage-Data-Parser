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

            foreach (string file in files)
            {
                jsonInputData = File.ReadAllText(file);
                //ParsedJSONFile jsonParsedData = new JsonSerializer().Deserialize<ParsedJSONFile>(jsonInputData);
                ParsedJSONFile jsonParsedData = parseDodgyFile(jsonInputData);
                string sessionN = jsonParsedData.ToString();
            }
            //    }
            //}
        }

        public ParsedJSONFile parseDodgyFile(string data)
        {
            ParsedJSONFile parsedFile = new ParsedJSONFile();
            int n = 0;
            int i = 0;
            int charCount = data.Length;

            bool key = false;
            var keyValue = new StringBuilder();

            var decoded = getKeyValueArray(1, data); //1 is so it passes the first character of the file.

            return parsedFile;
        }

        public (string value, int charToSkip) getValue(int startingPos, string data)
        {
            string value = "";
            int charToSkip = 0;

            bool valueStarted = false;

            var tempString = new StringBuilder();

            for (int i = startingPos; i < data.Length; i++)
            {
                char c = data.ElementAt(i);
                charToSkip++;

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
                    charToSkip++; //skip comma as well.
                    value = tempString.ToString();
                    break;
                }

                tempString.Append(c);
            }

            return (value, charToSkip);
        }

        public (KeyValueArray, int) getKeyValueArray(int startingPos, string data) //Recursive function to build nested array of json file.
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            for (int i = startingPos; i < data.Length; i++)
            {
                char c = data.ElementAt(i);
                skipReturn++;

                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                if (((c == ':') || c == (' ') || (c == '{') || ((int)c < 0x20)) && (!key))
                {
                    continue;
                }

                if ((c == '}') && (data.ElementAt(i + 1) == ','))
                {
                    skipReturn--; //extra skip for comma
                    break;
                }

                if (c == '"')
                {
                    if (!key)
                    {
                        keyValue = new StringBuilder();
                        key = true;
                        continue;
                    }
                    else
                    {
                        var builtString = keyValue.ToString();
                        if (data.ElementAt(i + 3) == '{')
                        {
                            skip += 3;//skip all chars up to "{"

                            var arrayReturn = getKeyValueArray(i + 1, data);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);

                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            var returned = getValue(i + 1, data);
                            keyValuePair.value = returned.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = returned.charToSkip;
                        }
                        key = false;
                    }
                }

                if (key) //new key value, but not in value yet.
                {
                    keyValue.Append(c);
                    continue;
                }
            }

            return (keyValueArray, skipReturn);
        }

        public class KeyValueArray
        {
            public List<KeyValuePair> pairs = new List<KeyValuePair>();
            public List<KeyValueArray> array = new List<KeyValueArray>();
            public string arrayName = "";
        }

        public class KeyValuePair
        {
            public string key;
            public string value;
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
            public string serialNum;
            public string fwVer;
            public string chirality;
            public string nMotors;
        }

        public class Time
        {
            public string onTime;
            public string activeTime;
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