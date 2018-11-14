using System;
using System.IO;

namespace Coal.Draw {
    public static class Info {
        public static void Execute(int height, int width, object target) {
            DrawBackground(width, height);
            DrawBorder(width, height);
            if (target is FileInfo) {
                DrawFileInfo(width, height, target as FileInfo);
            }
            else if (target is DirectoryInfo) {
                DrawDirectoryInfo(width, height, target as DirectoryInfo);
            }
        }

        private static void DrawBackground(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 12; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 6) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
        }

        private static void DrawBorder(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            //top border
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            //middle border
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            //bottom border
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 10);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
        }

        private static void DrawFileInfo(int width, int height, FileInfo file) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            Console.Write($"Creation time: {file.CreationTime}");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 5);
            Console.Write($"Last access time: {file.LastAccessTime}");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
            Console.Write($"Size: {(double)(file.Length / 1000000)}MB");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 7);
            Console.Write($"Full name: {(file.FullName.Length < width / 2 - 19 ? file.FullName : file.FullName.Remove(width / 2 - 19))}");
        }

        private static void DrawDirectoryInfo(int width, int height, DirectoryInfo directory) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            Console.Write($"Creation time: {directory.CreationTime}");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 5);
            Console.Write($"Last access time: {directory.LastAccessTime}");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
            Console.Write($"Full name: {(directory.FullName.Length < width / 2 - 19 ? directory.FullName : directory.FullName.Remove(width / 2 - 19))}");
        }
    }
}
