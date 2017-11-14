using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    class Command
    {
        public static bool Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
                return OpenFile.Execute(fileHandler, mainViewModel);
            if (Keyboard.Modifiers == ModifierKeys.Alt)
                return InsertClef.Execute(fileHandler, mainViewModel);
            return false;
        }
    }
}
