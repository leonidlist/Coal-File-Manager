using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using Coal.Draw;
using Coal.Search;
using Coal.CopyDir;
using System.Diagnostics;

namespace Coal {
    class Tab {
        private bool _isSecond = false;
        private CoalCore _core;
        private ArrayList _currentTabContains;
        private DirectoryInfo _currentDirectory;
        public DirectoryInfo CurrentDirectory { get => _currentDirectory; }
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
                else if(_currentTabContains[i] is DirectoryInfo) {
                    if ((_currentTabContains[i] as DirectoryInfo).Name.Length > max) {
                        max = (_currentTabContains[i] as DirectoryInfo).Name.Length;
                    }
                }
                else if(_currentTabContains[i] is DriveInfo) {
                    if ((_currentTabContains[i] as DriveInfo).Name.Length > max) {
                        max = (_currentTabContains[i] as DriveInfo).Name.Length;
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
            Borders.DrawBorder(TabHeight, TabWidth, _isSecond);
            Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, 0, _isSecond);
            Directories.DrawCurrentDirectory(TabWidth, _currentDirectory?.FullName, _isSecond, _core.CurrentTab==this?true:false);
            if (!_isSecond)
                BottomMenu.DrawMenu(TabHeight);
        }

        public void EscapeKeyPressedHandler() {
            if (_currentDirectory?.Parent != null) {
                _scrollOffset = 0;
                _selectedIndex = 0;
                _currentDirectory = _currentDirectory.Parent;
                Events.CallDirectoryChanged();
                Directories.DrawCurrentDirectory(TabWidth, _currentDirectory?.FullName, _isSecond, true);
                Clear.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                CalculateMax();
            }
            else if(_currentDirectory == null) {
                _scrollOffset = 0;
                _selectedIndex = 0;
                _currentDirectory = DriveInfo.GetDrives()[0].RootDirectory;
                Events.CallDirectoryChanged();
                Directories.DrawCurrentDirectory(TabWidth, _currentDirectory?.FullName, _isSecond, true);
                Clear.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                CalculateMax();
            }
            else if(_currentDirectory.Parent == null) {
                _scrollOffset = 0;
                _selectedIndex = 0;
                _currentDirectory = null;
                _currentTabContains.Clear();
                _currentTabContains.AddRange(DriveInfo.GetDrives());
                Directories.DrawCurrentDirectory(TabWidth, _currentDirectory?.FullName, _isSecond, true);
                Clear.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                CalculateMax();
            }
            _core.CallEventDrawClock();
        }

        public void ArrowDownKeyPressedHandler() {
            _selectedIndex++;
            if (_selectedIndex > _currentTabContains.Count - 1) {
                _selectedIndex = _currentTabContains.Count - 1;
            }
            else if (_selectedIndex > TabHeight - 3 + _scrollOffset && _selectedIndex < _currentTabContains.Count) {
                _scrollOffset++;
                Clear.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                return;
            }
            Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
        }

        public void ArrowUpKeyPressedHandler() {
            _selectedIndex--;
            if (_selectedIndex < 0) {
                _selectedIndex = 0;
                return;
            }
            if (_selectedIndex < _scrollOffset) {
                _scrollOffset--;
                Clear.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue,_isSecond);
                Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                return;
            }
            Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
        }
        public void F2KeyPressedHandler() {
            bool isPressed = false;
            Info.Execute(TabHeight, TabWidth*2, _selectedItem);
            while(!isPressed) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if(key.Key == ConsoleKey.Escape) {
                    isPressed = true;
                    _core.DrawBothTabs();
                }
            }
        }
        private void DeleteFile() {
            if(Confirmation.Execute(TabHeight, TabWidth*2, "Are you sure that you want to delete this file?")) {
                (_selectedItem as FileInfo).Delete();
                Events.CallDirectoryChanged();
                if(_core.NonActiveTab._currentDirectory != null)
                    _core.NonActiveTab.DirectoryChangedEventHandler();
                _core.DrawBothTabs();
            }
            else {
                _core.DrawBothTabs();
            }
        }
        private void DeleteDirectory() {
            bool res = Confirmation.Execute(TabHeight, TabWidth * 2, "Warning! All files inside directory will be also deleted!");
            if(res) {
                (_selectedItem as DirectoryInfo).Delete(true);
                Events.CallDirectoryChanged();
                _core.NonActiveTab.DirectoryChangedEventHandler();
            }
            _core.DrawBothTabs();
        }
        public void EnterKeyPressedHandler() {
            if (_selectedItem is DirectoryInfo) {
                try {
                    _selectedIndex = 0;
                    _scrollOffset = 0;
                    _currentDirectory = _selectedItem as DirectoryInfo;
                    Events.CallDirectoryChanged();
                    Directories.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName, _isSecond, true);
                    Clear.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                    Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                    CalculateMax();
                }
                catch (Exception) {
                    Error.Execute(TabHeight, TabWidth*2, "You have no permissions to enter this folder");
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
            else if(_selectedItem is FileInfo) {
                try {
                    Process.Start((_selectedItem as FileInfo).FullName);
                } catch(Exception e) {
                    Error.Execute(TabHeight, TabWidth*2, e.Message);
                    _core.DrawBothTabs();
                }
            }
            else if(_selectedItem is DriveInfo) {
                try {
                    _selectedIndex = 0;
                    _scrollOffset = 0;
                    _currentDirectory = (_selectedItem as DriveInfo).RootDirectory;
                    Events.CallDirectoryChanged();
                    Directories.DrawCurrentDirectory(TabWidth, _currentDirectory.FullName, _isSecond, true);
                    Clear.ClearTab(TabHeight, TabWidth, _scrollMaxClearValue, _isSecond);
                    Directories.DrawDirectoriesAndFiles(_currentTabContains, ref _selectedItem, _selectedIndex, TabHeight, TabWidth, _scrollOffset, _isSecond);
                    CalculateMax();
                } catch (Exception) {
                    Error.Execute(TabHeight, TabWidth * 2, "You have no permissions to enter this drive");
                    _core.DrawBothTabs();
                }
            }
            _core.CallEventDrawClock();
        }
        public void F3KeyPressedHandler() {
            string movePath = Move.Execute(TabHeight, TabWidth*2);
            if (movePath != null && movePath != String.Empty) {
                if (_selectedItem is FileInfo) {
                    try {
                        (_selectedItem as FileInfo).MoveTo(movePath + (_selectedItem as FileInfo).Name);
                    }
                    catch (Exception e) {
                        Error.Execute(TabHeight, TabWidth*2, e.Message);
                    }
                }
                else if (_selectedItem is DirectoryInfo) {
                    try {
                        (_selectedItem as DirectoryInfo).MoveTo(movePath + (_selectedItem as DirectoryInfo).Name);
                    }
                    catch (Exception e) {
                        Error.Execute(TabHeight, TabWidth*2, e.Message);
                    }
                }
                Events.CallDirectoryChanged();
                _core.NonActiveTab.DirectoryChangedEventHandler();
            }
            _core.DrawBothTabs();
        }
        public void F4KeyPressHandler() {
            string newName = Edit.Execute(TabWidth*2, TabHeight);
            if(newName != null && newName != String.Empty) {
                if(_selectedItem is FileInfo) {
                    try {
                        (_selectedItem as FileInfo).CopyTo(_currentDirectory.FullName + "\\" + newName + (_selectedItem as FileInfo).Extension);
                        (_selectedItem as FileInfo).Delete();
                    } catch(Exception e) {
                        Error.Execute(TabHeight, TabWidth, e.Message);
                    }
                }
                else if (_selectedItem is DirectoryInfo) {
                    try {
                        (_selectedItem as DirectoryInfo).MoveTo(CurrentDirectory.FullName + newName);
                    }
                    catch (Exception e) {
                        Error.Execute(TabHeight, TabWidth, e.Message);
                    }
                }
                Events.CallDirectoryChanged();
                _core.NonActiveTab.DirectoryChangedEventHandler();
            }
            _core.DrawBothTabs();
        }
        public void F5KeyPressedHandler() {
            string copyPath = Copy.Execute(TabHeight, TabWidth*2);
            if(copyPath != null && copyPath != String.Empty) {
                if (_selectedItem is FileInfo) {
                    try {
                        (_selectedItem as FileInfo).CopyTo(copyPath);
                    }
                    catch (Exception e) {
                        Error.Execute(TabHeight, TabWidth * 2, e.Message);
                    }
                }
                else if (_selectedItem is DirectoryInfo) {
                    try {
                        CopyDirectory cp = new CopyDirectory();
                        cp.CopyDir(_selectedItem as DirectoryInfo, new DirectoryInfo(copyPath));
                    } catch(Exception e) {
                        Error.Execute(TabHeight, TabWidth * 2, e.Message);
                    }
                }
                Events.CallDirectoryChanged();
                _core.NonActiveTab.DirectoryChangedEventHandler();
            }
            _core.DrawBothTabs();
        }
        public void F6KeyPressedHandler() {
            string path = MakeDir.Execute(TabHeight, TabWidth * 2);
            if(path != null && path != String.Empty) {
                Directory.CreateDirectory(path);
                Events.CallDirectoryChanged();
                _core.NonActiveTab.DirectoryChangedEventHandler();
                _core.DrawBothTabs();
            } 
            else {
                _core.DrawBothTabs();
            }
        }
        public void F7KeyPressedHandler() {
            if (_selectedItem is FileInfo) {
                DeleteFile();
            }
            else if (_selectedItem is DirectoryInfo) {
                DeleteDirectory();
            }
        }
        public void F8KeyPressedHandler() {
            string filename = MkFile.Execute(TabWidth*2, TabHeight);
            if(filename != null && filename != string.Empty) {
                try {
                    FileStream fs = File.Create(_currentDirectory.FullName + "/" + filename);
                    Events.CallDirectoryChanged();
                    if(_core.NonActiveTab._currentDirectory != null)
                        _core.NonActiveTab.DirectoryChangedEventHandler();
                    fs.Dispose();
                } catch(Exception e) {
                    _core.DrawBothTabs();
                    Error.Execute(TabHeight, TabWidth*2, e.Message);
                }
            }
            else {
                _core.DrawBothTabs();
                Error.Execute(TabHeight, TabWidth*2, "Cannot create file.");
            }
            _core.DrawBothTabs();
        }
        //TODO: Переход в директорию и обратно в поиске
        public void F9KeyPressedHandler() {
            try {
                SearchEngine.ClearSearchResult();
                string[] searchParams = Coal.Draw.Search.Execute(TabHeight, TabWidth * 2);
                if (searchParams != null && searchParams[0] != string.Empty && searchParams[1] != string.Empty) {
                    SearchEngine.SearchByExpression(searchParams[0], new DirectoryInfo(searchParams[1]));
                    _currentTabContains = SearchEngine.GetSearchResult();
                    CalculateMax();
                    _currentDirectory = null;
                    _core.DrawBothTabs();
                }
                else {
                    _core.DrawBothTabs();
                }
            } catch(Exception e) {
                Error.Execute(TabHeight, TabWidth*2, e.Message);
                _core.DrawBothTabs();
            }
        }
    }
}
