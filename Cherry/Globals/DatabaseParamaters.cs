using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cherry.Web.Globals
{
    public static class DatabaseParamaters
    {
        public static string DBtype
        {
            get
            {
#if DEVELOPMENT
                return "_local";
#else
                return "_online";
#endif
            }
        }
        public static bool DBtypebool
        {
            get
            {
#if DEVELOPMENT
                return true;
#else
                return false;
#endif
            }
        }
    }
}
