using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TerminalEmulator {
    class Tab {
        private bool _isSecond = false;
        private CoalCore _core;
        private ArrayList _currentTabContains;
        private DirectoryInfo _currentDirectory;
        private object _selectedItem;
        private int _scrollMaxClearValue;
        private int _selectedIndex;
        private int _scrollOffset;
        public int TabHeight { get; set; }
        public int TabWidth { get; set; }

        public Tab() {
            _currentDirectory = null;
            _currentTabContains = null;
            _selectedItem = null;
            _core = null;
            _selectedIndex = _scrollOffset = TabHeight = TabWidth = 0;
        }

        public Tab(CoalCore core, DirectoryInfo currentDirectory, bool iS = false) {
            _isSecond = iS;
            _core = core;
            _currentDirectory = currentDirectory;
            _currentTabContains = new ArrayList();
            _currentTabContains.AddRange(currentDirectory.GetDirectories());
            _currentTabContains.AddRange(currentDirectory.GetFiles());
            _selectedItem = _currentTabContains[0];
            _selectedIndex = _scrollOffset = 0;
            TabHeight = Console.BufferHeight - 5;
            TabWidth = Console.BufferWidth/2;
            CalculateMax();
        }

        public void CalculateMax() {
            int max = 0;
            for (int i = 0; i < _currentTabContains.Count; i++) {
                if (_currentTabContains[i] is FileInfo) {
                    if ((_currentTabContains[i] as FileInfo).Name.Length > max) {
                        max = (_currentTabContains[i] as FileInfo).Name.Length;
                    }
                }
                else {
                    if ((_currentTabContains[i] as DirectoryInfo).Name.Length > max) {
                        max = (_currentTabContains[i] as DirectoryInfo).Name.Length;
                    }
                }
            }
            _scrollMaxClearValue = max + 1;
        }

        public void DirectoryChangedEventHandler() {
            _currentTabContains.Clear();
            _currentTabContains.AddRange(_currentDirectory.GetDirectories());
            _currentTabContains.AddRange(_currentDirectory.GetFiles());
        }

        public void Draw() {
            Drawers.DrawBorder(TabHeight, TabWidth, _isSecond);
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, 0, _isSecond);
            Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory?.FullName, _isSecond);
            if (!_isSecond)
                Drawers.DrawMenu(TabHeight);
        }

        public void EscapeKeyPressedHandler() {
            if (_currentDirectory?.Parent != null) {
                _scrollOffset = 0;
                _selectedIndex = 0;
                _currentDirectory = _currentDirectory.Parent;
                Events.CallDirectoryChanged();
                Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName, _isSecond);
                Drawers.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                CalculateMax();
            }
            else if(_currentDirectory == null) {
                _scrollOffset = 0;
                _selectedIndex = 0;
                _currentDirectory = DriveInfo.GetDrives()[0].RootDirectory;
                Events.CallDirectoryChanged();
                Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName, _isSecond);
                Drawers.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                CalculateMax();
            }
        }

        public void ArrowDownKeyPressedHandler() {
            _selectedIndex++;
            if (_selectedIndex > _currentTabContains.Count - 1) {
                _selectedIndex = _currentTabContains.Count - 1;
            }
            else if (_selectedIndex > TabHeight - 3 + _scrollOffset && _selectedIndex < _currentTabContains.Count) {
                _scrollOffset++;
                Drawers.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                return;
            }
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
        }

        public void ArrowUpKeyPressedHandler() {
            _selectedIndex--;
            if (_selectedIndex < 0) {
                _selectedIndex = 0;
                return;
            }
            if (_selectedIndex < _scrollOffset) {
                _scrollOffset--;
                Drawers.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue,_isSecond);
                Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                return;
            }
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
        }
        public void F2KeyPressedHandler() {
            bool isPressed = false;
            Drawers.DrawInfoMenu(TabHeight, TabWidth*2, _selectedItem);
            while(!isPressed) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if(key.Key == ConsoleKey.Escape) {
                    isPressed = true;
                    _core.DrawBothTabs();
                }
            }
        }
        private void DeleteFile() {
            (_selectedItem as FileInfo).Delete();
            Events.CallDirectoryChanged();
            _core.NonActiveTab.DirectoryChangedEventHandler();
            _core.DrawBothTabs();
        }
        private void DeleteDirectory() {
            bool res = Drawers.DrawConfirmationMenu(TabHeight, TabWidth * 2, "Warning! All files inside directory will be also deleted!");
            if(res) {
                (_selectedItem as DirectoryInfo).Delete(true);
                Events.CallDirectoryChanged();
                _core.NonActiveTab.DirectoryChangedEventHandler();
            }
            _core.DrawBothTabs();
        }
        private void OpenTxtFile() {
            int canDraw = 58;
            using (StreamReader sr = new StreamReader(File.Open((_selectedItem as FileInfo).FullName, FileMode.Open), Encoding.Default)) {
                
                string allText = sr.ReadToEnd();
                List<string> lines = new List<string>();
                for (int i = 0; i < allText.Length / canDraw; i++) {
                    lines.Add(allText.Substring(i * canDraw, canDraw));
                }
                for (int i = 0; i < lines.Count; i++) {
                    Console.SetCursorPosition(TabWidth - 59, 2 + i);
                    Console.Write(lines[i]);
                }
            }
        }
        public void EnterKeyPressedHandler() {
            if (_selectedItem is DirectoryInfo) {
                try {
                    _selectedIndex = 0;
                    _scrollOffset = 0;
                    _currentDirectory = _selectedItem as DirectoryInfo;
                    Events.CallDirectoryChanged();
                    Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName, _isSecond);
                    Drawers.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                    Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                    CalculateMax();
                }
                catch (Exception) {
                    bool isPressed = false;
                    Drawers.DrawError(TabHeight, TabWidth*2, "You have no permissions to enter this folder");
                    Console.SetCursorPosition(2, TabHeight + 1);
                    while (!isPressed) {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter) {
                            isPressed = true;
                        }
                    }
                    if (_currentDirectory.Parent != null) {
                        _currentDirectory = _currentDirectory.Parent;
                    }
                    else {
                        _currentDirectory = DriveInfo.GetDrives()[0].RootDirectory;
                    }
                    Events.CallDirectoryChanged();
                    _core.DrawBothTabs();
                }
            }
        }
        public void F3KeyPressedHandler() {
            Console.SetCursorPosition(TabWidth - 59, 4);
            Console.Write("Input target path to move > ");
            string movePath = Console.ReadLine();
            if (_selectedItem is FileInfo) {
                try {
                    (_selectedItem as FileInfo).MoveTo(movePath + (_selectedItem as FileInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(TabWidth - 59, 5);
                    Console.Write(e.Message);
                }
            }
            else if (_selectedItem is DirectoryInfo) {
                try {
                    (_selectedItem as DirectoryInfo).MoveTo(movePath + (_selectedItem as DirectoryInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(TabWidth - 59, 5);
                    Console.Write(e.Message);
                }
            }
            Drawers.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
        }
        public void F5KeyPressedHandler() {
            Console.SetCursorPosition(TabWidth - 59, 4);
            Console.Write("Input target path to copy > ");
            string copyPath = Console.ReadLine();
            if (_selectedItem is FileInfo) {
                try {
                    (_selectedItem as FileInfo).CopyTo(copyPath + (_selectedItem as FileInfo).Name);
                }
                catch (Exception e) {
                    Console.SetCursorPosition(TabWidth - 59, 5);
                    Console.Write(e.Message);
                }
            }
            else if (_selectedItem is DirectoryInfo) {
                //TODO: Создать способ копирования папок
            }
            Drawers.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
        }
        public void F6KeyPressedHandler() {
            Directory.CreateDirectory(Drawers.DrawMkDirMenu(TabHeight, TabWidth*2));
            Events.CallDirectoryChanged();
            _core.NonActiveTab.DirectoryChangedEventHandler();
            _core.DrawBothTabs();
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
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
            SearchEngine.ClearSearchResult();
            string[] searchParams = Drawers.DrawSearchMenu(TabHeight, TabWidth*2);
            SearchEngine.SearchByExpression(searchParams[0], new DirectoryInfo(searchParams[1]));
            _currentTabContains = SearchEngine.GetSearchResult();
            CalculateMax();
            _currentDirectory = null;
            _core.DrawBothTabs();
        }
    }
}
