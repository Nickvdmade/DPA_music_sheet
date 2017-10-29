using System.Collections.Generic;
using System.Text.RegularExpressions;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    public class Note : NoteRestFactory
    {
        private string pitch;
        private int dots;
        private int octave;

        public Note(string notePitch, int noteOctave)
        {
            pitch = notePitch;
            octave = noteOctave - 1;
        }

        public override void SetLength(int noteLength, int dotAmount)
        {
            length = noteLength;
            dots = dotAmount;
        }

        public override double GetLength()
        {
            double noteLength = 1.0 / length;
            double tempLength = length * 2.0;
            for (int i = 0; i < dots; i++)
            {
                noteLength += 1 / tempLength;
                tempLength = tempLength * 2;
            }
            return noteLength;
        }

        public override Dictionary<int, int> GetLength(Dictionary<int, int> lengths)
        {
            lengths[length]++;
            int tempLength = length * 2;
            for (int i = 0; i < dots; i++)
            {
                lengths[tempLength]++;
                tempLength = tempLength * 2;
            }
            return lengths;
        }

        public override void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
            int alter = 0;
            alter += Regex.Matches(pitch, "is").Count;
            alter -= Regex.Matches(pitch, "es|as").Count;
            var note = new PSAMControlLibrary.Note(pitch[0].ToString().ToUpper(), alter, octave,
                (MusicalSymbolDuration) length, NoteStemDirection.Up, NoteTieType.None,
                new List<NoteBeamType>() {NoteBeamType.Single}) {NumberOfDots = dots};
            WPFStaffs.Add(note);
        }

        public override string GetString(int previousOctave, out int newOctave)
        {
            string result = pitch;
            if (octave < previousOctave)
                for (int i = octave; i < previousOctave; i++)
                    result += ",";
            if (octave > previousOctave)
                for (int i = octave; i > previousOctave; i--)
                    result += "'";
            newOctave = octave;
            result += length;
            for (int i = 0; i < dots; i++)
                result += ".";
            return result;
        }
    }
}
