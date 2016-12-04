using System.Threading.Tasks;
using Common.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Common.Model;
using System.Collections.Generic;

namespace Common
{
    public static class Music
    {
        const string actionMethodBaseUri = "sets";
        static string guestPlayToken = null;

        public enum ChangeSongUserAction
        {
            PLAY,
            NEXT,
            SKIP
        }

        private static async Task<string> GetPlayToken()
        {
            HttpResponseMessage res = await ApiClient.GetAsync(actionMethodBaseUri + "/new.json");
            string content = await res.Content.ReadAsStringAsync();

            string playToken;

            if (GlobalConfigs.CurrentUser == null)
            {
                if (guestPlayToken == null)
                {
                    playToken = JObject.Parse(content).SelectToken("$.play_token").Value<string>();
                    guestPlayToken = playToken;
                }
                else
                    playToken = guestPlayToken;
            }
            else if (GlobalConfigs.CurrentUser.PlayToken == string.Empty)
            {
                playToken = JObject.Parse(content).SelectToken("$.play_token").Value<string>();
                GlobalConfigs.CurrentUser.PlayToken = playToken;
            }
            else
                playToken = GlobalConfigs.CurrentUser.PlayToken;

            return playToken;
        }

        public static async Task<CurrentMixProperties> Play(string mixId, ChangeSongUserAction action)
        {
            string actionTxt;
            switch (action)
            {
                case ChangeSongUserAction.PLAY:
                    actionTxt = "play";
                    break;
                case ChangeSongUserAction.NEXT:
                    actionTxt = "next";
                    break;
                case ChangeSongUserAction.SKIP:
                    actionTxt = "skip";
                    break;
                default:
                    throw new System.ArgumentException("Invalid user action");
            }

            string playToken = await GetPlayToken();

            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/{2}.json", actionMethodBaseUri, playToken, actionTxt);
            HttpResponseMessage res = await ApiClient.GetAsync(actionMethod, queryParams: new Dictionary<string, string>() { { "mix_id", mixId.ToString() } });
            JObject resObj = JObject.Parse(await res.Content.ReadAsStringAsync());
            CurrentMixProperties mix = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentMixProperties>(resObj.SelectToken("$.set").ToString());

            return mix;
        }

        public static void ReportSongPlay(int trackId, int mixId)
        {
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/report.json", actionMethodBaseUri, GlobalConfigs.CurrentUser.PlayToken);
            Dictionary<string, string> qparams = new Dictionary<string, string>()
            {
                {"track_id", trackId.ToString() },
                {"mix_id", mixId.ToString() }
            };
            ApiClient.Get(actionMethod, queryParams: qparams);
        }
    }
}
