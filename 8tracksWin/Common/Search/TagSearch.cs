using Common.Model;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Common.Search
{
    public static class TagSearch
    {
        const string tagSearchBaseUri = "tags.json";

        public static async Task<List<Tag>> FetchTags(string prefix = null)
        {
            System.Collections.Generic.Dictionary<string, string> qp = null;
            if (!string.IsNullOrEmpty(prefix))
                qp = new System.Collections.Generic.Dictionary<string, string>() { { "q", prefix } };

            string rep = await ApiClient.Get(tagSearchBaseUri, queryParams: qp).Content.ReadAsStringAsync();
            JEnumerable<JToken> result = JObject.Parse(rep).SelectToken("$.tag_cloud.tags").Children();

            List<Tag> tags = new List<Tag>();

            foreach (var item in result)
            {
                tags.Add(item.ToObject<Tag>());
            }

            return tags;
        }
    }
}
