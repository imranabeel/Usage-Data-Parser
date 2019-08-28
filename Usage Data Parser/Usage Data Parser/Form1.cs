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

            var decoded = getKeyValueArray(1, data); //1 is so it passes the first character ('"') of the file. This would otherwise cause this function to classify the first field incorrectly.

            foreach (KeyValuePair pair in decoded.Item1.pairs)
            {
                switch (pair.key)
                {
                    case "sessionN":
                        {
                            parsedFile.sessionN = Convert.ToInt32(pair.value);
                        }
                        break;

                    case "resetCause":
                        {
                            parsedFile.resetCause = pair.value;
                        }
                        break;

                    default: break;
                }
            }

            foreach (KeyValueArray keyValueArray in decoded.Item1.array)
            {
                switch (keyValueArray.arrayName)
                {
                    case "handConfig":
                        {
                            parsedFile = splitHandConfig(keyValueArray, parsedFile);
                        }
                        break;

                    case "time":
                        {
                            parsedFile = splitTime(keyValueArray, parsedFile);
                        }
                        break;

                    case "grip":
                        {
                            parsedFile = splitGrip(keyValueArray, parsedFile);
                        }
                        break;

                    case "battery":
                        {
                            parsedFile = splitBattery(keyValueArray, parsedFile);
                        }
                        break;

                    case "temp":
                        {
                            parsedFile = splitTemp(keyValueArray, parsedFile);
                        }
                        break;

                    case "magFlux":
                        {
                            parsedFile = splitMagFlux(keyValueArray, parsedFile);
                        }
                        break;

                    default: break;
                }
            }

            return parsedFile;
        }

        public ParsedJSONFile splitHandConfig(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.handConfig = new HandConfig();
            foreach (KeyValuePair pair in array.pairs)
            {
                switch (pair.key)
                {
                    case "serialNum":
                        {
                            currentFile.handConfig.serialNum = pair.value;
                        }
                        break;

                    case "fwVer":
                        {
                            currentFile.handConfig.fwVer = pair.value;
                        }
                        break;

                    case "chirality":
                        {
                            currentFile.handConfig.chirality = pair.value;
                        }
                        break;

                    case "nMotors":
                        {
                            currentFile.handConfig.nMotors = pair.value;
                        }
                        break;

                    default: break;
                }
            }

            return currentFile;
        }

        public ParsedJSONFile splitTime(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.time = new Time();
            foreach (KeyValuePair pair in array.pairs)
            {
                switch (pair.key)
                {
                    case "onTime":
                        {
                            currentFile.time.onTime = pair.value;
                        }
                        break;

                    case "activeTime":
                        {
                            currentFile.time.activeTime = pair.value;
                        }
                        break;

                    default: break;
                }
            }

            return currentFile;
        }

        public ParsedJSONFile splitGrip(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.grip = new GripParent();

            foreach (KeyValueArray array1 in array.array)
            {
                switch (array1.arrayName)
                {
                    case "group":
                        {
                            Group group = new Group();
                            foreach (KeyValuePair pair in array1.pairs)
                            {
                                switch (pair.key)
                                {
                                    case "n":
                                        {
                                            group.n = pair.value;
                                        }
                                        break;

                                    case "size":
                                        {
                                            group.size = pair.value;
                                        }
                                        break;

                                    default: break;
                                }
                            }

                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "grip":
                                        {
                                            GripChild gripChild = new GripChild();
                                            foreach (KeyValuePair pair in array2.pairs)
                                            {
                                                switch (pair.key)
                                                {
                                                    case "n":
                                                        {
                                                            gripChild.n = pair.value;
                                                        }
                                                        break;

                                                    case "name":
                                                        {
                                                            gripChild.name = pair.value;
                                                        }
                                                        break;

                                                    case "gripStr":
                                                        {
                                                            gripChild.gripStr = pair.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            gripChild.duration = pair.value;
                                                        }
                                                        break;

                                                    default: break;
                                                }
                                            }
                                            group.grip.Add(gripChild);
                                        }
                                        break;

                                    default: break;
                                }
                            }
                            currentFile.grip.group.Add(group);
                        }
                        break;

                    default: break;
                }
            }

            return currentFile;
        }

        public ParsedJSONFile splitBattery(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.battery = new Battery();

            foreach (KeyValueArray array1 in array.array)
            {
                switch (array1.arrayName)
                {
                    case "min":
                        {
                            currentFile.battery.min = new MinBattery();
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "battSample":
                                        {
                                            currentFile.battery.min.battSample = new BattSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.battery.min.battSample.n = pairs.value;
                                                        }
                                                        break;

                                                    case "battV":
                                                        {
                                                            currentFile.battery.min.battSample.battV = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.battery.min.battSample.duration = pairs.value;
                                                        }
                                                        break;

                                                    default: break;
                                                }
                                            }
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    case "max":
                        {
                            currentFile.battery.max = new MaxBattery();
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "battSample":
                                        {
                                            currentFile.battery.max.battSample = new BattSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.battery.max.battSample.n = pairs.value;
                                                        }
                                                        break;

                                                    case "battV":
                                                        {
                                                            currentFile.battery.max.battSample.battV = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.battery.max.battSample.duration = pairs.value;
                                                        }
                                                        break;

                                                    default: break;
                                                }
                                            }
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    case "battSample":
                        {
                            BattSample battSample = new BattSample();
                            foreach (KeyValuePair pairs in array1.pairs)
                            {
                                switch (pairs.key)
                                {
                                    case "n":
                                        {
                                            battSample.n = pairs.value;
                                        }
                                        break;

                                    case "battV":
                                        {
                                            battSample.battV = pairs.value;
                                        }
                                        break;

                                    case "duration":
                                        {
                                            battSample.duration = pairs.value;
                                        }
                                        break;

                                    default: break;
                                }
                            }
                            currentFile.battery.battSample.Add(battSample);
                        }
                        break;

                    default: break;
                }
            }

            return currentFile;
        }

        public ParsedJSONFile splitTemp(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.temp = new Temp();

            foreach (KeyValueArray array1 in array.array)
            {
                switch (array1.arrayName)
                {
                    case "min":
                        {
                            currentFile.temp.minTemp = new MinTemp();
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "tempSample":
                                        {
                                            currentFile.temp.minTemp.tempSample = new TempSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.temp.minTemp.tempSample.n = pairs.value;
                                                        }
                                                        break;

                                                    case "tempC":
                                                        {
                                                            currentFile.temp.minTemp.tempSample.tempC = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.temp.minTemp.tempSample.duration = pairs.value;
                                                        }
                                                        break;

                                                    default: break;
                                                }
                                            }
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    case "max":
                        {
                            currentFile.temp.maxTemp = new MaxTemp();
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "tempSample":
                                        {
                                            currentFile.temp.maxTemp.tempSample = new TempSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.temp.maxTemp.tempSample.n = pairs.value;
                                                        }
                                                        break;

                                                    case "tempC":
                                                        {
                                                            currentFile.temp.maxTemp.tempSample.tempC = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.temp.maxTemp.tempSample.duration = pairs.value;
                                                        }
                                                        break;

                                                    default: break;
                                                }
                                            }
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    case "tempSample":
                        {
                            TempSample tempSample = new TempSample();
                            foreach (KeyValuePair pairs in array1.pairs)
                            {
                                switch (pairs.key)
                                {
                                    case "n":
                                        {
                                            tempSample.n = pairs.value;
                                        }
                                        break;

                                    case "tempC":
                                        {
                                            tempSample.tempC = pairs.value;
                                        }
                                        break;

                                    case "duration":
                                        {
                                            tempSample.duration = pairs.value;
                                        }
                                        break;

                                    default: break;
                                }
                            }
                            currentFile.temp.tempSample.Add(tempSample);
                        }
                        break;

                    default: break;
                }
            }

            return currentFile;
        }

        public ParsedJSONFile splitMagFlux(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.magFlux = new MagFlux();

            foreach (KeyValuePair pairs in array.pairs)
            {
                switch (pairs.key)
                {
                    case "unit":
                        {
                            currentFile.magFlux.unit = pairs.value;
                        }
                        break;

                    default: break;
                }
            }

            foreach (KeyValueArray array1 in array.array)
            {
                switch (array1.arrayName)
                {
                    case "X":
                        {
                            currentFile.magFlux.X = new MagFluxX();
                            foreach (KeyValuePair pair in array1.pairs)
                            {
                                switch (pair.key)
                                {
                                    case "max":
                                        {
                                            currentFile.magFlux.X.max = pair.value;
                                        }
                                        break;

                                    case "duration":
                                        {
                                            currentFile.magFlux.X.duration = pair.value;
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    case "Y":
                        {
                            currentFile.magFlux.Y = new MagFluxY();
                            foreach (KeyValuePair pair in array1.pairs)
                            {
                                switch (pair.key)
                                {
                                    case "max":
                                        {
                                            currentFile.magFlux.Y.max = pair.value;
                                        }
                                        break;

                                    case "duration":
                                        {
                                            currentFile.magFlux.Y.duration = pair.value;
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    default: break;
                }
            }

            return currentFile;
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
            public int sessionN;
            public HandConfig handConfig;
            public string resetCause;
            public Time time;
            public GripParent grip;
            public Battery battery;
            public Temp temp;
            public MagFlux magFlux;
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
            public List<Group> group = new List<Group>();
        }

        public class Group
        {
            public string n;
            public string size;
            public List<GripChild> grip = new List<GripChild>();
        }

        public class GripChild
        {
            public string n;
            public string name;
            public string gripStr;
            public string duration;
        }

        public class Battery
        {
            public MinBattery min;
            public MaxBattery max;
            public List<BattSample> battSample = new List<BattSample>();
        }

        public class MinBattery
        {
            public BattSample battSample;
        }

        public class MaxBattery
        {
            public BattSample battSample;
        }

        public class BattSample
        {
            public string n;
            public string battV;
            public string duration;
        }

        public class Temp
        {
            public MinTemp minTemp;
            public MaxTemp maxTemp;
            public List<TempSample> tempSample = new List<TempSample>();
        }

        public class MaxTemp
        {
            public TempSample tempSample;
        }

        public class MinTemp
        {
            public TempSample tempSample;
        }

        public class TempSample
        {
            public string n;
            public string tempC;
            public string duration;
        }

        public class MagFlux
        {
            public string unit;
            public MagFluxX X;
            public MagFluxY Y;
        }

        public class MagFluxX
        {
            public string max;
            public string duration;
        }

        public class MagFluxY
        {
            public string max;
            public string duration;
        }
    }
}