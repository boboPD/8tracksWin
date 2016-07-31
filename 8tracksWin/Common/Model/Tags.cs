using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Tag
    {
        public string cool_taggings_count { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

}
