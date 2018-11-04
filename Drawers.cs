using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalEmulator {
    static class Drawers {
        public static void DrawDirectoriesAndFiles(MyConsole console, ref ArrayList directoryContains, ref object selectedItem, int selected, int height, int offset = 0) {
            directoryContains = new ArrayList();
            directoryContains.AddRange(console.GetCurrentDirectory.GetDirectories());
            directoryContains.AddRange(console.GetCurrentDirectory.GetFiles());
            for (int i = 0 + offset; i < height - 2 + offset && i < directoryContains.Count; i++) {
                Console.SetCursorPosition(2, i + 1 - offset);
                if (directoryContains[i] is DirectoryInfo) {
                    if (i == selected) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        selectedItem = directoryContains[i];
                    }
                    Console.Write((directoryContains[i] as DirectoryInfo).Name + "/");
                    Console.ResetColor();
                }
                else if (directoryContains[i] is FileInfo) {
                    if (i == selected) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        selectedItem = directoryContains[i];
                    }
                    Console.Write((directoryContains[i] as FileInfo).Name);
                    Console.ResetColor();
                }
            }
        }
        public static void DrawBorder(int height, int width) {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    Console.SetCursorPosition(j, i);
                    if (i == 0) {
                        if (j == 0) {
                            Console.WriteLine("╔");
                        }
                        else if (j == width - 1) {
                            Console.WriteLine("╗");
                        }
                        else {
                            Console.WriteLine("═");
                        }
                    }
                    else if (i == height - 1) {
                        if (j == 0) {
                            Console.WriteLine("╚");
                        }
                        else if (j == width - 1) {
                            Console.WriteLine("╝");
                        }
                        else {
                            Console.WriteLine("═");
                        }
                    }
                    else {
                        if (j == 0 || j == width - 1) {
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

        public static void DrawTopBorder(int width) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < width; i++) {
                Console.SetCursorPosition(i, 0);
                if (i == 0) {
                    Console.WriteLine("╔");
                }
                else if (i == width - 1) {
                    Console.WriteLine("╗");
                }
                else {
                    Console.WriteLine("═");
                }
            }
            Console.ResetColor();
        }

        public static void DrawCurrentDirectory(int width, string directoryName) {
            DrawTopBorder(width);
            Console.SetCursorPosition(5, 0);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(directoryName);
            Console.ResetColor();
        }

        public static void ClearMainWindow(int height, int width, int stopper = 0) {
            Console.BackgroundColor = ConsoleColor.Black;
            if(stopper == 0) {
                for (int i = 1; i < height - 1; i++) {
                    Console.SetCursorPosition(1, i);
                    Console.Write(" ".MultiplySpace((width - stopper) - 3));
                }
            }
            else {
                for (int i = 1; i < height - 1; i++) {
                    Console.SetCursorPosition(1, i);
                    Console.Write(" ".MultiplySpace(stopper));
                }
            }
        }

        public static void DrawMenu(int height) {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
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
            Console.Write("F9 Quit");
            Console.ResetColor();
        }

        public static void DrawAdditionalPanel(int height, int width) {
            for (int i = 0; i < height - 2; i++) {
                Console.SetCursorPosition(width - 60, i + 1);
                Console.Write("│");
            }
        }

        //public static void DrawBlackBgForMenu(int windowHeight, int bufferHeight, int bufferWidth) {
        //    Console.BackgroundColor = ConsoleColor.Black;
        //    for (int i = windowHeight; i < bufferHeight; i++) {
        //        Console.SetCursorPosition(0, i);
        //        Console.Write(" ".MultiplySpace(bufferWidth));
        //    }
        //    Console.ResetColor();
        //}
    }
}
