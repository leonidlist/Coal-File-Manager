using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coal.Draw {
    public static class Confirmation {
        public static bool Execute(int height, int width, string message) {
            DrawBackground(width, height);
            DrawBorder(width, height);
            DrawMessage(width, height, message);
            return DrawButtons(width, height);
        }

        private static void DrawBackground(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 4) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
        }

        private static void DrawBorder(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            //top border
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 1);
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            //middle border
            for (int i = 0; i < 4; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            //botttom border
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 6);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
        }

        private static void DrawMessage(int width, int height, string message) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - message.Length) / 2, (height / 2 - 4) + 3);
            Console.Write(message);
        }

        private static bool DrawButtons(int width, int height) {
            bool isRunning = true;
            bool state = false;

            Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 - 10, (height / 2 - 4) + 5);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" OK ");
            Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 + 10, (height / 2 - 4) + 5);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" CANCEL ");

            while (isRunning) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Tab || key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow) {
                    state = !state;
                    if (state) {
                        Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 - 10, (height / 2 - 4) + 5);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" OK ");
                        Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 + 10, (height / 2 - 4) + 5);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" CANCEL ");
                    }
                    else {
                        Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 - 10, (height / 2 - 4) + 5);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" OK ");
                        Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2 + 10, (height / 2 - 4) + 5);
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
