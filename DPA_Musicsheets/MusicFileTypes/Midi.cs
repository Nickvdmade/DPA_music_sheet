using System;
using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.MusicProperties;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using Note = PSAMControlLibrary.Note;
using Rest = PSAMControlLibrary.Rest;

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

        public Staff ReadMidi(Staff staff)
        {
            MidiMessage midiMessage = new MidiMessage();
            for (int i = 0; i < MidiSequence.Count(); i++)
            {
                Track track = MidiSequence[i];
                foreach (var midiEvent in track.Iterator())
                {
                    midiMessage.SetMessage(midiEvent.MidiMessage, midiEvent, MidiSequence.Division);
                    midiMessage.HandleMessageType(staff);
                }
            }
            staff.CheckLastBar();
            return staff;
        }

        public void SaveToFile(Staff staff)
        {
            Sequence sequence = GetSequenceFromStaff(staff);
            sequence.Save(fileName);
        }

        public Sequence GetSequenceFromStaff(Staff staff)
        {
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            int absoluteTicks = 0;
            Sequence sequence = new Sequence();
            Track metaTrack = new Track();
            sequence.Add(metaTrack);
            Track notesTrack = new Track();
            sequence.Add(notesTrack);

            int speed = 60000000 / staff.GetTempo();
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));
            
            List<MusicalSymbol> WPFStaffs = new List<MusicalSymbol>();
            WPFManager.getMusicSymbols(staff, WPFStaffs);
            foreach (MusicalSymbol musicalSymbol in WPFStaffs)
            {
                int[] time;
                double absoluteLength;
                double relationToQuartNote;
                double percentageOfBeatNote;
                double deltaTicks;
                switch (musicalSymbol.Type)
                {
                    case MusicalSymbolType.Note:
                        Note note = musicalSymbol as Note;

                        absoluteLength = 1.0 / (double)note.Duration;
                        absoluteLength += (absoluteLength / 2.0) * note.NumberOfDots;

                        time = staff.GetTime();
                        relationToQuartNote = time[1] / 4.0;
                        percentageOfBeatNote = (1.0 / time[1]) / absoluteLength;
                        deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                        // Calculate height
                        int noteHeight = notesOrderWithCrosses.IndexOf(note.Step.ToLower()) + ((note.Octave + 1) * 12);
                        noteHeight += note.Alter;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                        absoluteTicks += (int)deltaTicks;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume
                        break;
                    case MusicalSymbolType.Rest:
                        Rest rest = musicalSymbol as Rest;

                        absoluteLength = 1.0 / (double)rest.Duration;
                        absoluteLength += (absoluteLength / 2.0) * rest.NumberOfDots;

                        time = staff.GetTime();
                        relationToQuartNote = time[1] / 4.0;
                        percentageOfBeatNote = (1.0 / time[1]) / absoluteLength;
                        deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;
                        absoluteTicks += (int)deltaTicks;
                        break;
                    case MusicalSymbolType.TimeSignature:
                        time = staff.GetTime();
                        byte[] timeSignature = new byte[4];
                        timeSignature[0] = (byte) time[0];
                        timeSignature[1] = (byte) (Math.Log(time[1]) / Math.Log(2));
                        metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
                        break;
                    default:
                        break;
                }
            }
            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            return sequence;
        }
    }
}
