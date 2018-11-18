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
            if(DateTime.Now.Minute < 10) {
                Console.WriteLine(DateTime.Now.Hour + ":0" + DateTime.Now.Minute);
            }
            else {
                Console.WriteLine(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            }
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
                    if (DateTime.Now.Minute < 10) {
                        Console.WriteLine(time.Hour + ":0" + time.Minute);
                    }
                    else {
                        Console.WriteLine(time.Hour + ":" + time.Minute);
                    }
                }
            }
        }
    }
}
