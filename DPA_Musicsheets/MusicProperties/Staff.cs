using System;
using System.Collections.Generic;
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
        private int relativeOctave;
        private int previousOctave;
        private string relativePitch;
        private string previousPitch;

        public Staff()
        {
            bars = new List<Bar>();
            Bar bar = new Bar();
            bars.Add(bar);
            clefType = "treble";
            relativeOctave = 4;
            previousOctave = 4;
            relativePitch = "c";
            previousPitch = "c";
        }

        public void Clear()
        {
            bars = new List<Bar>();
            Bar bar = new Bar();
            bars.Add(bar);
            clefType = "treble";
            relativeOctave = 4;
            previousOctave = 4;
            relativePitch = "c";
            previousPitch = "c";
        }

        public void SetTempo(int tempo)
        {
            this.tempo = tempo;
        }

        public int GetTempo()
        {
            return tempo;
        }

        public void SetBar(int amount, int type)
        {
            if (GetPowerValue(type) != 0)
                throw new Exception("Impossible time");
            Bar bar = bars[bars.Count - 1];
            this.amount = amount;
            bar.SetAmount(amount);
            this.type = type;
            bar.SetType(type);
        }

        public int GetBarAmount()
        {
            return bars.Count;
        }

        public string GetBar(int position)
        {
            string result = bars[position].GetNotes(previousOctave, previousPitch, out previousOctave, out previousPitch);
            if (position != bars.Count - 1)
                result += "|";
            return result;
        }

        public int[] GetTime()
        {
            int[] time = {amount, type};
            return time;
        }

        public void SetClef(string clef)
        {
            clefType = clef;
        }

        public string GetClef()
        {
            return clefType;
        }

        public void SetRelativeOctave(int octave)
        {
            relativeOctave = octave;
            previousOctave = octave;
        }

        public int GetRelativeOctave()
        {
            return relativeOctave;
        }

        public void SetRelativePitch(string pitch)
        {
            relativePitch = pitch;
            previousPitch = pitch;
        }

        public string GetRelativePitch()
        {
            return relativePitch;
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
            if (clefType == "bass")
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
