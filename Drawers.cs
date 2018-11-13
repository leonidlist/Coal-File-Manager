using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalEmulator {
    static class Drawers {
        public static void DrawBorder(int height, int width, bool isSecondTab = false) {
            for (int i = 0; i < height; i++) {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(isSecondTab ? width : 0, i);
                if (i == 0) {
                    Console.Write(Helpers.GetTopBorder(width));
                }
                else if (i == height - 1) {
                    Console.Write(Helpers.GetBottomBorder(width));
                }
                else {
                    Console.Write(Helpers.GetMiddleBorder(width));
                }
            }
            Console.ResetColor();
        }
        public static void DrawDirectoriesAndFiles(ArrayList directoryContains, ref object selectedItem, int selected, int height, int width, int offset = 0, bool isSecondTab = false) {
            for (int i = 0 + offset; i < height - 2 + offset && i < directoryContains.Count; i++) {
                Console.SetCursorPosition(isSecondTab?width+2:2, i + 1 - offset);
                if (directoryContains[i] is DirectoryInfo) {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (i == selected) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        selectedItem = directoryContains[i];
                    }
                    if((directoryContains[i] as DirectoryInfo).Name.Length > (width-2))
                        Console.Write(((directoryContains[i] as DirectoryInfo).Name + "/").Remove(width - 4));
                    else {
                        Console.Write((directoryContains[i] as DirectoryInfo).Name + "/");
                    }
                    Console.ResetColor();
                }
                else if (directoryContains[i] is FileInfo) {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (i == selected) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        selectedItem = directoryContains[i];
                    }
                    if((directoryContains[i] as FileInfo).Name.Length > (width-2)) {
                        Console.Write(((directoryContains[i] as FileInfo).Name).Remove(width - 4));
                    }
                    else {
                        Console.Write((directoryContains[i] as FileInfo).Name);
                    }
                    Console.ResetColor();
                }
            }
        }

        public static void DrawCurrentDirectory(int width, string directoryName, bool isSecondTab = false) {
            Console.SetCursorPosition(isSecondTab ? width : 0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Helpers.GetTopBorder(width));
            Console.SetCursorPosition(isSecondTab?width+5:5, 0);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(directoryName);
            Console.ResetColor();
        }

        public static void ClearTab(int height, int width, int maxClear, bool isSecondTab = false) {
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 1; i < height - 1; i++) {
                Console.SetCursorPosition(isSecondTab ? 1 + width : 1, i);
                if(maxClear < width-4) {
                    Console.Write(" ".MultiplySpace(maxClear));
                }
                else {
                    Console.Write(" ".MultiplySpace(width-3));
                }
            }
        }

        public static void DrawMenu(int height) {
            Console.BackgroundColor = ConsoleColor.Yellow;
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
            Console.Write("F9 Search");
            Console.SetCursorPosition(103, height + 3);
            Console.Write("F10 Quit");
            Console.ResetColor();
        }

        public static string[] DrawSearchMenu(int height, int width) {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            //Draw gray background
            for(int i = 0; i < 12; i++) {
                Console.SetCursorPosition(width/2-(width/4), (height/2-6)+i);
                Console.Write(" ".MultiplySpace(width/2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 1);
            Console.Write(Helpers.GetTopBorder(width / 2 - 4));
            for(int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 2 + i);
                Console.Write(Helpers.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 10);
            Console.Write(Helpers.GetBottomBorder(width / 2 - 4));
            //Draw
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 2);
            Console.Write("Input your search query (regex supported): ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ".MultiplySpace(width/2-10));
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Input where to search: ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 8);
            Console.Write(" ".MultiplySpace(width / 2 - 10));
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            string expression = Console.ReadLine();
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 8);
            string whereTo = Console.ReadLine();
            return new string[] { expression, whereTo };
        }

        public static void DrawInfoMenu(int height, int width, object target) {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            //Draw gray background
            for (int i = 0; i < 12; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 6) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 1);
            Console.Write(Helpers.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 2 + i);
                Console.Write(Helpers.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 10);
            Console.Write(Helpers.GetBottomBorder(width / 2 - 4));
            //Draw
            if(target is FileInfo) {
                FileInfo tmp = target as FileInfo;
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
                Console.Write($"Creation time: {tmp.CreationTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 5);
                Console.Write($"Last access time: {tmp.LastAccessTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
                Console.Write($"Size: {(double)(tmp.Length/1000000)}MB");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 7);
                Console.Write($"Full name: {(tmp.FullName.Length < width/2-19 ? tmp.FullName: tmp.FullName.Remove(width/2-19))}");
            }
            else if(target is DirectoryInfo) {
                DirectoryInfo tmp = target as DirectoryInfo;
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
                Console.Write($"Creation time: {tmp.CreationTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 5);
                Console.Write($"Last access time: {tmp.LastAccessTime}");
                Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 6);
                Console.Write($"Full name: {(tmp.FullName.Length < width / 2 - 19 ? tmp.FullName : tmp.FullName.Remove(width / 2 - 19))}");
            }
        }

        public static void DrawError(int height, int width, string message) {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            //Draw gray background
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 4) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 1);
            Console.Write(Helpers.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 4; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 2 + i);
                Console.Write(Helpers.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 4) + 6);
            Console.Write(Helpers.GetBottomBorder(width / 2 - 4));
            //
            Console.SetCursorPosition(width / 2 - (width / 4) + (width/2-message.Length)/2, (height / 2 - 4) + 3);
            Console.Write(message);
            //button ok draw
            Console.SetCursorPosition(width / 2 - (width / 4) + (width / 2 - 4) / 2, (height / 2 - 4) + 5);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" OK ");
        }

        public static string DrawMkDirMenu(int height, int width) {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            //Draw gray background
            for (int i = 0; i < 12; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4), (height / 2 - 6) + i);
                Console.Write(" ".MultiplySpace(width / 2));
            }
            //Draw border inside menu
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 1);
            Console.Write(Helpers.GetTopBorder(width / 2 - 4));
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 2 + i);
                Console.Write(Helpers.GetMiddleBorder(width / 2 - 4));
            }
            Console.SetCursorPosition(width / 2 - (width / 4) + 2, (height / 2 - 6) + 10);
            Console.Write(Helpers.GetBottomBorder(width / 2 - 4));
            //Draw
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 2);
            Console.Write("Input your new path name: ");
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ".MultiplySpace(width / 2 - 10));
            Console.SetCursorPosition(width / 2 - (width / 4) + 4, (height / 2 - 6) + 4);
            string expression = Console.ReadLine();
            return expression;
        }
    }
}
