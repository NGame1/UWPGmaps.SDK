using Windows.Web.Http;

namespace GMapsUWP
{
    class Initializer
    {
        private static HttpClient _http;
        public static HttpClient httpclient
        {
            get
            {
                if (_http == null)
                {
                    _http = new HttpClient();
                    return _http;
                }
                else return _http;
            }
        }
        public static string GoogleMapRequestsLanguage { get; private set; }
        public static string GoogleMapAPIKey { get; private set; }
        static void Initialize(string GMapAPIKey, string APILanguage = "en-US")
        {
            GoogleMapAPIKey = GMapAPIKey;
            GoogleMapRequestsLanguage = APILanguage;
        }
    }
}
