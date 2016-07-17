using System.Threading.Tasks;

namespace Common.Model
{
    public class LoggedInUserInfo : UserInfo
    {
        public string UserToken { get; private set; }
        public bool NsfwPreference { get; set; }
        public string PlayToken { get; set; }

        public async Task InitialiseLoggedInUserInfo(int userId, string userToken)
        {
            UserToken = userToken;
            NsfwPreference = false;
            PlayToken = string.Empty;

            await PopulateUserDetails(userId);
        }
    }
}
