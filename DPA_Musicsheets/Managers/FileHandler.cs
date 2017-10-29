
using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.MusicFileTypes;
using DPA_Musicsheets.MusicProperties;
using Note = PSAMControlLibrary.Note;
using Rest = PSAMControlLibrary.Rest;

namespace DPA_Musicsheets.Managers
{
    public class FileHandler
    {
        private string _lilypondText;
        public string LilypondText
        {
            get { return _lilypondText; }
            set
            {
                _lilypondText = value;
                LilypondTextChanged?.Invoke(this, new LilypondEventArgs() { LilypondText = value });
            }
        }
        public List<MusicalSymbol> WPFStaffs { get; set; } = new List<MusicalSymbol>();
        private static List<Char> notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };

        public Sequence MidiSequence { get; set; }

        public event EventHandler<LilypondEventArgs> LilypondTextChanged;
        public event EventHandler<WPFStaffsEventArgs> WPFStaffsChanged;
        public event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;

        private int _beatNote = 4;    // De waarde van een beatnote.
        private int _bpm = 120;       // Aantal beatnotes per minute.
        private int _beatsPerBar;     // Aantal beatnotes per maat.

        public Staff Staff = new Staff();

        public void OpenFile(string fileName)
        {
            Staff.Clear();
            Midi midi = new Midi(fileName);
            Lilypond lily = new Lilypond(fileName);
            if (Path.GetExtension(fileName).EndsWith(".mid"))
            {
                midi.Open();
                midi.ReadMidi(Staff);
            }
            else if (Path.GetExtension(fileName).EndsWith(".ly"))
            {
                lily.Open();
                lily.ReadLily(Staff);
            }
            else
            {
                throw new NotSupportedException($"File extension {Path.GetExtension(fileName)} is not supported.");
            }
            WPFStaffs.Clear();
            Staff.GetMusicSymbols(WPFStaffs);
            WPFStaffsChanged?.Invoke(this, new WPFStaffsEventArgs() { Symbols = WPFStaffs});

            LilypondText = lily.GetLilyFromStaff(Staff);

            MidiSequence = midi.GetSequenceFromStaff(Staff);
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
        }


        public void ShowStaff()
        {
            WPFStaffs.Clear();
            Staff.GetMusicSymbols(WPFStaffs);
            WPFStaffsChanged?.Invoke(this, new WPFStaffsEventArgs() { Symbols = WPFStaffs });

            Lilypond lily = new Lilypond("");
            LilypondText = lily.GetLilyFromStaff(Staff);

            Midi midi = new Midi("");
            MidiSequence = midi.GetSequenceFromStaff(Staff);
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
        }

        #region Saving to files
        internal void SaveToMidi(string fileName)
        {
            Midi midi = new Midi(fileName);
            midi.SaveToFile(Staff);
        }

        internal void SaveToPDF(string fileName)
        {
            PDF pdf = new PDF(fileName);
            pdf.SaveToFile(Staff);
        }

        internal void SaveToLilypond(string fileName)
        {
            Lilypond lily = new Lilypond(fileName);
            lily.SaveToFile(Staff);
        }
        #endregion Saving to files
    }
}
