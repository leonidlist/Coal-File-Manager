using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coal.Draw {
    public static class MkFile {
        public static string Execute(int width, int height) {
            DrawBackground(width, height);
            DrawBorder(width, height);
            DrawField(width, height);
            DrawButtons(width, height);
            string name = GetPath(width, height);
            if (ButtonsHandling(width, height))
                return name;
            else
                return null;
        }

        private static void DrawBackground(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 10; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 5) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
        }

        private static void DrawBorder(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            //top border
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 5) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            //middle border
            for (int i = 0; i < 6; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 5) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            //bottom border
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 5) + 8);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
        }

        private static void DrawField(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 5) + 2);
            Console.Write("Input file name (file will be created in current dir): ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 5) + 4);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" ".MultiplySpace(width / 2 - 10));
        }

        private static string GetPath(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 5) + 4);
            Console.Write(">");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 5) + 4);
            string path = Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 5) + 4);
            Console.Write(" ");
            return path;
        }

        private static void DrawButtons(int width, int height) {
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 5) + 7);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" OK ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 11, (height / 2 - 5) + 7);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" CANCEL ");
        }

        private static bool ButtonsHandling(int width, int height) {
            bool isRunning = true;
            bool state = false;
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 5) + 7);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(">");
            while (isRunning) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Tab || key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow) {
                    state = !state;
                    if (state) {
                        Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 5) + 7);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" OK ");
                        Console.SetCursorPosition(width / 2 - (width / 4) + 11, (height / 2 - 5) + 7);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" CANCEL ");
                    }
                    else {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 5) + 7);
                        Console.Write(" OK ");
                        Console.SetCursorPosition(width / 2 - (width / 4) + 11, (height / 2 - 5) + 7);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" CANCEL ");
                    }
                }
                else if (key.Key == ConsoleKey.Enter) {
                    return state;
                }
            }
            return false;
        }
    }
}
