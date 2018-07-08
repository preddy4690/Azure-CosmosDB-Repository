namespace AzureCosmosDB.Dao
{ 
    public class DocumentDBContext
    {
        /// <summary>
        ///     The Collection ID
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        ///     The Database ID
        /// </summary>
        public string DatabaseId { get; set; }
    }
}