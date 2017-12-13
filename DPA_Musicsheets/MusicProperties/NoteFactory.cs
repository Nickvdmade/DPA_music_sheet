namespace DPA_Musicsheets.MusicProperties
{
    class NoteFactory : NoteRestFactory
    {
        public override NoteRest create(string pitch, int octave, int length, int dots)
        {
            NoteRest note = new Note(pitch, octave, length, dots);
            return note;
        }
    }
}
