namespace DPA_Musicsheets.MusicProperties
{
    class Rest : NoteRest
    {
        public Rest(int length)
        {
            this.length = length;
        }

        public override double GetLength()
        {
            return 1.0 / length;
        }

        public override void GetInfo(ref string Notepitch, ref int Noteoctave, ref int Notelength, ref int Notedots)
        {
            Notepitch = "r";
            Notelength = length;
        }

        public override string GetString(int previousOctave, string relativePitch, out int newOctave, out string newPitch)
        {
            newOctave = previousOctave;
            newPitch = relativePitch;
            return "r" + length;
        }
    }
}
