using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    abstract class Command
    {
        public abstract void Execute(FileHandler fileHandler, MainViewModel mainViewModel);
    }
}
