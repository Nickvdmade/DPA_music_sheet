using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using Microsoft.Win32;

namespace DPA_Musicsheets.Shortcuts
{
    class OpenFile
    {
        public static bool Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            if (Keyboard.IsKeyDown(Key.O))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    mainViewModel.FileName = openFileDialog.FileName;
                }
                return true;
            }
            return SaveToPDF.Execute(fileHandler, mainViewModel);
        }
    }
}
