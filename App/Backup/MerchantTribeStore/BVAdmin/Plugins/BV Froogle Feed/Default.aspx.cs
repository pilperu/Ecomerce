using System;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using MerchantTribe.Web;

namespace MerchantTribeStore
{

    partial class BVAdmin_Plugins_Default : BaseAdminPage
    {

        const string FroogleComponentId = "9E26F02A-975F-4206-9CB5-EC614FD6725F";

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadSettings();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Froogle/GoogleBase Feed";
            this.CurrentTab = AdminTabType.Plugins;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        private void LoadSettings()
        {
            //this.gbaseUsernameField.Text = settingsManager.GetSetting("GoogleBaseUsername");
            //string pass = settingsManager.GetSetting("GoogleBasePassword");
            //if (pass.Trim().Length > 0) {
            //    this.gbasePasswordField.Text = "**********";
            //}
            //this.gbaseFileNameField.Text = settingsManager.GetSetting("GoogleBaseFileName");
            //if (this.gbaseFileNameField.Text == string.Empty) {
            //    this.gbaseFileNameField.Text = "Froogle.txt";
            //}
            //this.gbaseFtpField.Text = settingsManager.GetSetting("GoogleBaseFtp");
            //if (this.gbaseFtpField.Text == string.Empty) {
            //    this.gbaseFtpField.Text = "ftp://uploads.google.com";
            //}

            //string condition = settingsManager.GetSetting("Condition");
            //if (this.lstCondition.Items.FindByValue(condition) != null) {
            //    this.lstCondition.ClearSelection();
            //    this.lstCondition.Items.FindByValue(condition).Selected = true;
            //}

        }

        private void SaveSettings()
        {
            //settingsManager.SaveSetting("GoogleBaseUsername", this.gbaseUsernameField.Text.Trim(), "bvsoftware", "GoogleBaseFeed", "BVC5Froogle");
            //if (this.gbasePasswordField.Text.Trim() != "**********") {
            //    settingsManager.SaveSetting("GoogleBasePassword", this.gbasePasswordField.Text.Trim(), "bvsoftware", "GoogleBaseFeed", "BVC5Froogle");
            //}
            //settingsManager.SaveSetting("GoogleBaseFileName", this.gbaseFileNameField.Text.Trim(), "bvsoftware", "GoogleBaseFeed", "BVC5Froogle");
            //settingsManager.SaveSetting("GoogleBaseFtp", this.gbaseFtpField.Text.Trim(), "bvsoftware", "GoogleBaseFeed", "BVC5Froogle");
            //if (this.gbasePasswordField.Text != "") {
            //    this.gbasePasswordField.Text = "**********";
            //}
            //settingsManager.SaveSetting("Condition", this.lstCondition.SelectedItem.Value, "bvsoftware", "GoogleBaseFeed", "BVC5Froogle");
        }

        private string SafeString(string input)
        {
            string result = input.Replace("\t", "");
            result = result.Replace(System.Environment.NewLine, " ");
            result = result.Replace("\r", " ");
            result = result.Replace("\n", " ");
            return result;
        }

        private string SafeBool(bool input)
        {
            if (input == true)
            {
                return "True";
            }
            else
            {
                return "False";
            }
        }

        protected void btnGo_Click(object sender, System.EventArgs e)
        {
            this.lblStatus.Text = string.Empty;
            SaveSettings();
            BuildFeed();
        }

        private void BuildFeed()
        {
            this.OutputField.Text = string.Empty;

            int expirationDays = 30;
            DateTime expDate = DateTime.Now.AddDays(expirationDays);
            string expirationDate = expDate.Year + "-";
            if (expDate.Month < 10)
            {
                expirationDate += "0";
            }
            expirationDate += expDate.Month.ToString() + "-";
            if (expDate.Day < 10)
            {
                expirationDate += "0";
            }
            expirationDate += expDate.Day.ToString();

            StringBuilder sb = new StringBuilder();
            WriteHeaders(ref sb);

            List<MerchantTribe.Commerce.Catalog.Product> prods = MTApp.CatalogServices.Products.FindAllPaged(1,3000);
            foreach (MerchantTribe.Commerce.Catalog.Product p in prods)
            {
                if (p.Status == MerchantTribe.Commerce.Catalog.ProductStatus.Active)
                {
                    WriteItem(p, ref sb, expirationDate);
                }
            }

            this.OutputField.Text = sb.ToString();
            this.lblStatus.Text = "Feed Generated!";
        }

        private void WriteHeaders(ref StringBuilder sb)
        {

            sb.Append("c:bvin:string\t");
            sb.Append("id\t");
            sb.Append("title\t");
            sb.Append("description\t");
            sb.Append("image_link\t");
            sb.Append("link\t");
            sb.Append("price\t");
            sb.Append("price_type\t");
            sb.Append("quantity\t");
            sb.Append("payment_accepted\t");
            sb.Append("payment_notes\t");
            sb.Append("shipping\t");
            sb.Append("expiration_date\t");
            sb.Append("actor\t");
            sb.Append("apparel_type\t");
            sb.Append("artist\t");
            sb.Append("author\t");
            sb.Append("brand\t");
            sb.Append("color\t");
            sb.Append("condition\t");
            sb.Append("model_number\t");
            sb.Append("format\t");
            sb.Append("isbn\t");
            sb.Append("memory\t");
            sb.Append("pickup\t");
            sb.Append("processor_speed\t");
            sb.Append("size\t");
            sb.Append("tax_percent\t");
            sb.Append("tax_region\t");
            sb.Append("upc\t");
            sb.Append("product_type\t");
            sb.Append("condition");
            sb.Append(System.Environment.NewLine);


        }

        private void WriteItem(MerchantTribe.Commerce.Catalog.Product p, ref StringBuilder sb, string expirationDate)
        {
            sb.Append(SafeString(p.Bvin));
            sb.Append("\t");
            sb.Append(SafeString(p.Sku));
            sb.Append("\t");
            sb.Append(SafeString(p.ProductName));
            sb.Append("\t");
            sb.Append(MerchantTribe.Web.Text.TrimToLength(SafeString(p.LongDescription), 10000));
            sb.Append("\t");
            sb.Append(RemoteImageUrl(p.ImageFileMedium));
            sb.Append("\t");
            sb.Append(SafeString(RemoteUrl(p)));
            sb.Append("\t");
            sb.Append(SafeString(p.GetCurrentPrice(string.Empty, 0m, null, string.Empty).ToString()));
            sb.Append("\t");
            sb.Append(SafeString("starting"));
            // Price Type
            sb.Append("\t");
            sb.Append(SafeString("1"));
            sb.Append("\t");

            sb.Append(string.Empty);
            // payment_accepted - cash,check,GoogleCheckout,Visa,MasterCard,AmericanExpress,Discover,wiretransfer
            sb.Append("\t");
            sb.Append(string.Empty);
            // payment_notes
            sb.Append("\t");

            List<MerchantTribe.Commerce.Catalog.ProductProperty> props = MTApp.CatalogServices.ProductPropertiesFindForType(p.ProductTypeId);

            sb.Append(SafeString(PropertyMatcher("shipping", props, p.Bvin)));
            sb.Append("\t");

            int expirationDays = 30;
            DateTime expDate = DateTime.Now.AddDays(expirationDays);
            sb.Append(SafeString(expirationDate));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("actor", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("apparel_type", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("artist", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("author", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("brand", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("color", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("condition", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("model_number", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("format", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("isbn", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("memory", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("pickup", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("processor_speed", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("size", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("tax_percent", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("tax_region", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("upc", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(PropertyMatcher("product_type", props, p.Bvin)));
            sb.Append("\t");
            sb.Append(SafeString(lstCondition.SelectedItem.Value));
            sb.Append(System.Environment.NewLine);
        }

        private string RemoteImageUrl(string localImage)
        {
            string temp = localImage.Replace("\\", "/");
            if (temp.ToLower().StartsWith("http://") == false)
            {
                return System.IO.Path.Combine(MTApp.CurrentStore.RootUrl(), temp.TrimStart('/'));
            }
            else
            {
                return temp;
            }
        }

        private string RemoteUrl(MerchantTribe.Commerce.Catalog.Product p)
        {

            string temp = BuildUrlForProduct(p);
            temp = temp.Replace("\\", "/");

            if (temp.ToLower().StartsWith("http://") == false)
            {
                return System.IO.Path.Combine(MTApp.CurrentStore.RootUrl(), temp.TrimStart('/'));
            }
            else
            {
                return temp;
            }
        }

        private string PropertyMatcher(string googleBaseName, List<MerchantTribe.Commerce.Catalog.ProductProperty> props, string productId)
        {
            string result = string.Empty;

            if (props != null)
            {
                for (int i = 0; i <= props.Count - 1; i++)
                {
                    if (props[i].PropertyName.Trim().ToLower() == googleBaseName.Trim().ToLower())
                    {
                        result = MTApp.CatalogServices.ProductPropertyValues.GetPropertyValue(productId, props[i].Id);
                        break;
                    }
                }
            }

            return result;
        }

        protected void btnGenerateAndSend_Click(object sender, System.EventArgs e)
        {
            this.lblStatus.Text = string.Empty;
            SaveSettings();
            BuildFeed();
            SendFeed();
        }

        private void SendFeed()
        {
            try
            {
                FileInfo writerInfo = this.GetTemporaryFileInfo();
                File.WriteAllText(writerInfo.FullName, this.OutputField.Text.Trim());
                UploadFile(writerInfo.FullName, this.gbaseFileNameField.Text.Trim());
                this.lblStatus.Text = "Feed Generated and Sent via FTP!";
            }
            catch (Exception ex)
            {
                this.lblStatus.Text = "Error: " + ex.Message;
            }
        }

        #region " URL Functions "

        private string BuildUrlForProduct(MerchantTribe.Commerce.Catalog.Product p)
        {
            return MerchantTribe.Commerce.Utilities.UrlRewriter.BuildUrlForProduct(p, this);
        }

        private string BuildPhysicalUrlForProduct(MerchantTribe.Commerce.Catalog.Product p)
        {
            return MerchantTribe.Commerce.Utilities.UrlRewriter.BuildUrlForProduct(p, this.Page);
        }

        private string CleanNameForUrl(string input)
        {
            string result = input;

            result = result.Replace(" ", "-");
            result = result.Replace("\"", "");
            result = result.Replace("&", "and");
            result = result.Replace("?", "");
            result = result.Replace("=", "");
            result = result.Replace("/", "");
            result = result.Replace("\\", "");
            result = result.Replace("%", "");
            result = result.Replace("#", "");
            result = result.Replace("*", "");
            result = result.Replace("!", "");
            result = result.Replace("$", "");
            result = result.Replace("+", "-plus-");
            result = result.Replace(",", "-");
            result = result.Replace("@", "-at-");
            result = result.Replace(":", "-");
            result = result.Replace(";", "-");
            result = result.Replace(">", "");
            result = result.Replace("<", "");
            result = result.Replace("{", "");
            result = result.Replace("}", "");
            result = result.Replace("~", "");
            result = result.Replace("|", "-");
            result = result.Replace("^", "");
            result = result.Replace("[", "");
            result = result.Replace("]", "");
            result = result.Replace("`", "");
            result = result.Replace("'", "");
            result = result.Replace("�", "");
            result = result.Replace("�", "");
            result = result.Replace("�", "");
            result = result.Replace(".", "");

            return result;
        }
        #endregion
        #region " FTP Classes "

        private bool UploadFile(string localFilename, string targetFilename)
        {
            //1. check source
            if (!File.Exists(localFilename))
            {
                throw new ApplicationException("File " + localFilename + " not found");
            }
            //copy to FI
            FileInfo fi = new FileInfo(localFilename);
            return Upload(fi, targetFilename);
        }

        private bool Upload(FileInfo fi, string targetFilename)
        {
            //copy the file specified to target file: target file can be full path or just filename (uses current dir)

            string host = this.gbaseFtpField.Text.Trim();
            if (host.EndsWith("/") == false)
            {
                host = host + "/";
            }
            string URI = host + targetFilename;
            //perform copy
            System.Net.FtpWebRequest ftp = GetRequest(URI);

            //Set request to upload a file in binary
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftp.UseBinary = true;

            //Notify FTP of the expected size
            ftp.ContentLength = fi.Length;

            //create byte array to store: ensure at least 1 byte!
            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize - 1];
            int dataRead;

            //open file for reading 
            using (FileStream fs = fi.OpenRead())
            {
                try
                {
                    //open request to send
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        do
                        {
                            dataRead = fs.Read(content, 0, BufferSize);
                            rs.Write(content, 0, dataRead);
                        }
                        while (!(dataRead < BufferSize));
                        rs.Close();
                    }
                }
                catch (Exception ex)
                {
                    EventLog.LogEvent(ex);
                }

                finally
                {
                    //ensure file closed
                    fs.Close();
                }

            }
            ftp = null;
            return true;
        }

        private System.Net.FtpWebRequest GetRequest(string URI)
        {
            //create request
            System.Net.FtpWebRequest result = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create(URI);
            //Set the login details
            result.Credentials = GetCredentials();
            //Do not keep alive (stateless mode)
            result.KeepAlive = false;
            return result;
        }

        private System.Net.ICredentials GetCredentials()
        {
            string pass = this.gbasePasswordField.Text.Trim();
            //if (this.gbasePasswordField.Text.Trim() == "**********") {
            //    pass = settingsManager.GetSetting("GoogleBasePassword");
            //}
            return new System.Net.NetworkCredential(this.gbaseUsernameField.Text.Trim(), pass);
        }
        #endregion
        #region " File Helpers "

        private FileInfo GetTemporaryFileInfo()
        {
            string tempFileName;
            FileInfo myFileInfo;
            try
            {
                tempFileName = Path.GetTempFileName();
                myFileInfo = new FileInfo(tempFileName);
                myFileInfo.Attributes = FileAttributes.Temporary;
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                return null;
            }
            return myFileInfo;
        }
        #endregion


    }

}