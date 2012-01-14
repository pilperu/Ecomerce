using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Content
{
    public class ContentColumnRepository: ConvertingRepositoryBase<Data.EF.bvc_ContentColumn, ContentColumn>
    {
        private RequestContext context = null;
        private ContentBlockRepository blockRepository = null;

        public static ContentColumnRepository InstantiateForMemory(RequestContext c)
        {
            return new ContentColumnRepository(c, new MemoryStrategy<Data.EF.bvc_ContentColumn>(PrimaryKeyType.Bvin),  
                                            new MemoryStrategy<Data.EF.bvc_ContentBlock>(PrimaryKeyType.Bvin),                         
                                           new TextLogger());
        }
        public static ContentColumnRepository InstantiateForDatabase(RequestContext c)
        {
            ContentColumnRepository result = null;
            result = new ContentColumnRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ContentColumn>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new EntityFrameworkRepository<Data.EF.bvc_ContentBlock>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public ContentColumnRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ContentColumn> r,
                                    IRepositoryStrategy<Data.EF.bvc_ContentBlock> subr,                        
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
            this.blockRepository = new ContentBlockRepository(c, subr, this.logger);
        }

        protected override void CopyDataToModel(Data.EF.bvc_ContentColumn data, ContentColumn model)
        {
            model.Bvin = data.bvin;            
            model.StoreId = data.StoreId;
            model.LastUpdated = data.LastUpdated;
            model.DisplayName = data.DisplayName;
            model.SystemColumn = data.SystemColumn == 1;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ContentColumn data, ContentColumn model)
        {
            data.bvin = model.Bvin;
            data.StoreId = model.StoreId;
            data.LastUpdated = model.LastUpdated;
            data.DisplayName = model.DisplayName;
            data.SystemColumn = model.SystemColumn ? 1 : 0;
        }

        protected override void DeleteAllSubItems(ContentColumn model)
        {
            blockRepository.DeleteForColumn(model.Bvin, model.StoreId);
        }
        protected override void GetSubItems(ContentColumn model)
        {
            model.Blocks = blockRepository.FindForColumn(model.Bvin, model.StoreId);
        }
        protected override void MergeSubItems(ContentColumn model)
        {
            blockRepository.MergeList(model.Bvin,model.StoreId, model.Blocks);
        }


        public ContentColumn Find(string bvin)
        {
            ContentColumn result = FindForAllStores(bvin);                                   
            if (result != null)
            {
                // Skip store check if this is a system column
                if (this.IsSystemColumn(result))
                {                    
                    result.StoreId = context.CurrentStore.Id;
                    GetSubItems(result);
                    return result;
                }

                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }

            return null;
        }       
        public ContentColumn FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(ContentColumn item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;
 	        return base.Create(item);
        }
        public bool Update(ContentColumn c)
        {
            if (this.IsSystemColumn(c))
            {
                // Don't update system columns, just their blocks
                c.StoreId = context.CurrentStore.Id;
                MergeSubItems(c);
                return true;
            }
            else
            {
                if (c.StoreId != context.CurrentStore.Id)
                {
                    return false;
                }
                c.LastUpdated = DateTime.UtcNow;
                return this.Update(c, new PrimaryKey(c.Bvin));            
            }            
        }
        public bool Delete(string bvin)
        {            
            long storeId = context.CurrentStore.Id;
            ContentColumn item = Find(bvin);
            if (item == null) return false;
            if (item.SystemColumn) return false;
           return Delete(new PrimaryKey(bvin));            
        }

        public List<ContentColumn> FindAll()
        {
            int totalCount = 0;
            return FindAllPaged(1, int.MaxValue, ref totalCount);
        }
        public List<ContentColumn> FindAllPaged(int pageNumber, int pageSize, ref int totalCount)
        {
            List<ContentColumn> result = new List<ContentColumn>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ContentColumn> data = repository.Find().Where(y => y.StoreId == storeId)
                                      .OrderBy(y => y.DisplayName);
            
            result = ListPoco(data);
            result.InsertRange(0, SystemColumns(storeId));
            totalCount = result.Count;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            result = result.Skip(skip).Take(take).ToList();                      
            return result;
        }

        private bool IsSystemColumn(ContentColumn c)
        {
            List<string> systemIds = this.SystemColumns(c.StoreId).Select(y => y.Bvin.Trim()).ToList();
            if (systemIds.Contains(c.Bvin.Trim())) return true;
            return false;
        }
        private List<ContentColumn> SystemColumns(long forStoreId)
        {
            List<ContentColumn> result = new List<ContentColumn>();

            //result.Add(new ContentColumn() { Bvin = "101", DisplayName = "ADMIN: Dashboard 1", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            //result.Add(new ContentColumn() { Bvin = "102", DisplayName = "ADMIN: Dashboard 2", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            //result.Add(new ContentColumn() { Bvin = "103", DisplayName = "ADMIN: Dashboard 3", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "1", StoreId=forStoreId, DisplayName = "STORE: Homepage 1", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "2", StoreId = forStoreId, DisplayName = "STORE: Homepage 2", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "3", StoreId = forStoreId, DisplayName = "STORE: Homepage 3", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "4", StoreId = forStoreId, DisplayName = "STORE: Category Sidebar", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "5", StoreId = forStoreId, DisplayName = "STORE: Product Sidebar", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "201", StoreId = forStoreId, DisplayName = "STORE: My Account Sidebar", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "202", StoreId = forStoreId, DisplayName = "STORE: Customer Service Sidebar", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            result.Add(new ContentColumn() { Bvin = "601", StoreId = forStoreId, DisplayName = "STORE: Checkout Sidebar", SystemColumn = true, LastUpdated = new DateTime(2009, 11, 1) });
            return result;
        }

        public ContentColumn FindByDisplayName(string displayName)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ContentColumn> data = repository.Find().Where(y => y.StoreId == storeId)
                                      .Where(y => y.DisplayName == displayName);

            return FirstPoco(data);
        }
       
        public ContentColumn Clone(string sourceBvin, string newColumnName)
        {            

            Content.ContentColumn c = new Content.ContentColumn();
            c.DisplayName = newColumnName;
            c.SystemColumn = false;

            ContentColumn source = Find(sourceBvin);
            
                if (source != null)
                {
                    foreach(ContentBlock b in source.Blocks)
                    {
                        c.Blocks.Add(b.Clone());
                    }
                }
            
            Create(c);
            return c;
        }

        public bool DeleteBlock(string blockId)
        {
            return this.blockRepository.Delete(blockId);
        }
        public bool ResortBlocksItems(string policyId, List<string> sortedItemIds)
        {
            return this.blockRepository.Resort(policyId, sortedItemIds);
        }
        public ContentBlock FindBlock(string blockId)
        {
            return this.blockRepository.Find(blockId);
        }
        public bool UpdateBlock(ContentBlock item)
        {
            return this.blockRepository.Update(item);
        }
        public bool MoveBlockUp(string blockBvin, string columnId, long storeId)
        {
            bool bRet = false;


            try
            {
                List<ContentBlock> blocks;
                blocks = blockRepository.FindForColumn(columnId, storeId);

                if (blocks.Count > 0)
                {
                    // we have more than 1 item so there is a chance to switch
                    int CurrentSort = -1;
                    int LowestSort = 0;
                    int NewSort = 1;
                    string SwitchID = string.Empty;

                    // Find lowest sort order for type
                    LowestSort = blocks[0].SortOrder;

                    int iCount;
                    for (iCount = 0; iCount <= blocks.Count - 1; iCount++)
                    {
                        if (blocks[iCount].Bvin == blockBvin)
                        {
                            CurrentSort = blocks[iCount].SortOrder;
                        }

                        // no match yet
                        if (CurrentSort == -1)
                        {
                            NewSort = blocks[iCount].SortOrder;
                            SwitchID = blocks[iCount].Bvin;
                        }
                    }

                    // If we can move up, find the row to switch with
                    if (CurrentSort > LowestSort)
                    {
                        if (blockRepository.UpdateSortOrderForBlock(columnId, blockBvin, NewSort) == true)
                        {
                            if (blockRepository.UpdateSortOrderForBlock(columnId, SwitchID, CurrentSort) == true)
                            {
                                bRet = true;
                            }
                            else
                            {
                                bRet = false;
                                // Return current
                                blockRepository.UpdateSortOrderForBlock(columnId, blockBvin, CurrentSort);
                            }
                        }
                        else
                        {
                            bRet = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.Message);
                bRet = false;
            }

            return bRet;
        }
        public bool MoveBlockDown(string blockBvin, string columnId, long storeId)
        {
            bool bRet = false;

            try
            {
                List<ContentBlock> blocks;
                blocks = blockRepository.FindForColumn(columnId, storeId);

                if (blocks.Count > 0)
                {
                    // we have more than 1 item so there is a chance to switch
                    int CurrentSort = -1;
                    int MaxSort = 0;
                    int NewSort = -1;
                    string SwitchId = string.Empty;

                    // Find highest sort order for type
                    MaxSort = blocks[blocks.Count - 1].SortOrder;

                    int iCount;
                    for (iCount = 0; iCount <= blocks.Count - 1; iCount++)
                    {

                        // got the match so grab this one
                        if (NewSort == -2)
                        {
                            NewSort = blocks[iCount].SortOrder;
                            SwitchId = blocks[iCount].Bvin;
                        }

                        if (blocks[iCount].Bvin == blockBvin)
                        {
                            CurrentSort = blocks[iCount].SortOrder;
                            // Trigger New Sort
                            NewSort = -2;
                        }
                    }

                    // If we can move up, find the row to switch with
                    if (CurrentSort < MaxSort)
                    {
                        if (blockRepository.UpdateSortOrderForBlock(columnId, blockBvin, NewSort) == true)
                        {
                            if (blockRepository.UpdateSortOrderForBlock(columnId,SwitchId, CurrentSort) == true)
                            {
                                bRet = true;
                            }
                            else
                            {
                                bRet = false;
                                // Return current
                                blockRepository.UpdateSortOrderForBlock(columnId, blockBvin, CurrentSort);
                            }
                        }
                        else
                        {
                            bRet = false;
                        }
                    }

                }
            }

            catch
            {
                bRet = false;
            }

            return bRet;
        }
        public bool CopyBlockToColumn(string blockBvin, string columnId)
        {
            ContentBlock b = this.blockRepository.Find(blockBvin);
            if (b != null)
            {
                ContentBlock clone = b.Clone();
                clone.ColumnId = columnId;
                clone.Bvin = string.Empty;
                ContentColumn newColumn = Find(columnId);
                if (newColumn != null)
                {
                    newColumn.Blocks.Add(clone);
                    return Update(newColumn);
                }
                return false;
            }
            return false;
        }
        public bool MoveBlockToColumn(string blockBvin, string columnId)
        {
            ContentBlock b = this.blockRepository.Find(blockBvin);
            if (b != null)
            {
                b.ColumnId = columnId;
                return blockRepository.Update(b);                
            }
            return false;
        }
    }
}
