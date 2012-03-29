using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribe.Commerce.Content
{
    public class ThemeManager
    {

        private MerchantTribeApplication MTApp = null;

        public ThemeManager(MerchantTribeApplication app)        
        {
            MTApp = app;           
        }

        public static string DefaultThemeGuid = "bv-simplegray";


        public List<ThemeView> FindAvailableThemes()
        {
            return FindAvailableThemes(false);
        }
        /// <summary>
        /// Locate all available themes for a store
        /// </summary>
        /// <returns></returns>
        public List<ThemeView> FindAvailableThemes(bool includeInstalled)
        {
            List<ThemeView> result = new List<ThemeView>();
            List<ThemeView> installed = FindInstalledThemes();

            string[] availablePaths = Storage.DiskStorage.ListAvailableThemePaths();
            if (availablePaths == null) return result;
            if (availablePaths.Length < 1) return result;

            foreach (string themePath in availablePaths)
            {                
                string themeId = System.IO.Path.GetFileName(themePath);
                if (!themeId.StartsWith("theme-")) continue;
                themeId = themeId.Replace("theme-", "");

                if (includeInstalled || !AlreadyInstalled(installed, themeId))
                {
                    ThemeView v = new ThemeView();
                    v.LoadAvailableTheme(themeId);
                    result.Add(v);
                }                
            }

            return result.OrderBy(y => y.Info.Title).ToList();
        }

        private bool AlreadyInstalled(List<ThemeView> installed, string themeId)
        {
            bool result = false;

            if (installed == null) return result;
            foreach (ThemeView t in installed)
            {
                if (t.Info.UniqueIdAsString == themeId.ToLower())
                {
                    return true;
                }
            }
            return result;
        }

        /// <summary>
        /// Locate all themes already installed for a given store
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<ThemeView> FindInstalledThemes()
        {
            List<ThemeView> result = new List<ThemeView>();
            
            string[] installedPaths = Storage.DiskStorage.ListInstalledThemePaths(MTApp.CurrentStore.Id);
            if (installedPaths == null) return result;
            if (installedPaths.Length < 1) return result;

            foreach (string themePath in installedPaths)
            {
                string themeId = System.IO.Path.GetFileName(themePath);
                themeId = themeId.Replace("theme-", "");
                ThemeView v = new ThemeView();
                v.LoadInstalledTheme(MTApp, themeId);
                result.Add(v);
            }

            return result.OrderBy(y => y.Info.Title).ToList();
        }

        //public static List<Theme> FindAllThemes(long storeId)
        //{
        //    List<Theme> result = FindAvailableThemes();
        //    result.AddRange(FindInstalledThemes(storeId));
        //    return result;
        //}
       
        //internal static Theme FindTheme(string name, long storeId)
        //{
        //    string testName = name.Trim().ToLowerInvariant();
        //    List<Theme> all = FindAllThemes(storeId);
        //    foreach (Theme t in all)
        //    {
        //        if (t.Name.Trim().ToLowerInvariant() == testName)
        //        {
        //            return t;
        //        }
        //    }
        //    // If we can't find the default, something is VERY wrong!
        //    if (name == "Default") return null;

        //    // Can't find the requested theme, go back to default
        //    return FindTheme("Default", storeId);

        //}

        //public static string CurrentStyleSheet(long storeId)
        //{
        //    return CurrentStyleSheet(storeId, false);
        //}

        //public static string CurrentStyleSheet(long storeId, bool isSecure)
        //{
        //    Accounts.Store s = Accounts.StoreManager.FindById(storeId);
        //    return CurrentStyleSheet(s, isSecure);
        //}

        public string CurrentStyleSheet(MerchantTribeApplication app)
        {
            return CurrentStyleSheet(app, false);
        }

        public string CurrentStyleSheet(MerchantTribeApplication app, bool isSecure)
        {
            string defaultCss = "/content/themes/theme-" + DefaultThemeGuid + "/styles.css";

            if (MTApp.CurrentStore == null) return defaultCss;                        
            return FullCss(app, isSecure);
        }

        public ThemeInfo GetThemeInfo(string themeId)
        {
            string newInfoXml = Storage.DiskStorage.ReadThemeFile(MTApp.CurrentStore.Id, themeId, "bvtheme.xml");
            ThemeInfo newInfo = new ThemeInfo();
            newInfo.LoadFromString(newInfoXml);
            return newInfo;            
        }

        public bool SaveThemeInfo(string themeId, ThemeInfo info)
        {
            Storage.DiskStorage.WriteThemeFile(MTApp.CurrentStore.Id, themeId, "bvtheme.xml", info.WriteToXmlString());
            return true;
        }
     
        public string FullCss(MerchantTribeApplication app, bool isSecure)
        {            
            string defaultCss = "/content/themes/theme-" + DefaultThemeGuid + "/styles.css";                                    
            if (MTApp.CurrentStore == null) return defaultCss;

            string result = app.StoreUrl(isSecure, false);
            result += "css/theme-" + MTApp.CurrentStore.Settings.ThemeId + "/styles.css";            

            return result;                                    
        }
    
        public string ButtonUrl(string buttonName, bool isSecure)
        {
            string fileName = buttonName + ".png";
            return Storage.DiskStorage.ThemeButtonUrl(MTApp, MTApp.CurrentStore.Settings.ThemeId, fileName, isSecure);
        }

        public string ThemeFileUrl(string fileName, MerchantTribeApplication app)
        {
            string result = Storage.DiskStorage.BaseUrlForStoreTheme(app, app.CurrentStore.Settings.ThemeId);
            result = result + fileName;
            return result;
        }

        public bool InstallTheme(string themeId)
        {
            bool result = false;

            result = Storage.DiskStorage.InstallTheme(MTApp.CurrentStore.Id, themeId);
            if (result)
            {
                MTApp.SwitchTheme(themeId);
                CopyColumnsFromThemeToStore(themeId);                
            }
            return result;
        }

        public bool DeleteTheme(string themeId)
        {
            bool result = false;

            result = Storage.DiskStorage.DeleteThemeFolder(MTApp.CurrentStore.Id,themeId);
            if (result)
            {
                // If we're deleting the current theme, reset the store theme id
                if (MTApp.CurrentStore.Settings.ThemeId.ToLower() == themeId)
                {
                    MTApp.CurrentStore.Settings.SetProp("ThemeId", string.Empty);
                    MTApp.AccountServices.Stores.Update(MTApp.CurrentStore);                    
                }
            }

            return result;
        }

        public bool DuplicateTheme(string themeId)
        {
            // Generate a new ID for the copy
            string newThemeId = System.Guid.NewGuid().ToString().ToLower();
            
            // Create the new folder
            Storage.FileHelper.CreateAndCheckDirectory(Storage.DiskStorage.BaseStoreThemePhysicalPath(MTApp.CurrentStore.Id, newThemeId));

            if (Storage.FileHelper.CopyAllFiles(Storage.DiskStorage.BaseStoreThemePhysicalPath(MTApp.CurrentStore.Id, themeId),
                                            Storage.DiskStorage.BaseStoreThemePhysicalPath(MTApp.CurrentStore.Id, newThemeId)))
            {                
                ThemeInfo newInfo = GetThemeInfo(newThemeId);                
                newInfo.Title = "Copy of " + newInfo.Title;
                newInfo.UniqueId = newThemeId;
                SaveThemeInfo(newThemeId, newInfo);                
                return true;
            }

            return false;
        }
        
        
        // Minify is no longer needed. Done dynamically now.
        //
        // Minify's style sheet and other files as needed
        //public void GenerateDynamicThemeFiles(string themeId)
        //{
        //    Storage.DiskStorage.MinifyStyleSheet(_CurrentStore.Id, themeId);
        //}

        public string CurrentStyleSheetContent(string themeId)
        {
            string result = string.Empty;
            result = Storage.DiskStorage.ReadStyleSheet(MTApp.CurrentStore.Id, themeId);
            return result;
        }

        public string CurrentStyleSheetContentMinifiedAndReplaced(string themeId)
        {
            return Storage.DiskStorage.GetMinifiedStyleSheet(MTApp.CurrentStore.Id, themeId);            
        }

        public string AdminStyleSheetContentMinifiedAndReplaced(string physicalFile, string baseUrl)
        {
            return Storage.DiskStorage.GetMinifiedAdminStyleSheet(physicalFile, baseUrl);
        }

        public bool UpdateStyleSheet(string themeId, string updatedCSS)
        {
            return Storage.DiskStorage.WriteStyleSheet(MTApp, themeId, updatedCSS);
        }

        public bool CopyCurrentContentColumnsToTheme(string themeId)
        {
            bool result = false;

            XmlWriterSettings writersettings = new XmlWriterSettings();      
            
            string data = string.Empty;
            StringBuilder sb = new StringBuilder();
            writersettings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlWriter xw = XmlWriter.Create(sb, writersettings);

            xw.WriteStartElement("ColumnData");

            AddColumn("1", ref xw);
            AddColumn("2", ref xw);
            AddColumn("3", ref xw);
            AddColumn("4", ref xw);
            AddColumn("5", ref xw);
            AddColumn("201", ref xw);
            AddColumn("202", ref xw);
            AddColumn("601", ref xw);
            
            xw.WriteEndElement();            
            xw.Flush();
            xw.Close();
            data = sb.ToString();
        
            result = Storage.DiskStorage.WriteThemeFile(MTApp.CurrentStore.Id, themeId, "ColumnData.xml", data);

            return result;
        }

        private void AddColumn(string columnbvin, ref XmlWriter xw)
        {
            ContentColumn col = MTApp.ContentServices.Columns.Find(columnbvin);
            if (col != null)
            {
                col.ToXmlWriter(ref xw);
            }
        }

        public bool ClearContentColumnDataFromTheme(string themeId)
        {
            return Storage.DiskStorage.WriteThemeFile(this.MTApp.CurrentStore.Id, themeId, "ColumnData.xml", string.Empty);
        }

        public bool CopyColumnsFromThemeToStore(string themeId)
        {            
            string columnData = Storage.DiskStorage.ReadThemeFile(this.MTApp.CurrentStore.Id, themeId, "ColumnData.xml");

            if (columnData == string.Empty) return false;

            try
            {
               
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(columnData);

                    XmlNodeList columnNodes;
                    columnNodes = xdoc.SelectNodes("/ColumnData/ContentColumn");

                    if (columnNodes != null)
                    {
                        for (int i = 0; i <= columnNodes.Count - 1; i++)
                        {
                            InstallColumn(columnNodes[i].OuterXml);                            
                        }
                    }
               
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                return false;
            }

           

            return true;
        }

        private void InstallColumn(string xmlData)
        {            
            if (xmlData == string.Empty) return;

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xmlData);
            
            // Find Column
            XmlNode bvinNode = xdoc.SelectSingleNode("/ContentColumn/Bvin");
            if (bvinNode == null) return;
            string columnBvin = bvinNode.InnerText;

            ContentColumn col = MTApp.ContentServices.Columns.Find(columnBvin);
            if (col == null) return;
            if (col.SystemColumn == false) return;

            // remove existing blocks
            col.Blocks.Clear();
            MTApp.ContentServices.Columns.Update(col);

            // add blocks from xml
            XmlNodeList blockNodes;
            blockNodes = xdoc.SelectNodes("/ContentColumn/Blocks/ContentBlock");

            if (blockNodes != null)
            {
                for (int i = 0; i <= blockNodes.Count - 1; i++)
                {

                    ContentBlock b = new ContentBlock();
                    b.FromXmlString(blockNodes[i].OuterXml);                    
                    b.Bvin = string.Empty;
                    col.Blocks.Add(b);
                }
            }
            MTApp.ContentServices.Columns.Update(col);

        }
        
        public string GetTemplatePartFromCurrentTheme(string partName)
        {
            string result = Storage.DiskStorage.ReadTemplatePart(partName, MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
            if (result == string.Empty) result = GetSystemTemplatePart(partName);
            return result;
        }
        public string GetTemplateFromCurrentTheme(params string[] templates)
        {
            string result = string.Empty;

            // Check in merchant theme first
            foreach (string s in templates)
            {
                result = Storage.DiskStorage.ReadTemplate(s, MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
                if (!String.IsNullOrEmpty(result))
                {
                    break;
                }
            }

            // If nothing, check system templates
            if (result.Trim().Length < 1)
            {
                foreach (string s in templates)
                {
                    result = GetSystemTemplate(s);
                    if (!String.IsNullOrEmpty(result))
                    {
                        break;
                    }
                }
            }
            
            // Last resort, check system default template
            if (result.Trim().Length < 1)
            {
                result = GetSystemTemplate("default.html");
            }            

            return result;
        }
        public string GetSystemTemplatePart(string partName)
        {
            string result = Storage.DiskStorage.ReadSystemTemplatePart(partName);
            return result;
        }
        public string GetSystemTemplate(string templateName)
        {
            string result = Storage.DiskStorage.ReadSystemTemplate(templateName);
            if (result == string.Empty) result = Storage.DiskStorage.ReadSystemTemplate("default.html");
            return result;
        }
        public List<string> ListTemplatesForCurrentTheme()
        {
            List<string> result = Storage.DiskStorage.ListAllAvailableTemplates(MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
            if (result.Count < 1) result.Add("default.html");
            if (!result.Contains("default-no-menu.html")) result.Add("default-no-menu.html");
            if (!result.Contains("home.html")) result.Add("home.html");

            return result;
        }
        
    }
}
