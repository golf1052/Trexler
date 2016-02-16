using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Trexler
{
    public static class CommonExtensions
    {
        public static string ToInvariantString(this object obj)
        {
            IConvertible c = obj as IConvertible;
            if (c != null)
            {
                return c.ToString(CultureInfo.InvariantCulture);
            }

            IFormattable f = obj as IFormattable;
            if (f != null)
            {
                return f.ToString(null, CultureInfo.InvariantCulture);
            }

            return obj.ToString();
        }
    }
}
