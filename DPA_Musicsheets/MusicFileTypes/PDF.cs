using System;
using System.Diagnostics;
using System.IO;
using DPA_Musicsheets.MusicProperties;

namespace DPA_Musicsheets.MusicFileTypes
{
    class PDF
    {
        private string fileName;

        public PDF(string file)
        {
            fileName = file;
        }

        public void SaveToFile(Staff staff)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string tmpFileName = $"{fileName}-tmp.ly";
            Lilypond lily = new Lilypond(tmpFileName);
            lily.SaveToFile(staff);

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited)
            { /* Wait for exit */
            }
            if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }
    }
}
