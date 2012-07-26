using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantTribe.Commerce;
using MerchantTribeStore.code.TemplateEngine;
using MerchantTribeStore.code.TemplateEngine.Actions;

namespace MerchantTribeStore.Tests.Code.TemplateEngine
{
    [TestClass]
    public class ProcessorTest
    {
        RequestContext context;
        MerchantTribeApplication app;
        ITagProvider tagProvider;

        [TestInitialize]
        public void Setup()
        {
            WebAppSettings.SetUnitTestPhysicalPath(@"C:\git\MerchantTribe\App\MerchantTribeStore");
                        
            context = ContextHelper.GetFakeRequestContext("", "http://demo.localhost.dev/", "");            
            app = MerchantTribeApplication.InstantiateForMemory(context);                        

            tagProvider = new TagProvider();
        }

        [TestMethod]
        public void CanProcessTemplateWithNoTags()
        {
            string template = "<html>\n<head><title>Page Title</title></head>\n<body><h1>My Page</h1></body>\n</html>\n";
            
            Processor target = new Processor(app, template, tagProvider);

            var actual = target.RenderForDisplay();

            List<ITemplateAction> expected = new List<ITemplateAction>();
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("<html>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("\n"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("<head>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("<title>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("Page Title"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("</title>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("</head>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("\n"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("<body>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("<h1>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("My Page"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("</h1>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("</body>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("\n"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("</html>"));
            expected.Add(new MerchantTribeStore.code.TemplateEngine.Actions.LiteralText("\n"));

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(TemplateActionType.Custom, actual[4].ActionType());
            Assert.AreEqual("Page Title", actual[4].RenderCustom());
        }

        [TestMethod]
        public void CanProcessTemplateWithTag()
        {
            string template = "<html><sys:adminpanel /></html>";

            Processor target = new Processor(app, template, tagProvider);

            var actual = target.RenderForDisplay();

            List<ITemplateAction> expected = new List<ITemplateAction>();
            expected.Add(new LiteralText("<html>"));
            expected.Add(new PartialView("~/views/shared/_adminpanel.cshtml", null));
            expected.Add(new LiteralText("</html>"));            

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(TemplateActionType.PartialView, actual[1].ActionType());
            Assert.AreEqual("~/views/shared/_adminpanel.cshtml", ((PartialView)actual[1]).ViewName);
        }

        [TestMethod]
        public void SpeedTestTokenization()
        {            
            long count = 10000;
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            for (long i = 0; i < count; i++)
            {
                string template = app.ThemeManager().GetSystemTemplate("default.html");
                Processor target = new Processor(app, template, tagProvider);
                var tokens = target.Tokenize();
            }

            watch.Stop();
            decimal avg = ((decimal)watch.ElapsedMilliseconds / (decimal)count);
            Console.WriteLine("Avg Milliseconds = " + avg);            
        }

        [TestMethod]
        public void SpeedTestRenderActions()
        {
            long count = 1;
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            for (long i = 0; i < count; i++)
            {
                string template = app.ThemeManager().GetSystemTemplate("default.html");
                Processor target = new Processor(app, template, tagProvider);
                var actions = target.RenderForDisplay();
            }

            watch.Stop();
            decimal avg = ((decimal)watch.ElapsedMilliseconds / (decimal)count);
            Console.WriteLine("Avg Milliseconds = " + avg);
        }
    }
}
