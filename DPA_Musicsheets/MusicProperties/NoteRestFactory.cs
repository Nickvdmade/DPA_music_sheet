namespace DPA_Musicsheets.MusicProperties
{
    abstract class NoteRestFactory
    {
        private static NoteFactory NOTE_FACTORY = new NoteFactory();
        private static RestFactory REST_FACTORY = new RestFactory();

        public static NoteRestFactory getFactory(char type)
        {
            NoteRestFactory factory = null;
            switch (type)
            {
                case 'r':
                    factory = REST_FACTORY;
                    break;
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                    factory = NOTE_FACTORY;
                    break;
            }
            return factory;
        }

        public abstract NoteRest create(string pitch, int octave, int length, int dots);
    }
}
