namespace _8tracksWin.ViewModel
{
    public class UserDetailsViewModel
    {
        public string Name { get; private set; }
        public string Location { get; private set; }
        public string FollowCounts { get; private set; }
        public string PublishedMixesCount { get; private set; }
        public Common.Model.AvatarUrls AvatarUrlsObj { get; private set; } 

        public UserDetailsViewModel(Common.Model.UserInfo info)
        {
            this.Name = info.Name;
            Location = info.City + ", " + info.Country;
            FollowCounts = string.Format("{0} followers, follows {1}", info.FollowersCount, info.FollowsCount);
            PublishedMixesCount = info.MixesCount + "mixes published";
            AvatarUrlsObj = info.AvatarUrlsCollection;
        }
    }
}
