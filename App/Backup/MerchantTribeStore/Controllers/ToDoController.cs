using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribeStore.Controllers
{
    [HandleError]
    [ValidateInput(false)]
    public class ToDoController : Shared.BaseAdminController
    {

      
        //
        // GET: /ToDo/
        public ActionResult Index()
        {
            bool failed = false;
            if (Request.QueryString["failed"] != null)
            {
                failed = true;
            }
            
            List<ToDoItem> items = new List<ToDoItem>();
            UserAccount adminUser = MTApp.CurrentRequestContext.CurrentAdministrator(MTApp);
            if (adminUser != null)
            {
                ToDoItemRepository repository = new ToDoItemRepository(MTApp.CurrentRequestContext);
                items = repository.FindByAccountId(adminUser.Id);
            }

            FlashFailure("Failed to Create Task. Please Try Again.");            
            return View(items);
        }

        // Render a single item
        // GET: /ToDo/Render/
        public ActionResult Render(ToDoItem item)
        {
            return View(item);
        }

        // Create a new item
        // POST: /ToDoItem/Create
        [HttpPost]
        public ActionResult Create(string title, string details)
        {
            bool fail = false;

            try
            {
                ToDoItem item = new ToDoItem()
                {
                    AccountId = MTApp.CurrentRequestContext.CurrentAdministrator(MTApp).Id,
                    Details = details,
                    IsComplete = false,
                    Title = title
                };

                ToDoItemRepository repository = new ToDoItemRepository(MTApp.CurrentRequestContext);
                if (repository.Create(item))
                {
                    fail = false;                    
                }
                else
                {
                    fail = true;
                }
            }
            catch (Exception ex)
            {
                fail = true;                
            }

            string destination = "~/todo";
            if (fail)
            {
                destination += "?failed=1";
            }
            return new RedirectResult(destination);
        }
            
        // Delete a Task
        // POST: /ToDo/Delete/5
        [HttpPost]
        public ActionResult Delete(long id, FormCollection collection)
        {
            bool success = false;

            try
            {

                long accountId = MTApp.CurrentRequestContext.CurrentAdministrator(MTApp).Id;
                ToDoItemRepository repository = new ToDoItemRepository(MTApp.CurrentRequestContext);
                ToDoItem item = repository.Find(id);
                if (item != null)
                {
                    if (item.AccountId == accountId)
                    {
                        success = repository.Delete(id);
                    }
                }
            }
            catch
            {
                
            }

            return new JsonResult() { Data = MerchantTribe.Web.Json.ObjectToJson(new { result = success }) };
        }

        // Toggle Complete Status of Task
        // POST: /ToDo/Toggle
        [HttpPost]
        public ActionResult Toggle(long id)
        {
            bool success = false;

            try
            {

                long accountId = MTApp.CurrentRequestContext.CurrentAdministrator(MTApp).Id;
                ToDoItemRepository repository = new ToDoItemRepository(MTApp.CurrentRequestContext);
                ToDoItem item = repository.Find(id);
                if (item != null)
                {
                    if (item.AccountId == accountId)
                    {
                        item.IsComplete = !item.IsComplete;
                        success = repository.Update(item);
                    }
                }
            }
            catch
            {

            }

            return new RedirectResult("~/todo");            
        }

        // Resorts items on the list to the new order
        // POST: /ToDo/Sort
        [HttpPost]
        public ActionResult Sort()
        {

            string ids = Request.Form["ids"];

            string[] sorted = ids.Split(',');
            List<long> l = new List<long>();
            foreach (string id in sorted)
            {
                long temp = 0;
                if (long.TryParse(id.Replace("item",""),out temp))
                {
                    l.Add(temp);
                }
            }

            long accountId = MTApp.CurrentRequestContext.CurrentAdministrator(MTApp).Id;
            ToDoItemRepository repository = new ToDoItemRepository(MTApp.CurrentRequestContext);

            List<ToDoItem> items = repository.FindByAccountId(accountId);
            repository.AutoSubmit = false;

            int currentSort = 1;
            
            foreach (long itemid in l)
            {
               foreach (ToDoItem item in items)
               {
                    if (item.Id == itemid )
                        {
                            item.SortOrder = currentSort;                            
                            currentSort += 1;
                            repository.Update(item);
                        }
               }
            }

            repository.SubmitChanges();

            return new JsonResult() { Data = "result:true" };
        }

    }
}
