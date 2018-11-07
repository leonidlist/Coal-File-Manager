using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TerminalEmulator {
    class Window {
        private ConsoleCore _core;
        private DirectoryInfo _currentDirectory;
        private ArrayList _currentWindowContains;
        private ConsoleColor _bgColor;
        private ConsoleColor _fontColor;
        private object _selectedItem;
        private int _selected;
        private int _scrollOffset;
        private int _maxClearPoint;
        public int Height { get; set; }
        public int Width { get; set; }
        private bool _isPanelOpened;

        public Window() {
            _currentDirectory = null;
            _currentWindowContains = null;
            _selectedItem = null;
            _core = null;
            _bgColor = ConsoleColor.DarkBlue;
            _fontColor = ConsoleColor.Yellow;
            _selected = _scrollOffset = _maxClearPoint = Height = Width = 0;
        }

        public Window(ConsoleCore core, DirectoryInfo currentDirectory) {
            _core = core;
            _bgColor = ConsoleColor.DarkBlue;
            _fontColor = ConsoleColor.Yellow;
            _currentDirectory = currentDirectory;
            _currentWindowContains = new ArrayList();
            _currentWindowContains.AddRange(_currentDirectory.GetDirectories());
            _currentWindowContains.AddRange(_currentDirectory.GetFiles());
            _selectedItem = _currentWindowContains[0];
            _selected = _scrollOffset = 0;
            Height = Console.BufferHeight - 5;
            Width = Console.BufferWidth;
            CalculateMaxClearPoint();
        }

        public Window(ConsoleCore core, ArrayList currentWindowContains, DirectoryInfo currentDirectory = null) {
            _core = core;
            _bgColor = ConsoleColor.DarkBlue;
            _fontColor = ConsoleColor.Yellow;
            _currentDirectory = currentDirectory;
            _currentWindowContains = currentWindowContains;
            _selectedItem = _currentWindowContains[0];
            _selected = _scrollOffset = 0;
            Height = Console.BufferHeight - 5;
            Width = Console.BufferWidth;
            CalculateMaxClearPoint();
        }

        public void DirectoryChangedEventHandler() {
            _currentWindowContains = new ArrayList();
            _currentWindowContains.AddRange(_currentDirectory.GetDirectories());
            _currentWindowContains.AddRange(_currentDirectory.GetFiles());
        }

        private void CalculateMaxClearPoint() {
            int max = 0;
            for (int i = 0; i < _currentWindowContains.Count; i++) {
                if (_currentWindowContains[i] is DirectoryInfo) {
                    if ((_currentWindowContains[i] as DirectoryInfo).Name.Length > max) {
                        max = (_currentWindowContains[i] as DirectoryInfo).Name.Length;
                        continue;
                    }
                }
                if (_currentWindowContains[i] is FileInfo) {
                    if ((_currentWindowContains[i] as FileInfo).Name.Length > max) {
                        max = (_currentWindowContains[i] as FileInfo).Name.Length;
                        continue;
                    }
                }
            }
            _maxClearPoint = max;
        }

        public void Draw() {
            Drawers.DrawBorder(Height, Width);
            Drawers.DrawCurrentDirectory(Width, _currentDirectory?.FullName);
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
            Drawers.DrawMenu(Height);
        }

        public void EscapeKeyPressedHandler() {
            if (_currentDirectory.Parent != null) {
                _scrollOffset = 0;
                _selected = 0;
                _currentDirectory = _currentDirectory.Parent;
                Events.CallDirectoryChanged();
                CalculateMaxClearPoint();
                Drawers.DrawCurrentDirectory(Width, _currentDirectory.FullName);
                Drawers.ClearMainWindow(Height, Width);
                Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
            }
        }
        public void F2KeyPressedHandler() {
            if (!_isPanelOpened) {
                Drawers.DrawAdditionalPanel(Height, Width);
                _isPanelOpened = true;
                if (_selectedItem is FileInfo) {
                    ShowFileInfo();
                }
                else if (_selectedItem is DirectoryInfo) {
                    ShowDirectoryInfo();
                }
                _isPanelOpened = true;
            }
            else {
                Drawers.ClearMainWindow(Height, Width);
                Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
                _isPanelOpened = false;
            }
        }
        public void ShowFileInfo() {
            FileInfo tmp = _selectedItem as FileInfo;
            Console.SetCursorPosition(Width - 58, 2);
            Console.Write($"File: {tmp.Name}");
            Console.SetCursorPosition(Width - 58, 3);
            Console.Write($"File size: {(double)(tmp.Length / 1000000)} MB");
            Console.SetCursorPosition(Width - 58, 4);
            Console.Write($"File creation time: {tmp.CreationTime}");
        }
        public void ShowDirectoryInfo() {
            DirectoryInfo tmp = _selectedItem as DirectoryInfo;
            Console.SetCursorPosition(Width - 58, 2);
            Console.Write($"Folder: {tmp.Name}");
            Console.SetCursorPosition(Width - 58, 3);
            Console.Write($"Folder creation time: {tmp.CreationTime}");
            Console.SetCursorPosition(Width - 58, 4);
            Console.Write($"Folders inside: {tmp.GetDirectories().Length}");
            Console.SetCursorPosition(Width - 58, 5);
            Console.Write($"Files inside: {tmp.GetFiles().Length}");
            Console.SetCursorPosition(Width - 58, 6);
            double totalSize = 0;
            for (int i = 0; i < tmp.GetFiles().Length - 1; i++) {
                totalSize += tmp.GetFiles()[i].Length;
            }
            Console.WriteLine($"Space usage(excluding folders): {(double)(totalSize / 1000000)} MB");
        }
        private void DeleteFile() {
            (_selectedItem as FileInfo).Delete();
            Drawers.ClearMainWindow(Height, Width);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
        }
        private void DeleteDirectory() {
            Console.Write("Warning! All files inside directory will be also deleted!");
            (_selectedItem as DirectoryInfo).Delete(true);
            System.Threading.Thread.Sleep(3000);
            Console.SetCursorPosition(2, Height + 1);
            Console.Write(" ".MultiplySpace(Width - 1));
            Drawers.ClearMainWindow(Height, Width);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
        }
        private void OpenTxtFile() {
            int canDraw = 58;
            using (StreamReader sr = new StreamReader(File.Open((_selectedItem as FileInfo).FullName, FileMode.Open), Encoding.Default)) {
                Drawers.DrawAdditionalPanel(Height, Width);
                string allText = sr.ReadToEnd();
                List<string> lines = new List<string>();
                for (int i = 0; i < allText.Length / canDraw; i++) {
                    lines.Add(allText.Substring(i * canDraw, canDraw));
                }
                for (int i = 0; i < lines.Count; i++) {
                    Console.SetCursorPosition(Width - 59, 2 + i);
                    Console.Write(lines[i]);
                }
            }
        }
        public void ArrowDownKeyPressedHandler() {
            _selected++;
            if (_selected > _currentWindowContains.Count - 1) {
                _selected = _currentWindowContains.Count - 1;
            }
            else if (_selected > Height - 3 + _scrollOffset && _selected < _currentWindowContains.Count) {
                _scrollOffset++;
                Drawers.ClearMainWindow(Height, Width, _maxClearPoint);
                Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
                return;
            }
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
        }
        public void ArrowUpKeyPressedHandler() {
            _selected--;
            if (_selected < 0) {
                _selected = 0;
                return;
            }
            if (_selected < _scrollOffset) {
                _scrollOffset--;
                Drawers.ClearMainWindow(Height, Width, _maxClearPoint);
                Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
                return;
            }
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
        }
        public void EnterKeyPressedHandler() {
            if (_selectedItem is DirectoryInfo) {
                try {
                    _selected = 0;
                    _scrollOffset = 0;
                    _isPanelOpened = false;
                    _currentDirectory = _selectedItem as DirectoryInfo;
                    Events.CallDirectoryChanged();
                    CalculateMaxClearPoint();
                    Drawers.DrawCurrentDirectory(Width, _currentDirectory.FullName);
                    Drawers.ClearMainWindow(Height, Width);
                    Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
                }
                catch (Exception) {
                    Console.SetCursorPosition(1, 1);
                    Console.Write("You have not permissions to access this folder...");
                    System.Threading.Thread.Sleep(3000);
                    _currentDirectory = _currentDirectory.Parent;
                    Drawers.DrawCurrentDirectory(Width, _currentDirectory.FullName);
                    Drawers.ClearMainWindow(Height, Width);
                    Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
                }
            }
            else if ((_selectedItem as FileInfo).Extension == ".txt") {
                OpenTxtFile();
            }
        }
        public void F3KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(Height, Width);
            _isPanelOpened = true;
            Console.SetCursorPosition(Width - 59, 4);
            Console.Write("Input target path to move > ");
            string movePath = Console.ReadLine();
            if (_selectedItem is FileInfo) {
                try {
                    (_selectedItem as FileInfo).MoveTo(movePath + (_selectedItem as FileInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(Width - 59, 5);
                    Console.Write(e.Message);
                }
            }
            else if (_selectedItem is DirectoryInfo) {
                try {
                    (_selectedItem as DirectoryInfo).MoveTo(movePath + (_selectedItem as DirectoryInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(Width - 59, 5);
                    Console.Write(e.Message);
                }
            }
            Drawers.ClearMainWindow(Height, Width);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F5KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(Height, Width);
            _isPanelOpened = true;
            Console.SetCursorPosition(Width - 59, 4);
            Console.Write("Input target path to copy > ");
            string copyPath = Console.ReadLine();
            if (_selectedItem is FileInfo) {
                try {
                    (_selectedItem as FileInfo).CopyTo(copyPath + (_selectedItem as FileInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(Width - 59, 5);
                    Console.Write(e.Message);
                }
            }
            else if (_selectedItem is DirectoryInfo) {
                //TODO: Создать способ копирования папок
            }
            Drawers.ClearMainWindow(Height, Width);
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F6KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(Height, Width);
            _isPanelOpened = true;
            Console.SetCursorPosition(Width - 59, 4);
            Console.Write("Input new directory path > ");
            string dirName = Console.ReadLine();
            Directory.CreateDirectory(dirName);
            Drawers.ClearMainWindow(Height, Width);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(ref _currentWindowContains, ref _selectedItem, _selected, Height, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F7KeyPressedHandler() {
            if (_selectedItem is FileInfo) {
                DeleteFile();
            }
            else if (_selectedItem is DirectoryInfo) {
                DeleteDirectory();
            }
        }
        public void F9KeyPressedHandler() {
            string[] searchParams = Drawers.DrawSearchMenu(Height, Width);
            SearchEngine.SearchByExpression(searchParams[0], new DirectoryInfo(searchParams[1]));
            Window resultWindow = new Window(_core,SearchEngine.GetSearchResult());
            _core.Events.Unsubscribe(this);
            _core.Events.Subscribe(resultWindow);
            resultWindow.Draw();
        }
    }
}
