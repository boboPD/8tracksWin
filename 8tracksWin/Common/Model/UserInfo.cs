using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Common.Model
{
    public class AvatarUrls
    {
        public string original { get; set; }
        public string original_imgix_url { get; set; }
        public string static_cropped_imgix_url { get; set; }
        public string cropped_imgix_url { get; set; }
        public int cropped_imgix_size { get; set; }
        public string sq56 { get; set; }
        public string sq72 { get; set; }
        public string sq100 { get; set; }
        public string sq200 { get; set; }
        public string sq400 { get; set; }
        public string sq640 { get; set; }
        public string sq750 { get; set; }
        public string max200 { get; set; }
        public string max250w { get; set; }
        public string discourse { get; set; }
        public bool animated { get; set; }
    }

    public class UserInfo
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string BioHtml { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string MemberSince { get; set; }
        public int MixesCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowsCount { get; set; }
        public AvatarUrls AvatarUrlsCollection { get; set; }

        protected UserInfo()
        { }

        protected async Task PopulateUserDetails(int userId)
        {
            string actionMethod = string.Format("/users/{0}.json", userId.ToString());
            System.Net.Http.HttpResponseMessage response = await ApiClient.GetAsync(actionMethod, queryParams: new Dictionary<string, string>() { { "include", "profile_counts+profile" } });
            JObject obj = JObject.Parse(await response.Content.ReadAsStringAsync());

            UserId = userId;
            this.Name = obj.SelectToken("$.user.login").Value<string>();
            this.BioHtml = obj.SelectToken("$.user.bio_html").Value<string>();
            this.City = obj.SelectToken("$.user.city").Value<string>();
            this.Country = obj.SelectToken("$.user.country").Value<string>();
            this.MemberSince = obj.SelectToken("$.user.member_since").Value<string>();
            this.MixesCount = obj.SelectToken("$.user.public_mixes_count").Value<int>();
            this.FollowersCount = obj.SelectToken("$.user.followers_count").Value<int>();
            this.FollowsCount = obj.SelectToken("$.user.follows_count").Value<int>();
            this.AvatarUrlsCollection = obj.SelectToken("$.user.avatar_urls").ToObject<AvatarUrls>();
        }

        public void RefreshData()
        {
            Task.WhenAll(PopulateUserDetails(this.UserId));
        }

        public static async Task<UserInfo> FetchUserDetails(int userId)
        {
            UserInfo info = new UserInfo();

            await info.PopulateUserDetails(userId);

            return info;
        }
    }
}
