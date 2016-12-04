using Common.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

namespace Common
{
    public static class CollectionHandler
    {
        private static string m_listenLaterCollId = string.Empty;

        /// <summary>
        /// Function to add mixes to or remove mixes from the collection.
        /// </summary>
        /// <param name="mixId">Mix id to be added or removed.</param>
        /// <param name="collectionId">Collection to which the edit is to be made.</param>
        /// <param name="toAdd">Add the mix to collection if true. Remove if false.</param>
        /// <returns>True is operation succeeded. False otherwise.</returns>
        private static async Task EditCollection(string mixId, string collectionId, bool toAdd)
        { 
            string data = string.Format("collection_mix[collection_id]={0}&collection_mix[mix_id]={1}", collectionId, mixId);
            HttpResponseMessage response = toAdd ? await ApiClient.PostAsync("collections_mixes.json", data) : await ApiClient.DeleteAsync("collections_mixes.json", data);

            if (!response.IsSuccessStatusCode)
                throw new Exceptions.EditCollectionException(mixId, collectionId, toAdd);
        }

        private static async Task<string> GetListenLaterCollectionId()
        {
            if (m_listenLaterCollId == string.Empty)
            {
                List<Model.UserMixCollection> editableColls = await CollectionHandler.FetchEditablecollections();
                m_listenLaterCollId = (from c in editableColls where c.slug == "listen-later" select c.id).First();
            }

            return m_listenLaterCollId;
        }

        /// <summary>
        /// Fetches the list of collections editable by the current logged in user.
        /// </summary>
        /// <returns>List of collections.</returns>
        public static async Task<List<Model.UserMixCollection>> FetchEditablecollections()
        {
            string actionMethod = string.Format("users/{0}/editable_collections.jsonh", GlobalConfigs.CurrentUser.UserId);
            HttpResponseMessage response = await ApiClient.GetAsync(actionMethod);
            var obj = Newtonsoft.Json.Linq.JObject.Parse(await response.Content.ReadAsStringAsync());
            var colls = Newtonsoft.Json.JsonConvert.DeserializeObject <List<Model.UserMixCollection>>(obj.SelectToken("$.collections").ToString());

            return colls;
        }

        public static async Task AddMixToCollection(string mixId, string collectionId)
        {
            await EditCollection(mixId, collectionId, true);
        }

        public static async Task RemoveMixFromCollection(string mixId, string collectionId)
        {
            await EditCollection(mixId, collectionId, false);
        }

        public static async Task AddMixToListenLater(string mixId)
        {
            string listenLaterCollId = await GetListenLaterCollectionId();
            await EditCollection(mixId, listenLaterCollId, true);
        }

        public static async Task RemoveMixFromListenLater(string mixId)
        {
            string listenLaterCollId = await GetListenLaterCollectionId();
            await EditCollection(mixId, listenLaterCollId, false);
        }
    }
}
