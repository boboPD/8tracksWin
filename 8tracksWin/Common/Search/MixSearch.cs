using Common.Model;
using Common.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Common.Search
{
    public static class MixSearch
    {
        public enum ListType
        {
            RECOMMENDED,
            LIKED,
            LISTEN_LATER,
            FEED,
            PUBLISHED,
            TRENDING,
            HISTORY
        }

        const string mixSearchBaseUri = "mix_sets/";

        private static string CreateSmartID(string smartType, string id = null, string sortType = null)
        {
            StringBuilder smartID = new StringBuilder(smartType);
            if (!string.IsNullOrEmpty(id))
                smartID.Append(":" + id);
            if (!string.IsNullOrEmpty(sortType))
                smartID.Append(":" + sortType);
            if (Configuration.GlobalConfigs.CurrentUser != null && Configuration.GlobalConfigs.CurrentUser.NsfwPreference)
                smartID.Append(":safe");

            return smartID.ToString();
        }

        private static string ConvertListTypeToString(ListType type)
        {
            string s;
            switch (type)
            {
                case ListType.RECOMMENDED:
                    s = "recommended";
                    break;
                case ListType.LIKED:
                    s = "liked";
                    break;
                case ListType.LISTEN_LATER:
                    s = "listen_later";
                    break;
                case ListType.FEED:
                    s = "feed";
                    break;
                case ListType.PUBLISHED:
                    s = "dj";
                    break;
                case ListType.HISTORY:
                    s = "listened";
                    break;
                default:
                    throw new System.ArgumentException("Invalid ListType argument passed");
            }

            return s;
        }

        private static async Task<SearchResult> SearchMixes(string actionMethod, int pageNumber = 1)
        {
            HttpResponseMessage response = await ApiClient.GetAsync(actionMethod, null, new Dictionary<string, string>() { { "include", "mixes+pagination" }, { "page", pageNumber.ToString() } });
            JObject resObj = JObject.Parse(await response.Content.ReadAsStringAsync());
            SearchResult results = new SearchResult(JsonConvert.DeserializeObject<MixSet>(resObj.SelectToken("$.mix_set").ToString()));

            return results;
        }

        public static async Task<SearchResult> GetPageForSet(string webPathToMix, int pageToGet)
        {
            webPathToMix = webPathToMix + ".json";
            return (await SearchMixes(webPathToMix, pageToGet));
        }

        public static async Task<SearchResult> SearchMixesByTags(List<string> tags)
        {
            StringBuilder smartIdbuilder = new StringBuilder();
            foreach (string tag in tags)
            {
                smartIdbuilder.Append(tag);
                smartIdbuilder.Append("+");
            }
            smartIdbuilder.Remove(smartIdbuilder.Length - 1, 1);
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}.json", mixSearchBaseUri,
                CreateSmartID("tags", smartIdbuilder.ToString()));

            return (await SearchMixes(actionMethod));
        }

        public static async Task<SearchResult> SearchMixesByArtist(string artist)
        {
            string smartID = CreateSmartID("artist", artist);
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}.json", mixSearchBaseUri, smartID);
            return (await SearchMixes(actionMethod));
        }

        public static async Task<SearchResult> FetchTrendingMixes()
        {
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}.json", mixSearchBaseUri, CreateSmartID("all", sortType: "popular"));
            return (await SearchMixes(actionMethod));
        }

        public static async Task<SearchResult> FetchMixesforUser(ListType type, int userId)
        {
            string smartId = CreateSmartID(ConvertListTypeToString(type), userId.ToString());
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}.json", mixSearchBaseUri, smartId);
            return (await SearchMixes(actionMethod));
        }
    }
}
