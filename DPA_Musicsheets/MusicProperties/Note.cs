using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicProperties
{
    class Note
    {
        private char pitch;
        private int octave;
        private int change; //1 voor kruis, -1 voor mol
        private double length;

        public Note(char notePitch, double noteLength, int noteChange = 0, int noteOctave = 0)
        {
            pitch = notePitch;
            octave = noteOctave;
            change = noteChange;
            length = noteLength;
        }
    }
}
