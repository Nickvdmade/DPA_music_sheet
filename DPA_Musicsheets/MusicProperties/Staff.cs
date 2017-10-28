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
        private string clefType;

        public Staff()
        {
            bars = new List<Bar>();
            Bar bar = new Bar();
            bars.Add(bar);
            clefType = "treble";
        }

        public void setTempo(int tempo)
        {
            this.tempo = tempo;
        }

        public void setBar(int amount, int type)
        {
            if (GetPowerValue(type) != 0)
                throw new Exception("Impossible time");
            Bar bar = bars[bars.Count - 1];
            this.amount = amount;
            bar.SetAmount(amount);
            this.type = type;
            bar.SetType(type);
        }

        public void SetClef(string clef)
        {
            clefType = clef;
        }

        public void AddBar()
        {
            Bar bar = bars[bars.Count - 1];
            if (!bar.IsFull(bars))
            {
                bar.Fill();
                bar = new Bar();
                bar.SetAmount(amount);
                bar.SetType(type);
                bars.Add(bar);
            }
        }

        public void AddNote(NoteRestFactory note)
        {
            Bar bar = bars[bars.Count - 1];
            if (bar.IsFull(bars))
            {
                bar = bars[bars.Count - 1];
            }
            bar.AddNote(note);
        }

        public void CheckLastBar()
        {
            Bar bar = bars[bars.Count - 1];
            if (!bar.IsFull(bars))
                bar.Fill();
        }

        public void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
            Clef clef = new Clef(ClefType.GClef, 2);
            if (this.clefType == "bass")
            {
                clef = new Clef(ClefType.CClef, 4);
            }
            WPFStaffs.Add(clef);
            WPFStaffs.Add(new TimeSignature(TimeSignatureType.Numbers, (UInt32)amount, (UInt32)type));
            foreach (Bar bar in bars)
                bar.GetMusicSymbols(WPFStaffs);
        }

        private int GetPowerValue(int x)
        {
            int result = x & (x - 1);
            return result;
        }
    }
}
