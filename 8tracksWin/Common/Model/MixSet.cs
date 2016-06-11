using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class MixSet
    {
        public string decoded_smart_id { get; set; }
        public Mix[] mixes { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public bool playback_stays_in_set { get; set; }
        public string smart_id { get; set; }
        public string smart_type { get; set; }
        public string sort { get; set; }
        public string web_path { get; set; }
    }
}
