using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public class FlexPageEditorViewModel
    {
        public bool IsEditMode { get; set; }        
        public string CurrentPageUrl { get; set; }
        public string CategoryId { get; set; }
        public bool IsPreview { get; set; }
    }

}