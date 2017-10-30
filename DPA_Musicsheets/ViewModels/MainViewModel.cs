﻿using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Shortcuts;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; RaisePropertyChanged(() => CurrentState); }
        }

        private FileHandler _fileHandler;

        public MainViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";

            MessengerInstance.Register<CurrentStateMessage>(this, (message) => CurrentState = message.State);
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        });
        public ICommand LoadCommand => new RelayCommand(() =>
        {
            _fileHandler.OpenFile(FileName);
        });
        
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            Command command = null;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (Keyboard.IsKeyDown(Key.S))
                {
                    if (Keyboard.IsKeyDown(Key.P))
                        command = new SaveToPDF();
                    else
                        command = new SaveAsLily();
                }
                if (Keyboard.IsKeyDown(Key.O))
                    command = new OpenFile();
            }
            if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                if (Keyboard.IsKeyDown(Key.C))
                    command = new InsertClef();
                if (Keyboard.IsKeyDown(Key.S))
                    command = new InsertTempo();
                if (Keyboard.IsKeyDown(Key.T))
                {
                    if (Keyboard.IsKeyDown(Key.D4) || Keyboard.IsKeyDown(Key.NumPad4))
                        command = new InsertTime(4, 4);
                    if (Keyboard.IsKeyDown(Key.D3) || Keyboard.IsKeyDown(Key.NumPad3))
                        command = new InsertTime(3, 4);
                    else if (Keyboard.IsKeyDown(Key.D6) || Keyboard.IsKeyDown(Key.NumPad6))
                        command = new InsertTime(6, 8);
                    else
                        command = new InsertTime(4, 4);
                }
            }
            if (command != null)
                command.Execute(_fileHandler, this);
            Console.WriteLine($"Key down: {e.Key}");
        });

        public ICommand OnKeyUpCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Key Up");
        });

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            _fileHandler.CheckForChange();
            ViewModelLocator.Cleanup();
        });
    }
}
