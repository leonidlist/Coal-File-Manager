using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Coal.Draw {
    public static class Clock {
        public static int Width { get; set; }
        public static void Execute() {
            DateTime time = DateTime.Now;
            while(true) {
                if(DateTime.Now.Minute != time.Minute) {
                    time = DateTime.Now;
                    Console.SetCursorPosition(Width - 4, 0);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(time.TimeOfDay);
                }
            }
        }
    }
}
