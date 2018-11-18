using System;
using System.IO;
using Coal.Draw;
using System.Threading;

namespace Coal {
    sealed class CoalCore {
        private Events _events;
        public Events Events {
            get => _events;
        }
        private Tab _tab1;
        private Tab _tab2;
        public Tab CurrentTab { get; set; }
        public Tab NonActiveTab { get; set; }
        private int _maxBufferHeight;
        private int _maxBufferWidth;
        public CoalCore() {
            SetConsoleSettings();
            _tab1 = new Tab(this, DriveInfo.GetDrives()[0].RootDirectory);
            _tab2 = new Tab(this, DriveInfo.GetDrives()[0].RootDirectory, true);
            _events = new Events(this);
        }
        public void DrawBothTabs() {
            _tab1.Draw();
            _tab2.Draw();
        }
        public void Start() {
            CurrentTab = _tab1;
            NonActiveTab = _tab2;
            Clear.ClearBottom(_tab1.TabHeight, _maxBufferWidth);
            _tab1.Draw();
            _tab2.Draw();
            _events.Subscribe(_tab1);
            Clock.Width = _maxBufferWidth;
            Thread clockThread = new Thread(new ThreadStart(Clock.Execute));
            clockThread.Start();
            _events.Selecter(_tab1.TabHeight);
        }
        public void TabHandler() {
            if(CurrentTab == _tab1) {
                _events.Unsubscribe(_tab1);
                CurrentTab = _tab2;
                Directories.DrawCurrentDirectory(_tab2.TabWidth, _tab2?.CurrentDirectory?.FullName, true, true);
                NonActiveTab = _tab1;
                Directories.DrawCurrentDirectory(_tab1.TabWidth, _tab1?.CurrentDirectory?.FullName, false, false);
                _events.Subscribe(_tab2);
            }
            else {
                _events.Unsubscribe(_tab2);
                CurrentTab = _tab1;
                Directories.DrawCurrentDirectory(_tab1.TabWidth, _tab1?.CurrentDirectory?.FullName, false, true);
                NonActiveTab = _tab2;
                Directories.DrawCurrentDirectory(_tab2.TabWidth, _tab2?.CurrentDirectory?.FullName, true, false);
                _events.Subscribe(_tab1);
            }
        }
        private void SetConsoleSettings() {
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.SetWindowPosition(0,0);
            _maxBufferHeight = Console.BufferHeight - 1;
            _maxBufferWidth = Console.BufferWidth;
            Console.CursorVisible = false;
        }
        public void F10KeyPressedHandler() {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Environment.Exit(0);
        }
        ~CoalCore() {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
