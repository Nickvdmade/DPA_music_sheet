using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.MusicProperties;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.MusicFileTypes
{
    class MidiMessage
    {
        private IMidiMessage midiMessage;
        private MidiEvent midiEvent;
        private int division;
        private int previousMidiKey = 60; // Central C;
        private int previousNoteAbsoluteTicks = 0;
        bool startedNoteIsClosed = true;

        public MidiMessage(IMidiMessage message, MidiEvent midiEvent, int division)
        {
            midiMessage = message;
            this.midiEvent = midiEvent;
            this.division = division;
        }

        public void HandleMessageType(Staff staff)
        {
            switch (midiMessage.MessageType)
            {
                case MessageType.Meta:
                    HandleMeta(staff);
                    break;
                case MessageType.Channel:
                    HandleChannel(staff);
                    break;
                default:
                    break;
            }
        }

        private void HandleMeta(Staff staff)
        {
            var metaMessage = midiMessage as MetaMessage;
            switch (metaMessage.MetaType)
            {
                case MetaType.TimeSignature:
                    byte[] timeSignatureBytes = metaMessage.GetBytes();
                    int beatNote = timeSignatureBytes[0];
                    int beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
                    staff.setBar(beatNote, beatsPerBar);
                    break;
                case MetaType.Tempo:
                    byte[] tempoBytes = metaMessage.GetBytes();
                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                    int bpm = 60000000 / tempo;
                    staff.setTempo(bpm);
                    break;
                default: break;
            }
        }

        private void HandleChannel(Staff staff)
        {
            var channelMessage = midiMessage as ChannelMessage;
            if (channelMessage.Command == ChannelCommand.NoteOn)
            {
                if (channelMessage.Data2 > 0) // Data2 = loudness
                {
                    /*Dictionary<int, string> MIDInotes = new Dictionary<int, string>();
                    MIDInotes.Add(0, "c");
                    MIDInotes.Add(1, "cis");
                    MIDInotes.Add(2, "d");
                    MIDInotes.Add(3, "dis");
                    MIDInotes.Add(4, "e");
                    MIDInotes.Add(5, "f");
                    MIDInotes.Add(6, "fis");
                    MIDInotes.Add(7, "g");
                    MIDInotes.Add(8, "gis");
                    MIDInotes.Add(9, "a");
                    MIDInotes.Add(10, "ais");
                    MIDInotes.Add(11, "b");
                    MusicProperties.Note newNote = new MusicProperties.Note(pitch, length, octave);
                    */

                    previousMidiKey = channelMessage.Data1;
                    startedNoteIsClosed = false;
                }
                else if (!startedNoteIsClosed)
                {
                    // Finish the previous note with the length.
                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                    startedNoteIsClosed = true;
                }
                else
                {
                }
            }
        }
    }
}
