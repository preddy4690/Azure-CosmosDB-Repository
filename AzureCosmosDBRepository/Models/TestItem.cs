using Newtonsoft.Json;

namespace AzureCosmosDB.Models
{
    public class TestItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "itemType")]
        public string ItemType { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        /// <summary>
        ///     Override equals to compare one TestItem with another
        /// </summary>
        /// <param name="obj">The object we're compared to</param>
        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            TestItem t = (TestItem)obj;
            return Id == t.Id
                && UserId == t.UserId
                && ItemType == t.ItemType
                && Content == t.Content;
        }

        /// <summary>
        ///     Override the hashcode for a TestItem
        /// </summary>
        public override int GetHashCode()
        {
            string hash = Id + UserId + ItemType + Content;
            return hash.GetHashCode();
        }
    }
}