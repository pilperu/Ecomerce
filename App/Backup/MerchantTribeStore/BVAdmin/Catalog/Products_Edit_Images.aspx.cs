using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class products_products_edit_images : BaseProductAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Product Images";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Response.Cache.SetExpires(System.DateTime.UtcNow.ToLocalTime());
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetValidUntilExpires(true);
            
            if (!Page.IsPostBack)
            {
                this.ProductIdField.Value = Request.QueryString["id"];                
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "imagesorter", RenderJQuery(this.ProductIdField.Value), true);

            LoadImages();
        }

        private string RenderJQuery(string productBvin)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("$(document).ready(function() {");

            sb.Append("$(\"#dragitem-list\").sortable({");
            sb.Append("placeholder:  'ui-state-highlight',");
            sb.Append("axis:   'y',");
            sb.Append("opacity:  '0.75',");
            sb.Append("cursor:  'move',");
            sb.Append("update: function(event, ui) {");
            sb.Append(" var sorted = $(this).sortable('toArray');");
            sb.Append(" sorted += '';");
            sb.Append("$.post('Products_Edit_Images_Sort.aspx',");
            sb.Append("  { \"ids\": sorted,");
            sb.Append("    \"bvin\": \"" + productBvin + "\"");
            sb.Append("  });");
            sb.Append(" }");
            sb.Append("});");

            sb.Append("$('#dragitem-list').disableSelection();");

            sb.Append("$('.trash').click(function() {");          
            sb.Append(" RemoveItem($(this));");
            sb.Append(" return false;");
            sb.Append("});");

            sb.Append("});");
            // End of Document Ready

            sb.Append("function RemoveItem(lnk) {");
            sb.Append("  var id = $(lnk).attr('id');");
            sb.Append("$.post('Products_Edit_Images_Delete.aspx',");
            sb.Append("  { \"id\": id.replace('rem', ''),");
            sb.Append("  \"bvin\": \"" + productBvin + "\"");
            sb.Append("  },");
            sb.Append("  function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().slideUp('slow', function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().remove();});");
            sb.Append("  }");
            sb.Append(" );");
            sb.Append("}");

            return sb.ToString();
        }

        private void LoadImages()
        {
            List<ProductImage> images = MTApp.CatalogServices.ProductImages.FindByProductId(this.ProductIdField.Value);

            RenderImages(images);
        }

        private void RenderImages(List<ProductImage> images)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append("<div id=\"dragitem-list\">");
                      
            foreach (ProductImage img in images)
            {
                RenderSingleItem(sb, img);
                
            }

            sb.Append("</div>");

            this.litImages.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, ProductImage img)
        {        
            //string destinationLink = "ProductChoices_Edit.aspx?cid=" + o.Bvin + "&id=" + productBvin;
            string destinationLink = "#";

            sb.Append("<div class=\"dragitem\" id=\"img" + img.Bvin + "\"><table class=\"formtable\" width=\"100%\"><tr>");
            sb.Append("<td width=\"25%\"><a href=\"" + destinationLink + "\">");            
            sb.Append("<img src=\"");
            sb.Append(MerchantTribe.Commerce.Storage.DiskStorage.ProductAdditionalImageUrlTiny(MTApp,
                                                                                                      img.ProductId,
                                                                                                      img.Bvin,
                                                                                                      img.FileName,
                                                                                                      true));
            sb.Append("\" border=\"0\" alt=\"" + img.AlternateText + "\" />");
            sb.Append("</a></td>");

            sb.Append("<td>");
            sb.Append(img.Caption);            
            sb.Append("</td>");

            //sb.Append("<td width=\"75\"><a href=\"" + destinationLink + "\"><img src=\"../images/buttons/edit.png\" alt=\"edit\" /></a></td>");

            sb.Append("<td width=\"30\"><a href=\"#\" class=\"trash\" id=\"rem" + img.Bvin + "\"");            
            sb.Append("><img src=\"../../images/system/trashcan.png\" alt=\"Delete\" /></a></td>");
            sb.Append("<td width=\"30\"><a href=\"#\" class=\"handle\"><img src=\"../../images/system/draghandle.png\" alt=\"Move\" /></a></td>");

            sb.Append("</tr></table></div>");
        }

        //private void LoadEditor(string bvin)
        //{
        //    ProductImage pi = ProductImage.FindByBvin(bvin);
        //    btnSave.Visible = false;
        //    btnUpdate.Visible = true;
        //    LoadEditorWithImage(pi);
        //}

        //private void LoadEditorWithImage(ProductImage pi)
        //{
        //    this.EditBvin.Value = pi.Bvin;
        //    this.FileNameField.Text = pi.FileName;
        //    this.CaptionField.Text = pi.Caption;
        //    this.AlternateTextField.Text = pi.AlternateText;

        //    if (File.Exists(Path.Combine(Request.PhysicalApplicationPath, pi.FileName)) == true)
        //    {
        //        ImageInfo info;
        //        info = ImageHelper.GetImageInformation(Path.Combine(Request.PhysicalApplicationPath, pi.FileName));
        //        ImageInfo maxInfo = ImageHelper.GetProportionalImageDimensionsForImage(info, 220, 220);
        //        imgPreview.Width = maxInfo.Width;
        //        imgPreview.Height = maxInfo.Height;
        //        string pictureUrl = "~/" + pi.FileName;
        //        imgPreview.ImageUrl = pictureUrl.Replace("\\", "/");
        //    }
        //    else
        //    {
        //        imgPreview.ImageUrl = "~/BVAdmin/images/NoPreview.gif";
        //        imgPreview.Width = Unit.Pixel(110);
        //        imgPreview.Height = Unit.Pixel(110);
        //    }

        //}

        //private void ClearEditor()
        //{
        //    this.EditBvin.Value = string.Empty;
        //    this.FileNameField.Text = string.Empty;
        //    this.CaptionField.Text = string.Empty;
        //    this.AlternateTextField.Text = string.Empty;
        //    imgPreview.ImageUrl = "~/BVAdmin/images/NoPreview.gif";
        //    imgPreview.Width = Unit.Pixel(110);
        //    imgPreview.Height = Unit.Pixel(110);
        //}

        //protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    pnlEditor.Visible = true;
        //    msg.ClearMessage();
        //    btnSave.Visible = true;
        //    btnUpdate.Visible = false;
        //    //Dim pi As New Catalog.ProductImage
        //    //pi.ProductID = Me.ProductIdField.Value
        //    //pi.FileName = ""
        //    //pi.Caption = "New Image"
        //    //If Catalog.ProductImage.Insert(pi) = True Then
        //    //    LoadEditorWithImage(pi)
        //    //    Me.LoadImages()
        //    //Else
        //    //    msg.ShowError("Couldn't Save New Product Image!")
        //    //End If
        //}

        //protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    pnlEditor.Visible = false;
        //    msg.ClearMessage();
        //    ClearEditor();
        //    LoadImages();
        //}

        //protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {

        //        ProductImage pi = (ProductImage)e.Row.DataItem;

        //        System.Web.UI.WebControls.Image preview;
        //        preview = (System.Web.UI.WebControls.Image)e.Row.Cells[0].FindControl("Image1");
        //        if (preview != null)
        //        {
        //            if (File.Exists(Path.Combine(Request.PhysicalApplicationPath, pi.FileName)) == true)
        //            {
        //                ImageInfo info;
        //                info = ImageHelper.GetImageInformation(Path.Combine(Request.PhysicalApplicationPath, pi.FileName));
        //                ImageInfo maxInfo = ImageHelper.GetProportionalImageDimensionsForImage(info, 110, 110);
        //                preview.Width = maxInfo.Width;
        //                preview.Height = maxInfo.Height;
        //                string pictureUrl = "~/" + pi.FileName;
        //                preview.ImageUrl = pictureUrl.Replace("\\", "/");
        //            }
        //            else
        //            {
        //                preview.ImageUrl = "~/Bvadmin/images/NoPreview.gif";
        //                preview.Width = Unit.Pixel(110);
        //                preview.Height = Unit.Pixel(110);
        //            }

        //        }

        //        ImageButton btn = (ImageButton)e.Row.FindControl("btnUp");
        //        btn.CommandArgument = pi.Bvin;

        //        btn = (ImageButton)e.Row.FindControl("btnDown");
        //        btn.CommandArgument = pi.Bvin;
        //    }
        //}

        //protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        //{
        //    msg.ClearMessage();
        //    string bvin = (string)GridView1.DataKeys[e.RowIndex].Value;
        //    ProductImage.Delete(bvin);
        //    LoadImages();
        //}

        //protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        //{
        //    msg.ClearMessage();
        //    string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
        //    pnlEditor.Visible = true;
        //    this.LoadEditor(bvin);

        //}

        //protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        Save();
        //    }
        //}

        protected override bool Save()
        {
            return true;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // Image Upload
            if ((this.imgupload.HasFile))
            {

                string fileName = System.IO.Path.GetFileNameWithoutExtension(imgupload.FileName);
                string ext = System.IO.Path.GetExtension(imgupload.FileName);

                if (MerchantTribe.Commerce.Storage.DiskStorage.ValidateImageType(ext))
                {
                    fileName = MerchantTribe.Web.Text.CleanFileName(fileName);

                    ProductImage img = new ProductImage();
                    img.Bvin = System.Guid.NewGuid().ToString();

                    if (MerchantTribe.Commerce.Storage.DiskStorage.UploadAdditionalProductImage(MTApp.CurrentStore.Id, this.ProductIdField.Value, img.Bvin, this.imgupload.PostedFile))
                    {
                        img.AlternateText = fileName + ext;
                        img.FileName = fileName + ext;
                        img.Caption = string.Empty;
                        img.ProductId = this.ProductIdField.Value;                        
                        if (MTApp.CatalogServices.ProductImages.Create(img))
                        {
                            this.MessageBox1.ShowOk("New Image Added at " + DateTime.Now.ToString() + ".");
                        }
                        else
                        {
                            this.MessageBox1.ShowWarning("Unable to save image record. Unknown error.");
                        }
                    }
                    else
                    {
                        this.MessageBox1.ShowWarning("Unable to save image. Unknown error.");
                    }

                    LoadImages();
                }
                else
                {
                    this.MessageBox1.ShowError("Only .PNG, .JPG, .GIF file types are allowed for icon images");                    
                }
            }
        }

        //protected override bool Save()
        //{
        //    bool result = false;
        //    if (btnSave.Visible)
        //    {
        //        ProductImage pi = new ProductImage();
        //        pi.Bvin = this.EditBvin.Value;
        //        pi.ProductId = this.ProductIdField.Value;
        //        pi.AlternateText = this.AlternateTextField.Text;
        //        pi.Caption = this.CaptionField.Text;
        //        pi.FileName = this.FileNameField.Text;
        //        if (ProductImage.Insert(pi) == true)
        //        {
        //            this.ClearEditor();
        //            msg.ShowOk("Changes Saved");
        //            result = true;
        //        }
        //        else
        //        {
        //            msg.ShowError("Error while saving changes");
        //        }
        //        LoadImages();
        //        pnlEditor.Visible = false;
        //    }
        //    else
        //    {
        //        result = true;
        //    }

        //    return result;
        //}

        //private void RegisterWindowScripts()
        //{

        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("var w;");
        //    sb.Append("function popUpWindow(parameters) {");
        //    sb.Append("w = window.open(parameters, 'imageBrowser', 'resizable=yes, scrollbars=yes,height=480, width=640');");
        //    sb.Append("w.focus();");
        //    sb.Append("}");

        //    sb.Append("function closePopup() {");
        //    sb.Append("w.close();");
        //    sb.Append("self.focus();");
        //    sb.Append("}");

        //    sb.Append("function SetEditorImage(fileName) {");
        //    sb.Append("document.getElementById('");
        //    sb.Append(this.FileNameField.ClientID);
        //    sb.Append("').value = fileName;");
        //    sb.Append("document.getElementById('");
        //    sb.Append(this.imgPreview.ClientID);
        //    sb.Append("').src = '../../'+fileName;");
        //    sb.Append("w.close();");
        //    sb.Append("}");

        //    Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "WindowScripts", sb.ToString(), true);

        //}

        //protected void btnUpdate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    ProductImage pi = new ProductImage();
        //    pi.Bvin = this.EditBvin.Value;
        //    pi.ProductId = this.ProductIdField.Value;
        //    pi.AlternateText = this.AlternateTextField.Text;
        //    pi.Caption = this.CaptionField.Text;
        //    pi.FileName = this.FileNameField.Text;
        //    if (ProductImage.Update(pi) == true)
        //    {
        //        this.ClearEditor();
        //        msg.ShowOk("Changes Saved");
        //    }
        //    else
        //    {
        //        msg.ShowError("Error while saving changes");
        //    }
        //    LoadImages();
        //    pnlEditor.Visible = false;
        //}

        //protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Up")
        //    {
        //        ProductImage img = ProductImage.FindByBvin((string)e.CommandArgument);
        //        if (img != null && img.Bvin != string.Empty)
        //        {
        //            ProductImage.MoveUp(img);
        //        }
        //        LoadImages();
        //    }
        //    else if (e.CommandName == "Down")
        //    {
        //        ProductImage img = ProductImage.FindByBvin((string)e.CommandArgument);
        //        if (img != null && img.Bvin != string.Empty)
        //        {
        //            ProductImage.MoveDown(img);
        //        }
        //        LoadImages();
        //    }
        //}
    }
}