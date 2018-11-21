using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Coal.Search
{
    public static class SearchEngine {
        private static ArrayList _searchResult;
        static SearchEngine() {
            _searchResult = new ArrayList();
        }
        public static ArrayList GetSearchResult() {
            return _searchResult;
        }
        public static void ClearSearchResult() {
            _searchResult.Clear();
        }
        public static void SearchInsideFiles(string exp, DirectoryInfo where) {
            ArrayList arrtmp = new ArrayList();
            SearchByExpression(@"\w*.txt", where);
            foreach (var item in _searchResult) {
                if (item is FileInfo) {
                    FileInfo tmp = item as FileInfo;
                    using (StreamReader sr = new StreamReader(File.Open(tmp.FullName, FileMode.Open))) {
                        string all = sr.ReadToEnd();
                        if (Regex.IsMatch(all, exp)) {
                            arrtmp.Add(item as FileInfo);
                        }
                    }
                }
            }
            _searchResult = arrtmp;
        }

        public static void SearchByExpression(string expression, DirectoryInfo curr) {
            try {
                ArrayList contains = new ArrayList();
                contains.AddRange(curr.GetDirectories());
                contains.AddRange(curr.GetFiles());
                foreach (var i in contains) {
                    if (i is DirectoryInfo) {
                        if (Regex.IsMatch((i as DirectoryInfo).Name, expression)) {
                            _searchResult.Add(i as Directory);
                        }
                        SearchByExpression(expression, i as DirectoryInfo);
                    }
                    else {
                        if (Regex.IsMatch((i as FileInfo).Name, expression)) {
                            _searchResult.Add(i as FileInfo);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public static void SearchBySize(long size, DirectoryInfo curr) {
            ArrayList contains = new ArrayList();
            contains.AddRange(curr.GetDirectories());
            contains.AddRange(curr.GetFiles());
            foreach (var i in contains) {
                if (i is DirectoryInfo) {
                    if (GetFolderLength(i as DirectoryInfo) == size) {
                        _searchResult.Add(i as DirectoryInfo);
                    }
                    SearchBySize(size, i as DirectoryInfo);
                }
                else {
                    if ((i as FileInfo).Length == size) {
                        _searchResult.Add(i as FileInfo);
                    }
                }
            }
        }

        public static long GetFolderLength(DirectoryInfo dir) {
            long local = 0;
            ArrayList contains = new ArrayList();
            contains.AddRange(dir.GetDirectories());
            contains.AddRange(dir.GetFiles());
            foreach (var i in contains) {
                if (i is FileInfo) {
                    local += (i as FileInfo).Length;
                }
            }
            foreach (var i in contains) {
                if (i is DirectoryInfo) {
                    local += GetFolderLength(i as DirectoryInfo);
                }
            }
            return local;
        }

        public static void SearchByCreationTime(DateTime time, DirectoryInfo curr) {
            ArrayList contains = new ArrayList();
            contains.AddRange(curr.GetDirectories());
            contains.AddRange(curr.GetFiles());
            foreach (var i in contains) {
                if (i is DirectoryInfo) {
                    if ((i as DirectoryInfo).CreationTime.Year == time.Year && (i as DirectoryInfo).CreationTime.Month == time.Month && (i as DirectoryInfo).CreationTime.Day == time.Day) {
                        _searchResult.Add(i as DirectoryInfo);
                    }
                    SearchByCreationTime(time, i as DirectoryInfo);
                }
                else {
                    if ((i as FileInfo).CreationTime.Year == time.Year && (i as FileInfo).CreationTime.Month == time.Month && (i as FileInfo).CreationTime.Day == time.Day) {
                        _searchResult.Add(i as FileInfo);
                    }
                }
            }
        }

        public static void SearchByLastAccessTime(DateTime time, DirectoryInfo curr) {
            ArrayList contains = new ArrayList();
            contains.AddRange(curr.GetDirectories());
            contains.AddRange(curr.GetFiles());
            foreach (var i in contains) {
                if (i is DirectoryInfo) {
                    if ((i as DirectoryInfo).LastAccessTime == time) {
                        _searchResult.Add(i as DirectoryInfo);
                    }
                    SearchByLastAccessTime(time, i as DirectoryInfo);
                }
                else {
                    if ((i as FileInfo).LastAccessTime == time) {
                        _searchResult.Add(i as FileInfo);
                    }
                }
            }
        }

        public static void SearchByModificationTime(DateTime time, DirectoryInfo curr) {
            ArrayList contains = new ArrayList();
            contains.AddRange(curr.GetDirectories());
            contains.AddRange(curr.GetFiles());
            foreach (var i in contains) {
                if (i is DirectoryInfo) {
                    if ((i as DirectoryInfo).LastWriteTime == time) {
                        _searchResult.Add(i as DirectoryInfo);
                    }
                    SearchByModificationTime(time, i as DirectoryInfo);
                }
                else {
                    if ((i as FileInfo).LastWriteTime == time) {
                        _searchResult.Add(i as FileInfo);
                    }
                }
            }
        }
    }
}
