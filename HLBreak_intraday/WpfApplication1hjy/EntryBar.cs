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
        public double Close
        {
            get
            {
                return entryBar.Close;
            }
        }

        public EntryBar(Bar bar, int direction)
        {
            entryBar = bar;
            Direction = direction;
        }
        
    }
}
