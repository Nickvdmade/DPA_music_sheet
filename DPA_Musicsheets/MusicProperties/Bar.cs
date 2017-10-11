using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    class Bar //maat
    {
        private double amount;
        private double type;
        List<Note> notes;

        public Bar()
        {
            notes = new List<Note>();
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        public void SetType(int type)
        {
            this.type = type;
        }

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        public bool IsFull()
        {
            double length = 0;
            foreach (Note note in notes)
            {
                length += note.GetLength();
            }
            if (length >= (amount / type))
                return true;
            return false;
        }

        public void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
            foreach(Note note in notes)
                note.GetMusicSymbols(WPFStaffs);
            WPFStaffs.Add(new Barline());
        }
    }
}
