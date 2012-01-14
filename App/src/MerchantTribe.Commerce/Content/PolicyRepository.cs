using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Content
{
    public class PolicyRepository: ConvertingRepositoryBase<Data.EF.bvc_Policy, Policy>
    {
        private RequestContext context = null;
        private PolicyBlockRepository blockRepository = null;

        public static PolicyRepository InstantiateForMemory(RequestContext c)
        {
            return new PolicyRepository(c, new MemoryStrategy<Data.EF.bvc_Policy>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_PolicyBlock>(PrimaryKeyType.Bvin),
                                           new TextLogger());
        }
        public static PolicyRepository InstantiateForDatabase(RequestContext c)
        {
            PolicyRepository result = null;
            result = new PolicyRepository(c, 
                new EntityFrameworkRepository<Data.EF.bvc_Policy>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_PolicyBlock>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public PolicyRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Policy> r,
                                    IRepositoryStrategy<Data.EF.bvc_PolicyBlock> subr, 
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
            blockRepository = new PolicyBlockRepository(c, subr, this.logger);
        }

        protected override void CopyDataToModel(Data.EF.bvc_Policy data, Policy model)
        {
            model.Bvin = data.bvin;
            model.StoreId = data.StoreId;
            model.Kind = (PolicyType)data.PolicyType;
            model.LastUpdated = data.LastUpdated;
            model.SystemPolicy = data.SystemPolicy == 1;
            model.Title = data.Title;                        
        }
        protected override void CopyModelToData(Data.EF.bvc_Policy data, Policy model)
        {
            data.bvin = model.Bvin;
            data.StoreId = model.StoreId;
            data.PolicyType = (int)model.Kind;
            data.LastUpdated = model.LastUpdated;
            data.SystemPolicy = model.SystemPolicy ? 1 : 0;
            data.Title = model.Title;                        
        }

        protected override void DeleteAllSubItems(Policy model)
        {
            blockRepository.DeleteForPolicy(model.Bvin);
        }
        protected override void GetSubItems(Policy model)
        {
            model.Blocks = blockRepository.FindForPolicy(model.Bvin);
        }
        protected override void MergeSubItems(Policy model)
        {
            blockRepository.MergeList(model.Bvin, model.Blocks);
        }

        public Policy Find(string bvin)
        {
            Policy result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Policy FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(Policy item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;
 	        return base.Create(item);
        }
        public bool Update(Policy c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            c.LastUpdated = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));            
        }
        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            Policy item = Find(bvin);
            if (item == null) return false;

           return Delete(new PrimaryKey(bvin));            
        }


        public Policy FindByType(PolicyType type)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Policy> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.PolicyType == (int)type);                                                                   
            return FirstPoco(data);
        }
        public Content.Policy FindOrCreateByType(PolicyType type)
        {
            Policy result = FindByType(type);
            if (result == null)
            {
                result = this.Generate(type);
            }            
            return result;
        }
        public List<Policy> FindAll()
        {
            List<Policy> result = new List<Policy>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Policy> data = repository.Find().Where(y => y.StoreId == storeId)
                                      .OrderBy(y => y.Title);
            result = ListPoco(data);
            if (result == null) result = new List<Policy>();
            
            if (result.Count < 3)
            {
                Policy p = FindOrCreateByType(PolicyType.Faq);
                result.Add(p);

                Policy p2 = FindOrCreateByType(PolicyType.Privacy);
                result.Add(p2);

                Policy p3 = FindOrCreateByType(PolicyType.Returns);
                result.Add(p3);

                Policy p4 = FindOrCreateByType(PolicyType.TermsAndConditions);
                result.Add(p4);
            }
            return result;
        }
       
        public bool DeleteBlock(string blockId)
        {
            return this.blockRepository.Delete(blockId);
        }
        public bool ResortBlocksItems(string policyId, List<string> sortedItemIds)
        {
            return this.blockRepository.Resort(policyId, sortedItemIds);
        }
        public PolicyBlock FindBlock(string blockId)
        {
            return this.blockRepository.Find(blockId);
        }
        public bool UpdateBlock(PolicyBlock item)
        {
            return this.blockRepository.Update(item);
        }

        public Policy Generate(PolicyType type)
        {
            Policy result = new Policy();
            result.Kind = type;

            switch (type)
            {
                case PolicyType.Faq:
                    result.Title = "Frequently Asked Questions";
                    result.SystemPolicy = true;
                    result.Blocks.Add(new PolicyBlock() { Name = "How can I contact you?", Description = "<p>Click on the Contact Us link at the top of the web site.</p>"});                                        
                    Create(result);
                    break;
                case PolicyType.Privacy:
                    result.Title = "Privacy Policy";
                    result.SystemPolicy = true;                    
                    result.Blocks.Add(new PolicyBlock() { Name = "", Description = "<p>Thank you for visiting our web site. This privacy policy tells you how we use personal information collected at this site. Please read this privacy policy before using the site or submitting any personal information. By using the site, you are accepting the practices described in this privacy policy. These practices may be changed, but any changes will be posted and changes will only apply to activities and information on a going forward, not retroactive basis. You are encouraged to review the privacy policy whenever you visit the site to make sure that you understand how any personal information you provide will be used.</p>\r\n<p>Note, the privacy practices set forth in this privacy policy are for this web site only. If you link to other web sites, please review the privacy policies posted at those sites.</p>", SortOrder = 1 });
                    result.Blocks.Add(new PolicyBlock() { Name = "Collection of Information", Description = "<p>We collect personally identifiable information, like names, postal addresses, email addresses, etc., when voluntarily submitted by our visitors. The information you provide is used to fulfill your specific request. This information is only used to fulfill your specific request, unless you give us permission to use it in another manner, for example to add you to one of our mailing lists.</p>", SortOrder = 2 });
                    result.Blocks.Add(new PolicyBlock() { Name = "Cookie/Tracking Technology", Description = "<p>The Site may use cookie and tracking technology depending on the features offered. Cookie and tracking technology are useful for gathering information such as browser type and operating system, tracking the number of visitors to the Site, and understanding how visitors use the Site. Cookies can also help customize the Site for visitors. Personal information cannot be collected via cookies and other tracking technology, however, if you previously provided personally identifiable information, cookies may be tied to such information. Aggregate cookie and tracking information may be shared with third parties.</p>", SortOrder = 3 });
                    result.Blocks.Add(new PolicyBlock() { Name = "Distribution of Information", Description = "<p>We may share information with governmental agencies or other companies assisting us in fraud prevention or investigation. We may do so when: (1) permitted or required by law; or, (2) trying to protect against or prevent actual or potential fraud or unauthorized transactions; or, (3) investigating fraud which has already taken place. The information is not provided to these companies for marketing purposes.</p>", SortOrder = 4 });
                    result.Blocks.Add(new PolicyBlock() { Name = "Commitment to Data Security", Description = "<p>Your personally identifiable information is kept secure. Only authorized employees, agents and contractors (who have agreed to keep information secure and confidential) have access to this information. All emails and newsletters from this site allow you to opt out of further mailings.</p>", SortOrder = 5});
                    result.Blocks.Add(new PolicyBlock() { Name = "Privacy Contact information", Description = "<p>If you have any questions, concerns, or comments about our privacy policy you may contact us using the information below:</p>\r\n<ul>\r\n<li>By E-Mail: xxxxxx@xxxxxx.xxx</li>\r\n<li>By Phone: xxx-xxx-xxxx</li>\r\n</ul>", SortOrder = 6 });
                    Create(result);
                    break;
                case PolicyType.Returns:
                    result.Title = "Return Policy";
                    result.SystemPolicy = true;
                    result.Blocks.Add(new PolicyBlock() { Name = "", Description = "<p>All returns are subject to approval by a store manager.</p>", SortOrder = 1});
                    Create(result);
                    break;
                case PolicyType.TermsAndConditions:
                    result.Title = "Terms and Conditions";
                    result.SystemPolicy = true;
                    result.Blocks.Add(new PolicyBlock() { Name = "Terms And Conditions", Description = "<p>By visiting or shopping at this web site, you accept the following terms and conditions. Please read them carefully.</p>", SortOrder = 1});
                    result.Blocks.Add(new PolicyBlock() { Name = "Copyright", Description = "<p>All content included on this site, such as text, graphics, logos, button icons, images, audio clips, digital downloads, data compilations, and software, is the property of this site's owner or its content suppliers and protected by United States and international copyright laws. The compilation of all content on this site is the exclusive property of this site's owner and protected by U.S. and international copyright laws. All software used on this site is the property of this site's owner or its software suppliers and protected by United States and international copyright laws.</p>", SortOrder = 2});
                    result.Blocks.Add(new PolicyBlock() { Name = "Disclaimer of Warranties and Limitation of Liability", Description = "<p>THIS SITE IS PROVIDED ON AN \"AS IS\" AND \"AS AVAILABLE\" BASIS. NO REPRESENTATIONS OR WARRANTIES OF ANY KIND ARE MADE, EXPRESS OR IMPLIED, AS TO THE OPERATION OF THIS SITE OR THE INFORMATION, CONTENT, MATERIALS, OR PRODUCTS INCLUDED ON THIS SITE. YOU EXPRESSLY AGREE THAT YOUR USE OF THIS SITE IS AT YOUR SOLE RISK.</p>", SortOrder = 3});
                    result.Blocks.Add(new PolicyBlock() { Name = "", Description = "<p>TO THE FULL EXTENT PERMISSIBLE BY APPLICABLE LAW, THIS SITE'S OWNER DISCLAIMS ALL WARRANTIES, EXPRESS OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THIS SITE'S OWNER DOES NOT WARRANT THAT THIS SITE, ITS SERVERS, OR E-MAIL SENT FROM THIS SITE ARE FREE OF VIRUSES OR OTHER HARMFUL COMPONENTS. THIS SITE'S OWNER WILL NOT BE LIABLE FOR ANY DAMAGES OF ANY KIND ARISING FROM THE USE OF THIS SITE, INCLUDING, BUT NOT LIMITED TO DIRECT, INDIRECT, INCIDENTAL, PUNITIVE, AND CONSEQUENTIAL DAMAGES. </p>", SortOrder = 4});
                    result.Blocks.Add(new PolicyBlock() { Name = "", Description = "<p>CERTAIN STATE LAWS DO NOT ALLOW LIMITATIONS ON IMPLIED WARRANTIES OR THE EXCLUSION OR LIMITATION OF CERTAIN DAMAGES. IF THESE LAWS APPLY TO YOU, SOME OR ALL OF THE ABOVE DISCLAIMERS, EXCLUSIONS, OR LIMITATIONS MAY NOT APPLY TO YOU, AND YOU MIGHT HAVE ADDITIONAL RIGHTS. </p>", SortOrder = 5});
                    Create(result);        
                    break;
            }

            return result;
        }
    }
}
