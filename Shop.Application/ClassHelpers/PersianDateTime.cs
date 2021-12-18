using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ClassHelpers
{
    public static class PersianDateTime
    {
        public static string Now()
        {
            var date = DateTime.Now;
            PersianCalendar persian = new PersianCalendar();
            return $"{persian.GetYear(date)}/{persian.GetMonth(date)}/{persian.GetDayOfMonth(date)}";
        }
    }
}
