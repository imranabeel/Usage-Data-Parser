using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usage_Data_Parser
{
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