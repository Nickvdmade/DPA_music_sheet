using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.MusicProperties
{
    public class Bar //maat
    {
        private int amount;
        private int type;
        private List<NoteRest> notes;

        public Bar()
        {
            notes = new List<NoteRest>();
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        public void SetType(int type)
        {
            this.type = type;
        }

        public void AddNote(NoteRest note)
        {
            notes.Add(note);
        }

        public string GetNotes(int previousOctave, string relativePitch, out int newOctave, out string newPitch)
        {
            string result = "";
            for (int i = 0; i < notes.Count; i++)
                result += notes[i].GetString(previousOctave, relativePitch, out previousOctave, out relativePitch) + " ";
            newOctave = previousOctave;
            newPitch = relativePitch;
            return result;
        }

        public void Fill()
        {
            Dictionary<int, int> lengths = new Dictionary<int, int>();
            for (int i = 32; i >= 1; i = i / 2)
                lengths[i] = 0;
            foreach (NoteRest note in notes)
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
            foreach (NoteRest note in notes)
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
                NoteRest lastNote = notes[notes.Count - 1];
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

        public List<NoteRest> GetNotes()
        {
            return notes;
        }
    }
}
