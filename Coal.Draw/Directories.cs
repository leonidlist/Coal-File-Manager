using System;
using System.Collections;
using System.IO;

namespace Coal.Draw {
    public static class Directories {
        public static void DrawCurrentDirectory(int width, string directoryName, bool isSecondTab = false, bool isAcive = false) {
            Console.SetCursorPosition(isSecondTab ? width : 0, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(Borders.GetTopBorder(width));
            Console.SetCursorPosition(isSecondTab ? width + 5 : 5, 0);
            if(!isAcive) {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write(directoryName);
        }
        public static void DrawDirectoriesAndFiles(ArrayList directoryContains, ref object selectedItem, int selected, int height, int width, int offset = 0, bool isSecondTab = false) {
            for (int i = 0 + offset; i < height - 2 + offset && i < directoryContains.Count; i++) {
                Console.SetCursorPosition(isSecondTab ? width + 2 : 2, i + 1 - offset);
                if (directoryContains[i] is DirectoryInfo) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (i == selected) {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        selectedItem = directoryContains[i];
                    }
                    if ((directoryContains[i] as DirectoryInfo).Name.Length > (width - 2))
                        Console.Write(((directoryContains[i] as DirectoryInfo).Name + "/").Remove(width - 4));
                    else {
                        Console.Write((directoryContains[i] as DirectoryInfo).Name + "/");
                    }
                }
                else if (directoryContains[i] is FileInfo) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (i == selected) {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        selectedItem = directoryContains[i];
                    }
                    if ((directoryContains[i] as FileInfo).Name.Length > (width - 2)) {
                        Console.Write(((directoryContains[i] as FileInfo).Name).Remove(width - 4));
                    }
                    else {
                        Console.Write((directoryContains[i] as FileInfo).Name);
                    }
                }
                else if(directoryContains[i] is DriveInfo) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (i == selected) {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        selectedItem = directoryContains[i];
                    }
                    if ((directoryContains[i] as DriveInfo).Name.Length > (width - 2)) {
                        Console.Write(((directoryContains[i] as DriveInfo).Name).Remove(width - 4));
                    }
                    else {
                        Console.Write((directoryContains[i] as DriveInfo).Name);
                    }
                }
            }
        }
    }
}
