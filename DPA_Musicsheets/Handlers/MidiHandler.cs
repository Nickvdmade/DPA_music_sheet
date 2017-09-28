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
            
        }

        void IFileTypeHandler.SaveToFile(string fileName)
        {
            
        }
    }
}
