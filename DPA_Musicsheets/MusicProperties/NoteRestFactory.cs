using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
