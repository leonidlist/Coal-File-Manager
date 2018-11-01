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
            DrawDirectories();
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
        private void DrawBlackBg() {
            for (int i = Console.BufferHeight - 4; i < Console.BufferHeight - 1; i++) {
                for (int j = 0; j < Console.BufferWidth; j++) {
                    Console.SetCursorPosition(j, i);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
            }
            Console.ResetColor();
        }
        private void DrawDirectories() {
            DirectoryInfo[] dirs = mc.GetCurrentDirectory.GetDirectories();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 3; i < Console.BufferHeight-5 && i < dirs.Length; i++) {
                Console.SetCursorPosition(5, i);
                Console.Write($"- {dirs[i-3].Name}");
            }
            Console.ResetColor();
        }
        private void DrawDownMenu() {
            DrawBlackBg();
            int remember = 0;
            while (true) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                for (int i = 10; i < 10+remember; i++) {
                    Console.SetCursorPosition(i, Console.BufferHeight - 3);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(10, Console.BufferHeight - 3);
                Console.Write("> ");
                string command = Console.ReadLine();
                remember = command.Length+2;
                string[] pars = command.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                List<string> give = pars.ToList();
                give.RemoveAt(0);
                if (pars[0] == "cd") {
                    if(mc.CdCommand(give)) {
                        DrawBorder();
                        DrawDirectories();
                    }
                }
                DrawCurrentDirectory();
            }
        }
        private void DrawBorder() {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
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
                        else {
                            Console.WriteLine(" ");
                        }
                    }
                }
            }
            Console.ResetColor();
        }
    }
}
