using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1hjy
{
    class EntryBar
    {
        public Bar entryBar;
        public int Direction;
        public int InitRisk;
        public int Max1;
        public int Max2;
        public int Max3;
        public int Max4;
        public double PrevDayHighLow;
        public double EntryPrice;

        public double Close
        {
            get
            {
                return entryBar.Close;
            }
        }

        public EntryBar(Bar bar, int direction, int initRisk, int max1, int max2, int max3, int max4, double prevDayHighLow, double entryPrice)
        {
            entryBar = bar;
            Direction = direction;
            InitRisk = initRisk;
            Max1 = max1;
            Max2 = max2;
            Max3 = max3;
            Max4 = max4;
            PrevDayHighLow = prevDayHighLow;
            EntryPrice = entryPrice;
        }
        
    }
}
