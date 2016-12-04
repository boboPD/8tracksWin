using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Mix
    {
        public string id { get; set; }
        public string path { get; set; }
        public string web_path { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
        public bool published { get; set; }
        public bool unlisted { get; set; }
        public bool read_only { get; set; }
        public Cover_Urls cover_urls { get; set; }
        public string description { get; set; }
        public int plays_count { get; set; }
        public string tag_list_cache { get; set; }
        public DateTime first_published_at { get; set; }
        public long first_published_at_timestamp { get; set; }
        public int likes_count { get; set; }
        public string certification { get; set; }
        public bool is_promoted { get; set; }
        public int duration { get; set; }
        public int tracks_count { get; set; }
        public string[] color_palette { get; set; }
        public string[] artist_list { get; set; }
    }

    public class Cover_Urls
    {
        public string original { get; set; }
        public string original_imgix_url { get; set; }
        public string static_cropped_imgix_url { get; set; }
        public string cropped_imgix_url { get; set; }
        public int cropped_imgix_size { get; set; }
        public string sq56 { get; set; }
        public string sq72 { get; set; }
        public string sq100 { get; set; }
        public string sq133 { get; set; }
        public string max133w { get; set; }
        public string max200 { get; set; }
        public string sq250 { get; set; }
        public string sq500 { get; set; }
        public string max1024 { get; set; }
    }

}
