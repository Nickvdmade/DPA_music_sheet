using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    public class Staff //notenbalk
    {
        private List<Bar> bars;
        private int tempo;
        private int amount;
        private int type;

        public Staff()
        {
            bars = new List<Bar>();
            Bar bar = new Bar();
            bars.Add(bar);
        }

        public void setTempo(int tempo)
        {
            this.tempo = tempo;
        }

        public void setBar(int amount, int type)
        {
            Bar bar = bars[bars.Count - 1];
            this.amount = amount;
            bar.SetAmount(amount);
            this.type = type;
            bar.SetType(type);
        }

        public void AddNote(Note note)
        {
            Bar bar = bars[bars.Count - 1];
            if (bar.IsFull())
            {
                bar = new Bar();
                bar.SetAmount(amount);
                bar.SetType(type);
                bars.Add(bar);
            }
            bar.AddNote(note);
        }

        public void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
            Clef clef = new Clef(ClefType.GClef, 2);
            WPFStaffs.Add(clef);
            WPFStaffs.Add(new TimeSignature(TimeSignatureType.Numbers, (UInt32)amount, (UInt32)type));
            foreach (Bar bar in bars)
                bar.GetMusicSymbols(WPFStaffs);
        }
    }
}
