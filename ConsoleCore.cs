using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TerminalEmulator {
    sealed class ConsoleCore {
        private Events _events;
        public Events Events {
            get => _events;
        }
        private Window _currentWindow;
        private int _maxBufferHeight;
        private int _maxBufferWidth;
        public ConsoleCore() {
            SetConsoleSettings();
            _currentWindow = new Window(this, DriveInfo.GetDrives()[0].RootDirectory);
            _events = new Events(this);
        }
        public void Start() {
            _currentWindow.Draw();
            _events.Subscribe(_currentWindow);
            _events.Selecter(_currentWindow.Height);
        }
        private void SetConsoleSettings() {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.SetWindowPosition(0,0);
            _maxBufferHeight = Console.BufferHeight - 1;
            _maxBufferWidth = Console.BufferWidth;
        }
        public void F10KeyPressedHandler() {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Environment.Exit(0);
        }
        ~ConsoleCore() {
            Console.ResetColor();
        }
    }
}
