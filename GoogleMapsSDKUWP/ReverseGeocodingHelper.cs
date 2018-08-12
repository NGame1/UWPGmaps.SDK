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
    public class ReverseGeoCode
    {
        /// <summary>
        /// Get Location Latitude and Longitude from Address
        /// </summary>
        /// <param name="Address">The address for reverse geocoding</param>
        /// <returns>returns a geopoint contains address Latitude and Longitude</returns>
        public static async Task<Geopoint> GetLocationGeopoint(string Address)
        {
            try
            {
                var http = Initializer.httpclient;
                var r = await http.GetStringAsync(new Uri($"http://maps.googleapis.com/maps/api/geocode/json?address={Address}&sensor=false", UriKind.RelativeOrAbsolute));
                var res = JsonConvert.DeserializeObject<Rootobject>(r).Results.FirstOrDefault().Geometry.Location;
                return new Geopoint(new BasicGeoposition() { Latitude = res.Latitude, Longitude = res.Longitude });
            }
            catch { return null; }
        }

        public static async Task<Result> GetLocation(string Address)
        {
            try
            {
                var http = Initializer.httpclient;
                var r = await http.GetStringAsync(new Uri($"http://maps.googleapis.com/maps/api/geocode/json?address={Address}&sensor=false", UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(r).Results.FirstOrDefault();
                //return new Geopoint(new BasicGeoposition() { Latitude = res.Latitude, Longitude = res.Longitude });
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
            public Address_Components[] AddressCSomponents { get; set; }
            [JsonProperty(PropertyName = "formatted_address")]
            public string FormattedAddress { get; set; }
            [JsonProperty(PropertyName = "geometry")]
            public Geometry Geometry { get; set; }
            [JsonProperty(PropertyName = "partial_match")]
            public bool partialMatch { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            [JsonProperty(PropertyName = "types")]
            public string[] Types { get; set; }
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
