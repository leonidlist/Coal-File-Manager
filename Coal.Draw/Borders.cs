using System;
using System.Text;

namespace Coal.Draw
{
    public static class Borders {
        public static void DrawBorder(int height, int width, bool isSecondTab = false) {
            for (int i = 0; i < height; i++) {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(isSecondTab ? width : 0, i);
                if (i == 0) {
                    Console.Write(GetTopBorder(width));
                }
                else if (i == height - 1) {
                    Console.Write(GetBottomBorder(width));
                }
                else {
                    Console.Write(GetMiddleBorder(width));
                }
            }
            Console.ResetColor();
        }
        public static string GetTopBorder(int width) {
            StringBuilder sb = new StringBuilder("╔");
            sb.Append('═', width - 2);
            sb.Append('╗');
            return sb.ToString();
        }
        public static string GetMiddleBorder(int width) {
            StringBuilder sb = new StringBuilder("║");
            sb.Append(' ', width - 2);
            sb.Append('║');
            return sb.ToString();
        }
        public static string GetBottomBorder(int width) {
            StringBuilder sb = new StringBuilder("╚");
            sb.Append('═', width - 2);
            sb.Append('╝');
            return sb.ToString();
        }
    }
}
