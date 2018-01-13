using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMapsUWP
{
    class Initializer
    {
        public static string GoogleMapRequestsLanguage { get; private set; }
        public static string GoogleMapAPIKey { get; private set; }
        static void Initialize(string GMapAPIKey, string APILanguage = "en-US")
        {
            GoogleMapAPIKey = GMapAPIKey;
            GoogleMapRequestsLanguage = APILanguage;
        }
    }
}
