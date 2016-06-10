using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Windows.Security.Credentials;

namespace Common
{
    public static class Authentication
    {
        private static string userToken;
        private const string userTokenSettingName = "userTokenSetting";

        static Authentication()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object value = localSettings.Values[userTokenSettingName];
            if (value == null)
                userToken = string.Empty;
            else
                userToken = (string)value;
        }

        public static string UserToken { get { return userToken; } }

        /// <summary>
        /// Login with username and password.
        /// </summary>
        /// <returns>True on successful login and false otherwise</returns>
        public static async System.Threading.Tasks.Task<bool> Login(string username, string password)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("login", username);
            queryParams.Add("password", password);

            HttpResponseMessage response = ApiClient.Post("sessions.json", null, queryParams);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject responseObj = JObject.Parse(responseContent);
                if (responseObj["logged_in"].Value<bool>())
                {
                    userToken = responseObj.SelectToken("$.user.user_token").Value<string>();
                    Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    localSettings.Values[userTokenSettingName] = userToken;
                    return true;
                }
            }

            return false;
        }

        public static bool LoginFacebook()
        {
            return true;
        }
    }
}
