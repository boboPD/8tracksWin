using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class User
    {
        public int UserId { get; }
        public string UserToken { get; }
        public bool NsfwPreference { get; set; }
        public string PlayToken { get; set; }

        public User(int userId, string userToken)
        {
            UserId = userId;
            UserToken = userToken;
            NsfwPreference = false;
            PlayToken = string.Empty;
        }
    }
}
