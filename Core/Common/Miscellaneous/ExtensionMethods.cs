using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Miscellaneous
{
    public static class ExtensionMethods
    {
        public static int ToInt(this object val)
        {
            var number = Convert.ToInt32(val);
            return number;
        }

        public static decimal ToDecimal(this object val)
        {
            if (val == null) return 0;
            decimal number = 0;
            bool success = Decimal.TryParse(val.ToString(), out number);
            if (success)
                return number;
            else
                return 0;
        }

        public static bool ToBool(this object val)
        {
            if (val == null) return false;
            Boolean result = false;
            bool success = Boolean.TryParse(val.ToString(), out result);
            if (success)
                return result;
            else
                return false;
        }

        public static bool ToBoolean(this object val)
        {
            if (val == null) return false;
            else
            {
                if (val.ToInt() >= 1) return true;
                else return false;
            };
        }

        public static DateTime? ToDateTime(this object val)
        {
            if (val == null) return null;
            DateTime dt;
            bool success = DateTime.TryParse(val.ToString(), out dt);
            if (success)
                return dt;
            else
                return null;
        }

    }
}
