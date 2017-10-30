using System;
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
                    staff.SetBar(beatNote, beatsPerBar);
                    break;
                case MetaType.Tempo:
                    byte[] tempoBytes = metaMessage.GetBytes();
                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                    int bpm = 60000000 / tempo;
                    staff.SetTempo(bpm);
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
                    if (midiEvent.AbsoluteTicks != previousNoteAbsoluteTicks)
                    {
                        int dots = 0;
                        int length = GetNoteLength(midiEvent.AbsoluteTicks, out dots);
                        Rest rest = new Rest(length);
                        staff.AddNote(rest);

                        previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                    }
                    int octave = channelMessage.Data1 / 12;
                    string pitch = Enum.GetName(typeof(MIDInotes), channelMessage.Data1 % 12);
                    note = new Note(pitch, octave);

                    startedNoteIsClosed = false;
                }
                else if (!startedNoteIsClosed)
                {
                    int dots = 0;
                    int length = GetNoteLength(midiEvent.AbsoluteTicks, out dots);
                    note.SetLength(length, dots);
                    staff.AddNote(note);

                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                    startedNoteIsClosed = true;
                }
                else
                {
                }
            }
        }

        private int GetNoteLength(int absoluteTicks, out int dots)
        {
            double deltaTicks = absoluteTicks - previousNoteAbsoluteTicks;
            if (deltaTicks <= 0)
            {
                dots = 0;
                return 0;
            }
            double percentageOfBeatNote = deltaTicks / division;
            double noteLength = percentageOfBeatNote / beatsPerBar;
            int singleLength = 0;
            while (noteLength >= 0.03125)
            {
                noteLength -= 0.03125;
                singleLength++;
            }
            int baseLength = GetPowerValue(singleLength);
            noteLength = (singleLength - baseLength) * 0.03125;
            int baseNote = 32 / baseLength;
            dots = GetDotAmount(noteLength, baseNote);
            return baseNote;
        }

        private int GetDotAmount(double dotLength, double baseNote)
        {
            int dots = 0;
            while (dotLength >= 1 / (baseNote * 2))
            {
                dots++;
                dotLength -= 1 / (baseNote * 2);
                baseNote = baseNote * 2;
            }
            return dots;
        }

        private int GetPowerValue(int x)
        {
            int result = x & (x - 1);
            if (result != 0)
                return result;
            return x;
        }
    }
}
