using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    class InsertTime
    {
        
        public static bool Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            if (Keyboard.IsKeyDown(Key.T))
            {
                if (Keyboard.IsKeyDown(Key.D4) || Keyboard.IsKeyDown(Key.NumPad4))
                    return true;
                if (Keyboard.IsKeyDown(Key.D3) || Keyboard.IsKeyDown(Key.NumPad3))
                    return true;
                if (Keyboard.IsKeyDown(Key.D6) || Keyboard.IsKeyDown(Key.NumPad6))
                    return true;
                else
                    return true;
            }
            return false;
        }
    }
}
