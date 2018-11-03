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
        private event Action F2KeyPressed;
        private event Action F5KeyPressed;
        private event Action F7KeyPressed;
        private event Action F9KeyPressed;
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
        bool _isPanelOpened = false;
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
            F2KeyPressed += F2KeyPressedHandler;
            F5KeyPressed += F5KeyPressedHandler;
            F7KeyPressed += F7KeyPressedHandler;
            F9KeyPressed += F9KeyPressedHandler;
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
        }
        private void ArrowDownKeyPressedHandler() {
            _selected++;
            if(_selected > directoryContains.Count-1) {
                _selected = directoryContains.Count-1;
            }
            else if(_selected > _mainWindowHeight-3+_scrollOffset && _selected < directoryContains.Count) {
                _scrollOffset++;
                ClearMainWindow();
                DrawDirectoriesAndFiles(_scrollOffset);
                return;
            }
            DrawDirectoriesAndFiles(_scrollOffset);
        }
        private void ArrowUpKeyPressedHandler() {
            _selected--;
            if(_selected < 0) {
                _selected = 0;
                return;
            }
            if(_selected < _scrollOffset) {
                _scrollOffset--;
                ClearMainWindow();
                DrawDirectoriesAndFiles(_scrollOffset);
                return;
            }
            DrawDirectoriesAndFiles(_scrollOffset);
        }
        private void EnterKeyPressedHandler() {
            if(_selectedItem is DirectoryInfo) {
                _selected = 0;
                _scrollOffset = 0;
                _isPanelOpened = false;
                console.CdCommand(new List<string> {
                    $"{console.GetCurrentDirectory}/{((DirectoryInfo)_selectedItem).Name}"
                });
                DrawOnTopCurrentDirectory();
                ClearMainWindow();
                DrawDirectoriesAndFiles(_scrollOffset);
            }
            else {

            }
        }
        private void EscapeKeyPressedHandler() {
            _scrollOffset = 0;
            _selected = 0;
            console.CdCommand(new List<string> {
                $"{console.GetCurrentDirectory}/.."
            });
            DrawOnTopCurrentDirectory();
            ClearMainWindow();
            DrawDirectoriesAndFiles();
        }
        private void F2KeyPressedHandler() {
            if(!_isPanelOpened) {
                DrawAdditionalPanel();
                _isPanelOpened = true;
                if (_selectedItem is FileInfo) {
                    FileInfo tmp = _selectedItem as FileInfo;
                    Console.SetCursorPosition(_mainWindowWidth - 58, 2);
                    Console.Write($"File: {tmp.Name}");
                    Console.SetCursorPosition(_mainWindowWidth - 58, 3);
                    Console.Write($"File size: {(double)(tmp.Length / 1000000)} MB");
                    Console.SetCursorPosition(_mainWindowWidth - 58, 4);
                    Console.Write($"File creation time: {tmp.CreationTime}");
                }
                else if (_selectedItem is DirectoryInfo) {
                    DirectoryInfo tmp = _selectedItem as DirectoryInfo;
                    Console.SetCursorPosition(_mainWindowWidth - 58, 2);
                    Console.Write($"Folder: {tmp.Name}");
                    Console.SetCursorPosition(_mainWindowWidth - 58, 3);
                    Console.Write($"Folder creation time: {tmp.CreationTime}");
                    Console.SetCursorPosition(_mainWindowWidth - 58, 4);
                    Console.Write($"Folders inside: {tmp.GetDirectories().Length}");
                    Console.SetCursorPosition(_mainWindowWidth - 58, 5);
                    Console.Write($"Files inside: {tmp.GetFiles().Length}");
                    Console.SetCursorPosition(_mainWindowWidth - 58, 6);
                    double totalSize = 0;
                    for (int i = 0; i < tmp.GetFiles().Length - 1; i++) {
                        totalSize += tmp.GetFiles()[i].Length;
                    }
                    Console.WriteLine($"Space usage(excluding folders): {(double)(totalSize / 1000000)} MB");
                }
                _isPanelOpened = true;
            }
            else {
                ClearMainWindow();
                DrawDirectoriesAndFiles(_scrollOffset);
                _isPanelOpened = false;
            }
        }
        private void F5KeyPressedHandler() {
            if(_selectedItem is FileInfo) {
                Console.SetCursorPosition(2, _mainWindowHeight + 1);
                Console.Write("Input target path to copy > ");
                string copyPath = Console.ReadLine();
                console.CpCommand(new List<string> {
                    "\\" + (_selectedItem as FileInfo).Name,
                    copyPath
                });
                Console.SetCursorPosition(2, _mainWindowHeight + 1);
                Console.Write(" ".MultiplySpace(_mainWindowWidth - 1));
            }
        }
        private void F7KeyPressedHandler() {
            if(_selectedItem is FileInfo) {
                (_selectedItem as FileInfo).Delete();
                ClearMainWindow();
                DrawDirectoriesAndFiles(_scrollOffset);
            }
        }
        private void F9KeyPressedHandler() {
            Console.Clear();
            Environment.Exit(0);
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
                if(keyInfo.Key == ConsoleKey.F2) {
                    F2KeyPressed?.Invoke();
                }
                if(keyInfo.Key == ConsoleKey.F5) {
                    F5KeyPressed?.Invoke();
                }
                if(keyInfo.Key == ConsoleKey.F7) {
                    F7KeyPressed?.Invoke();
                }
                if(keyInfo.Key == ConsoleKey.F9) {
                    F9KeyPressed?.Invoke();
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
        private void DrawAdditionalPanel() {
            for(int i = 0; i < _mainWindowHeight-2; i++) {
                Console.SetCursorPosition(_mainWindowWidth - 60, i+1);
                Console.Write("│");
            }
        }
        //private void DrawInfoLine() {
        //    Console.SetCursorPosition(2, _mainWindowHeight + 2);
        //    Console.Write(" ".MultiplySpace(_mainWindowWidth));
        //    Console.SetCursorPosition(2, _mainWindowHeight + 2);
        //    if (_selectedItem is FileInfo) {
        //        Console.Write((_selectedItem as FileInfo).Name);
        //    }
        //    if (_selectedItem is FileInfo) {
        //        Console.Write($"    {(double)((_selectedItem as FileInfo).Length / 1000000)} MB  {(_selectedItem as FileInfo).CreationTime}");
        //    }
        //}
        private void DrawDownMenu() {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(2, _mainWindowHeight + 3);
            Console.Write("F1 Help");
            Console.SetCursorPosition(12, _mainWindowHeight + 3);
            Console.Write("F2 SelInfo");
            Console.SetCursorPosition(25, _mainWindowHeight + 3);
            Console.Write("F3 Move");
            Console.SetCursorPosition(35, _mainWindowHeight + 3);
            Console.Write("F4 Edit");
            Console.SetCursorPosition(45, _mainWindowHeight + 3);
            Console.Write("F5 Copy");
            Console.SetCursorPosition(55, _mainWindowHeight + 3);
            Console.Write("F6 MkFold");
            Console.SetCursorPosition(67, _mainWindowHeight + 3);
            Console.Write("F7 Delete");
            Console.SetCursorPosition(79, _mainWindowHeight + 3);
            Console.Write("F8 MkFile");
            Console.SetCursorPosition(91, _mainWindowHeight + 3);
            Console.Write("F9 Quit");
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
