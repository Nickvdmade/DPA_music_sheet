using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    class InsertTime : Command
    {
        private int type;
        private int amount;
        public InsertTime(int type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
        public override void Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {

        }
    }
}
