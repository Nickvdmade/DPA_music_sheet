using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    public class Note
    {
        private string pitch;
        private int length;
        private int dots;
        private int octave;

        public Note(string notePitch, int noteOctave)
        {
            pitch = notePitch;
            octave = noteOctave - 1;
        }

        public void SetLength(int noteLength, int dotAmount)
        {
            length = noteLength;
            dots = dotAmount;
        }

        public double GetLength()
        {
            double noteLength = 1.0 / length;
            double tempLength = length / 2.0;
            for (int i = 0; i < dots; i++)
            {
                noteLength += 1 / tempLength;
                tempLength = tempLength / 2;
            }
            return noteLength;
        }

        public void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
            int alter = 0;
            alter += Regex.Matches(pitch, "is").Count;
            alter -= Regex.Matches(pitch, "es|as").Count;
            var note = new PSAMControlLibrary.Note(pitch[0].ToString().ToUpper(), alter, octave,
                (MusicalSymbolDuration) length, NoteStemDirection.Up, NoteTieType.None,
                new List<NoteBeamType>() {NoteBeamType.Single}) {NumberOfDots = dots};
            WPFStaffs.Add(note);
        }
    }
}
