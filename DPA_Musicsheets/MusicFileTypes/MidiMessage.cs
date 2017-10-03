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
        private int beatNote; //amount of notes in one bar
        private int beatsPerBar; //type of note in one bar
        private int division;
        private int previousNoteAbsoluteTicks;
        private bool startedNoteIsClosed = true;
        private Note note;

        public void SetMessage(IMidiMessage message, MidiEvent midiEvent, int division)
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
                    beatNote = timeSignatureBytes[0];
                    beatsPerBar = (int)Math.Pow(timeSignatureBytes[1], 2);
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
                    int octave = channelMessage.Data1 / 12;
                    string pitch = Enum.GetName(typeof(MIDInotes), channelMessage.Data1 % 12);
                    note = new Note(pitch, octave);

                    startedNoteIsClosed = false;
                }
                else if (!startedNoteIsClosed)
                {
                    double length = GetNoteLength(midiEvent.AbsoluteTicks);
                    note.SetLength(length);
                    staff.AddNote(note);

                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                    startedNoteIsClosed = true;
                }
                else
                {
                }
            }
        }

        private double GetNoteLength(int absoluteTicks)
        {
            double deltaTicks = absoluteTicks - previousNoteAbsoluteTicks;
            if (deltaTicks <= 0)
                return 0.0;
            double percentageOfBeatNote = deltaTicks / division;
            return (1.0 / beatsPerBar) * percentageOfBeatNote;
        }
    }
}
