using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.MusicFileTypes;
using DPA_Musicsheets.MusicProperties;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Handlers
{
    class MidiHandler:IFileTypeHandler
    {
        public Midi midi;
        public Lilypond lilypond;
        
        void IFileTypeHandler.Load(string fileName)
        {
            midi = new Midi(fileName);
            lilypond = new Lilypond(midi);
        }

        private StringBuilder CreateLilypond(Sequence midiSequence)
        {
            StringBuilder lilypondContent = new StringBuilder();
            lilypondContent.AppendLine("\\relative c' {");
            lilypondContent.AppendLine("\\clef treble");

            int division = midiSequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;
            
            for (int i = 0; i < midiSequence.Count(); i++)
            {
                
            }

            return lilypondContent;
        }

        void IFileTypeHandler.SaveToFile(string fileName)
        {
            
        }
    }
}
