using System;

namespace Common.Model
{
    public class UserMixCollection
    {
        public bool? contains_mix { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public int mixes_count { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string smart_id { get; set; }
        public DateTime updated_at { get; set; }
        public string web_path { get; set; }
    }
}
