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
        MyConsole console;
        private Events _events;
        private int _maxBufferHeight;
        private int _maxBufferWidth;
        private int _mainWindowHeight;
        private int _mainWindowWidth;
        private int _selected;
        private object _selectedItem;
        private int _scrollOffset = 0;
        private int _maxClearPoint;
        bool _isPanelOpened = false;
        public ConsoleCore() {
            _selected = 0;
            console = new MyConsole();
            _events = new Events(this);
            SetConsoleSettings();
        }
        public void Start() {
            Drawers.DrawBorder(_mainWindowHeight, _mainWindowWidth);
            Drawers.DrawCurrentDirectory(_mainWindowWidth, console.GetCurrentDirectory.FullName);
            Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
            Drawers.DrawMenu(_mainWindowHeight);
            _events.Selecter(_mainWindowHeight);
        }
        private void CalculateMaxClearPoint() {
            int max = 0;
            for(int i = 0; i < directoryContains.Count; i++) {
                if(directoryContains[i] is DirectoryInfo) {
                    if((directoryContains[i] as DirectoryInfo).Name.Length > max) {
                        max = (directoryContains[i] as DirectoryInfo).Name.Length;
                        continue;
                    }
                }
                if (directoryContains[i] is FileInfo) {
                    if ((directoryContains[i] as FileInfo).Name.Length > max) {
                        max = (directoryContains[i] as FileInfo).Name.Length;
                        continue;
                    }
                }
            }
            _maxClearPoint = max;
        }
        private void SetConsoleSettings() {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.SetWindowPosition(0,0);
            _maxBufferHeight = Console.BufferHeight - 1;
            _maxBufferWidth = Console.BufferWidth;
            _mainWindowHeight = _maxBufferHeight - 4;
            _mainWindowWidth = _maxBufferWidth;
        }
        public void ArrowDownKeyPressedHandler() {
            _selected++;
            if(_selected > directoryContains.Count-1) {
                _selected = directoryContains.Count-1;
            }
            else if(_selected > _mainWindowHeight-3+_scrollOffset && _selected < directoryContains.Count) {
                _scrollOffset++;
                CalculateMaxClearPoint();
                Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth, _maxClearPoint);
                Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
                return;
            }
            Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
        }
        public void ArrowUpKeyPressedHandler() {
            _selected--;
            if(_selected < 0) {
                _selected = 0;
                return;
            }
            if(_selected < _scrollOffset) {
                _scrollOffset--;
                CalculateMaxClearPoint();
                Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth, _maxClearPoint);
                Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
                return;
            }
            Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
        }
        public void EnterKeyPressedHandler() {
            if(_selectedItem is DirectoryInfo) {
                _selected = 0;
                _scrollOffset = 0;
                _isPanelOpened = false;
                console.CdCommand(new List<string> {
                    $"{console.GetCurrentDirectory}/{((DirectoryInfo)_selectedItem).Name}"
                });
                Drawers.DrawCurrentDirectory(_mainWindowWidth, console.GetCurrentDirectory.FullName);
                Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
                Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
            }
            else {
                if ((_selectedItem as FileInfo).Extension == ".txt") {
                    int canDraw = 58;
                    using (StreamReader sr = new StreamReader(File.Open((_selectedItem as FileInfo).FullName, FileMode.Open), System.Text.Encoding.Default)) {
                        Drawers.DrawAdditionalPanel(_mainWindowHeight, _mainWindowWidth);
                        string allText = sr.ReadToEnd();
                        List<string> lines = new List<string>();
                        for(int i = 0; i < allText.Length/canDraw; i++) {
                            lines.Add(allText.Substring(i * canDraw, canDraw));
                        }
                        for(int i = 0; i < lines.Count; i++) {
                            Console.SetCursorPosition(_mainWindowWidth - 59, 2+i);
                            Console.Write(lines[i]);
                        }
                    }
                }
            }
        }
        public void EscapeKeyPressedHandler() {
            _scrollOffset = 0;
            _selected = 0;
            console.CdCommand(new List<string> {
                $"{console.GetCurrentDirectory}/.."
            });
            Drawers.DrawCurrentDirectory(_mainWindowWidth, console.GetCurrentDirectory.FullName);
            Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
            Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
        }
        public void F2KeyPressedHandler() {
            if(!_isPanelOpened) {
                Drawers.DrawAdditionalPanel(_mainWindowHeight, _mainWindowWidth);
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
                Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
                Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
                _isPanelOpened = false;
            }
        }
        public void F3KeyPressHandler() {
            Drawers.DrawAdditionalPanel(_mainWindowHeight, _mainWindowWidth);
            _isPanelOpened = true;
            Console.SetCursorPosition(_mainWindowWidth - 59, 4);
            Console.Write("Input target path to move > ");
            string movePath = Console.ReadLine();
            if (_selectedItem is FileInfo) {
                try {
                    (_selectedItem as FileInfo).MoveTo(movePath + (_selectedItem as FileInfo).Name);
                } catch (Exception e) {
                    Console.SetCursorPosition(_mainWindowWidth - 59, 5);
                    Console.Write(e.Message);
                }
            }
            else if(_selectedItem is DirectoryInfo) {
                try {
                    (_selectedItem as DirectoryInfo).MoveTo(movePath + (_selectedItem as DirectoryInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(_mainWindowWidth - 59, 5);
                    Console.Write(e.Message);
                }
            }
            Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
            Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F5KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(_mainWindowHeight, _mainWindowWidth);
            _isPanelOpened = true;
            Console.SetCursorPosition(_mainWindowWidth - 59, 4);
            Console.Write("Input target path to copy > ");
            string copyPath = Console.ReadLine();
            if (_selectedItem is FileInfo) {
                try {
                    (_selectedItem as FileInfo).CopyTo(copyPath + (_selectedItem as FileInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(_mainWindowWidth - 59, 5);
                    Console.Write(e.Message);
                }
            }
            else if(_selectedItem is DirectoryInfo) {
                //TODO: Создать способ копирования папок
            }
            Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
            Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F6KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(_mainWindowHeight, _mainWindowWidth);
            _isPanelOpened = true;
            Console.SetCursorPosition(_mainWindowWidth - 59, 4);
            Console.Write("Input new directory name or path > ");
            string dirName = Console.ReadLine();
            console.MkDirCommand(new List<string>() {
                dirName
            });
            Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
            Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F7KeyPressedHandler() {
            if(_selectedItem is FileInfo) {
                (_selectedItem as FileInfo).Delete();
                Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
                Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
            }
            if(_selectedItem is DirectoryInfo) {
                Console.Write("Warning! All files inside directory will be also deleted!");
                (_selectedItem as DirectoryInfo).Delete(true);
                System.Threading.Thread.Sleep(3000);
                Console.SetCursorPosition(2, _mainWindowHeight + 1);
                Console.Write(" ".MultiplySpace(_mainWindowWidth - 1));
                Drawers.ClearMainWindow(_mainWindowHeight, _mainWindowWidth);
                Drawers.DrawDirectoriesAndFiles(console, ref directoryContains, ref _selectedItem, _selected, _mainWindowHeight, _scrollOffset);
            }
        }
        public void F9KeyPressedHandler() {
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
