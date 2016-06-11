using Common.Model;
using System.Threading.Tasks;

namespace Common.Search
{
    public static class TagSearch
    {
        const string tagSearchBaseUri = "tags.json";

        public static async Task<TagSet> FetchTags(string prefix = null)
        {
            System.Collections.Generic.Dictionary<string, string> qp = null;
            if (!string.IsNullOrEmpty(prefix))
                qp = new System.Collections.Generic.Dictionary<string, string>() { { "q", prefix } };

            string rep = await ApiClient.Get(tagSearchBaseUri, queryParams: qp).Content.ReadAsStringAsync();
            TagSet result = Newtonsoft.Json.JsonConvert.DeserializeObject<TagSet>(rep);

            return result;
        }
    }
}
