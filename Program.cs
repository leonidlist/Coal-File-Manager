using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coal.Draw;

namespace Coal {
    class Program {
        static void Main(string[] args) {
            try {
                CoalCore cc = new CoalCore();
                cc.Start();
            } catch(Exception e) {
                Error.Execute(Console.BufferHeight, Console.BufferWidth, e.Message);
            }
        }
    }
}