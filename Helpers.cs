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
    }
}
