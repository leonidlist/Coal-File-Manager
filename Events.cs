using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalEmulator {
    class Events {
        private event Action ArrowDownKeyPressed;
        private event Action ArrowUpKeyPressed;
        private event Action EscapeKeyPressed;
        private event Action EnterKeyPressed;
        private event Action F2KeyPressed;
        private event Action F3KeyPressed;
        private event Action F5KeyPressed;
        private event Action F6KeyPressed;
        private event Action F7KeyPressed;
        private event Action F9KeyPressed;
        public Events(ConsoleCore app) {
            ArrowUpKeyPressed += app.ArrowUpKeyPressedHandler;
            ArrowDownKeyPressed += app.ArrowDownKeyPressedHandler;
            EnterKeyPressed += app.EnterKeyPressedHandler;
            EscapeKeyPressed += app.EscapeKeyPressedHandler;
            F2KeyPressed += app.F2KeyPressedHandler;
            F3KeyPressed += app.F3KeyPressHandler;
            F5KeyPressed += app.F5KeyPressedHandler;
            F6KeyPressed += app.F6KeyPressedHandler;
            F7KeyPressed += app.F7KeyPressedHandler;
            F9KeyPressed += app.F9KeyPressedHandler;
        }
        public void Selecter(int height) {
            while (true) {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(2, height + 1);
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.DownArrow) {
                    ArrowDownKeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.UpArrow) {
                    ArrowUpKeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.Enter) {
                    EnterKeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.Escape) {
                    EscapeKeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.F2) {
                    F2KeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.F3) {
                    F3KeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.F5) {
                    F5KeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.F6) {
                    F6KeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.F7) {
                    F7KeyPressed?.Invoke();
                }
                if (keyInfo.Key == ConsoleKey.F9) {
                    F9KeyPressed?.Invoke();
                }
            }
        }
    }
}
