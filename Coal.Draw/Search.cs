using System;

namespace Coal.Draw {
    public static class Search {
        public static string[] Execute(int height, int width) {
            string expression, whereTo;

            DrawBackground(width, height);
            DrawBorder(width, height);
            DrawFields(width, height);
            DrawButtons(width, height);
            expression = GetExpression(width, height);
            whereTo = GetWhereTo(width, height);
            return ButtonsHandling(width, height)?new string[] { expression, whereTo }:null;
        }

        private static void DrawBackground(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 14; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 7) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
        }

        private static void DrawBorder(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 7) + 1);
            //top border
            Console.Write(Borders.GetTopBorder(width / 2 - 4));
            //middle border
            for (int i = 0; i < 10; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 7) + 2 + i);
                Console.Write(Borders.GetMiddleBorder(width / 2 - 4));
            }
            //bottom border
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 7) + 12);
            Console.Write(Borders.GetBottomBorder(width / 2 - 4));
        }

        private static void DrawFields(int width, int height) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 2);
            Console.Write("Input your search query (regex supported): ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 4);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" ".MultiplySpace(width / 2 - 10));
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 6);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input where to search: ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 8);
            Console.Write(" ".MultiplySpace(width / 2 - 10));
        }

        private static void DrawButtons(int width, int height) {
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 11);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" OK ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 11, (height / 2 - 7) + 11);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" CANCEL ");
        }

        private static string GetExpression(int width, int height) {
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 7) + 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(">");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 4);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            string expression = Console.ReadLine();
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 7) + 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
            return expression;
        }

        private static string GetWhereTo(int width, int height) {
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 7) + 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(">");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 8);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            string whereTo = Console.ReadLine();
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 7) + 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
            return whereTo;
        }

        private static bool ButtonsHandling(int width, int height) {
            bool isRunning = true;
            bool state = false;
            Console.SetCursorPosition(width / 2 - (width / 4) + 3, (height / 2 - 7) + 11);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(">");
            while (isRunning) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Tab || key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow) {
                    state = !state;
                    if (state) {
                        Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 11);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" OK ");
                        Console.SetCursorPosition(width / 2 - (width / 4) + 11, (height / 2 - 7) + 11);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" CANCEL ");
                    }
                    else {
                        Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 7) + 11);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" OK ");
                        Console.SetCursorPosition(width / 2 - (width / 4) + 11, (height / 2 - 7) + 11);
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
