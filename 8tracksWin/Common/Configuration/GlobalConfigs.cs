using Common.Model;
using Newtonsoft.Json;

namespace Common.Configuration
{
    public static class GlobalConfigs
    {
        const string currentUserConfigName = "currentUserSetting";

        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        static User currentUser = null;

        public static User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    object val = localSettings.Values[currentUserConfigName];
                    if (val != null)
                        currentUser = JsonConvert.DeserializeObject<User>((string)val);
                }

                return currentUser;
            }

            set
            {
                string serialisedObj = JsonConvert.SerializeObject(value);
                localSettings.Values[currentUserConfigName] = serialisedObj;
            }
        }
    }
}
