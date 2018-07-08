using System;
using System.Collections.Generic;
using AzureCosmosDB.Dao;
using AzureCosmosDB.Models;

namespace AzureCosmosDB.Repositories
{
    public class TestItemRepository : BaseRepository<TestItem>
    {
        /// <summary>
        ///     Test Item repository
        /// </summary>
        /// <param name="context">The DocumentDB Context</param>
        public TestItemRepository(DocumentDBContext context) : base(context)
        {
        }        
    }
}
