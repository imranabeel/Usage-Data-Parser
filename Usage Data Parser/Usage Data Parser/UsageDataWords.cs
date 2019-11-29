using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usage_Data_Parser
{
    internal static class WORDS
    {
        public const string _SCHEMA = "$schema";
        public const string _SESSIONN = "sessionN";
        public const string _RESETCAUSE = "resetCause";

        public static class HANDCONFIG
        {
            public const string _HANDCONFIG = "handConfig";
            public const string _SERIALNUM = "serialNum";
            public const string _FWVER = "fwVer";
            public const string _CHIRALITY = "chirality";
            public const string _NMOTORS = "nMotors";
        }

        public static class ERROR
        {
            public const string _ERROR = "error";
            public const string _NUM = "num";
            public const string _SEVERITY = "severity";
            public const string _DESCRIPTION = "description";
        }

        public static class TIME
        {
            public const string _TIME = "time";
            public const string _ONTIME = "onTime";
            public const string _ACTIVETIME = "activeTime";
        }

        public static class GRIP
        {
            public const string _GRIP = "grip";

            public static class GROUP
            {
                public const string _GROUP = "group";
                public const string _N = "n";
                public const string _SIZE = "size";

                public static class GRIP
                {
                    public const string _GRIP = "grip";
                    public const string _N = "n";
                    public const string _NAME = "name";
                    public const string _GRIPSTR = "gripStr";
                    public const string _DURATION = "duration";
                }
            }
        }

        public static class BATTERY
        {
            public const string _BATTERY = "battery";
            public const string _MIN = "min";
            public const string _MAX = "max";

            public static class BATTSAMPLE
            {
                public const string _BATTSAMPLE = "battSample";
                public const string _N = "n";
                public const string _BATTV = "battV";
                public const string _DURATION = "duration";
            }
        }

        public static class TEMP
        {
            public const string _TEMP = "temp";
            public const string _MIN = "min";
            public const string _MAX = "max";

            public static class TEMPSAMPLE
            {
                public const string _TEMPSAMPLE = "tempSample";
                public const string _N = "n";
                public const string _TEMPC = "tempC";
                public const string _DURATION = "duration";
            }
        }

        public static class MAGFLUX
        {
            public const string _MAGFLUX = "magFlux";
            public const string _UNIT = "unit";
            public const string _MAX = "max";
            public const string _DURATION = "duration";

            public const string _X = "X";
            public const string _Y = "Y";
            public const string _Z = "Z";
        }

        public static class ACCEL
        {
            public const string _ACCEL = "accel";
            public const string _UNIT = "unit";
            public const string _MAX = "max";
            public const string _DURATION = "duration";

            public const string _X = "X";
            public const string _Y = "Y";
            public const string _Z = "Z";
        }

        public static class MIN
        {
        }
    }
}