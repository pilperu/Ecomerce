using System.Collections.Generic;
using System.Web.Mvc;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Filters;
using MerchantTribeStore.Models;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore.Areas.account.Controllers
{
    [CustomerSignedInFilter]
    public class AddressBookController : BaseStoreController
    {
        //
        // GET: /account/AddressBook/
        public ActionResult Index()
        {
            ViewBag.Title = "Address Book";
            ViewBag.MetaDescription = "Address Book | " + MTApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountaddressbookpage";
            ViewBag.EditButtonUrl = MTApp.ThemeManager().ButtonUrl("edit", Request.IsSecureConnection);
            ViewBag.DeleteButtonUrl = MTApp.ThemeManager().ButtonUrl("x", Request.IsSecureConnection);
            ViewBag.AddNewButtonUrl = MTApp.ThemeManager().ButtonUrl("new", Request.IsSecureConnection);

            List<Address> addresses = LoadAddresses();

            return View(addresses);
        }
        private List<Address> LoadAddresses()
        {
            CustomerAccount u = MTApp.CurrentCustomer;
            if (u != null)
            {
                return u.Addresses;
            }
            return new List<Address>();
        }

        //
        // POST: /account/AddressBook/Delete/{id}
        [HttpPost]
        public ActionResult Delete(string id)
        {
            CustomerAccount u = MTApp.CurrentCustomer;
            if (u != null)
            {
                u.DeleteAddress(id);
                MTApp.MembershipServices.UpdateCustomer(u);
            }

            return RedirectToAction("Index");
        }

        private void EditSetup()
        {
            ViewBag.Title = "Edit Address";
            ViewBag.MetaDescription = "Edit Address | " + MTApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountaddresseditpage";
            ViewBag.SaveButtonUrl = MTApp.ThemeManager().ButtonUrl("savechanges", Request.IsSecureConnection);
        }        
        // GET: /account/AddressBook/Edit/{id}
        public ActionResult Edit(string id)
        {
            EditSetup();

            Address a = LoadAddress(id);            
            return View(new AddressViewModel(a));
        }
        private Address LoadAddress(string bvin)
        {
            CustomerAccount u = MTApp.CurrentCustomer;
            if (u != null)
            {
                switch (bvin.ToLower())
                {
                    case "new":
                        MerchantTribe.Commerce.Contacts.Address a = new MerchantTribe.Commerce.Contacts.Address();
                        a.Bvin = System.Guid.NewGuid().ToString();
                        return a;                        
                    default:
                        foreach (MerchantTribe.Commerce.Contacts.Address a2 in u.Addresses)
                        {
                            if (a2.Bvin == bvin)
                            {
                                return a2;                                
                            }
                        }
                        break;
                }
            }

            return new Address();
        }

        // 
        // POST: /account/AddressBook/Edit/{id}
        [ActionName("Edit")]
        [HttpPost]
        public ActionResult EditPosted(string id, FormCollection posted)
        {
            EditSetup();
                        
            AddressViewModel model = new AddressViewModel();
            if (TryUpdateModel(model))
            {
                CustomerAccount u = MTApp.MembershipServices.Customers.Find(MTApp.CurrentCustomerId);
                if (u == null) return View(model);
                
                Address a = LoadAddress(id);
                model.CopyTo(a);
                                
                string slug = id;
                switch (slug.ToLower())
                {
                    case "new":
                        MTApp.MembershipServices.CheckIfNewAddressAndAddWithUpdate(u, a);
                        MTApp.MembershipServices.UpdateCustomer(u);
                        break;
                    default:
                        u.UpdateAddress(a);
                        MTApp.MembershipServices.UpdateCustomer(u);
                        break;
                }
                return RedirectToAction("Index");
            }
                       
            return View(model);
        }

    }


}
