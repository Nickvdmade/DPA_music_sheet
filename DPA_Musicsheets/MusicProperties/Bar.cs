using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicProperties
{
    class Bar //maat
    {
        private int amount;
        private int type;
        List<Note> notes;

        public Bar()
        {
            notes = new List<Note>();
        }

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        /*public bool IsFull()
        {
            
        }*/
    }
}
