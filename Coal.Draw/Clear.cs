using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coal.Draw {
    public static class Clear {
        public static void ClearTab(int height, int width, int maxClear, bool isSecondTab = false) {
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 1; i < height - 1; i++) {
                Console.SetCursorPosition(isSecondTab ? 1 + width : 1, i);
                if (maxClear < width - 4) {
                    Console.Write(" ".MultiplySpace(maxClear));
                }
                else {
                    Console.Write(" ".MultiplySpace(width - 3));
                }
            }
        }
    }
}
