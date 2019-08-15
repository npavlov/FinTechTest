using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using FintechEx1.Interfaces;

namespace FintechEx1.Processors
{
    class TextProcessor : IProcessor
    {
        public bool SupportFileType(string ext) =>
            true;

        public bool DefaultProcessor => true;

        public void ProcessFile(string fileName)
        {
            Pipeline pipeline = new Pipeline(fileName);
            pipeline.AddMiddleware(new Middleware(CountSymbols, "CountSymbols"));
            pipeline.Start();
        }

        public string CountSymbols(string input, Dictionary<string, object> storage)
        {
            var data = File.ReadAllText(input);
            var symbols = new[] { ';', '.', ',', ':', '-', '"', '(', ')', '!', '?' };
            return data.Count(f => symbols.Contains(f)).ToString();
        }
    }
}
