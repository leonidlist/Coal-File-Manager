using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TerminalEmulator {
    sealed class ConsoleCore {
        MyConsole mc;
        public ConsoleCore() {
            mc = new MyConsole();
            Console.SetWindowPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }
        public void Start() {
            DrawBorder();
            DrawCurrentDirectory();
            DrawDownMenu();
            //Console.SetCursorPosition(0, Console.BufferHeight - 2);
        }
        private void DrawCurrentDirectory() {
            Console.SetCursorPosition(10, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(mc.GetCurrentDirectory.FullName);
            Console.ResetColor();
        }
        private void DrawDownMenu() {
            for (int i = Console.BufferHeight - 4; i < Console.BufferHeight - 1; i++) {
                for (int j = 0; j < Console.BufferWidth; j++) {
                    Console.SetCursorPosition(j, i);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
            }
            Console.SetCursorPosition(10, Console.BufferHeight - 3);
            while(true) {
                Console.Write("> ");
                string command = Console.ReadLine();
                string[] pars = command.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                List<string> give = pars.ToList();
                give.RemoveAt(0);
                if (pars[0] == "cd") {
                    mc.CdCommand(give);
                }
                DrawCurrentDirectory();
            }
            //Console.ReadKey();
        }
        private void DrawBorder() {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < Console.BufferHeight - 4; i++) {
                for (int j = 0; j < Console.BufferWidth; j++) {
                    Console.SetCursorPosition(j, i);
                    if (i == 0) {
                        if (j == 0) {
                            Console.WriteLine("╔");
                        }
                        else if (j == Console.BufferWidth - 1) {
                            Console.WriteLine("╗");
                        }
                        else {
                            Console.WriteLine("═");
                        }
                    }
                    else if (i == Console.BufferHeight - 5) {
                        if (j == 0) {
                            Console.WriteLine("╚");
                        }
                        else if (j == Console.BufferWidth - 1) {
                            Console.WriteLine("╝");
                        }
                        else {
                            Console.WriteLine("═");
                        }
                    }
                    else {
                        if (j == 0 || j == Console.BufferWidth - 1) {
                            Console.WriteLine("║");
                        }
                    }
                }
            }
            Console.ResetColor();
        }
    }
}
