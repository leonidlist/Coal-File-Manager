using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Terminal
{
    public class Core
    {
        public static int OffsetTop { get; set; }
        public static void Start() {
            while(true) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.BackgroundColor = ConsoleColor.White;
                Console.SetCursorPosition(4, OffsetTop + 2);
                Console.Write(DriveInfo.GetDrives()[0].Name + " > ");
                Console.ReadLine();
            }
        }
    }
}
