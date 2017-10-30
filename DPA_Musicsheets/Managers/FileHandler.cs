using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using DPA_Musicsheets.MusicFileTypes;
using DPA_Musicsheets.MusicProperties;
using Microsoft.Win32;

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

        public Sequence MidiSequence { get; set; }

        public event EventHandler<LilypondEventArgs> LilypondTextChanged;
        public event EventHandler<WPFStaffsEventArgs> WPFStaffsChanged;
        public event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;

        private int _beatNote = 4;    // De waarde van een beatnote.
        private int _bpm = 120;       // Aantal beatnotes per minute.
        private int _beatsPerBar;     // Aantal beatnotes per maat.

        public Staff Staff = new Staff();
        private string originalLily;

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
            originalLily = lily.GetLilyFromStaff(Staff);
            ShowStaff();
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

        public void ShowStaff(string content)
        {
            if (content != null)
            {
                LilypondText = content;
            }
            Staff.Clear();
            Lilypond lily = new Lilypond("");
            Staff = lily.ReadLily(Staff, LilypondText);
            ShowStaff();
        }

        #region Saving to files
        void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        }
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

        public void CheckForChange()
        {
            Lilypond lily = new Lilypond("");
            string finalLily = lily.GetLilyFromStaff(Staff);
            if (finalLily != originalLily)
            {
                var result = MessageBox.Show("Save changes?", "Close program", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    Save();
            }
        }
    }
}
