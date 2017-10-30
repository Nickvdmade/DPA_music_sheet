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

        public override string GetString(int previousOctave, string relativePitch, out int newOctave, out string newPitch)
        {
            newOctave = previousOctave;
            newPitch = relativePitch;
            return "r" + length;
        }
    }
}
