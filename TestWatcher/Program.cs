// See https://aka.ms/new-console-template for more information

using System.Configuration;
using System;
using System.IO;
using System.Diagnostics;

namespace TaskStarter
{
    class TaskStarterForDevyser
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
while(true){
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
               Console.WriteLine("No I won't Press enter to exit.");
        }
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
             Console.WriteLine($"Sequencing Finished: {name}");
            var processToStart = ConfigurationManager.AppSettings.Get("processToStart");
             string myArgument = ConfigurationManager.AppSettings.Get("argument");
         
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
            Console.WriteLine($"Organize finished: {name}");
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
