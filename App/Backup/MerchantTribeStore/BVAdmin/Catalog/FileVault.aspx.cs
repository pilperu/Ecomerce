using System;
using System.IO;
using System.Web;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_FileVault : BaseAdminPage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                BindFileGridView();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "File Vault";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected void BindFileGridView()
        {
            List<ProductFile> files = MTApp.CatalogServices.ProductFiles.FindAll(1, int.MaxValue);
            RenderFiles(files);
        }

        private void RenderFiles(List<ProductFile> files)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ProductFile f in files)
            {
                int productCount = MTApp.CatalogServices.ProductFiles.CountOfProductsUsingFile(f.Bvin);
                sb.Append("<tr>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(f.FileName) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(f.ShortDescription) + "</td>");
                sb.Append("<td>" + productCount + "</td>");
                sb.Append("<td><a href=\"FileVaultDelete.aspx?id=" + HttpUtility.UrlEncode(f.Bvin) + "\" onclick=\"return window.confirm('Delete this file?');\">Delete</a></td>");
                sb.Append("<td><a href=\"FileVaultDetailsView.aspx?id=" + HttpUtility.UrlEncode(f.Bvin) + "\"><img src=\"../images/buttons/edit.png\" alt=\"Edit\"><a></td>");
                sb.Append("</tr>");
            }

            this.litFiles.Text = sb.ToString();
        }
        
        //protected void FileGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        //{
        //    string key = (string)FileGridView.DataKeys[e.RowIndex].Value;
        //    ProductFile.Delete(key, Server.MapPath(Request.ApplicationPath));
        //    BindFileGridView();
        //    e.Cancel = true;
        //}

        protected void AddNewImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                FilePicker.DownloadOrLinkFile(null, MessageBox1);
                BindFileGridView();
            }
        }
         
        protected void ImportLinkButton_Click(object sender, System.EventArgs e)
        {
            string[] files = System.IO.Directory.GetFiles(Server.MapPath(Request.ApplicationPath) + "\\Files\\");
            bool errorOccurred = false;
            foreach (string fileName in files)
            {
                if (!fileName.ToLower().EndsWith(".config"))
                {
                    ProductFile file = new ProductFile();
                    file.FileName = System.IO.Path.GetFileName(fileName);
                    file.ShortDescription = file.FileName;
                    if (MTApp.CatalogServices.ProductFiles.Create(file))
                    {
                        try
                        {
                            System.IO.FileStream fileStream = new System.IO.FileStream(fileName, FileMode.Open);
                            try
                            {
                                if (ProductFile.SaveFile(MTApp.CurrentStore.Id, file.Bvin, file.FileName, fileStream))
                                {
                                    fileStream.Close();
                                    try
                                    {
                                        File.Delete(fileName);
                                    }
                                    catch (Exception ex)
                                    {
                                        errorOccurred = true;
                                        EventLog.LogEvent(ex);
                                    }
                                }
                            }
                            finally
                            {
                                try
                                {
                                    fileStream.Close();
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorOccurred = true;
                            EventLog.LogEvent(ex);
                        }
                    }
                }
            }

            if (errorOccurred)
            {
                MessageBox1.ShowError("An error occurred during import. Please check the event log.");
            }
            else
            {
                MessageBox1.ShowOk("Files Imported Successfully.");
            }

            BindFileGridView();
        }
    }
}