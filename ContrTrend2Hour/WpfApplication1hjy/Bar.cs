using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1hjy
{
    class Bar
    {
        public DateTime Date;
        public int Hour;
        public int Month;
        public int Day;
        public int Year;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public double Volume;


        public Bar()
        {
            Date = new DateTime();    
        }

        public Bar(string date, string time, string open, string high, string low, string close, string volume)
        {
            Date = parseDate(date, time);
            Open = double.Parse(open, CultureInfo.InvariantCulture);
            High = double.Parse(high, CultureInfo.InvariantCulture);
            Low = double.Parse(low, CultureInfo.InvariantCulture);
            Close = double.Parse(close, CultureInfo.InvariantCulture);
            Volume = double.Parse(volume, CultureInfo.InvariantCulture);
        }

        private DateTime parseDate(string date, string time)
        {
            DateTime res;

            int year = int.Parse(date.Substring(0, 4));
            Year = year;
            int month = int.Parse(date.Substring(4, 2));
            Month = month;
            int day = int.Parse(date.Substring(6, 2));
            Day = day;

            int hour = int.Parse(time.Substring(0, 2));
            Hour = hour;

            int min = int.Parse(time.Substring(2, 2));
            int sec = int.Parse(time.Substring(4, 2));

            res = new DateTime(year, month, day, hour, min, sec);

            return res;
        }
    }

}
