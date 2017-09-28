using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicProperties
{
    public class Note
    {
        private char pitch;
        private double length;
        private int octave;
        private int change; //1 voor kruis, -1 voor mol

        public Note(char notePitch, double noteLength, int noteOctave = 0, int noteChange = 0)
        {
            pitch = notePitch;
            octave = noteOctave;
            change = noteChange;
            length = noteLength;
        }
    }
}
