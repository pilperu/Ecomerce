using System;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;

namespace MerchantTribe.Commerce.Content
{

	public class ModuleController
	{        
		private ModuleController()
		{
            
		}

#region " Category Templates "

		public static List<string> FindCategoryTemplates()
		{
            List<string> result = new List<string>();
            
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            List<string> raw = ListFiles("Content\\SystemTemplates");
            foreach (string s in raw)
            {
                string temp = Path.GetFileNameWithoutExtension(s);
                if (temp.ToLowerInvariant().StartsWith("category-"))
                {
                    if (temp.Length > 9)
                    {
                        temp = temp.Substring(9, temp.Length - 9);
                        temp = temp.Replace('-', ' ');                        
                        temp = textInfo.ToTitleCase(temp);                        
                        result.Add(temp);
                    }
                }
            }
			
			return result;
		}

        //public static StringCollection FindCategoryTemplateEditors()
        //{
        //    StringCollection result = new StringCollection();
        //    result = ListFolders("BVModules\\CategoryTemplates", "Edit.ascx");
        //    return result;
        //}

        //public static CategoryEditorTemplate LoadCategoryEditor(string templateName, System.Web.UI.Page p)
        //{
        //    string fullName = "BVModules\\CategoryTemplates\\" + templateName + "\\Edit.ascx";
        //    return (CategoryEditorTemplate)LoadSingleControl(fullName,ref p);
        //}

#endregion

#region " Content Blocks "

		public static StringCollection FindContentBlocks()
		{
			StringCollection result = new StringCollection();
            result = ListFolders("BVModules\\ContentBlocks", string.Empty);			
			return result;
		}    
		public static System.Web.UI.Control LoadContentBlockAdminView(string blockName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\ContentBlocks\\" + blockName + "\\adminview.ascx";
			return LoadSingleControl(fullName,ref p);
		}
		public static System.Web.UI.Control LoadContentBlockEditor(string blockName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\ContentBlocks\\" + blockName + "\\editor.ascx";
			return LoadSingleControl(fullName,ref p);
		}
#endregion
#region " Reports "

		public static StringCollection FindReports()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\Reports", "view.aspx");
			return result;
		}
#endregion
#region " Product Templates "

		public static StringCollection FindProductTemplates()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\ProductTemplates", "Product.aspx");
			result.Remove("FixedPriceGiftCertificate");
			result.Remove("ArbitraryPriceGiftCertificate");
			return result;
		}
#endregion
#region " Kit Templates "

		public static StringCollection FindKitTemplates()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\KitTemplates", "Kit.aspx");
			return result;
		}
#endregion
#region " Themes "

		public static StringCollection FindThemes()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\Themes", "default.master");
			return result;
		}

		public static string BuildThemeInfoPath(string themeName, string physicalPath)
		{
			string result = string.Empty;
			result = BuildFilePath(themeName, "BVModules\\Themes", physicalPath, "BVTheme.xml");
			return result;
		}
#endregion
#region " Tasks "


		public static StringCollection FindProductTaskEditor()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\ProductTasks", "Edit.ascx");
			return result;
		}
		public static StringCollection FindOrderTaskEditor()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\OrderTasks", "Edit.ascx");
			return result;
		}
		public static System.Web.UI.Control LoadProductTaskEditor(string taskName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\ProductTasks\\" + taskName + "\\Edit.ascx";
			return LoadSingleControl(fullName,ref p);
		}
		public static System.Web.UI.Control LoadOrderTaskEditor(string taskName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\OrderTasks\\" + taskName + "\\Edit.ascx";
			return LoadSingleControl(fullName,ref p);
		}
#endregion
#region " Offers "


		public static StringCollection FindOffers()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\Offers", "Edit.ascx");
			return result;
		}

		public static System.Web.UI.Control LoadOfferEditor(string taskName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\Offers\\" + taskName + "\\Edit.ascx";
			return LoadSingleControl(fullName,ref p);
		}
#endregion
#region " Preview Images "

		public static string FindCategoryTemplatePreviewImage(string templateName, string physicalPath)
		{
			string result = string.Empty;
			result = FindPreviewImage(templateName, "BVModules\\CategoryTemplates\\", physicalPath);
			return result;
		}

		public static string FindProductTemplatePreviewImage(string templateName, string physicalPath)
		{
			string result = string.Empty;
			result = FindPreviewImage(templateName, "BVModules\\ProductTemplates\\", physicalPath);
			return result;
		}

		public static string FindKitTemplatePreviewImage(string templateName, string physicalPath)
		{
			string result = string.Empty;
			result = FindPreviewImage(templateName, "BVModules\\KitTemplates\\", physicalPath);
			return result;
		}

		public static string FindThemePreviewImage(string templateName, string physicalPath)
		{
			string result = string.Empty;
			result = FindPreviewImage(templateName, "BVModules\\Themes\\", physicalPath);
			return result;
		}
#endregion
#region " Checkouts "
		public static StringCollection FindCheckouts()
		{
			StringCollection result;
			result = ListFolders("BVModules\\Checkouts", "Checkout.aspx");
			StringCollection specialCheckouts = ListFolders("BVModules\\Checkouts", "hidden.txt");
			foreach (string item in specialCheckouts) {
				result.Remove(item);
			}
			return result;
		}
#endregion
#region " Payment Methods "

		public static System.Web.UI.Control LoadPaymentMethodView(string methodName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\PaymentMethods\\" + methodName + "\\view.ascx";
			return LoadSingleControl(fullName,ref p);
		}
		public static System.Web.UI.Control LoadPaymentMethodAdminView(string methodName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\PaymentMethods\\" + methodName + "\\adminview.ascx";
			return LoadSingleControl(fullName,ref p);
		}
		public static System.Web.UI.Control LoadPaymentMethodEditor(string methodName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\PaymentMethods\\" + methodName + "\\edit.ascx";
			return LoadSingleControl(fullName,ref p);
		}
		public static System.Web.UI.Control LoadCreditCardGatewayEditor(string gatewayName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\CreditCardGateways\\" + gatewayName + "\\edit.ascx";
			return LoadSingleControl(fullName,ref p);
		}
#endregion
#region " Shipping "

		public static System.Web.UI.Control LoadShippingEditor(string shippingName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\Shipping\\" + shippingName + "\\edit.ascx";
			return LoadSingleControl(fullName,ref p);
		}
#endregion
#region " Editors "
#region " Content Blocks "

		public static StringCollection FindEditors()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVModules\\Editors", "editor.ascx");
			return result;
		}
		public static System.Web.UI.Control LoadEditor(string editorName, System.Web.UI.Page p)
		{
			string fullName = "BVModules\\Editors\\" + editorName + "\\editor.ascx";
			return LoadSingleControl(fullName,ref p);
		}
		public static System.Web.UI.Control LoadDefaultEditor(System.Web.UI.Page p)
		{
			return LoadEditor(WebAppSettings.DefaultTextEditor, p);
		}
#endregion
#endregion
#region " Admin Plugins "
		public static StringCollection FindAdminPlugins()
		{
			StringCollection result = new StringCollection();
			result = ListFolders("BVAdmin\\Plugins", "default.aspx");
			return result;
		}
#endregion

		private static StringCollection ListFolders(string startingFolder, string checkForFileName)
		{
			StringCollection result = new StringCollection();

			if (HttpContext.Current != null) {
				string controlPath = HttpContext.Current.Request.PhysicalApplicationPath;
				controlPath = Path.Combine(controlPath, startingFolder);
				if (Directory.Exists(controlPath)) {
					string[] modules = Directory.GetDirectories(controlPath);
					if (modules != null) {
						for (int k = 0; k <= modules.Length - 1; k++) {
							if (File.Exists(Path.Combine(modules[k], checkForFileName)) == true ||
                                checkForFileName.Trim() == string.Empty) {
								result.Add(Path.GetFileName(Path.GetFileName(modules[k])));
							}
						}
					}
				}
			}

			return result;
		}

        private static List<string> ListFiles(string startingFolder)
        {
            List<string> result = new List<string>();

            if (HttpContext.Current != null)
            {
                string rootAppPath = HttpContext.Current.Request.PhysicalApplicationPath;
                rootAppPath = Path.Combine(rootAppPath, startingFolder);
                if (Directory.Exists(rootAppPath))
                {
                    foreach (string s in Directory.EnumerateFiles(rootAppPath))
                    {
                        result.Add(s);
                    }
                }
            }

            return result;
        }

		private static System.Web.UI.Control LoadSingleControl(string blockName, ref System.Web.UI.Page p)
		{
			System.Web.UI.Control result = null;

			if (p != null) {
				string controlName = p.Request.PhysicalApplicationPath;
				controlName = Path.Combine(controlName, blockName);
				if (File.Exists(controlName)) {
					string virtualPath = "~/" + blockName.Replace("\\", "/");
					result = p.LoadControl(virtualPath);
				}
			}

			return result;
		}

		private static string FindPreviewImage(string templateName, string modulePath, string physicalPath)
		{
			string result = string.Empty;

			string fullPath = modulePath.Replace("/", "\\") + templateName.Replace("/", "\\") + "\\";
			fullPath = Path.Combine(physicalPath, fullPath);

			if (File.Exists(fullPath + "preview.png")) {
				result = "~/" + modulePath.Replace("\\", "/") + templateName.Replace("\\", "/") + "\\preview.png";
			}
			else {
				if (File.Exists(fullPath + "preview.gif")) {
					result = "~/" + modulePath.Replace("\\", "/") + templateName.Replace("\\", "/") + "\\preview.gif";
				}
				else {
					if (File.Exists(fullPath + "preview.jpg")) {
						result = "~/" + modulePath.Replace("\\", "/") + templateName.Replace("\\", "/") + "\\preview.jpg";
					}
				}
			}

			if (result == string.Empty) {
				result = "~/BVAdmin/Images/NoPreview.gif";
			}

			return result;
		}

		private static string BuildFilePath(string templateName, string modulePath, string physicalPath, string fileName)
		{
			string result = string.Empty;
			string fullPath = modulePath.Replace("/", "\\") + "\\" + templateName.Replace("/", "\\") + "\\";
			fullPath = Path.Combine(physicalPath, fullPath);
			result = Path.Combine(fullPath, fileName);
			return result;
		}

	}

}
