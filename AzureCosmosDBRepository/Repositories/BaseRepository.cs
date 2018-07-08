using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AzureCosmosDB.Config;
using AzureCosmosDB.Dao;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace AzureCosmosDB.Repositories
{
    public abstract class BaseRepository<T>
    {
        /// <summary>
        ///     The Database Interface
        /// </summary>
        private readonly IDocumentDBDao<T> _database;

        /// <summary>
        ///     The Minimum Provisioned Throughput for Collections
        /// </summary>
        private const int MinThroughput = 400;

        /// <summary>
        ///     Base Class for a repository
        /// </summary>
        /// <param name="context">The DocumentDB Context</param>
        protected BaseRepository(DocumentDBContext context)
        {
            DocumentClient client = new DocumentClient(new Uri(CosmosDBCredentials.Endpoint), CosmosDBCredentials.Key);
            _database = new DocumentDBDao<T>(client, context);
        }

        /// <summary>
        ///     Delete the Database
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        public Task DeleteDatabaseAsync(string databaseId)
            => _database.DeleteDatabaseAsync(databaseId);

        /// <summary>
        ///     Delete the Collection
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        /// <param name="collectionId">The Collection Id</param>
        public Task DeleteCollectionAsync(string databaseId, string collectionId)
            => _database.DeleteCollectionAsync(databaseId, collectionId);

        /// <summary>
        ///     Creates the Database if it does not exist
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        public Task CreateDatabaseAsync(string databaseId)
            => _database.CreateDatabaseAsync(databaseId);

        /// <summary>
        ///     Creates the Collection if it does not exist
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        /// <param name="collectionId">The Collection Id</param>
        /// <param name="throughput">The desired Throughput for the Collection</param>
        public Task CreateCollectionAsync(string databaseId, string collectionId, int throughput = MinThroughput)
            => _database.CreateCollectionAsync(databaseId, collectionId, throughput);

        /// <summary>
        ///     Delete an Item
        /// </summary>
        /// <param name="id">The ID of the Item to delete</param>
        public Task DeleteItemAsync(string id)
            => _database.DeleteItemAsync(id);

        /// <summary>
        ///     Retrieve an Item
        /// </summary>
        /// <param name="id">The ID of the Item to retrieve</param>
        public Task<T> GetItemAsync(string id)
            => _database.GetItemAsync(id);

        /// <summary>
        ///     Retrieve the Items which match a supplied predicate
        /// </summary>
        /// <param name="predicate">The expression matching the Items to be retrieved</param>
        public Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
            => _database.GetItemsAsync(predicate);

        /// <summary>
        ///     Save an Item
        /// </summary>
        /// <param name="item">The Item</param>
        public Task<Document> SaveItemAsync(T item)
            => _database.SaveItemAsync(item);
    }
}
