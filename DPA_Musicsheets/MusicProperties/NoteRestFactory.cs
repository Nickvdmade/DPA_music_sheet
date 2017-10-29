using System.Collections.Generic;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    public class NoteRestFactory
    {
        protected int length;

        public virtual double GetLength()
        {
            return 0;
        }

        public virtual void SetLength(int noteLength, int dotAmount)
        {
        }
        public virtual void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
        }

        public virtual Dictionary<int, int> GetLength(Dictionary<int, int> lengths)
        {
            return lengths;
        }

        public virtual string GetString(int previousOctave, out int newOctave)
        {
            newOctave = previousOctave;
            return "";
        }
    }
}
