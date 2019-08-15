using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FintechEx1.Interfaces;

namespace FintechEx1
{
   
    class Program
    {
        private static IProcessor[] processors;
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            processors = assembly.GetTypes().Where(x => x.GetInterfaces().Any(t => t == typeof(IProcessor)))
                .Select(processor => (IProcessor) Activator.CreateInstance(processor, null)).ToArray();


            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = args.FirstOrDefault() ?? "",
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*", //Use of multiple filters such as "*.txt|*.doc" is not supported.
            };
            watcher.Created += OnChanged;

            watcher.EnableRaisingEvents = true;

            Console.ReadLine();
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            var ext = e.Name.Split(".").LastOrDefault();
            var filePath = e.FullPath;
            if (Directory.Exists(filePath))
                return;
            var proccessorsList = processors.Where(x => x.SupportFileType(ext) && !x.DefaultProcessor).ToList();
            if (proccessorsList.Any())
            {
                proccessorsList.ForEach(processor => { processor.ProcessFile(filePath); });
            }
            else
            {
                processors.FirstOrDefault(x => x.SupportFileType(ext) && x.DefaultProcessor)?.ProcessFile(filePath);
            }

            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }
    }
}
