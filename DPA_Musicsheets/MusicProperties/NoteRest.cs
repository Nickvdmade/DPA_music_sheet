using System.Collections.Generic;

namespace DPA_Musicsheets.MusicProperties
{
    public class NoteRest
    {
        protected int length;

        public virtual double GetLength()
        {
            return 0;
        }

        public virtual void SetLength(int noteLength, int dotAmount)
        {
        }

        public virtual void GetInfo(ref string Notepitch, ref int Noteoctave, ref int Notelength, ref int Notedots)
        {
        }

        public virtual Dictionary<int, int> GetLength(Dictionary<int, int> lengths)
        {
            return lengths;
        }

        public virtual string GetString(int previousOctave, string relativePitch, out int newOctave, out string newPitch)
        {
            newOctave = previousOctave;
            newPitch = relativePitch;
            return "";
        }
    }
}
