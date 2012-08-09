using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.Actions
{
    public class LiteralText: ITemplateAction
    {
        private string Text { get; set; }

        public LiteralText(string text)
        {            
            this.Text = text;            
        }

        public string Render()
        {
            return this.Text;
        }
    }
}