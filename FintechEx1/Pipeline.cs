using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace FintechEx1
{
    public sealed class Middleware
    {
        public Middleware()
        {
        }

        public Middleware(Func<string,Dictionary<string, object>, string> action,
            string name = "")
        {
            Name = name;
            Action = action;
        }

        public string Name { get; }

        private Func<string,Dictionary<string, object>, string> Action { get; }

        public string Execute(string data, Dictionary<string, object> storage)
        {
            return Action(data, storage);
        }
    }


    public sealed class Pipeline
    {
        private readonly HashSet<Middleware> _middlewares;
       
        private Dictionary<string, object> storage { get; }

        private string fileName { get; }

        private const string outfile = "results.txt";


        public Pipeline(string fileName)
        {
            _middlewares = new HashSet<Middleware>();
            storage = new Dictionary<string, object>();
            this.fileName = fileName;
        }
    
        public void AddMiddleware(Middleware middleware)
        {
            if (null == middleware || _middlewares.Contains(middleware)) return;
            _middlewares.Add(middleware);
        }
        public void Start()
        {
  

            foreach (var middleware in _middlewares)
            {
                var result = middleware.Execute(fileName, storage);
                File.AppendAllText(outfile, $"{fileName}-{middleware.Name}-{result}{Environment.NewLine}");
            }
        }
    }
}
