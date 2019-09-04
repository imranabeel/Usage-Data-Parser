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
        public Accel accel;
        public Error error;
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
        public List<Group> gripGroups = new List<Group>();
    }

    public class Group
    {
        public string n;
        public string size;
        public List<GripChild> grips = new List<GripChild>();
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
        public BattSample min;
        public BattSample max;
        public List<BattSample> battSamples = new List<BattSample>();
    }

    public class BattSample
    {
        public string n;
        public string battV;
        public string duration;
    }

    public class Temp
    {
        public TempSample minTemp;
        public TempSample maxTemp;
        public List<TempSample> tempSamples = new List<TempSample>();
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
        public MagFluxMeas X;
        public MagFluxMeas Y;
    }

    public class MagFluxMeas
    {
        public string max;
        public string duration;
    }

    public class Accel
    {
        public string unit;
        public AccelMeas X;
        public AccelMeas Y;
        public AccelMeas Z;
    }

    public class AccelMeas
    {
        public string max;
        public string duration;
    }

    public class Error
    {
        public string num;
        public string severity;
        public string description;
    }
}