namespace DPA_Musicsheets.MusicProperties
{
    class RestFactory : NoteRestFactory
    {
        public override NoteRest create(string pitch, int octave, int length, int dots)
        {
            NoteRest rest = new Rest(length);
            return rest;
        }
    }
}
