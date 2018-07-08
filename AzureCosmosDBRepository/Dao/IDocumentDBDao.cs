using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace AzureCosmosDB.Dao
{
    public interface IDocumentDBDao<T>
    {
        /// <summary>
        ///     Delete the Database
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        Task DeleteDatabaseAsync(string databaseId);

        /// <summary>
        ///     Delete the Collection
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        /// <param name="collectionId">The Collection Id</param>
        Task DeleteCollectionAsync(string databaseId, string collectionId);

        /// <summary>
        ///     Creates the Database if it does not exist
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        Task CreateDatabaseAsync(string databaseId);

        /// <summary>
        ///     Creates the Collection if it does not exist
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        /// <param name="collectionId">The Collection Id</param>
        /// <param name="throughput">The desired Throughput for the Collection</param>
        Task CreateCollectionAsync(string databaseId, string collectionId, int throughput);


        /// <summary>
        ///     Delete an Item
        /// </summary>
        /// <param name="id">The ID of the Item to delete</param>
        Task DeleteItemAsync(string id);

        /// <summary>
        ///     Retrieve an Item
        /// </summary>
        /// <param name="id">The ID of the Item to retrieve</param>
        Task<T> GetItemAsync(string id);

        /// <summary>
        ///     Retrieve the Items which match a supplied predicate
        /// </summary>
        /// <param name="predicate">The expression matching the Items to be retrieved</param>
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     Save an Item
        /// </summary>
        /// <param name="item">The Item</param>
        Task<Document> SaveItemAsync(T item);
    }
}