using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TerminalEmulator {
    static class Helpers {
        public static string MultiplySpace(this String s, int amount) {
            StringBuilder sb = new StringBuilder(s);
            sb.Append(' ', amount);
            return sb.ToString();
        }
        public static string GetTopBorder(int width) {
            StringBuilder sb = new StringBuilder("╔");
            sb.Append('═', width - 2);
            sb.Append('╗');
            return sb.ToString();
        }
        public static string GetMiddleBorder(int width) {
            StringBuilder sb = new StringBuilder("║");
            sb.Append(' ', width - 2);
            sb.Append('║');
            return sb.ToString();
        }
        public static string GetBottomBorder(int width) {
            StringBuilder sb = new StringBuilder("╚");
            sb.Append('═', width - 2);
            sb.Append('╝');
            return sb.ToString();
        }
    }
}
