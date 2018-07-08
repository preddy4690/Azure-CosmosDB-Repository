using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace AzureCosmosDB.Dao
{ 
    public class DocumentDBDao<T> : IDocumentDBDao<T>
    {
        /// <summary>
        ///     The Current DocumentDB Context
        /// </summary>
        private DocumentDBContext Context;

        /// <summary>
        ///     Client for the Database
        /// </summary>
        private readonly DocumentClient Client;

        /// <summary>
        ///     The DAO for the DocumentDB
        /// </summary>
        /// <param name="client">The Document Client</param>
        /// <param name="context">The DocumentDB Context</param>
        public DocumentDBDao(DocumentClient client, DocumentDBContext context)
        {
            Client = client;
            Context = context;
        }

        /// <summary>
        ///     Delete the Database
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        public async Task DeleteDatabaseAsync(string databaseId) 
            => await Client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));

        /// <summary>
        ///     Delete the Collection
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        /// <param name="collectionId">The Collection Id</param>
        public async Task DeleteCollectionAsync(string databaseId, string collectionId) 
            => await Client.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId));

        /// <summary>
        ///     Creates the Database if it does not exist
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        public async Task CreateDatabaseAsync(string databaseId) 
            => await Client.CreateDatabaseAsync(new Database { Id = databaseId });

        /// <summary>
        ///     Creates the Collection if it does not exist
        /// </summary>
        /// <param name="databaseId">The Database Id</param>
        /// <param name="collectionId">The Collection Id</param>
        /// <param name="throughput">The desired Throughput for the Collection</param>
        public async Task CreateCollectionAsync(string databaseId, string collectionId, int throughput) 
            => await Client.CreateDocumentCollectionAsync(
                UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection { Id = collectionId },
                new RequestOptions { OfferThroughput = throughput });

        /// <summary>
        ///     Retrieve an Item
        /// </summary>
        /// <param name="id">The ID of the Item to retrieve</param>
        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await Client.ReadDocumentAsync(UriFactory.CreateDocumentUri(Context.DatabaseId, Context.CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///     Retrieve the Items which match a supplied predicate
        /// </summary>
        /// <param name="predicate">The expression matching the Items to be retrieved</param>
        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = Client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(Context.DatabaseId, Context.CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        /// <summary>
        ///     Save an Item
        /// </summary>
        /// <param name="item">The Item</param>
        public async Task<Document> SaveItemAsync(T item)
            => await Client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(Context.DatabaseId, Context.CollectionId), item);

        /// <summary>
        ///     Delete an Item
        /// </summary>
        /// <param name="id">The ID of the Item to delete</param>
        public async Task DeleteItemAsync(string id) 
            => await Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(Context.DatabaseId, Context.CollectionId, id));
    }
}