using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{
    public class ViewUtilities
    {
        //public enum Sizes
        //{
        //    Small = 0,
        //    Medium = 1
        //}

        //public static void ForceImageSize(Control image, string imageFile, Sizes size, Page currentPage)
        //{
        //    // Force Image Size Code
        //    if (WebAppSettings.ForceImageSizes) {
        //        ImageInfo resizeInfo = new ImageInfo();
        //        if (imageFile.ToLower().StartsWith("http://")) {
        //            if (size == Sizes.Small) {
        //                resizeInfo.Width = WebAppSettings.ImagesSmallWidth;
        //                resizeInfo.Height = WebAppSettings.ImagesSmallHeight;
        //            }
        //            else if (size == Sizes.Medium) {
        //                resizeInfo.Width = WebAppSettings.ImagesMediumWidth;
        //                resizeInfo.Height = WebAppSettings.ImagesMediumHeight;
        //            }
        //        }
        //        else {
        //            ImageInfo iInfo = ImageHelper.GetImageInformation(currentPage.ResolveUrl(ImageHelper.GetValidImage(imageFile, true)));
        //            if (iInfo.Width == 0 & iInfo.Height == 0) {
        //                if (size == Sizes.Small) {
        //                    iInfo.Width = WebAppSettings.ImagesSmallWidth;
        //                    iInfo.Height = WebAppSettings.ImagesSmallHeight;
        //                }
        //                else if (size == Sizes.Medium) {
        //                    iInfo.Width = WebAppSettings.ImagesMediumWidth;
        //                    iInfo.Height = WebAppSettings.ImagesMediumHeight;
        //                }
        //            }
        //            if (size == Sizes.Small) {
        //                resizeInfo = ImageHelper.GetProportionalImageDimensionsForImage(iInfo, WebAppSettings.ImagesSmallWidth, WebAppSettings.ImagesSmallHeight);
        //            }
        //            else if (size == Sizes.Medium) {
        //                resizeInfo = ImageHelper.GetProportionalImageDimensionsForImage(iInfo, WebAppSettings.ImagesMediumWidth, WebAppSettings.ImagesMediumHeight);
        //            }

        //        }

        //        if (image is System.Web.UI.HtmlControls.HtmlImage) {
        //            System.Web.UI.HtmlControls.HtmlImage img = (System.Web.UI.HtmlControls.HtmlImage)image;
        //            img.Width = resizeInfo.Width;
        //            img.Height = resizeInfo.Height;
        //        }
        //        else if (image is System.Web.UI.WebControls.Image) {
        //            Image img = (Image)image;
        //            img.Width = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Width);
        //            img.Height = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Height);
        //        }
        //        else if (image is System.Web.UI.WebControls.ImageButton)
        //        {
        //            ImageButton img = (ImageButton)image;
        //            img.Width = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Width);
        //            img.Height = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Height);
        //        }
        //        else if (image is System.Web.UI.WebControls.HyperLink)
        //        {
        //            HyperLink link = (HyperLink)image;
        //            link.Width = resizeInfo.Width;
        //            link.Height = resizeInfo.Height;
        //        }
        //        else {
        //            throw new ArgumentException("Force Image Size Does Not Support A Control Of Type " + typeof(Image).Name);
        //        }
        //    }
        //}


        public static bool DownloadFile(MerchantTribe.Commerce.Catalog.ProductFile file, MerchantTribeApplication app)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);
            string name = System.IO.Path.GetFileName(file.FileName);
            double fileSize = 0;

            long storeId = app.CurrentRequestContext.CurrentStore.Id;
            string diskFileName = file.Bvin + "_" + file.FileName + ".config";
            if (!MerchantTribe.Commerce.Storage.DiskStorage.FileVaultFileExists(storeId, diskFileName)) return false;

            byte[] bytes = MerchantTribe.Commerce.Storage.DiskStorage.FileVaultGetBytes(storeId, diskFileName);

            string type = "";
            type = MerchantTribe.Commerce.Utilities.MimeTypes.FindTypeForExtension(extension);

            //System.IO.FileInfo f = new System.IO.FileInfo(filePath);
            fileSize = bytes.Length;// f.Length;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            if (type != "")
            {
                HttpContext.Current.Response.ContentType = type;
            }

            //If type = "application/octet-stream" Then
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + name);
            //End If

            if (fileSize > 0)
            {
                HttpContext.Current.Response.AddHeader("Content-Length", fileSize.ToString());
            }

            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

            return true;
        }
    }
}