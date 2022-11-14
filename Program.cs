// See https://aka.ms/new-console-template for more information

using System.Configuration;
using System;
using System.IO;
using System.Diagnostics;

namespace MyNamespace
{
    class MyClassCS
    {
        static void Main(string[] args)
        {

            var sequencedPath = ConfigurationManager.AppSettings.Get("sequenced");
             string fileFilter = ConfigurationManager.AppSettings.Get("fileFilter");
        

            if (!Directory.Exists(sequencedPath)) { return; }
            using var watcher = new FileSystemWatcher(sequencedPath);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = fileFilter;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            StartProcessing(e.Name, e.FullPath);
        }
        private static void StartProcessing(string? name, string fullPath)
        {
            var processToStart = ConfigurationManager.AppSettings.Get("processToStart");
            var myArgument = @$"";
            var startInfo = new ProcessStartInfo();
            var process = new Process { StartInfo = startInfo };

            startInfo = new ProcessStartInfo()
            {
                Verb = "runas",
                FileName = processToStart,
                Arguments=myArgument
            };
            process = new Process { StartInfo = startInfo };
            process.Start();
           
            //Console.WriteLine($"New RTLComplete Found: {fullPath}");
            //var fileName = Path.GetFileNameWithoutExtension(fullPath);
            //var path = Path.GetDirectoryName(fullPath);
            //var analyzedFilePath = Path.Combine(path);
            //var arggument = $@"";


            //ProcessStartInfo startInfo= new ProcessStartInfo(processToStart);
            //startInfo.Arguments = "-silent";
            //Process z = Process.Start(startInfo);
            //Console.WriteLine($"Organize finished: {path}");
            //Task.Delay(5000);
            //Console.WriteLine($"Organize finished: {fullPath}");
        }
        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
