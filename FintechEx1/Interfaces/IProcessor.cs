using System;
using System.Collections.Generic;
using System.Text;

namespace FintechEx1.Interfaces
{
    interface IProcessor
    {
        bool SupportFileType(string ext);
        void ProcessFile(string fileName);
        bool DefaultProcessor { get; }
    }
}
