using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Handlers
{
    interface IFileTypeHandler
    {
        void Load(string fileName);
        void SaveToFile(string fileName);
    }
}
