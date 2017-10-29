using System.Collections.Generic;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    class Rest : NoteRestFactory
    {
        public Rest(int length)
        {
            this.length = length;
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

        public override string GetString(int previousOctave, out int newOctave)
        {
            newOctave = previousOctave;
            return "r" + length;
        }
    }
}
