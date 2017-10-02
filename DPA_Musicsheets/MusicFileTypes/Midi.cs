using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.MusicProperties;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.MusicFileTypes
{
    public class Midi
    {
        private string fileName;
        private Sequence MidiSequence;
        private event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;

        public Midi(string file)
        {
            fileName = file;
        }

        public void Open()
        {
            MidiSequence = new Sequence();
            MidiSequence.Load(fileName);
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
        }

        public Staff ReadMidi()
        {
            Staff staff = new Staff();
            for (int i = 0; i < MidiSequence.Count(); i++)
            {
                Track track = MidiSequence[i];
                foreach (var midiEvent in track.Iterator())
                {
                    MidiMessage midiMessage = new MidiMessage(midiEvent.MidiMessage, midiEvent, MidiSequence.Division);
                    midiMessage.HandleMessageType(staff);
                }
            }
            return staff;
        }
    }
}
