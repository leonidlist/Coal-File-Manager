using System;
using System.Text;
using System.IO;

namespace Coal.Draw {
    public static class Widgets {
        public static string[] DrawSearchMenu(int height, int width) {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            //Draw gray background
            for (int i = 0; i < 12; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 6) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 10);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
            //Draw
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 2);
            Console.Write("Input your search query (regex supported): ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ".MultiplySpace(width / 2 - 10));
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Input where to search: ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 8);
            Console.Write(" ".MultiplySpace(width / 2 - 10));
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            string expression = Console.ReadLine();
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 8);
            string whereTo = Console.ReadLine();
            return new string[] { expression, whereTo };
        }

        public static void DrawInfoMenu(int height, int width, object target) {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            //Draw gray background
            for (int i = 0; i < 12; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 6) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 10);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
            //Draw
            if (target is FileInfo) {
                FileInfo tmp = target as FileInfo;
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
                Console.Write($"Creation time: {tmp.CreationTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 5);
                Console.Write($"Last access time: {tmp.LastAccessTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
                Console.Write($"Size: {(double)(tmp.Length / 1000000)}MB");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 7);
                Console.Write($"Full name: {(tmp.FullName.Length < width / 2 - 19 ? tmp.FullName : tmp.FullName.Remove(width / 2 - 19))}");
            }
            else if (target is DirectoryInfo) {
                DirectoryInfo tmp = target as DirectoryInfo;
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
                Console.Write($"Creation time: {tmp.CreationTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 5);
                Console.Write($"Last access time: {tmp.LastAccessTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
                Console.Write($"Full name: {(tmp.FullName.Length < width / 2 - 19 ? tmp.FullName : tmp.FullName.Remove(width / 2 - 19))}");
            }
        }

        public static void DrawError(int height, int width, string message) {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Gray;
            //Draw gray background
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 4) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 4; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 6);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
            //
            Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - message.Length) / 2, (height / 2 - 4) + 3);
            Console.Write(message);
            //button ok draw
            Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2, (height / 2 - 4) + 5);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" OK ");
        }
        public static bool DrawConfirmationMenu(int height, int width, string message) {
            bool state = true;
            bool isRunning = true;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Gray;
            //Draw gray background
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 4) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 4; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 6);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
            //
            Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - message.Length) / 2, (height / 2 - 4) + 3);
            Console.Write(message);
            while (isRunning) {
                //button ok draw
                Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 - 10, (height / 2 - 4) + 5);
                if (state) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                Console.Write(" OK ");
                //button cancel draw
                Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 + 10, (height / 2 - 4) + 5);
                if (!state) {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                Console.Write(" CANCEL ");
                Console.SetCursorPosition(2, height + 1);
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.LeftArrow:
                        state = !state;
                        break;
                    case ConsoleKey.RightArrow:
                        state = !state;
                        break;
                    case ConsoleKey.Tab:
                        state = !state;
                        break;
                    case ConsoleKey.Enter:
                        isRunning = false;
                        break;
                }
            }
            return state;
        }
        public static string DrawMkDirMenu(int height, int width) {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            //Draw gray background
            for (int i = 0; i < 12; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 6) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 10);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
            //Draw
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 2);
            Console.Write("Input your new path name: ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ".MultiplySpace(width / 2 - 10));
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            string expression = Console.ReadLine();
            return expression;
        }
    }
}
