using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Storage;
using MerchantTribeStore.Areas.AdminContent.Models;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Areas.AdminContent.Controllers
{
    public class FileManagerController : MerchantTribeStore.Controllers.Shared.BaseAdminController
    {

        private BreadCrumbViewModel BuildBreadCrumbs(string path)
        {
            BreadCrumbViewModel result = new BreadCrumbViewModel();
            result.HideHomeLink = true;

            List<BreadCrumbItem> items = new List<BreadCrumbItem>();

            string workingPath = path.TrimEnd('\\');

            bool finished = false;
            while (finished == false)
            {
                workingPath = workingPath.TrimEnd('\\');
                if (workingPath.Length < 1)
                {
                    finished = true;
                    break;
                }                
                string dir = System.IO.Path.GetFileName(workingPath);

                items.Add(new BreadCrumbItem()
                {
                    Link = Url.Content("~/bvadmin/content/filemanager?path=" + workingPath),
                    Name = dir,
                    Title = dir
                });
                workingPath = System.IO.Path.GetDirectoryName(workingPath);
            }
            items.Add(new BreadCrumbItem()
            {
                Link = Url.Content("~/bvadmin/content/filemanager"),
                Name = "Root",
                Title = "Root"
            });

            for (int i = items.Count - 1; i >= 0; i--)
            {
                result.Items.Enqueue(items[i]);
            }
            return result;
        }

        //
        // GET: /AdminContent/FileManager/        
        public ActionResult Index()
        {
            string path = Request.QueryString["path"] ?? string.Empty;
            string cleanPath = DiskStorage.FileManagerCleanPath(path);

            FileManagerViewModel model = new FileManagerViewModel(cleanPath);
            model.Directories = DiskStorage.FileManagerListDirectories(MTApp.CurrentStore.Id, cleanPath);
            model.Files = DiskStorage.FileManagerListFiles(MTApp.CurrentStore.Id, cleanPath);            
            model.BasePreviewUrl = "~/images/sites/" + MTApp.CurrentStore.Id + "/";

            var breadRender = new code.TemplateEngine.TagHandlers.BreadCrumbs();
            ViewBag.BreadCrumbsFinal = breadRender.Render(MTApp, BuildBreadCrumbs(cleanPath));
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDirectory()
        {
            string path = Request.Form["path"] ?? string.Empty;
            path = DiskStorage.FileManagerCleanPath(path);
            string newDirName = Request.Form["newdirectoryname"] ?? string.Empty;
            newDirName = DiskStorage.FileManagerCleanPath(newDirName);

            string fullPath = path + "\\" + newDirName;
            DiskStorage.FileManagerCreateDirectory(MTApp.CurrentStore.Id, fullPath);

            string destination = Url.Content("~/bvadmin/content/filemanager?path=" + path);
            return new RedirectResult(destination);
        }

        [HttpPost]
        public ActionResult DeleteDirectory()
        {
            string path = Request.Form["path"] ?? string.Empty;
            path = DiskStorage.FileManagerCleanPath(path);
            string deletePath = Request.Form["deletepath"] ?? string.Empty;
            deletePath = DiskStorage.FileManagerCleanPath(deletePath);

            if (!DiskStorage.FileManagerIsSystemPath(deletePath))
            {
                DiskStorage.FileManagerDeleteDirectory(MTApp.CurrentStore.Id, deletePath);
            }

            string destination = Url.Content("~/bvadmin/content/filemanager?path=" + path);
            return new RedirectResult(destination);
        }

        [HttpPost]
        public ActionResult Upload()
        {
            string path = Request.Form["path"] ?? string.Empty;
            path = DiskStorage.FileManagerCleanPath(path);

            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        string completeFileName = file.FileName;
                        string nameSmall = System.IO.Path.GetFileName(completeFileName);
                        string fullPathAndName = path + "\\" + nameSmall;
                        DiskStorage.FileManagerCreateFile(MTApp.CurrentStore.Id, fullPathAndName, file);
                    }
                }
            }
            catch (Exception ex)
            {
                FlashFailure(ex.Message);
                MerchantTribe.Commerce.EventLog.LogEvent(ex);
            }

            string destination = Url.Content("~/bvadmin/content/filemanager?path=" + path);
            return new RedirectResult(destination);
        }

        [HttpPost]
        public ActionResult DeleteFile()
        {
            string path = Request.Form["path"] ?? string.Empty;
            path = DiskStorage.FileManagerCleanPath(path);
            string fileName = Request.Form["filename"] ?? string.Empty;
            fileName = DiskStorage.FileManagerCleanPath(fileName);

            string fullPath = path + "\\" + fileName;
            DiskStorage.FileManagerDeleteFile(MTApp.CurrentStore.Id, fullPath);

            string destination = Url.Content("~/bvadmin/content/filemanager?path=" + path);
            return new RedirectResult(destination);
        }
    }
}
