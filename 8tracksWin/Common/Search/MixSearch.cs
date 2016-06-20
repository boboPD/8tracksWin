using Common.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Common.Search
{
    public static class MixSearch
    {
        const string mixSearchBaseUri = "mix_sets/";

        public static async Task<MixSet> FetchTrendingMixes()
        {
            string actionMethod = mixSearchBaseUri + CreateSmartID("all");
            HttpResponseMessage response = await ApiClient.GetAsync(actionMethod, null, new Dictionary<string, string>() { { "include", "mixes" } });
            MixSet results = JsonConvert.DeserializeObject<MixSet>(await response.Content.ReadAsStringAsync());
            return results;
        }

        private static string CreateSmartID(string smartType, string id = null, string sortType = null)
        {
            StringBuilder smartID = new StringBuilder(smartType);
            if (!string.IsNullOrEmpty(id))
                smartID.Append(":" + id);
            if (!string.IsNullOrEmpty(sortType))
                smartID.Append(":" + sortType);
            if (Configuration.GlobalConfigs.NsfwPerference)
                smartID.Append(":safe");

            return smartID.ToString();
        }

        private static async Task<MixSet> SearchMixes(string actionMethod)
        {
            string response = await ApiClient.Get(actionMethod, queryParams: new Dictionary<string, string>() { { "include", "mixes" } }).Content.ReadAsStringAsync();
            MixSet result = JsonConvert.DeserializeObject<MixSet>(response);
            return result;
        }

        public static async Task<MixSet> SearchMixesByTags(List<string> tags)
        {
            StringBuilder smartIdbuilder = new StringBuilder();
            foreach (string tag in tags)
            {            
                smartIdbuilder.Append(tag);
                smartIdbuilder.Append("+");
            }
            smartIdbuilder.Remove(smartIdbuilder.Length - 1, 1);
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}.json",mixSearchBaseUri, 
                CreateSmartID("tags", smartIdbuilder.ToString()));

            return (await SearchMixes(actionMethod));
        }

        public static async Task<MixSet> SearchMixesByArtist(string artist)
        {
            string smartID = CreateSmartID("artist", artist);
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}.json", mixSearchBaseUri, smartID);
            return (await SearchMixes(actionMethod));
        }

        public static async Task<MixSet> GetRecommendedMixes(int userId)
        {
            string smartId = CreateSmartID("recommended", userId.ToString());
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}.json", mixSearchBaseUri, smartId);
            return (await SearchMixes(actionMethod));
        }
    }
}
