using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public enum TwitterCountBoxPosition
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2
    }

    public class TwitterRelatedAccount
    {
        public string TwitterHandle { get; set; }
        public string Description { get; set; }

        public TwitterRelatedAccount()
        {
            this.TwitterHandle = string.Empty;
            this.Description = string.Empty;
        }
    }
    public class TwitterViewModel
    {
        public string LinkUrl { get; set; }
        public string DisplayUrl { get; set; }
        public string DefaultText { get; set; }
        public string ViaTwitterName { get; set; }
        public List<TwitterRelatedAccount> RelatedAccounts { get; set; }
        public TwitterCountBoxPosition CountPosition { get; set; }
        public string Language { get; set; }

        public TwitterViewModel()
        {
            LinkUrl = string.Empty;
            DisplayUrl = string.Empty;
            DefaultText = string.Empty;
            ViaTwitterName = string.Empty;
            this.RelatedAccounts = new List<TwitterRelatedAccount>();
            CountPosition = TwitterCountBoxPosition.Horizontal;
            Language = "en";
        }

    }
}