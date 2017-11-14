using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    class InsertTempo
    {
        public static bool Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            if (Keyboard.IsKeyDown(Key.S))
                return true;
            return InsertTime.Execute(fileHandler, mainViewModel);
        }
    }
}
