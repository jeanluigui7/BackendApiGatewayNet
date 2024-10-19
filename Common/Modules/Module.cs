using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Modules
{
    public enum Module
    {
        MarketPlace = 1,
        Finances = 2
    }

    public static class ModuleName
    {
        public static string getModuleName(int module)
        {
            return Enum.GetName(typeof(Module), module);
        }
    }
}

