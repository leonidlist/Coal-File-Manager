using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TerminalEmulator {
    sealed class ConsoleCore {
        private readonly int _maxBufferHeight;
        private readonly int _maxBufferWidth;
        private readonly int _mainWindowHeight;
        private readonly int _mainWindowWidth;
        private readonly int _menuWidth;
        private readonly int _menuHeight;
        private int _selected;
        private object _selectedItem;
        MyConsole console;
        public ConsoleCore() {
            _selected = 0;
            console = new MyConsole();
            SetConsoleSettings();
            _maxBufferHeight = Console.BufferHeight - 1;
            _maxBufferWidth = Console.BufferWidth;
            _mainWindowHeight = _maxBufferHeight - 4;
            _mainWindowWidth = _maxBufferWidth;
            _menuHeight = _maxBufferHeight - _mainWindowHeight;
            _menuWidth = _maxBufferWidth;
        }
        public void Start() {
            DrawBorder();
            DrawOnTopCurrentDirectory();
            DrawDirectoriesAndFiles();
            Selecter();
            //DrawDownMenu();
        }
        private void SetConsoleSettings() {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.SetWindowPosition(0,0);
        }
        private void DrawOnTopCurrentDirectory() {
            DrawTopBorder();
            Console.SetCursorPosition(5, 0);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(console.GetCurrentDirectory.FullName);
            Console.ResetColor();
        }
        private void DrawBlackBgForMenu() {
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = _mainWindowHeight; i < _maxBufferHeight; i++) {
                Console.SetCursorPosition(0, i);
                Console.Write(" ".MultiplySpace(_maxBufferWidth));
            }
            Console.ResetColor();
        }
        private void DrawDirectoriesAndFiles(int offset = 0) {
            DirectoryInfo[] dirs = console.GetCurrentDirectory.GetDirectories();
            FileInfo[] files = console.GetCurrentDirectory.GetFiles();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 2; i < _mainWindowHeight-1; i++) {
                if(i-2 == _selected) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if(_selected < dirs.Length) {
                        _selectedItem = dirs[i - 2];
                    }
                    else {
                        _selectedItem = files[i - 2 - dirs.Length];
                    }
                }
                Console.SetCursorPosition(1, i);
                if(i-2 < dirs.Length) {
                    Console.WriteLine($"{dirs[i-2].Name}/");
                }
                else if (i -dirs.Length-2 < files.Length) {
                    Console.WriteLine($"{files[i-dirs.Length-2].Name}");
                }
                if (i - 2 == _selected)
                    Console.ResetColor();
            }
            Console.ResetColor();
        }
        public void Selecter() {
            while (true) {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if(keyInfo.Key == ConsoleKey.DownArrow) {
                    _selected++;
                    DrawDirectoriesAndFiles();
                }
                if(keyInfo.Key == ConsoleKey.UpArrow) {
                    _selected--;
                    DrawDirectoriesAndFiles();
                }
                if(keyInfo.Key == ConsoleKey.Enter) {
                    _selected = 0;
                    console.CdCommand(new List<string> {
                        $"{console.GetCurrentDirectory}/{((DirectoryInfo)_selectedItem).Name}"
                    });
                    DrawTopBorder();
                    DrawOnTopCurrentDirectory();
                    ClearMainWindow();
                    DrawDirectoriesAndFiles();
                }
                if(keyInfo.Key == ConsoleKey.Escape) {
                    _selected = 0;
                    console.CdCommand(new List<string> {
                        $"{console.GetCurrentDirectory}/.."
                    });
                    DrawTopBorder();
                    DrawOnTopCurrentDirectory();
                    ClearMainWindow();
                    DrawDirectoriesAndFiles();
                }
            }
        }
        private void ClearMainWindow() {
            Console.BackgroundColor = ConsoleColor.Black;
            for(int i = 1; i < _mainWindowHeight-1; i++) {
                Console.SetCursorPosition(1, i);
                Console.Write(" ".MultiplySpace(_mainWindowWidth-2));
            }
        }
        private void TryScroll() {
            ConsoleKeyInfo keyPressed = Console.ReadKey();
            if(keyPressed.Key == ConsoleKey.Enter) {

            }
        }
        private void DrawDownMenu() {
            while (true) {
                DrawOnTopCurrentDirectory();
                DrawBlackBgForMenu();
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, _mainWindowHeight);
                Console.Write($"{console.GetCurrentDirectory.FullName}> ");
                string command = Console.ReadLine();
                string[] pars = command.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                List<string> give = pars.ToList();
                give.RemoveAt(0);
                if (pars[0] == "cd") {
                    if (console.CdCommand(give)) {
                        ClearMainWindow();
                        DrawDirectoriesAndFiles();
                    }
                }
            }
        }
        private void DrawTopBorder() {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for(int i = 0; i < _mainWindowWidth; i++) {
                Console.SetCursorPosition(i, 0);
                if (i == 0) {
                    Console.WriteLine("╔");
                }
                else if (i == _mainWindowWidth - 1) {
                    Console.WriteLine("╗");
                }
                else {
                    Console.WriteLine("═");
                }
            }
            Console.ResetColor();
        }
        private void DrawBorder() {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < _mainWindowHeight; i++) {
                for (int j = 0; j < _mainWindowWidth; j++) {
                    Console.SetCursorPosition(j, i);
                    if (i == 0) {
                        if (j == 0) {
                            Console.WriteLine("╔");
                        }
                        else if (j == _mainWindowWidth - 1) {
                            Console.WriteLine("╗");
                        }
                        else {
                            Console.WriteLine("═");
                        }
                    }
                    else if (i == _mainWindowHeight - 1) {
                        if (j == 0) {
                            Console.WriteLine("╚");
                        }
                        else if (j == _mainWindowWidth - 1) {
                            Console.WriteLine("╝");
                        }
                        else {
                            Console.WriteLine("═");
                        }
                    }
                    else {
                        if (j == 0 || j == _mainWindowWidth-1) {
                            Console.WriteLine("║");
                        }
                        else {
                            Console.WriteLine(" ");
                        }
                    }
                }
            }
            Console.ResetColor();
        }

        ~ConsoleCore() {
            Console.ResetColor();
        }
    }
}
