using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.MusicProperties;

namespace DPA_Musicsheets.MusicFileTypes
{
    public class Lilypond
    {
        private string fileName;
        private string lilypondText;
        private int relativeOctave;

        public Lilypond(string file)
        {
            fileName = file;
        }

        public void Open()
        {
           StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadAllLines(fileName))
            {
                sb.AppendLine(line);
            }
            lilypondText = sb.ToString();
        }

        public Staff ReadLily(Staff staff)
        {
            string content = lilypondText;
            content = content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
            return ReadTokens(staff, content);
        }

        private Staff ReadTokens(Staff staff, string content)
        {
            string[] splitContent = content.Split(' ');
            for (int i = 0; i < splitContent.Length; i++)
            {
                string s = splitContent[i];
                switch (s)
                {
                    case "\\relative":
                        i++;
                        string relative = splitContent[i];
                        relativeOctave = 4 + relative.Count(x => x == '\'') - relative.Count(x => x == ',');
                        break;
                    case "\\clef":
                        i++;
                        staff.SetClef(splitContent[i]);
                        break;
                    case "\\time":
                        i++;
                        string time = splitContent[i];
                        var times = time.Split('/');
                        staff.setBar((int) UInt32.Parse(times[0]), (int) UInt32.Parse(times[1]));
                        break;
                    case "\\tempo":
                        i++;
                        string tempoText = splitContent[i];
                        var tempo = tempoText.Split('=');
                        staff.setTempo((int) UInt32.Parse(tempo[1]));
                        break;
                    case "|":
                        staff.AddBar();
                        break;
                    default:
                        if (new Regex(@"[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                        {
                            staff.AddNote(AddNote(s));
                        }
                        if (new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                        {
                            staff.AddNote(AddRest(s));
                        }
                        break;
                }
            }
            return staff;
        }

        private NoteRestFactory AddNote(string info)
        {
            string pitch;
            if (Regex.Matches(info, "is").Count > 0)
                pitch = info[0].ToString() + info[1] + info[2];
            if (Regex.Matches(info, "es|as").Count > 0)
                pitch = info[0].ToString() + info[1];
            else
                pitch = info[0].ToString();
            if (Regex.Matches(info, "\'").Count > 0)
                relativeOctave++;
            if (Regex.Matches(info, ",").Count > 0)
                relativeOctave--;
            NoteRestFactory note = new Note(pitch, relativeOctave);
            int length = Int32.Parse(Regex.Match(info, @"\d+").Value);
            int dots = info.Count(c => c.Equals('.'));
            note.SetLength(length, dots);
            return note;
        }

        private NoteRestFactory AddRest(string info)
        {
            int length = Int32.Parse(info[1].ToString());
            NoteRestFactory rest = new Rest(length);
            return rest;
        }
    }
}
