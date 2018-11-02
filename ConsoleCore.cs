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
        ArrayList directoryContains;
        private event Action ArrowDownKeyPressed;
        private event Action ArrowUpKeyPressed;
        private event Action EscapeKeyPressed;
        private event Action EnterKeyPressed;
        private readonly int _maxBufferHeight;
        private readonly int _maxBufferWidth;
        private readonly int _mainWindowHeight;
        private readonly int _mainWindowWidth;
        private readonly int _menuWidth;
        private readonly int _menuHeight;
        private int _selected;
        private object _selectedItem;
        private int _scrollOffset = 0;
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
            ArrowUpKeyPressed += ArrowUpKeyPressedHandler;
            ArrowDownKeyPressed += ArrowDownKeyPressedHandler;
            EnterKeyPressed += EnterKeyPressedHandler;
            EscapeKeyPressed += EscapeKeyPressedHandler;
        }
        public void Start() {
            DrawBorder();
            DrawOnTopCurrentDirectory();
            DrawDirectoriesAndFiles();
            DrawDownMenu();
            Selecter();
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
            directoryContains = new ArrayList();
            directoryContains.AddRange(console.GetCurrentDirectory.GetDirectories());
            directoryContains.AddRange(console.GetCurrentDirectory.GetFiles());
            for(int i = 0+offset; i < _mainWindowHeight-2+offset && i < directoryContains.Count; i++) {
                Console.SetCursorPosition(2, i + 1 - offset);
                if(directoryContains[i] is DirectoryInfo) {
                    if(i == _selected) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        _selectedItem = directoryContains[i];
                    }
                    Console.Write((directoryContains[i] as DirectoryInfo).Name + "/");
                    Console.ResetColor();
                }
                else if(directoryContains[i] is FileInfo) {
                    if (i == _selected) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        _selectedItem = directoryContains[i];
                    }
                    Console.Write((directoryContains[i] as FileInfo).Name);
                    Console.ResetColor();
                }
            }
            #region comment
            //for (int i = 0; i < _mainWindowHeight-3; i++) {
            //    Console.SetCursorPosition(1, i+2);
            //    if (i+offset < dirs.Length) {
            //        if(i == _selected) {
            //            _selectedItem = dirs[i + offset];
            //            Console.BackgroundColor = ConsoleColor.White;
            //            Console.ForegroundColor = ConsoleColor.Black;
            //        }
            //        Console.WriteLine($"{dirs[i+offset].Name}/");
            //        Console.ResetColor();
            //    }
            //    else if (i - dirs.Length + offset < files.Length) {
            //        if (i == _selected) {
            //            Console.BackgroundColor = ConsoleColor.White;
            //            Console.ForegroundColor = ConsoleColor.Black;
            //        }
            //        Console.WriteLine($"{files[i-dirs.Length+offset].Name}");
            //        Console.ResetColor();
            //    }
            //}
            //Console.ResetColor();
            #endregion
        }
        private void ArrowDownKeyPressedHandler() {
            _selected++;
            if(_selected > directoryContains.Count)
                _selected = directoryContains.Count - 1;
            if(_selected > _mainWindowHeight-3) {
                _scrollOffset++;
                ClearMainWindow();
                DrawDirectoriesAndFiles(_scrollOffset);
                return;
            }
            DrawDirectoriesAndFiles(_scrollOffset);
        }
        private void ArrowUpKeyPressedHandler() {
            _selected--;
            if(_selected < 0)
                _selected = 0;
            if(_selected < _scrollOffset) {
                _scrollOffset--;
                ClearMainWindow();
                DrawDirectoriesAndFiles(_scrollOffset);
                return;
            }
            DrawDirectoriesAndFiles(_scrollOffset);
        }
        private void EnterKeyPressedHandler() {
            _selected = 0;
            console.CdCommand(new List<string> {
                $"{console.GetCurrentDirectory}/{((DirectoryInfo)_selectedItem).Name}"
            });
            DrawOnTopCurrentDirectory();
            ClearMainWindow();
            DrawDirectoriesAndFiles();
        }
        private void EscapeKeyPressedHandler() {
            _selected = 0;
            console.CdCommand(new List<string> {
                $"{console.GetCurrentDirectory}/.."
            });
            DrawOnTopCurrentDirectory();
            ClearMainWindow();
            DrawDirectoriesAndFiles();
        }
        private void Selecter() {
            while (true) {
                Console.SetCursorPosition(2, _mainWindowHeight + 1);
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if(keyInfo.Key == ConsoleKey.DownArrow) {
                    ArrowDownKeyPressed?.Invoke();
                }
                if(keyInfo.Key == ConsoleKey.UpArrow) {
                    ArrowUpKeyPressed?.Invoke();
                }
                if(keyInfo.Key == ConsoleKey.Enter) {
                    EnterKeyPressed?.Invoke();
                }
                if(keyInfo.Key == ConsoleKey.Escape) {
                    EscapeKeyPressed?.Invoke();
                }
            }
        }
        private void ClearMainWindow() {
            Console.BackgroundColor = ConsoleColor.Black;
            for(int i = 1; i < _mainWindowHeight-1; i++) {
                Console.SetCursorPosition(1, i);
                Console.Write(" ".MultiplySpace(_mainWindowWidth-3));
            }
        }
        private void DrawDownMenu() {
            DrawBlackBgForMenu();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(2, _mainWindowHeight + 3);
            Console.Write("F1 Help");
            Console.SetCursorPosition(12, _mainWindowHeight + 3);
            Console.Write("F2 UserMn");
            Console.ResetColor();
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
