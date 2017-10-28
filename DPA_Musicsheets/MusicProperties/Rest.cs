using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    class Rest : NoteRestFactory
    {
        public Rest(int length)
        {
            base.length = length;
        }

        public override double GetLength()
        {
            return 1.0 / length;
        }

        public override void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
            var rest = new PSAMControlLibrary.Rest((MusicalSymbolDuration) length);
            WPFStaffs.Add(rest);
        }
    }
}
