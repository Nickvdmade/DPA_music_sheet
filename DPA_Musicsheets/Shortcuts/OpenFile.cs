using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using Microsoft.Win32;

namespace DPA_Musicsheets.Shortcuts
{
    class OpenFile : Command
    {
        public override void Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                mainViewModel.FileName = openFileDialog.FileName;
            }
        }
    }
}
