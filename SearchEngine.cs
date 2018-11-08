using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalEmulator {
    static class SearchEngine {
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
        public static void SearchByExpression(string expression, DirectoryInfo curr) {
            try {
                ArrayList contains = new ArrayList();
                contains.AddRange(curr.GetDirectories());
                contains.AddRange(curr.GetFiles());
                foreach (var i in contains) {
                    if (i is DirectoryInfo) {
                        if (Regex.IsMatch((i as DirectoryInfo).Name, expression)) {
                            _searchResult.Add(i as DirectoryInfo);
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

        public static void SearchBySize(int size, DirectoryInfo curr) {
            try {
                ArrayList contains = new ArrayList();
                contains.AddRange(curr.GetDirectories());
                contains.AddRange(curr.GetFiles());
                foreach (var i in contains) {
                    if (i is DirectoryInfo) {
                        SearchBySize(size, i as DirectoryInfo);
                    }
                    else {
                        if ((i as FileInfo).Length == size) {
                            _searchResult.Add(i as FileInfo);
                        }
                    }
                }
            } catch(Exception) { }
        }

        public static void SearchByCreationTime(DateTime time, DirectoryInfo curr) {
            try {
                ArrayList contains = new ArrayList();
                contains.AddRange(curr.GetDirectories());
                contains.AddRange(curr.GetFiles());
                foreach (var i in contains) {
                    if (i is DirectoryInfo) {
                        if ((i as DirectoryInfo).CreationTime == time) {
                            _searchResult.Add(i as DirectoryInfo);
                        }
                        SearchByCreationTime(time, i as DirectoryInfo);
                    }
                    else {
                        if ((i as FileInfo).CreationTime == time) {
                            _searchResult.Add(i as FileInfo);
                        }
                    }
                }
            } catch (Exception) { }
        }
    }
}
