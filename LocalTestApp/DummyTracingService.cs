using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTestApp
{
    public class DummyTracingService : ITracingService
    {
        public void Trace(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }
    }
}
