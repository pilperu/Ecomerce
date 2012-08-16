using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MerchantTribeStore.Controllers
{
    public class PreJsonResult: ActionResult
    {
        private string data = string.Empty;

        public PreJsonResult(string json)
        {
            data = json;
        }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.Output.Write(data);
            }
        
    }
}