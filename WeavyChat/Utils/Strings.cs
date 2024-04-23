namespace WeavyChat.Utils
{
    public class Strings
    {
        public static class EndPointURLs
        {
            /*AppsAPI*/
            public const string ListApps = @"${weavyUrl}/api/apps/{app}/members";
            
            /*UsersAPI*/            
            public const string GetUserToken = @"${weavyUrl}/api/users/{uid}/tokens";
            public const string UpsertUser = @"$""{weavyUrl}/api/users/{currentUser.UserName}""";
        }
    }

    
}
