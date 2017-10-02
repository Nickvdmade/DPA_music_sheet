using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicProperties
{
    public class Note
    {
        private string pitch;
        private double length;
        private int octave;

        public Note(string notePitch, double noteLength, int noteOctave)
        {
            pitch = notePitch;
            octave = noteOctave;
            length = noteLength;
        }

        public double GetLength()
        {
            return length;
        }
    }
}
