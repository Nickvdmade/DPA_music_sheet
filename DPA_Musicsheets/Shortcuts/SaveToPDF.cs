﻿using System.IO;
using System.Windows;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using Microsoft.Win32;

namespace DPA_Musicsheets.Shortcuts
{
    class SaveToPDF : Command
    {
        public override void Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "PDF|*.pdf" };
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
        }
    }
}
