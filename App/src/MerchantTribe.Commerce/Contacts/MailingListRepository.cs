using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class MailingListRepository: ConvertingRepositoryBase<Data.EF.bvc_MailingList, MailingList>
    {

        public static MailingListRepository InstantiateForMemory(RequestContext c)
        {
            MailingListRepository result = null;
            result = new MailingListRepository(c, new MemoryStrategy<Data.EF.bvc_MailingList>(PrimaryKeyType.Long),
                                           new MemoryStrategy<Data.EF.bvc_MailingListMember>(PrimaryKeyType.Long),
                                           new TextLogger());
            return result;
        }
        public static MailingListRepository InstantiateForDatabase(RequestContext c)
        {
            MailingListRepository result = null;
            result = new MailingListRepository(c, 
                new EntityFrameworkRepository<Data.EF.bvc_MailingList>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_MailingListMember>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }

        private RequestContext context = null;
        private MailingListMemberRepository memberRepository = null;

        private MailingListRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_MailingList> r,
                                    IRepositoryStrategy<Data.EF.bvc_MailingListMember> subr,
                                    ILogger log)
        {
            context = c;
            repository = r;            
            this.logger = log;
            repository.Logger = this.logger;
            memberRepository = new MailingListMemberRepository(subr, this.logger);                        
        }

        protected override void CopyDataToModel(Data.EF.bvc_MailingList data, MailingList model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.Name = data.Name;
            model.IsPrivate = data.Private;
            model.LastUpdatedUtc = data.LastUpdatedUtc;                        
        }
        protected override void CopyModelToData(Data.EF.bvc_MailingList data, MailingList model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.Name = model.Name;
            data.Private = model.IsPrivate;
            data.LastUpdatedUtc = model.LastUpdatedUtc;                        
        }

        protected override void DeleteAllSubItems(MailingList model)
        {
            memberRepository.DeleteForList(model.Id);
        }
        protected override void GetSubItems(MailingList model)
        {
            model.Members = memberRepository.FindForList(model.Id);
        }
        protected override void MergeSubItems(MailingList model)
        {
            memberRepository.MergeList(model.Id, model.Members);
        }
    
        public MailingList Find(long id)
        {
            MailingList result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public MailingList FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));            
        }

        public override bool  Create(MailingList item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;            
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }        
        public bool Update(MailingList c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }

            c.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Id));            
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public bool CreateMemberOnly(MailingListMember m)
        {
            m.StoreId = context.CurrentStore.Id;
            return memberRepository.Create(m);
        }
        public bool CheckMembership(long listId, string email)
        {
            MailingListMember m = memberRepository.FindByEmailForList(listId, email, context.CurrentStore.Id);
            if (m != null)
            {
                if (m.Id > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public MailingListMember FindMemberOnlyById(long id)
        {
            return memberRepository.Find(id, context.CurrentStore.Id);
        }
        public bool UpdateMemberOnly(MailingListMember m)
        {            
            return memberRepository.Update(m);
        }

        public List<MailingListSnapShot> FindAll()
        {
            return FindAllPaged(1, 1000);            
        }        
        public new List<MailingListSnapShot> FindAllPaged(int pageNumber, int pageSize)
        {

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;            
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_MailingList> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.Name).Skip(skip).Take(take);

            List<MailingListSnapShot> output = new List<MailingListSnapShot>();
            if (result != null)
            {
                foreach (Data.EF.bvc_MailingList m in result)
                {
                    MailingListSnapShot snap = new MailingListSnapShot()
                    {
                        Id = m.Id,
                        IsPrivate = m.Private,
                        LastUpdatedUtc = m.LastUpdatedUtc,
                        Name = m.Name,
                        StoreId = m.StoreId
                    };
                    output.Add(snap);
                }
            }

            return output;                      
        }

        public List<MailingListSnapShot> FindAllPublicPaged(int pageNumber, int pageSize)
        {

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_MailingList> result = repository.Find().Where(y => y.StoreId == storeId)
                                                                    .Where(y => y.Private == false).OrderBy(y => y.Name).Skip(skip).Take(take);

            List<MailingListSnapShot> output = new List<MailingListSnapShot>();
            if (result != null)
            {
                foreach (Data.EF.bvc_MailingList m in result)
                {
                    MailingListSnapShot snap = new MailingListSnapShot()
                    {
                        Id = m.Id,
                        IsPrivate = m.Private,
                        LastUpdatedUtc = m.LastUpdatedUtc,
                        Name = m.Name,
                        StoreId = m.StoreId
                    };
                    output.Add(snap);
                }
            }

            return output;
        }
      
    }
}
