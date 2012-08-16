using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MerchantTribe.Commerce.Storage;

namespace MerchantTribeStore.Controllers
{
    public class FileUploadController : Shared.BaseAdminController
    {

        public enum FileUploadType
        {
            Unknown = -1,
            FlexPageImage = 1
        }

        // POST: /FileUploadHandler/{typecode}/{*details}
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string typecode, string details)
        {
            JsonResult result = new JsonResult();
            result.Data = new { success = 0 };

            // Get Upload Type
            FileUploadType uploadType = FileUploadType.Unknown;
            Enum.TryParse<FileUploadType>(typecode, out uploadType);

            string fileName = Request.QueryString["filename"];
            string firstPart = Request.QueryString["firstpart"];
            bool isFirstPart = false;
            if (firstPart == "1") isFirstPart = true;

            Stream inputStream = Request.InputStream;

            switch (uploadType)
            {
                case FileUploadType.FlexPageImage:

                    string[] detailParts = details.Split('/');
                    if (detailParts.Length > 1)
                    {
                        string categoryId = detailParts[0];
                        string pageVersion = detailParts[1];

                        if (DiskStorage.UploadFlexPageImagePartial(MTApp.CurrentStore.Id, inputStream, fileName, isFirstPart, categoryId, pageVersion) == true)
                        {                            
                            result.Data = new { success = "1", 
                                                imageurl=DiskStorage.FlexPageImageUrl(MTApp, categoryId, pageVersion, fileName, false),
                                                filename=MerchantTribe.Web.Text.CleanFileName(fileName)};
                        }
                        else
                        {
                            result.Data = new { success = "0", 
                                                imageurl=DiskStorage.FlexPageImageUrl(MTApp, categoryId, pageVersion, string.Empty, false),
                                                filename=string.Empty};
                        }                                                
                    }                    
                    break;
            }
                                    
            return result;
        }

    }
}
