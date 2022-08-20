using Newtonsoft.Json;

namespace WorldCup2022_MVC.Session
{
    public static class SessionExtensions
    {
        public static void SetSession(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetSession<T>(this ISession session, string key)
        {
            return JsonConvert.DeserializeObject<T>(session.GetString(key));
        }
    }
}
