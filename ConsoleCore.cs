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
        private Tab _tab1;
        private Tab _tab2;
        public Tab CurrentTab { get; set; }
        private int _maxBufferHeight;
        private int _maxBufferWidth;
        public ConsoleCore() {
            SetConsoleSettings();
            _tab1 = new Tab(this, DriveInfo.GetDrives()[0].RootDirectory);
            _tab2 = new Tab(this, DriveInfo.GetDrives()[0].RootDirectory, true);
            _events = new Events(this);
        }
        public void Start() {
            CurrentTab = _tab2;
            _tab1.Draw();
            _tab2.Draw();
            _events.Subscribe(_tab2);
            _events.Selecter(_tab1.TabHeight);
        }
        public void TabHandler() {
            if(CurrentTab == _tab1) {
                _events.Unsubscribe(_tab1);
                CurrentTab = _tab2;
                _events.Subscribe(_tab2);
            }
            else {
                _events.Unsubscribe(_tab2);
                CurrentTab = _tab1;
                _events.Subscribe(_tab1);
            }
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
