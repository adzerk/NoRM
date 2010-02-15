﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoSharp.Protocol.SystemMessages.Responses
{
    public class ProfilingInformationResponse
    {
        public ProfilingInformationResponse() { }

        public string Ts { get; set; }
        public string Info { get; set; }
        public double? Millis { get; set; }
    }
}
