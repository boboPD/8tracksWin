namespace Common.Model
{
    public class MixSet
    {
        public string decoded_smart_id { get; set; }
        public Mix[] mixes { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public Pagination pagination { get; set; }
        public bool playback_stays_in_set { get; set; }
        public string smart_id { get; set; }
        public string smart_type { get; set; }
        public string sort { get; set; }
        public string web_path { get; set; }
    }


    public class Pagination
    {
        public int current_page { get; set; }
        public int per_page { get; set; }
        public int offset_by { get; set; }
        public int? next_page { get; set; }
        public int? previous_page { get; set; }
        public int total_entries { get; set; }
        public int total_pages { get; set; }
        public string next_page_path { get; set; }
    }

}
