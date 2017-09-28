using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Managers;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.MusicFileTypes
{
    public class Midi
    {
        private Sequence MidiSequence;
        private event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;

        public Midi(string fileName)
        {
            MidiSequence = new Sequence();
            MidiSequence.Load(fileName);
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
        }
    }
}
