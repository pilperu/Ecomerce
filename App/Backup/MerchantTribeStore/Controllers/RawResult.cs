using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MerchantTribeStore.Controllers
{
    public class RawResult : ActionResult
    {
        private string data = string.Empty;
        private string contentType = "text/html";

        public RawResult(string content, string contenttype)
        {
            data = content;
            contentType = contenttype;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = contentType;
            context.HttpContext.Response.Output.Write(data);
        }

    }
}
