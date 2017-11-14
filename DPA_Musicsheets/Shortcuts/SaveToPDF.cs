using System.IO;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using Microsoft.Win32;

namespace DPA_Musicsheets.Shortcuts
{
    class SaveToPDF
    {
        public static bool Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            if (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.P))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() {Filter = "PDF|*.pdf"};
                if (saveFileDialog.ShowDialog() == true)
                {
                    string extension = Path.GetExtension(saveFileDialog.FileName);
                    if (extension.EndsWith(".pdf"))
                    {
                        fileHandler.SaveToPDF(saveFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show($"Extension {extension} is not supported.");
                    }
                }
                return true;
            }
            return SaveAsLily.Execute(fileHandler, mainViewModel);
        }
    }
}
