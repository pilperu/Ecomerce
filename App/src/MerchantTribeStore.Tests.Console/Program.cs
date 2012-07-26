using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribeStore.Tests;

namespace MerchantTribeStore.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {            
            var testSuite = new MerchantTribeStore.Tests.Code.TemplateEngine.ProcessorTest();
            testSuite.Setup();
            testSuite.SpeedTestRenderActions();
        }
    }
}
