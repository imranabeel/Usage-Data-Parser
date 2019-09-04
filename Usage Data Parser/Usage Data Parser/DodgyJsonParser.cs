using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Usage_Data_Parser
{
    internal class DodgyJsonParser
    {
        public ParsedJSONFile parseDodgyFile(string data)
        {
            ParsedJSONFile parsedFile = new ParsedJSONFile();

            var decoded = getKeyValueArray(1, data, 0); //1 is so it passes the first character ('"') of the file. This would otherwise cause this function to classify the first field incorrectly.

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

                    case "error":
                        {
                            parsedFile = splitError(keyValueArray, parsedFile);
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

        public ParsedJSONFile splitError(KeyValueArray array, ParsedJSONFile currentFile)
        {
            currentFile.error = new Error();
            foreach (KeyValuePair pair in array.pairs)
            {
                switch (pair.key)
                {
                    case "num":
                        {
                            currentFile.error.num = pair.value;
                        }
                        break;

                    case "severity":
                        {
                            currentFile.error.severity = pair.value;
                        }
                        break;

                    case "description":
                        {
                            currentFile.error.description = pair.value;
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

                if (c == '\0')
                {
                    break;
                }

                if (((c == ':') || c == (' ')) && (!valueStarted))
                {
                    continue;
                }

                if (c == '"')
                {
                    if (!valueStarted)
                    {
                        valueStarted = true;
                        continue;
                    }
                    else
                    {
                        if (data.ElementAt(i + 1) == ',') // comma after " char, so end of value.
                        {
                            value = tempString.ToString();
                            charToSkip++; //skip comma as well.
                            break;
                        }
                    }
                }

                if (valueStarted)
                {
                    tempString.Append(c);
                }
            }

            return (value, charToSkip);
        }

        public (KeyValueArray array, int skip, bool dodgy) getKeyValueArray(int startingPos, string data, int depth)
        {
            KeyValueArray keyValueArray = new KeyValueArray();
            bool dodgy = false;
            bool key = false;
            var keyString = new StringBuilder();

            int skipReturn = 0;
            int localSkip = 0;

            for (int i = startingPos; i < data.Length; i++)
            {
                char c = data.ElementAt(i);
                skipReturn++; //we're checking the next char, so skip this in the recursive function below this level.

                if (localSkip > 0) // skip over chars checked in getValue;
                {
                    localSkip--;
                    continue;
                }

                if (c == '\0') //null character, reached dodgy point in file.
                {
                    dodgy = true;
                    //check next 20 characters for a tab. This is to iterate over any additional null characters that are occasionally present when the first null char is found.
                    for (int j = 1; j < 20; j++) //starts at 1, because 0 would result in checking the same char we've just checked
                    {
                        int charIndex = i + j;
                        char tab = data.ElementAt(charIndex);
                        if (tab == '\t') // check for tab char
                        {
                            skipReturn += j; //increment skipReturn value by number of chars we've checked.
                            break;
                        }
                    }
                    break;
                }

                if (((c == ':') || c == (' ') || (c == '{') || ((int)c < 0x20)) && (!key)) // control chars. Skip these.
                {
                    continue;
                }

                if ((c == '}') && (data.ElementAt(i + 1) == ',')) // if close bracket and comma straight after, then we are done with this level of array. Skip out of this function and recursively drop down to previous level.
                {
                    skipReturn--; // this is -1 as we want the recursive function below this to also see this } character, so that the array depths don't just keep increasing.
                    break;
                }

                if (c == '"') //Start/End of Key String.
                {
                    if (!key) //Start of Key String
                    {
                        keyString = new StringBuilder();
                        key = true;
                        continue;
                    }
                    else //End of Key String - Get Value
                    {
                        var builtString = keyString.ToString();
                        if (data.ElementAt(i + 3) == '{')
                        {
                            localSkip += 3; //We've checked up to i+3, so skip next 3 chars.

                            int nextLevelStartPos = i + 3; //i is for char we've just checked, so pass i+3;
                            int nextLevelDepth = depth + 1; //pass depth + 1 to next level recursive function.

                            var arrayReturn = getKeyValueArray(nextLevelStartPos, data, nextLevelDepth); //Start Next Level Array Parsing

                            localSkip += arrayReturn.skip;

                            dodgy = arrayReturn.dodgy;

                            if (dodgy) //null char detected in string. Finish current
                            {
                                if (depth > 0) //Not at root node yet, so lets keep breaking out of this function until we get there.
                                {
                                    skipReturn += (localSkip - 1); //If this doesn't exist, recursively dropping down through the functions will result in 1 char being skipped for each function depth. Not sure why.
                                    break;
                                }
                            }
                            //If we've reached this point, its either Not Dodgy, or it is Dodgy and we're at the Root Node now.
                            arrayReturn.array.arrayName = keyString.ToString();
                            keyValueArray.array.Add(arrayReturn.array);
                        }
                        else
                        {
                            KeyValuePair keyValuePair = new KeyValuePair();
                            keyValuePair.key = builtString;

                            int valueStartPos = i + 1; //i is for char we've just checked, so pass i+1 to start value parsing on next character.

                            var returned = getValue(valueStartPos, data);

                            keyValuePair.value = returned.value;
                            keyValueArray.pairs.Add(keyValuePair);
                            localSkip = returned.charToSkip;
                        }
                        key = false;
                    }
                }

                if (key) //Building the Key String
                {
                    keyString.Append(c);
                    continue;
                }
            }

            return (keyValueArray, skipReturn, dodgy);
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

        public string[] expectedBatteryWords = { WORDS.BATTERY._MIN, WORDS.BATTERY._MAX, WORDS.BATTERY.BATTSAMPLE._BATTSAMPLE, WORDS.BATTERY.BATTSAMPLE._N, WORDS.BATTERY.BATTSAMPLE._BATTV, WORDS.BATTERY.BATTSAMPLE._DURATION };
        public string[] expectedTempWords = { WORDS.TEMP._MIN, WORDS.TEMP._MAX, WORDS.TEMP.TEMPSAMPLE._TEMPSAMPLE, WORDS.TEMP.TEMPSAMPLE._N, WORDS.TEMP.TEMPSAMPLE._TEMPC, WORDS.TEMP.TEMPSAMPLE._DURATION };
    }
}