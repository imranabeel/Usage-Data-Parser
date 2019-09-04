using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usage_Data_Parser
{
    internal static class Version
    {
        private static int majorVersion = 1;
        private static int minorVersion = 0;
        private static int patchNumber = 0;

        public static string getVersion()
        {
            return majorVersion + "." + minorVersion + "." + patchNumber;
        }
    }
}