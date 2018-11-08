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
        private ConsoleCore _core;
        private ArrayList _currentTabContains;
        private DirectoryInfo _currentDirectory;
        private object _selectedItem;
        private int _scrollMaxClearValue;
        private int _selectedIndex;
        private int _scrollOffset;
        public int TabHeight { get; set; }
        public int TabWidth { get; set; }
        private bool _isPanelOpened = false;

        public Tab() {
            _currentDirectory = null;
            _currentTabContains = null;
            _selectedItem = null;
            _core = null;
            _selectedIndex = _scrollOffset = TabHeight = TabWidth = 0;
        }

        public Tab(ConsoleCore core, DirectoryInfo currentDirectory, bool iS = false) {
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
        }

        public void DirectoryChangedEventHandler() {
            int max = 0;
            _currentTabContains.Clear();
            _currentTabContains.AddRange(_currentDirectory.GetDirectories());
            _currentTabContains.AddRange(_currentDirectory.GetFiles());
            for(int i = 0; i < _currentTabContains.Count; i++) {
                if(_currentTabContains[i] is FileInfo) {
                    if((_currentTabContains[i] as FileInfo).Name.Length > max) {
                        max = (_currentTabContains[i] as FileInfo).Name.Length;
                    }
                }
                else {
                    if ((_currentTabContains[i] as DirectoryInfo).Name.Length > max) {
                        max = (_currentTabContains[i] as DirectoryInfo).Name.Length;
                    }
                }
            }
            _scrollMaxClearValue = max;
        }

        public void Draw() {
            Drawers.DrawBorder(TabHeight, TabWidth, _isSecond);
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, 0, _isSecond);
            Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory?.FullName, _isSecond);
            if (!_isSecond)
                Drawers.DrawMenu(TabHeight);
        }

        public void EscapeKeyPressedHandler() {
            if (_currentDirectory.Parent != null) {
                _scrollOffset = 0;
                _selectedIndex = 0;
                _currentDirectory = _currentDirectory.Parent;
                Events.CallDirectoryChanged();
                Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName);
                Drawers.ClearTab(TabHeight, TabWidth, _isSecond);
                Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
            }
        }

        public void ArrowDownKeyPressedHandler() {
            _selectedIndex++;
            if (_selectedIndex > _currentTabContains.Count - 1) {
                _selectedIndex = _currentTabContains.Count - 1;
            }
            else if (_selectedIndex > TabHeight - 3 + _scrollOffset && _selectedIndex < _currentTabContains.Count) {
                _scrollOffset++;
                Drawers.ClearTab(TabHeight, TabWidth, _isSecond);
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
                Drawers.ClearTab(TabHeight, TabWidth, _isSecond);
                Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                return;
            }
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
        }
        public void F2KeyPressedHandler() {
            if (!_isPanelOpened) {
                Drawers.DrawAdditionalPanel(TabHeight, TabWidth);
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
                Drawers.ClearTab(TabHeight, TabWidth);
                Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                _isPanelOpened = false;
            }
        }
        public void ShowFileInfo() {
            FileInfo tmp = _selectedItem as FileInfo;
            Console.SetCursorPosition(TabWidth - 58, 2);
            Console.Write($"File: {tmp.Name}");
            Console.SetCursorPosition(TabWidth - 58, 3);
            Console.Write($"File size: {(double)(tmp.Length / 1000000)} MB");
            Console.SetCursorPosition(TabWidth - 58, 4);
            Console.Write($"File creation time: {tmp.CreationTime}");
        }
        public void ShowDirectoryInfo() {
            DirectoryInfo tmp = _selectedItem as DirectoryInfo;
            Console.SetCursorPosition(TabWidth - 58, 2);
            Console.Write($"Folder: {tmp.Name}");
            Console.SetCursorPosition(TabWidth - 58, 3);
            Console.Write($"Folder creation time: {tmp.CreationTime}");
            Console.SetCursorPosition(TabWidth - 58, 4);
            Console.Write($"Folders inside: {tmp.GetDirectories().Length}");
            Console.SetCursorPosition(TabWidth - 58, 5);
            Console.Write($"Files inside: {tmp.GetFiles().Length}");
            Console.SetCursorPosition(TabWidth - 58, 6);
            double totalSize = 0;
            for (int i = 0; i < tmp.GetFiles().Length - 1; i++) {
                totalSize += tmp.GetFiles()[i].Length;
            }
            Console.WriteLine($"Space usage(excluding folders): {(double)(totalSize / 1000000)} MB");
        }
        private void DeleteFile() {
            (_selectedItem as FileInfo).Delete();
            Drawers.ClearTab(TabHeight, TabWidth);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
        }
        private void DeleteDirectory() {
            Console.Write("Warning! All files inside directory will be also deleted!");
            (_selectedItem as DirectoryInfo).Delete(true);
            System.Threading.Thread.Sleep(3000);
            Console.SetCursorPosition(2, TabHeight + 1);
            Console.Write(" ".MultiplySpace(TabWidth - 1));
            Drawers.ClearTab(TabHeight, TabWidth);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
        }
        private void OpenTxtFile() {
            int canDraw = 58;
            using (StreamReader sr = new StreamReader(File.Open((_selectedItem as FileInfo).FullName, FileMode.Open), Encoding.Default)) {
                Drawers.DrawAdditionalPanel(TabHeight, TabWidth);
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
                    _isPanelOpened = false;
                    _currentDirectory = _selectedItem as DirectoryInfo;
                    Events.CallDirectoryChanged();
                    Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName, _isSecond);
                    Drawers.ClearTab(TabHeight, TabWidth, _isSecond);
                    Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                }
                catch (Exception) {
                    Console.SetCursorPosition(1, 1);
                    Console.Write("You have not permissions to access this folder...");
                    System.Threading.Thread.Sleep(3000);
                    if(_currentDirectory.Parent != null) {
                        _currentDirectory = _currentDirectory.Parent;
                    }
                    else {
                        _currentDirectory = DriveInfo.GetDrives()[0].RootDirectory;
                    }
                    Events.CallDirectoryChanged();
                    Drawers.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName, _isSecond);
                    Drawers.ClearTab(TabHeight, TabWidth, _isSecond);
                    Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                }
            }
            else if ((_selectedItem as FileInfo).Extension == ".txt") {
                OpenTxtFile();
            }
        }
        public void F3KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(TabHeight, TabWidth);
            _isPanelOpened = true;
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
            Drawers.ClearTab(TabHeight, TabWidth);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F5KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(TabHeight, TabWidth);
            _isPanelOpened = true;
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
            Drawers.ClearTab(TabHeight, TabWidth);
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
            _isPanelOpened = false;
        }
        public void F6KeyPressedHandler() {
            Drawers.DrawAdditionalPanel(TabHeight, TabWidth);
            _isPanelOpened = true;
            Console.SetCursorPosition(TabWidth - 59, 4);
            Console.Write("Input new directory path > ");
            string dirName = Console.ReadLine();
            Directory.CreateDirectory(dirName);
            Drawers.ClearTab(TabHeight, TabWidth);
            Events.CallDirectoryChanged();
            Drawers.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset);
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
            SearchEngine.ClearSearchResult();
            string[] searchParams = Drawers.DrawSearchMenu(TabHeight, TabWidth);
            SearchEngine.SearchByExpression(searchParams[0], new DirectoryInfo(searchParams[1]));
            //WindowSmall resultWindow = new WindowSmall(_core,SearchEngine.GetSearchResult());
            //_core.Events.Unsubscribe(this);
            //_core.Events.Subscribe(resultWindow);
            //resultWindow.Draw();
            //_core.CurrentWindow = resultWindow;
        }
    }
}
