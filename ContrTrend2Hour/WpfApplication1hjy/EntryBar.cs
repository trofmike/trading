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

        public EntryBar(Bar bar, int direction, double entryPrice, double fixPrice)
        {
            entryBar = bar;
            Direction = direction;
            EntryPrice = entryPrice;
            FixPrice = fixPrice;
        }
        
    }
}
