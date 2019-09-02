using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usage_Data_Parser
{
    internal class DodgyJsonParser
    {
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

                    case "accel":
                        {
                            parsedFile = splitAccel(keyValueArray, parsedFile);
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
                                            group.grips.Add(gripChild);
                                        }
                                        break;

                                    default: break;
                                }
                            }
                            currentFile.grip.gripGroups.Add(group);
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
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "battSample":
                                        {
                                            currentFile.battery.min = new BattSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.battery.min.n = pairs.value;
                                                        }
                                                        break;

                                                    case "battV":
                                                        {
                                                            currentFile.battery.min.battV = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.battery.min.duration = pairs.value;
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
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "battSample":
                                        {
                                            currentFile.battery.max = new BattSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.battery.max.n = pairs.value;
                                                        }
                                                        break;

                                                    case "battV":
                                                        {
                                                            currentFile.battery.max.battV = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.battery.max.duration = pairs.value;
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
                            currentFile.battery.battSamples.Add(battSample);
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
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "tempSample":
                                        {
                                            currentFile.temp.minTemp = new TempSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.temp.minTemp.n = pairs.value;
                                                        }
                                                        break;

                                                    case "tempC":
                                                        {
                                                            currentFile.temp.minTemp.tempC = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.temp.minTemp.duration = pairs.value;
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
                            foreach (KeyValueArray array2 in array1.array)
                            {
                                switch (array2.arrayName)
                                {
                                    case "tempSample":
                                        {
                                            currentFile.temp.maxTemp = new TempSample();
                                            foreach (KeyValuePair pairs in array2.pairs)
                                            {
                                                switch (pairs.key)
                                                {
                                                    case "n":
                                                        {
                                                            currentFile.temp.maxTemp.n = pairs.value;
                                                        }
                                                        break;

                                                    case "tempC":
                                                        {
                                                            currentFile.temp.maxTemp.tempC = pairs.value;
                                                        }
                                                        break;

                                                    case "duration":
                                                        {
                                                            currentFile.temp.maxTemp.duration = pairs.value;
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
                            currentFile.temp.tempSamples.Add(tempSample);
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
                            currentFile.magFlux.X = new MagFluxMeas();
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
                            currentFile.magFlux.Y = new MagFluxMeas();
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

        public ParsedJSONFile splitAccel(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.accel = new Accel();

            foreach (KeyValuePair pairs in array.pairs)
            {
                switch (pairs.key)
                {
                    case "unit":
                        {
                            currentFile.accel.unit = pairs.value;
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
                            currentFile.accel.X = new AccelMeas();
                            foreach (KeyValuePair pair in array1.pairs)
                            {
                                switch (pair.key)
                                {
                                    case "max":
                                        {
                                            currentFile.accel.X.max = pair.value;
                                        }
                                        break;

                                    case "duration":
                                        {
                                            currentFile.accel.X.duration = pair.value;
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    case "Y":
                        {
                            currentFile.accel.Y = new AccelMeas();
                            foreach (KeyValuePair pair in array1.pairs)
                            {
                                switch (pair.key)
                                {
                                    case "max":
                                        {
                                            currentFile.accel.Y.max = pair.value;
                                        }
                                        break;

                                    case "duration":
                                        {
                                            currentFile.accel.Y.duration = pair.value;
                                        }
                                        break;

                                    default: break;
                                }
                            }
                        }
                        break;

                    case "Z":
                        {
                            currentFile.accel.Z = new AccelMeas();
                            foreach (KeyValuePair pair in array1.pairs)
                            {
                                switch (pair.key)
                                {
                                    case "max":
                                        {
                                            currentFile.accel.Z.max = pair.value;
                                        }
                                        break;

                                    case "duration":
                                        {
                                            currentFile.accel.Z.duration = pair.value;
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
    }
}