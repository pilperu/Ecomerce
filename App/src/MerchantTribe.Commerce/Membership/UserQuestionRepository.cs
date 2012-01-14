using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;


namespace MerchantTribe.Commerce.Membership
{
    public class UserQuestionRepository: ConvertingRepositoryBase<Data.EF.bvc_UserQuestions, UserQuestion>
    {
        private RequestContext context = null;

        public static UserQuestionRepository InstantiateForMemory(RequestContext c)
        {
            return new UserQuestionRepository(c, new MemoryStrategy<Data.EF.bvc_UserQuestions>(PrimaryKeyType.Bvin),                                           
                                           new TextLogger());
        }
        public static UserQuestionRepository InstantiateForDatabase(RequestContext c)
        {
            UserQuestionRepository result = null;
            result = new UserQuestionRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_UserQuestions>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public UserQuestionRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_UserQuestions> r,                                    
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_UserQuestions data, UserQuestion model)
        {
            model.Bvin = data.bvin;            
            model.StoreId = data.StoreId;
            model.LastUpdated = data.LastUpdated;
            model.Name = data.QuestionName;
            model.Order = data.Order;
            model.Type = (UserQuestionType)data.QuestionType;
            model.ReadValuesFromXml(data.Values);                   
        }
        protected override void CopyModelToData(Data.EF.bvc_UserQuestions data, UserQuestion model)
        {
            data.bvin = model.Bvin;
            data.StoreId = model.StoreId;
            data.LastUpdated = model.LastUpdated;
            data.QuestionName = model.Name;
            data.Order = model.Order;
            data.QuestionType = (int)model.Type;
            data.Values = model.WriteValuesToXml();
        }
      
        public UserQuestion Find(string bvin)
        {
            UserQuestion result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public UserQuestion FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(UserQuestion item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;
 	        return base.Create(item);
        }
        public bool Update(UserQuestion c)
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
            UserQuestion item = Find(bvin);
            if (item == null) return false;

           return Delete(new PrimaryKey(bvin));            
        }

        public List<UserQuestion> FindAll()
        {
            int totalCount = 0;
            return FindAllPaged(1, int.MaxValue, ref totalCount);
        }
        public List<UserQuestion> FindAllPaged(int pageNumber, int pageSize, ref int totalCount)
        {
            List<UserQuestion> result = new List<UserQuestion>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_UserQuestions> data = repository.Find().Where(y => y.StoreId == storeId)
                                      .OrderBy(y => y.Order);

            var countData = data;
            totalCount = countData.Count();

            var paged = PageItems(pageNumber, pageSize, data);
            result = ListPoco(paged);
            if (result == null) result = new List<UserQuestion>();
                      
            return result;
        }

        public bool MoveUp(string bvin)
        {
            List<UserQuestion> questions = FindAll();
            for (int i = 0; i <= (questions.Count - 1); i++)
            {
                if (questions[i].Bvin == bvin)
                {
                    if (i != (questions.Count - 1))
                    {
                        questions[i].Order += 1;
                        questions[i + 1].Order -= 1;
                        Update(questions[i]);
                        Update(questions[i + 1]);
                    }
                }
            }
            return true;
        }
        public bool MoveDown(string bvin)
        {
            List<UserQuestion> questions = FindAll();
            for (int i = 0; i <= (questions.Count - 1); i++)
            {
                if (questions[i].Bvin == bvin)
                {
                    if (i > 0)
                    {
                        questions[i].Order -= 1;
                        questions[i - 1].Order += 1;
                        Update(questions[i]);
                        Update(questions[i - 1]);
                    }
                }
            }
            return true;
        }		
             
    }
}
