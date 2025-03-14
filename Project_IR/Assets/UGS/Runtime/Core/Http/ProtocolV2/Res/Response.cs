﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Protocol.v2.Res
{
    public class Response
    {
        public bool hasError() { return error != null; }
        public EInstruction instruction;
        public ErrorResponse error;
    }

    public class ErrorResponse
    {
        public string message;
        public JObject eReq;
        public string eType; 
        public string eStackTrace;
    }
}
