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

        public enum ChangeSongUserAction
        {
            PLAY,
            NEXT,
            SKIP
        }

        public static async Task GetPlayToken()
        {
            HttpResponseMessage res = await ApiClient.GetAsync(actionMethodBaseUri + "/new.json");
            string content = await res.Content.ReadAsStringAsync();
            GlobalConfigs.PlayToken = JObject.Parse(content).SelectToken("$.play_token").Value<string>();
        }

        public static async Task<CurrentMix> Play(int mixId, ChangeSongUserAction action)
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
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/{2}.json", actionMethodBaseUri, GlobalConfigs.PlayToken, actionTxt);
            HttpResponseMessage res = await ApiClient.GetAsync(actionMethod, queryParams: new Dictionary<string, string>() { { "mix_id", mixId.ToString() } });
            CurrentMix mix = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentMix>(await res.Content.ReadAsStringAsync());

            return mix;
        }

        public static void ReportSongPlay(int trackId, int mixId)
        {
            string actionMethod = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/report.json", actionMethodBaseUri, GlobalConfigs.PlayToken);
            Dictionary<string, string> qparams = new Dictionary<string, string>()
            {
                {"track_id", trackId.ToString() },
                {"mix_id", mixId.ToString() }
            };
            ApiClient.Get(actionMethod, queryParams: qparams);
        }
    }
}
