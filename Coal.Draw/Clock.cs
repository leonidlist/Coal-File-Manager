using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Coal.Draw {
    public static class Clock {
        public static int Width { get; set; }
        public static DateTime Start() {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(Width - 10, 0);
            Console.Write(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            return DateTime.Now;
        }
        public static void Execute() {
            DateTime time = Start();
            while(true) {
                if (DateTime.Now.Minute != time.Minute) {
                    time = DateTime.Now;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(Width - 10, 0);
                    Console.Write(time.Hour + ":" + time.Minute);
                }
            }
        }
    }
}
