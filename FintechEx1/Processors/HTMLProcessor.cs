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
    class HTMLProcessor : IProcessor
    {
        private readonly string[] fileTypes = { "html", "htm" };

        public bool SupportFileType(string ext) =>
            fileTypes.Contains(ext);

        public bool DefaultProcessor => false;

        public void ProcessFile(string fileName)
        {
            Pipeline pipeline = new Pipeline(fileName);
            pipeline.AddMiddleware(new Middleware(CountDivs, "CountDivs"));
            pipeline.Start();
        }

        public string CountDivs(string input, Dictionary<string, object> storage)
        {
            var data = File.ReadAllText(input);
            return Regex.Matches(data, @"<.*div[^>]*>").Count.ToString();
        }
    }
}
