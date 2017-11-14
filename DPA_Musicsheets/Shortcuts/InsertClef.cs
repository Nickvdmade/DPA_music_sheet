using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    class InsertClef
    {
        public static bool Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            if (Keyboard.IsKeyDown(Key.C))
                return true;
            return InsertTempo.Execute(fileHandler, mainViewModel);
        }
    }
}
