using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coal.Draw {
    public static class BottomMenu {
        public static void DrawMenu(int height) {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, height + 3);
            Console.Write("F1 Help");
            Console.SetCursorPosition(12, height + 3);
            Console.Write("F2 SelInfo");
            Console.SetCursorPosition(25, height + 3);
            Console.Write("F3 Move");
            Console.SetCursorPosition(35, height + 3);
            Console.Write("F4 Edit");
            Console.SetCursorPosition(45, height + 3);
            Console.Write("F5 Copy");
            Console.SetCursorPosition(55, height + 3);
            Console.Write("F6 MkFold");
            Console.SetCursorPosition(67, height + 3);
            Console.Write("F7 Delete");
            Console.SetCursorPosition(79, height + 3);
            Console.Write("F8 MkFile");
            Console.SetCursorPosition(91, height + 3);
            Console.Write("F9 Search");
            Console.SetCursorPosition(103, height + 3);
            Console.Write("F10 Quit");
        }
    }
}
