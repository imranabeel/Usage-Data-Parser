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
                    else //key completed
                    {
                        var builtString = keyValue.ToString();
                        key = false;
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
                                {
                                    var decoded = getKeyValueArray(n - (builtString.Length + 2), data, 0);
                                    i = decoded.Item2;
                                    i -= (builtString.Length + 2);
                                }

                                break;

                            case "resetCause":
                                {
                                    KeyValue keyValue2 = getValue(n, data);
                                    Debug.WriteLine(keyValue2.value);
                                    i = keyValue2.charToSkip;
                                }
                                break;

                            case "time":
                                {
                                    var decoded = getKeyValueArray(n - (builtString.Length + 2), data, 0);
                                    i = decoded.Item2;
                                    i -= (builtString.Length + 2);
                                }
                                break;

                            case "grip":
                                {
                                    var decoded = getKeyValueArray(n - (builtString.Length + 2), data, 0);
                                    i = decoded.Item2;
                                    i -= (builtString.Length + 2);
                                }
                                break;

                            case "battery":
                                {
                                    var decoded = getKeyValueArray(n - (builtString.Length + 2), data, 0);
                                    i = decoded.Item2;
                                    i -= (builtString.Length + 2);
                                }
                                break;

                            case "temp":
                                {
                                    var decoded = getKeyValueArray(n - (builtString.Length + 2), data, 0);
                                    i = decoded.Item2;
                                    i -= (builtString.Length + 2);
                                }
                                break;

                            case "magFlux":
                                {
                                    var decoded = getKeyValueArray(n - (builtString.Length + 2), data, 0);
                                    i = decoded.Item2;
                                    i -= (builtString.Length + 2);
                                }
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

        public (HandConfig, int) getHandConfig(int startingPos, string data)
        {
            HandConfig handConfig = new HandConfig();
            handConfig.serialNum = "";
            handConfig.fwVer = "";
            handConfig.chirality = "";
            handConfig.nMotors = "";

            int charToSkip = 0;

            int j = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            for (int i = startingPos; i < (data.Length - startingPos); i++)
            {
                char c = data.ElementAt(i);
                charToSkip++;

                if (j > 0)
                {
                    j--;
                    continue;
                }

                if (((c == ':') || c == (' ') || c == ('{') || ((int)c < 0x20)) && (!key))
                {
                    continue;
                }

                if ((c == '}') && (data.ElementAt(i + 1) == ','))
                {
                    charToSkip++;
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
                        key = false;
                        //KEY COMPLETE - do something with it.

                        Debug.WriteLine(builtString);
                        switch (builtString)
                        {
                            case "serialNum":
                                {
                                    KeyValue keyValue1 = getValue(i + 1, data);
                                    Debug.WriteLine(keyValue1.value);
                                    j = keyValue1.charToSkip;
                                    handConfig.serialNum = keyValue1.value;
                                }
                                break;

                            case "fwVer":
                                {
                                    KeyValue keyValue1 = getValue(i + 1, data);
                                    Debug.WriteLine(keyValue1.value);
                                    j = keyValue1.charToSkip;
                                    handConfig.fwVer = keyValue1.value;
                                }
                                break;

                            case "chirality":
                                {
                                    KeyValue keyValue1 = getValue(i + 1, data);
                                    Debug.WriteLine(keyValue1.value);
                                    j = keyValue1.charToSkip;
                                    handConfig.chirality = keyValue1.value;
                                }
                                break;

                            case "nMotors":
                                {
                                    KeyValue keyValue1 = getValue(i + 1, data);
                                    Debug.WriteLine(keyValue1.value);
                                    j = keyValue1.charToSkip;
                                    handConfig.nMotors = keyValue1.value;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }

                if (key) //new key value, but not in value yet.
                {
                    keyValue.Append(c);
                    continue;
                }
            }

            return (handConfig, charToSkip);
        }

        public (KeyValueArray, int, int) getKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int localDepth = depth;

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            int numPairs = 0;

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
                    skipReturn++; //extra skip for comma
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
                            //keyValueArray.arrayName = keyValue.ToString();
                            key = false;

                            localDepth++;
                            var arrayReturn = get2ndKeyValueArray(i + 1, data, localDepth);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);
                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            KeyValue keyValue1 = getValue(i + 1, data);
                            keyValuePair.value = keyValue1.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = keyValue1.charToSkip;
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

            return (keyValueArray, skipReturn, depth);
        }

        public (KeyValueArray, int, int) get2ndKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int localDepth = depth;

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            int numPairs = 0;

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
                            //keyValueArray.arrayName = keyValue.ToString();
                            key = false;
                            skip += 3;//skip all chars up to "{"

                            localDepth++;
                            var arrayReturn = get3rdKeyValueArray(i + 1, data, localDepth);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);

                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            KeyValue keyValue1 = getValue(i + 1, data);
                            keyValuePair.value = keyValue1.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = keyValue1.charToSkip;
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

            return (keyValueArray, skipReturn, depth);
        }

        public (KeyValueArray, int, int) get3rdKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int localDepth = depth;

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            int numPairs = 0;

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
                            //keyValueArray.arrayName = keyValue.ToString();
                            key = false;
                            skip += 3;//skip all chars up to "{"

                            localDepth++;
                            var arrayReturn = get4thKeyValueArray(i + 1, data, localDepth);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);
                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            KeyValue keyValue1 = getValue(i + 1, data);
                            keyValuePair.value = keyValue1.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = keyValue1.charToSkip;
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

            return (keyValueArray, skipReturn, depth);
        }

        public (KeyValueArray, int, int) get4thKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int localDepth = depth;

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            int numPairs = 0;

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
                            // keyValueArray.arrayName = keyValue.ToString();
                            key = false;
                            skip += 3;//skip all chars up to "{"

                            localDepth++;
                            var arrayReturn = get5thKeyValueArray(i + 1, data, localDepth);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);
                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            KeyValue keyValue1 = getValue(i + 1, data);
                            keyValuePair.value = keyValue1.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = keyValue1.charToSkip;
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

            return (keyValueArray, skipReturn, depth);
        }

        public (KeyValueArray, int, int) get5thKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int localDepth = depth;

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            int numPairs = 0;

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
                            // keyValueArray.arrayName = keyValue.ToString();
                            key = false;
                            skip += 3;//skip all chars up to "{"

                            localDepth++;
                            var arrayReturn = get6thKeyValueArray(i + 1, data, localDepth);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);
                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            KeyValue keyValue1 = getValue(i + 1, data);
                            keyValuePair.value = keyValue1.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = keyValue1.charToSkip;
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

            return (keyValueArray, skipReturn, depth);
        }

        public (KeyValueArray, int, int) get6thKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int localDepth = depth;

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            int numPairs = 0;

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
                            //keyValueArray.arrayName = keyValue.ToString();
                            key = false;
                            skip += 3;//skip all chars up to "{"

                            localDepth++;
                            var arrayReturn = get7thKeyValueArray(i + 1, data, localDepth);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);
                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            KeyValue keyValue1 = getValue(i + 1, data);
                            keyValuePair.value = keyValue1.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = keyValue1.charToSkip;
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

            return (keyValueArray, skipReturn, depth);
        }

        public (KeyValueArray, int, int) get7thKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();

            int localDepth = depth;

            int skipReturn = 0;

            int skip = 0;

            bool key = false;
            var keyValue = new StringBuilder();

            int numPairs = 0;

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
                            //keyValueArray.arrayName = keyValue.ToString();
                            key = false;
                            skip += 3;//skip all chars up to "{"

                            localDepth++;
                            var arrayReturn = getKeyValueArray(i + 1, data, localDepth);
                            arrayReturn.Item1.arrayName = keyValue.ToString();
                            keyValueArray.array.Add(arrayReturn.Item1);
                            skip += arrayReturn.Item2;
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            KeyValue keyValue1 = getValue(i + 1, data);
                            keyValuePair.value = keyValue1.value;

                            keyValueArray.pairs.Add(keyValuePair);

                            skip = keyValue1.charToSkip;
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

            return (keyValueArray, skipReturn, depth);
        }

        public class KeyValue
        {
            public int charToSkip { get; set; }
            public string value { get; set; }
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