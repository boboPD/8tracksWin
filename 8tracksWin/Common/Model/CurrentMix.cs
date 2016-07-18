using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class CurrentMixProperties
    {
        public bool at_beginning { get; set; }
        public bool at_end { get; set; }
        public bool at_last_track { get; set; }
        public bool skip_allowed { get; set; }
        public object skip_message { get; set; }
        public Track track { get; set; }

        public class Track
        {
            public bool faved_by_current_user { get; set; }
            public string buy_link { get; set; }
            public string buy_icon { get; set; }
            public string buy_text { get; set; }
            public string track_file_stream_url { get; set; }
            public string stream_source { get; set; }
            public int user_id { get; set; }
            public bool full_length { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string performer { get; set; }
            public string url { get; set; }
            public object year { get; set; }
            public int uid { get; set; }
            public int report_delay_s { get; set; }
            public string release_name { get; set; }
            public bool is_soundcloud { get; set; }
        }
    }
}
