﻿using System.IO;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using Microsoft.Win32;

namespace DPA_Musicsheets.Shortcuts
{
    class SaveAsLily
    {
        public static bool Execute(FileHandler fileHandler, MainViewModel mainViewModel)
        {
            if (Keyboard.IsKeyDown(Key.S))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() {Filter = "Lilypond|*.ly"};
                if (saveFileDialog.ShowDialog() == true)
                {
                    string extension = Path.GetExtension(saveFileDialog.FileName);
                    if (extension.EndsWith(".ly"))
                    {
                        fileHandler.SaveToLilypond(saveFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show($"Extension {extension} is not supported.");
                    }
                }
                return true;
            }
            return false;
        }
    }
}
