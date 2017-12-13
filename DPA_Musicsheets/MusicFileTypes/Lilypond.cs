using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public Staff ReadLily(Staff staff, string content)
        {
            content = content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
            return ReadTokens(staff, content);
        }

        public void SaveToFile(Staff staff)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write(GetLilyFromStaff(staff));
                outputFile.Close();
            }
        }

        public string GetLilyFromStaff(Staff staff)
        {
            StringBuilder lilypondContent = new StringBuilder();
            int relativeStaffOctave = staff.GetRelativeOctave();
            lilypondContent.Append("\\relative c");
            if (relativeStaffOctave < 3)
                for (int i = relativeStaffOctave; i < 3; i++)
                    lilypondContent.Append(",");
            if (relativeStaffOctave > 3)
                for (int i = relativeStaffOctave; i > 3; i--)
                    lilypondContent.Append("'");
            lilypondContent.AppendLine(" {");
            lilypondContent.AppendLine("\\clef " + staff.GetClef());
            int[] time = staff.GetTime();
            lilypondContent.AppendLine("\\time " + time[0] + "/" + time[1]);
            lilypondContent.AppendLine("\\tempo 4=" + staff.GetTempo());

            int barAmount = staff.GetBarAmount();
            for (int i = 0; i < barAmount; i++)
                lilypondContent.AppendLine("" + staff.GetBar(i));
            lilypondContent.AppendLine("}");
            staff.SetRelativePitch(staff.GetRelativePitch());
            staff.SetRelativeOctave(staff.GetRelativeOctave());
            return lilypondContent.ToString();
        }

        private Staff ReadTokens(Staff staff, string content)
        {
            string[] splitContent = content.Split(' ');
            string lastPitch = "";
            for (int i = 0; i < splitContent.Length; i++)
            {
                string s = splitContent[i];
                switch (s)
                {
                    case "\\relative":
                        i++;
                        string relative = splitContent[i];
                        relativeOctave = 3 + relative.Count(x => x == '\'') - relative.Count(x => x == ',');
                        staff.SetRelativeOctave(relativeOctave);
                        lastPitch = relative[0].ToString();
                        break;
                    case "\\clef":
                        i++;
                        staff.SetClef(splitContent[i]);
                        break;
                    case "\\time":
                        i++;
                        string time = splitContent[i];
                        var times = time.Split('/');
                        staff.SetBar((int) UInt32.Parse(times[0]), (int) UInt32.Parse(times[1]));
                        break;
                    case "\\tempo":
                        i++;
                        string tempoText = splitContent[i];
                        var tempo = tempoText.Split('=');
                        staff.SetTempo((int) UInt32.Parse(tempo[1]));
                        break;
                    case "|":
                        staff.AddBar();
                        break;
                    default:
                        if (s.Length != 0)
                        {
                            NoteRestFactory factory = NoteRestFactory.getFactory(s[0]);
                            if (factory != null)
                            {
                                int length;
                                int dots;
                                getInfo(s, ref lastPitch, out length, out dots);
                                staff.AddNote(factory.create(lastPitch, relativeOctave + 1, length, dots));
                            }
                        }
                        break;
                }
            }
            return staff;
        }

        private void getInfo(string info, ref string lastPitch, out int length, out int dots)
        {
            string pitch;
            if (Regex.Matches(info, "is").Count > 0)
                pitch = info[0].ToString() + info[1] + info[2];
            else if (Regex.Matches(info, "es|as").Count > 0)
                pitch = info[0].ToString() + info[1];
            else
                pitch = info[0].ToString();
            List<string> notesOrder = new List<string>() { "c", "d", "e", "f", "g", "a", "b" };
            int prevIndex = notesOrder.IndexOf(lastPitch);
            int currentIndex = notesOrder.IndexOf(pitch);
            if (currentIndex != -1)
            {
                if (prevIndex - currentIndex < -3)
                    relativeOctave--;
                if (prevIndex - currentIndex > 3)
                    relativeOctave++;
                if (Regex.Matches(info, "\'").Count > 0)
                    relativeOctave++;
                if (Regex.Matches(info, ",").Count > 0)
                    relativeOctave--;
            }
            length = Int32.Parse(Regex.Match(info, @"\d+").Value);
            dots = info.Count(c => c.Equals('.'));
            lastPitch = pitch;
        }
    }
}
