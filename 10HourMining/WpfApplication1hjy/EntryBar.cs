using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1hjy
{
    class EntryBar
    {
        public Bar entryBar;
        public double Open
        {
            get
            {
                return entryBar.Open;
            }
        }

        public EntryBar(Bar bar)
        {
            entryBar = bar;
        }
        
    }
}
