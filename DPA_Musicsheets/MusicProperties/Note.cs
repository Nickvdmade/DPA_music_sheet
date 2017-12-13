using System.Collections.Generic;

namespace DPA_Musicsheets.MusicProperties
{
    public class Note : NoteRest
    {
        private string pitch;
        private int dots;
        private int octave;

        public Note(string notePitch, int noteOctave, int noteLength, int dotAmount)
        {
            pitch = notePitch;
            octave = noteOctave - 1;
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

        public override void GetInfo(ref string Notepitch, ref int Noteoctave, ref int Notelength, ref int Notedots)
        {
            Notepitch = pitch;
            Noteoctave = octave;
            Notelength = length;
            Notedots = dots;
        }

        public override string GetString(int previousOctave, string relativePitch, out int newOctave, out string newPitch)
        {
            List<string> notesOrder= new List<string>() { "c", "d", "e", "f", "g", "a", "b" };
            int prevIndex = notesOrder.IndexOf(relativePitch);
            int currentIndex = notesOrder.IndexOf(pitch[0].ToString());
            if (prevIndex - currentIndex < -3)
                previousOctave--;
            if (prevIndex - currentIndex > 3)
                previousOctave++;
            string result = pitch;
            if (octave < previousOctave)
                for (int i = octave; i < previousOctave; i++)
                    result += ",";
            if (octave > previousOctave)
                for (int i = octave; i > previousOctave; i--)
                    result += "'";
            newOctave = octave;
            newPitch = pitch[0].ToString();
            result += length;
            for (int i = 0; i < dots; i++)
                result += ".";
            return result;
        }
    }
}
