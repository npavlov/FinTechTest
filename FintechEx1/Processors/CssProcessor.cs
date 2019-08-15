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
    class CssProcessor : IProcessor
    {
        private readonly string[] fileTypes = { "css" };

        public bool DefaultProcessor => false;

        public bool SupportFileType(string ext) =>
            fileTypes.Contains(ext);

        public void ProcessFile(string fileName)
        {
            Pipeline pipeline = new Pipeline(fileName);
            pipeline.AddMiddleware(new Middleware(DoesBracketsMatch, "DoesBracketsMatch"));
            pipeline.Start();
        }

        public string DoesBracketsMatch(string input, Dictionary<string, object> storage)
        {
            var data = File.ReadAllText(input);
            int countFirst = data.Count(f => f == '{');
            int countSecond = data.Count(f => f == '}');
            return (countFirst == countSecond).ToString();
        }
    }
}
