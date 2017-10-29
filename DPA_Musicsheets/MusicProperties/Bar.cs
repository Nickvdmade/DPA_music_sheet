using System;
using System.Collections.Generic;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicProperties
{
    class Bar //maat
    {
        private int amount;
        private int type;
        private List<NoteRestFactory> notes;

        public Bar()
        {
            notes = new List<NoteRestFactory>();
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        public void SetType(int type)
        {
            this.type = type;
        }

        public void AddNote(NoteRestFactory note)
        {
            notes.Add(note);
        }

        public string GetNotes(int previousOctave, out int newOctave)
        {
            string result = "";
            for (int i = 0; i < notes.Count; i++)
                result += notes[i].GetString(previousOctave, out previousOctave) + " ";
            result += "|";
            newOctave = previousOctave;
            return result;
        }

        public void Fill()
        {
            Dictionary<int, int> lengths = new Dictionary<int, int>();
            for (int i = 32; i >= 1; i = i / 2)
                lengths[i] = 0;
            foreach (NoteRestFactory note in notes)
            {
                lengths = note.GetLength(lengths);
            }
            for (int i = 32; i > type; i = i / 2)
            {
                while (lengths[i] >= 2)
                {
                    lengths[i / 2]++;
                    lengths[i] -= 2;
                }
            }
            for (int i = 1; i < type; i = i * 2)
            {
                while (lengths[i] >= 1)
                {
                    lengths[i * 2] += 2;
                    lengths[i]--;
                }
            }
            if (lengths[type] > amount)
                throw new Exception("Bar too full");
            if (lengths[type] < amount)
                for (int i = 32; i >= type; i = i / 2)
                {
                    if (i == type)
                    {
                        if (lengths[type] < amount)
                        {
                            int restLength = amount - lengths[type];
                            for (int j = 0; j < restLength; j++)
                                AddNote(new Rest(type));
                        }
                    }
                    else
                    {
                        if (lengths[i] == 1)
                        {
                            AddNote(new Rest(i));
                            lengths[i]++;
                        }
                        if (lengths[i] == 2)
                        {
                            lengths[i / 2]++;
                            lengths[i] -= 2;
                        }
                    }
                }
        }

        public bool IsFull(List<Bar> bars)
        {
            double length = 0;
            foreach (NoteRestFactory note in notes)
            {
                length += note.GetLength();
            }
            if (length == (double) amount / type)
            {
                Bar bar = new Bar();
                bar.SetAmount(amount);
                bar.SetType(type);
                bars.Add(bar);
                return true;
            }
            if (length > (double) amount / type)
            {
                NoteRestFactory lastNote = notes[notes.Count - 1];
                notes.Remove(lastNote);
                Bar bar = new Bar();
                bar.SetAmount(amount);
                bar.SetType(type);
                bar.AddNote(lastNote);
                bars.Add(bar);
                Fill();
                return true;
            }
            return false;
        }

        public void GetMusicSymbols(List<MusicalSymbol> WPFStaffs)
        {
            foreach(NoteRestFactory note in notes)
                note.GetMusicSymbols(WPFStaffs);
            WPFStaffs.Add(new Barline());
        }
    }
}
