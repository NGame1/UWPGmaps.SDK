using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GMapsUWP.GeoCoding
{
    public class GeocodeHelper
    {
        /// <summary>
        /// Get Address of the GeoPoint you provided 
        /// </summary>
        /// <param name="cn">GeoPoint of you want it's address</param>
        /// <returns>Address of Provided GeoPoint. throwing in Exception cause returning "Earth :D" as Address</returns>
        public static async Task<string> GetAddress(Geopoint cn)
        {
            try
            {
                var http = Initializer.httpclient;
                var r = await http.GetStringAsync(new Uri($"https://maps.googleapis.com/maps/api/geocode/json?latlng={cn.Position.Latitude},{cn.Position.Longitude}&sensor=false&language={Initializer.GoogleMapRequestsLanguage}&key={Initializer.GoogleMapAPIKey}", UriKind.RelativeOrAbsolute));
                var res = JsonConvert.DeserializeObject<Rootobject>(r);
                return res.Results.FirstOrDefault().FormattedAddress;
            }
            catch { return "Earth :D"; }
        }

        public static async Task<Rootobject> GetInfo(string PlaceID)
        {
            try
            {
                var http = Initializer.httpclient;
                var r = await http.GetStringAsync(new Uri($"https://maps.googleapis.com/maps/api/geocode/json?place_id={PlaceID}&language={Initializer.GoogleMapRequestsLanguage}&key={Initializer.GoogleMapAPIKey}", UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(r);
            }
            catch { return null; }
        }
        public static async Task<Rootobject> GetInfo(Geopoint cn)
        {
            try
            {
                var http = Initializer.httpclient;
                var r = await http.GetStringAsync(new Uri($"https://maps.googleapis.com/maps/api/geocode/json?latlng={cn.Position.Latitude},{cn.Position.Longitude}&language={Initializer.GoogleMapRequestsLanguage}&key={Initializer.GoogleMapAPIKey}", UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(r);
            }
            catch { return null; }
        }
        public class Rootobject
        {
            [JsonProperty(PropertyName = "results")]
            public Result[] Results { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

        public class Result
        {
            [JsonProperty(PropertyName = "address_components")]
            public Address_Components[] AddressComponents { get; set; }
            [JsonProperty(PropertyName = "formatted_address")]
            public string FormattedAddress { get; set; }
            [JsonProperty(PropertyName = "geometry")]
            public Geometry Geometry { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            [JsonProperty(PropertyName = "Types")]
            public string[] types { get; set; }
        }

        public class Geometry
        {
            [JsonProperty(PropertyName = "location")]
            public Location Location { get; set; }
            [JsonProperty(PropertyName = "location_type")]
            public string LocationType { get; set; }
            [JsonProperty(PropertyName = "viewport")]
            public Viewport Viewport { get; set; }
            [JsonProperty(PropertyName = "bounds")]
            public Bounds Bounds { get; set; }
        }

        public class Location
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Viewport
        {
            [JsonProperty(PropertyName = "northeast")]
            public Northeast NorthEast { get; set; }
            [JsonProperty(PropertyName = "southwest")]
            public Southwest SouthWest { get; set; }
        }

        public class Northeast
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Southwest
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Bounds
        {
            [JsonProperty(PropertyName = "northeast")]
            public Northeast1 NorthEast { get; set; }
            [JsonProperty(PropertyName = "southwest")]
            public Southwest1 SouthWest { get; set; }
        }

        public class Northeast1
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Southwest1
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Address_Components
        {
            [JsonProperty(PropertyName = "long_name")]
            public string LongName { get; set; }
            [JsonProperty(PropertyName = "short_name")]
            public string ShortName { get; set; }
            [JsonProperty(PropertyName = "types")]
            public string[] Types { get; set; }
        }

    }
}
