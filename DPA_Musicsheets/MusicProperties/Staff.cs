using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.MusicProperties
{
    public class Staff //notenbalk
    {
        private List<Bar> bars;
        private int tempo { get; set; }

        public Staff()
        {
            bars = new List<Bar>();
            Bar bar = new Bar();
            bars.Add(bar);
        }

        public void AddNote(Note note)
        {
            Bar bar = bars[bars.Count - 1];
            /*if (bar.isFull())
            {
                bar = new Bar();
                bars.Add(bar);
            }*/
            bar.AddNote(note);
        }

        public void Print()
        {
            
        }
    }
}
