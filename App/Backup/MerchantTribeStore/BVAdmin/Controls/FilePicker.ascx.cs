using System;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_FilePicker : MerchantTribe.Commerce.Content.BVUserControl
    {

        private string _productId = string.Empty;
        public string ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public bool DisplayShortDescription
        {
            get { return ShortDescriptionRow.Visible; }
            set { ShortDescriptionRow.Visible = value; }
        }

        private enum Mode
        {
            NewUpload = 1,
            DropDownList = 2,
            FileBrowsed = 3
        }

        private Mode _currentMode = Mode.NewUpload;

        protected void FileHasBeenSelectedCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (!NewFileUpload.HasFile)
            {
                if (FilesDropDownList.Visible)
                {
                    if (FilesDropDownList.SelectedIndex == 0)
                    {
                        args.IsValid = false;
                    }
                }
                else if (FileSelectedTextBox.Visible)
                {
                    if (FileSelectedTextBox.Text.Trim() == string.Empty)
                    {
                        args.IsValid = false;
                    }
                }
                else
                {
                    //if we got here, then somehow FilesDropDownList and FileSelectedTextBox are both not visible
                    args.IsValid = false;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RegisterWindowScripts();
            NewFileUpload.Attributes.Add("onchange", "FillFilename(this, ['" + ShortDescriptionTextBox.ClientID + "']);");
            FilesDropDownList.Attributes.Add("onchange", "FillFilename(this, ['" + ShortDescriptionTextBox.ClientID + "']);");
            if (!Page.IsPostBack)
            {
                InitializeFileLists();
            }
            if (NewFileUpload.HasFile)
            {
                _currentMode = Mode.NewUpload;
            }
            else if (FilesDropDownList.Visible)
            {
                _currentMode = Mode.DropDownList;
            }
            else if (FileSelectedTextBox.Visible)
            {
                _currentMode = Mode.FileBrowsed;
            }

            if (this.ProductId == string.Empty)
            {
                AvailableMinutesRow.Visible = false;
                NumberDownloadsRow.Visible = false;
            }
            else
            {
                AvailableMinutesRow.Visible = true;
                NumberDownloadsRow.Visible = true;
            }
        }

        protected void InitializeFileLists()
        {
            if (MyPage.MTApp.CatalogServices.ProductFiles.CountOfAll() > 30)
            {
                FilesDropDownList.Visible = false;
                FileSelectedTextBox.Visible = true;
                browseButton.Visible = true;
            }
            else
            {
                List<ProductFile> files = MyPage.MTApp.CatalogServices.ProductFiles.FindAll(1, 1000);
                FilesDropDownList.Visible = true;
                FileSelectedTextBox.Visible = false;
                browseButton.Visible = false;

                System.Web.UI.WebControls.ListItem item = (System.Web.UI.WebControls.ListItem)FilesDropDownList.Items[0];
                FilesDropDownList.Items.Clear();
                FilesDropDownList.Items.Add(item);

                FilesDropDownList.DataSource = files;
                FilesDropDownList.DataTextField = "CombinedDisplay";
                FilesDropDownList.DataValueField = "Bvin";
                FilesDropDownList.DataBind();
            }
        }

        private void RegisterWindowScripts()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("var w;");
            sb.Append("function popUpWindow(parameters) {");
            sb.Append("w = window.open('FileDownloadBrowser.aspx' + parameters, null, 'height=480, width=640');");
            sb.Append("}");

            sb.Append("function closePopup(id, shortDescription, fileName) {");
            sb.Append("w.close();");
            if (FileSelectedTextBox.Visible)
            {
                sb.Append("document.getElementById('" + FileSelectedTextBox.ClientID + "').value = fileName;");
            }
            if (ShortDescriptionTextBox.Visible)
            {
                sb.Append("document.getElementById('" + ShortDescriptionTextBox.ClientID + "').value = shortDescription;");
            }
            sb.Append("document.getElementById('" + FileIdHiddenField.ClientID + "').value = id;");
            sb.Append("}");

            sb.Append("String.prototype.trim = function() {");
            sb.Append("    a = this.replace(/^\\s+/, '');");
            sb.Append("    return a.replace(/\\s+$/, '');");
            sb.Append("};");

            sb.Append("function FillFilename(control, FieldsToFill){");
            sb.Append("    if (control.type == \"select-one\"){");
            sb.Append("        if (control.selectedIndex != 0){");
            sb.Append("            for(i = 0; i < FieldsToFill.length; i++){");
            sb.Append("                document.getElementById(FieldsToFill[i]).value = control.options[control.selectedIndex].text.split(\"[\")[0].trim();");
            sb.Append("            }");
            sb.Append("        } else {");
            sb.Append("            for(i = 0; i < FieldsToFill.length; i++){");
            sb.Append("                document.getElementById(FieldsToFill[i]).value = \"\";");
            sb.Append("            }");
            sb.Append("        }");
            sb.Append("    } else {");
            sb.Append("        var arr = control.value.split(\"\\\\\");");
            sb.Append("        for(i = 0; i < FieldsToFill.length; i++){");
            sb.Append("            document.getElementById(FieldsToFill[i]).value = arr[arr.length - 1];");
            sb.Append("        }");
            sb.Append("    }");
            sb.Append("}");

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "WindowScripts", sb.ToString(), true);
        }

        protected void DescriptionIsUniqueToProductCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = true;
            ProductFile file = null;
            if (_currentMode == Mode.DropDownList)
            {
            }
            //file = Catalog.ProductFile.FindByBvin(FilesDropDownList.SelectedValue)
            else if (_currentMode == Mode.FileBrowsed)
            {
            }
            //file = Catalog.ProductFile.FindByBvin(FileIdHiddenField.Value)
            else if (_currentMode == Mode.NewUpload)
            {
                file = new ProductFile();
            }
            if (file != null)
            {
                InitializeProductFile(file, false);
                List<ProductFile> result = MyPage.MTApp.CatalogServices.ProductFiles.FindByFileNameAndDescription(file.FileName, ShortDescriptionTextBox.Text);
                if (result.Count > 0)
                {
                    args.IsValid = false;
                }
            }
        }

        protected void InitializeProductFile(ProductFile file, bool updateFileName)
        {
            if (updateFileName)
            {
                ProductFile otherFile = null;
                if (_currentMode == Mode.DropDownList)
                {
                    otherFile = MyPage.MTApp.CatalogServices.ProductFiles.Find(FilesDropDownList.SelectedValue);
                    file.Bvin = otherFile.Bvin;
                    file.FileName = otherFile.FileName;
                }
                else if (_currentMode == Mode.FileBrowsed)
                {
                    otherFile = MyPage.MTApp.CatalogServices.ProductFiles.Find(FileIdHiddenField.Value);
                    file.Bvin = otherFile.Bvin;
                    file.FileName = otherFile.FileName;
                }
                else if (_currentMode == Mode.NewUpload)
                {
                    file.FileName = System.IO.Path.GetFileName(NewFileUpload.FileName);
                }
            }
            else
            {
                if (_currentMode == Mode.NewUpload)
                {
                    file.FileName = System.IO.Path.GetFileName(NewFileUpload.FileName);
                }
            }

            if (this.DisplayShortDescription)
            {
                file.ShortDescription = ShortDescriptionTextBox.Text.Trim();
            }

            if (this.ProductId != string.Empty)
            {
                file.ProductId = this.ProductId;
                file.SetMinutes(AvailableForTimespanPicker.Months, AvailableForTimespanPicker.Days, AvailableForTimespanPicker.Hours, AvailableForTimespanPicker.Minutes);
                if (NumberOfDownloadsTextBox.Text.Trim() != string.Empty)
                {
                    file.MaxDownloads = int.Parse(NumberOfDownloadsTextBox.Text);
                }
                else
                {
                    file.MaxDownloads = 0;
                }
            }
        }

        public void Clear()
        {
            ShortDescriptionTextBox.Text = "";
            AvailableForTimespanPicker.Months = 0;
            AvailableForTimespanPicker.Days = 0;
            AvailableForTimespanPicker.Hours = 0;
            AvailableForTimespanPicker.Minutes = 0;
            NumberOfDownloadsTextBox.Text = "";
        }

        protected void FileIsUniqueToProductCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            List<ProductFile> files = MyPage.MTApp.CatalogServices.ProductFiles.FindByProductId(this.ProductId);
            args.IsValid = true;
            foreach (ProductFile file in files)
            {
                if (_currentMode == Mode.DropDownList)
                {
                    if (file.Bvin == FilesDropDownList.SelectedValue)
                    {
                        args.IsValid = false;
                    }
                }
                else if (_currentMode == Mode.FileBrowsed)
                {
                    if (file.Bvin == FileIdHiddenField.Value)
                    {
                        args.IsValid = false;
                    }
                }
            }
        }

        public void DownloadOrLinkFile(ProductFile file, IMessageBox MessageBox1)
        {
            //if they browsed a file then this overrides all other behavior
            if (_currentMode == Mode.NewUpload)
            {
                if (file == null)
                {
                    file = new ProductFile();
                }
                InitializeProductFile(file, true);

                if (Save(file))
                {
                    if (ProductFile.SaveFile(MyPage.MTApp.CurrentStore.Id, file.Bvin, file.FileName, NewFileUpload.PostedFile))
                    {
                        MessageBox1.ShowOk("File saved to server successfully");
                    }
                    else
                    {
                        MessageBox1.ShowError("There was an error while trying to save your file to the file system. Please check your asp.net permissions.");
                    }
                }
                else
                {
                    MessageBox1.ShowError("There was an error while trying to save your file to the database.");
                }
            }

            else if (_currentMode == Mode.DropDownList)
            {
                if (FilesDropDownList.SelectedValue.Trim() != "")
                {
                    if (file == null)
                    {
                        file = new ProductFile();
                    }
                    InitializeProductFile(file, true);

                    if (Save(file))
                    {
                        MessageBox1.ShowOk("File saved to server successfully");
                    }
                    else
                    {
                        MessageBox1.ShowError("There was an error while trying to save your file to the database.");
                    }
                }
            }
            else if (_currentMode == Mode.FileBrowsed)
            {
                if (file == null)
                {
                    file = new ProductFile();
                }
                InitializeProductFile(file, true);
                if (Save(file))
                {
                    MessageBox1.ShowOk("File saved to server successfully");
                }
                else
                {
                    MessageBox1.ShowError("There was an error while trying to save your file to the database.");
                }
            }
            InitializeFileLists();
        }

        private bool Save(ProductFile f)
        {
            if (f.Bvin == string.Empty)
            {
                return MyPage.MTApp.CatalogServices.ProductFiles.Create(f);
            }
            else
            {
                return MyPage.MTApp.CatalogServices.ProductFiles.Update(f);
            }
        }
    }
}