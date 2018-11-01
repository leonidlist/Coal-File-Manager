using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TerminalEmulator {
    sealed class MyConsole {
        private DirectoryInfo _currentDirectory;
        public DirectoryInfo GetCurrentDirectory {
            get => _currentDirectory;
        }
        public MyConsole() {
            _currentDirectory = new DirectoryInfo(@"C:\");
        }

        public void Start() {
            while (true) {
                Console.Write($"{_currentDirectory}> ");
                string command = Console.ReadLine();
                string[] pars = command.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                List<string> give = pars.ToList();
                give.RemoveAt(0);
                if (pars[0] == "help") {
                    HelpCommand(give);
                }
                else if (pars[0] == "clear") {
                    ClearCommand(give);
                }
                else if (pars[0] == "ls") {
                    LsCommand(give);
                }
                else if (pars[0] == "cd") {
                    CdCommand(give);
                }
                else if (pars[0] == "cp") {
                    CpCommand(give);
                }
                else if (pars[0] == "rm") {
                    RmCommand(give);
                }
                else if (pars[0] == "mkdir") {
                    MkDirCommand(give);
                }
            }
        }

        private void HelpCommand(List<string> args) {
            Console.WriteLine(@"clear - очистить экран
ls - список файлов и директорий
cd - перейти в каталог
cp - копировать файл
rm - удалить файл
mkdir - создать каталог");
        }
        private void ClearCommand(List<string> args) {
            Console.Clear();
        }
        private void LsCommand(List<string> args) {
            FileInfo[] files = _currentDirectory.GetFiles();
            foreach (var directory in _currentDirectory.GetDirectories()) {
                Console.WriteLine($"\t{directory.Name}");
            }
            foreach (var file in files) {
                Console.WriteLine($"\t{file.Name}");
            }
        }
        public bool CdCommand(List<string> args) {
            try {
                DirectoryInfo directoryInfo = new DirectoryInfo(_currentDirectory + args[0]);
                if (directoryInfo.Exists) {
                    string newPath = Regex.Replace(directoryInfo.FullName + @"\", @"\\{2}", @"\");
                    _currentDirectory = new DirectoryInfo(newPath);
                    return true;
                }
                else {
                    Console.WriteLine("Такая директория не существует.");
                    return false;
                }
            }
            catch (Exception e) {
                DirectoryInfo directoryInfo = new DirectoryInfo(args[0]);
                if (directoryInfo.Exists) {
                    _currentDirectory = new DirectoryInfo(args[0]);
                    return true;
                }
                else {
                    Console.WriteLine("Такая директория не существует.");
                    return false;
                }
            }
        }
        private void CpCommand(List<string> args) {
            if (File.Exists(_currentDirectory.FullName + args[0]) && Directory.Exists(args[1])) {
                File.Copy(_currentDirectory.FullName + args[0], args[1] + args[0]);
                return;
            }
            FileInfo fileInfo = new FileInfo(args[0]);
            if (fileInfo.Exists && Directory.Exists(args[1])) {
                File.Copy(args[0], args[1] + fileInfo.Name);
                return;
            }
            Console.WriteLine("Что-то пошло не так.");
        }
        private void RmCommand(List<string> args) {
            if (File.Exists(_currentDirectory.FullName + args[0])) {
                File.Delete(_currentDirectory.FullName + args[0]);
                return;
            }
            if (File.Exists(args[0])) {
                File.Delete(args[0]);
                return;
            }
            Console.WriteLine("Что-то пошло не так.");
        }
        private void MkDirCommand(List<string> args) {
            try {
                if (!Directory.Exists(_currentDirectory.FullName + args[0])) {
                    Directory.CreateDirectory(_currentDirectory.FullName + args[0]);
                    return;
                }
            }
            catch (Exception e) {
                if (!Directory.Exists(args[0])) {
                    Directory.CreateDirectory(args[0]);
                    return;
                }
                Console.WriteLine("Что-то пошло не так.");
            }
        }
    }
}