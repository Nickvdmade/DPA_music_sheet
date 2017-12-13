using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DPA_Musicsheets.MusicProperties;
using PSAMControlLibrary;

namespace DPA_Musicsheets.Managers
{
    class WPFManager
    {
        public static void getMusicSymbols(Staff staff, List<MusicalSymbol> WPFStaffs)
        {
            Clef clef = new Clef(ClefType.GClef, 2);
            if (staff.GetClef() == "bass")
            {
                clef = new Clef(ClefType.CClef, 4);
            }
            WPFStaffs.Add(clef);
            int[] time = staff.GetTime();
            WPFStaffs.Add(new TimeSignature(TimeSignatureType.Numbers, (UInt32)time[0], (UInt32)time[1]));
            List<Bar> bars = staff.GetBars();
            foreach (Bar bar in bars)
            {
                List<NoteRest> notes = bar.GetNotes();
                foreach (NoteRest note in notes)
                {
                    string pitch = "";
                    int octave = 0;
                    int length = 0;
                    int dots = 0;
                    note.GetInfo(ref pitch, ref octave, ref length, ref dots);
                    if (pitch.Equals("r"))
                    {
                        var rest = new PSAMControlLibrary.Rest((MusicalSymbolDuration)length);
                        WPFStaffs.Add(rest);
                    }
                    else
                    {
                        int alter = 0;
                        alter += Regex.Matches(pitch, "is").Count;
                        alter -= Regex.Matches(pitch, "es|as").Count;
                        var WPFNote = new PSAMControlLibrary.Note(pitch[0].ToString().ToUpper(), alter, octave,
                                (MusicalSymbolDuration)length, NoteStemDirection.Up, NoteTieType.None,
                                new List<NoteBeamType>() { NoteBeamType.Single })
                            { NumberOfDots = dots };
                        WPFStaffs.Add(WPFNote);
                    }
                }
                WPFStaffs.Add(new Barline());
            }
        }
    }
}
