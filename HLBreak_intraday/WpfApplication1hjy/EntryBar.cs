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
        public double InitRisk;
        public double PrevDayHighLow;
        public double EntryPrice;
        public double Max1;
        public double Max2;
        public double Max3;
        public double Max4;
        public double FixPrice;
        public double Slipage;

        public double Close
        {
            get
            {
                return entryBar.Close;
            }
        }

        public EntryBar(Bar bar, int direction, double initRisk, double prevDayHighLow, double entryPrice, 
                        int max1, int max2, int max3, int max4, double fixPrice, double slipage)
        {
            entryBar = bar;
            Direction = direction;
            InitRisk = initRisk;
            PrevDayHighLow = prevDayHighLow;
            EntryPrice = entryPrice;
            Max1 = max1;
            Max2 = max2;
            Max3 = max3;
            Max4 = max4;
            FixPrice = fixPrice;
            Slipage = slipage;
        }
        
    }
}
