﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.IO
{
    public interface IFIleWriter
    {
        void WriteCS(string writePath, string content);
        void WriteData(string writePath, string content);
    }
}
