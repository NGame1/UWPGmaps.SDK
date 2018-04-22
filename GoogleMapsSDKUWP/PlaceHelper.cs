using GMapsUWP.Photos;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Web.Http;

namespace GMapsUWP.Place
{
    public class PlaceSearchHelper
    {
        public enum PlaceTypesEnum
        {
            NOTMENTIONED,
            accounting, airport, amusement_park, aquarium, art_gallery, atm, bakery, bank, bar, beauty_salon,
            bicycle_store, book_store, bowling_alley, bus_station, cafe, campground, car_dealer, car_rental,
            car_repair, car_wash, casino, cemetery, church, city_hall, clothing_store, convenience_store,
            courthouse, dentist, department_store, doctor, electrician, electronics_store, embassy, fire_station,
            florist, funeral_home, furniture_store, gas_station, gym, hair_care, hardware_store, hindu_temple,
            home_goods_store, hospital, insurance_agency, jewelry_store, laundry, lawyer, library, liquor_store,
            local_government_office, locksmith, lodging, meal_delivery, meal_takeaway, mosque, movie_rental,
            movie_theater, moving_company, museum, night_club, painter, park, parking, pet_store, pharmacy,
            physiotherapist, plumber, police, post_office, real_estate_agency, restaurant, roofing_contractor,
            rv_park, school, shoe_store, shopping_mall, spa, stadium, storage, store, subway_station, supermarket,
            synagogue, taxi_stand, train_station, transit_station, travel_agency, veterinary_care, zoo
        }
        public enum SearchPriceEnum
        {
            MostAffordable = 0,
            Affordable = 1,
            Normal = 2,
            Expensive = 3,
            MostExpensive = 4,
            NonSpecified = 5
        }
        /// <summary>
        /// Search nearby places in the mentioned Radius
        /// </summary>
        /// <param name="Location"> The latitude/longitude around which to retrieve place information. This must be specified as latitude,longitude</param>
        /// <param name="Radius">Defines the distance (in meters) within which to return place results. The maximum allowed radius is 50 000 meters. </param>
        /// <param name="Keyword">A term to be matched against all content that Google has indexed for this place, including but not limited to name, type, and address, as well as customer reviews and other third-party content.</param>
        /// <param name="MinPrice">Restricts results to only those places within the specified range. Valid values range between 0 (most affordable) to 4 (most expensive), inclusive.</param>
        /// <param name="MaxPrice">Restricts results to only those places within the specified range. Valid values range between 0 (most affordable) to 4 (most expensive), inclusive.</param>
        /// <returns>Search Result. LOL :D</returns>
        public static async Task<Rootobject> NearbySearch(BasicGeoposition Location, int Radius, string Keyword = "", SearchPriceEnum MinPrice = SearchPriceEnum.NonSpecified, SearchPriceEnum MaxPrice = SearchPriceEnum.NonSpecified, PlaceTypesEnum type = PlaceTypesEnum.NOTMENTIONED)
        {
            try
            {
                if (Radius > 50000)
                {
                    throw new IndexOutOfRangeException("Radious Value is out of expected range.");
                }
                string para = "";
                para += $"location={Location.Latitude},{Location.Longitude}&radius={Radius}";
                if (Keyword != "") para += $"&keyword={Keyword}"; if (MinPrice != SearchPriceEnum.NonSpecified) para += $"&minprice={(int)MinPrice}"; if (MaxPrice != SearchPriceEnum.NonSpecified) para += $"&maxprice={(int)MaxPrice}"; if (type != PlaceTypesEnum.NOTMENTIONED) para += $"&type={type.ToString()}";
                para += $"&key={Initializer.GoogleMapAPIKey}&language={Initializer.GoogleMapRequestsLanguage}";
                var http = Initializer.httpclient;
                var st = await http.GetStringAsync(new Uri("https://maps.googleapis.com/maps/api/place/nearbysearch/json?" + para, UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(st);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Rootobject> TextSearch(string query, Geopoint Location = null, int Radius = 0, string Region = "", string NextPageToken = "", SearchPriceEnum MinPrice = SearchPriceEnum.NonSpecified, SearchPriceEnum MaxPrice = SearchPriceEnum.NonSpecified, PlaceTypesEnum type = PlaceTypesEnum.NOTMENTIONED)
        {
            try
            {
                if (Radius > 50000)
                {
                    throw new IndexOutOfRangeException("Radious Value is out of expected range.");
                }
                if (Location != null && Radius == 0) { throw new Exception("Location and radius values must having values"); }
                string para = "";
                para += $"query={query.Replace(" ", "+")}";
                if (Location != null) para += $"&location={Location.Position.Latitude},{Location.Position.Longitude}&radius={Radius}";
                if (Region != "") para += $"&region={Region}"; if (MinPrice != SearchPriceEnum.NonSpecified) para += $"&minprice={(int)MinPrice}"; if (MaxPrice != SearchPriceEnum.NonSpecified) para += $"&maxprice={(int)MaxPrice}"; if (type != PlaceTypesEnum.NOTMENTIONED) para += $"&type={type.ToString()}";
                para += $"&language={Initializer.GoogleMapRequestsLanguage}&key={Initializer.GoogleMapAPIKey}";
                var http = Initializer.httpclient;
                var st = await http.GetStringAsync(new Uri("https://maps.googleapis.com/maps/api/place/textsearch/json?" + para, UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(st);
            }
            catch
            {
                return null;
            }
        }

        public class Rootobject
        {
            [JsonProperty(PropertyName = "html_attributions")]
            public object[] HtmlAttributions { get; set; }
            [JsonProperty(PropertyName = "next_page_token")]
            public string NextPageToken { get; set; }
            [JsonProperty(PropertyName = "results")]
            public Result[] Results { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

        public class Result
        {
            [JsonProperty(PropertyName = "geometry")]
            public Geometry Geometry { get; set; }
            [JsonProperty(PropertyName = "icon")]
            public string Icon { get; set; }
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "photos")]
            public Photo[] Photos { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            [JsonProperty(PropertyName = "reference")]
            public string Reference { get; set; }
            [JsonProperty(PropertyName = "scope")]
            public string Scope { get; set; }
            [JsonProperty(PropertyName = "types")]
            public string[] Types { get; set; }
            [JsonProperty(PropertyName = "vicinity")]
            public string Vicinity { get; set; }
            [JsonProperty(PropertyName = "opening_hours")]
            public Opening_Hours OpeningHours { get; set; }
            [JsonProperty(PropertyName = "rating")]
            public float Rating { get; set; }
        }

        public class Geometry
        {
            [JsonProperty(PropertyName = "location")]
            public Location Location { get; set; }
            [JsonProperty(PropertyName = "viewport")]
            public Viewport Viewport { get; set; }
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

        public class Opening_Hours
        {
            [JsonProperty(PropertyName = "open_now")]
            public bool OpenNow { get; set; }
            [JsonProperty(PropertyName = "weekday_text")]
            public object[] WeekDayText { get; set; }
        }

        public class Photo
        {
            [JsonProperty(PropertyName = "height")]
            public int Height { get; set; }
            [JsonProperty(PropertyName = "html_attributions")]
            public string[] HtmlAttributions { get; set; }
            [JsonProperty(PropertyName = "photo_reference")]
            public string PhotoReference { get; set; }
            [JsonProperty(PropertyName = "width")]
            public int Width { get; set; }
            public Uri PlaceThumbnail { get { return PhotosHelper.GetPhotoUri(PhotoReference, 350, 350); } }
        }

    }

    public class PlaceDetailsHelper
    {
        /// <summary>
        /// Get a place details using place id
        /// </summary>
        /// <param name="PlaceID">Google Maps place id</param>
        /// <returns>Details of a place including phone number, address and etc.</returns>
        public static async Task<Rootobject> GetPlaceDetails(string PlaceID)
        {
            try
            {
                var http = Initializer.httpclient;
                var res = await http.GetStringAsync(new Uri($"https://maps.googleapis.com/maps/api/place/details/json?placeid={PlaceID}&key={Initializer.GoogleMapAPIKey}", UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(res);
            }
            catch
            {
                return null;
            }
        }
        public static async Task<Rootobject> GetPlaceDetailsbyReference(string ReferenceID)
        {
            try
            {
                var http = Initializer.httpclient;
                var res = await http.GetStringAsync(new Uri($"https://maps.googleapis.com/maps/api/place/details/json?reference={ReferenceID}&key={Initializer.GoogleMapAPIKey}", UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(res);
            }
            catch
            {
                return null;
            }
        }
        public class Rootobject
        {
            [JsonProperty(PropertyName = "html_attributions")]
            public object[] HtmlAttributions { get; set; }
            [JsonProperty(PropertyName = "result")]
            public Result Result { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

        public class Result
        {
            [JsonProperty(PropertyName = "address_components")]
            public Address_Components[] AddressComponents { get; set; }
            [JsonProperty(PropertyName = "adr_address")]
            public string AdrAddress { get; set; }
            [JsonProperty(PropertyName = "formatted_address")]
            public string FormattedAddress { get; set; }
            [JsonProperty(PropertyName = "formatted_phone_number")]
            public string FormatedPhoneNumber { get; set; }
            [JsonProperty(PropertyName = "geometry")]
            public Geometry Geometry { get; set; }
            [JsonProperty(PropertyName = "icon")]
            public string Icon { get; set; }
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "international_phone_number")]
            public string InternationalPhoneNumber { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "opening_hours")]
            public Opening_Hours OpeningHours { get; set; }
            [JsonProperty(PropertyName = "photos")]
            public Photo[] Photos { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            [JsonProperty(PropertyName = "rating")]
            public float Rating { get; set; }
            [JsonProperty(PropertyName = "reference")]
            public string Reference { get; set; }
            [JsonProperty(PropertyName = "reviews")]
            public Review[] Reviews { get; set; }
            [JsonProperty(PropertyName = "scope")]
            public string Scope { get; set; }
            [JsonProperty(PropertyName = "types")]
            public string[] Types { get; set; }
            [JsonProperty(PropertyName = "url")]
            public string Url { get; set; }
            [JsonProperty(PropertyName = "utc_offset")]
            public int UtcOffset { get; set; }
            [JsonProperty(PropertyName = "vicinity")]
            public string Vicinity { get; set; }
            [JsonProperty(PropertyName = "website")]
            public string Website { get; set; }
        }

        public class Geometry
        {
            [JsonProperty(PropertyName = "location")]
            public Location Location { get; set; }
            [JsonProperty(PropertyName = "viewport")]
            public Viewport Viewport { get; set; }
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

        public class Opening_Hours
        {
            [JsonProperty(PropertyName = "open_now")]
            public bool OpenNow { get; set; }
            [JsonProperty(PropertyName = "periods")]
            public Period[] Periods { get; set; }
            [JsonProperty(PropertyName = "weekday_text")]
            public string[] WeekdayText { get; set; }
        }

        public class Period
        {
            [JsonProperty(PropertyName = "close")]
            public Close Close { get; set; }
            [JsonProperty(PropertyName = "open")]
            public Open Open { get; set; }
        }

        public class Close
        {
            [JsonProperty(PropertyName = "day")]
            public int Day { get; set; }
            [JsonProperty(PropertyName = "time")]
            public string Time { get; set; }
        }

        public class Open
        {
            [JsonProperty(PropertyName = "day")]
            public int Day { get; set; }
            [JsonProperty(PropertyName = "time")]
            public string Time { get; set; }
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

        public class Photo
        {
            [JsonProperty(PropertyName = "height")]
            public int Height { get; set; }
            [JsonProperty(PropertyName = "html_attributions")]
            public string[] HtmlAttributions { get; set; }
            [JsonProperty(PropertyName = "photo_reference")]
            public string PhotoReference { get; set; }
            [JsonProperty(PropertyName = "width")]
            public int Width { get; set; }
            public Uri PhotoThumbnail { get { return PhotosHelper.GetPhotoUri(PhotoReference, 350, 350); } }
        }

        public class Review
        {
            [JsonProperty(PropertyName = "author_name")]
            public string AuthorName { get; set; }
            [JsonProperty(PropertyName = "author_url")]
            public string AuthorUrl { get; set; }
            [JsonProperty(PropertyName = "language")]
            public string Language { get; set; }
            [JsonProperty(PropertyName = "profile_photo_url")]
            public string ProfilePhotoUrl { get; set; }
            [JsonProperty(PropertyName = "rating")]
            public int Rating { get; set; }
            [JsonProperty(PropertyName = "relative_time_description")]
            public string RelativeTimeDescription { get; set; }
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "time")]
            public int Time { get; set; }
        }

    }

    public class PlaceAddHelper
    {
        /// <summary>
        /// Add a missing place to Google Maps 
        /// </summary>
        /// <param name="PlaceInfo">Information about the place you want to add</param>
        /// <returns>return status about the place you added</returns>
        public static async Task<Response> AddPlace(Rootobject PlaceInfo)
        {
            try
            {
                var http = Initializer.httpclient;
                using (var r = await http.PostAsync(new Uri($"https://maps.googleapis.com/maps/api/place/add/json?key={Initializer.GoogleMapAPIKey}", UriKind.RelativeOrAbsolute), new HttpStringContent(JsonConvert.SerializeObject(PlaceInfo))))
                {
                    return JsonConvert.DeserializeObject<Response>((await r.Content.ReadAsStringAsync()));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public class Rootobject
        {
            [JsonProperty(PropertyName = "location")]
            public Location Location { get; set; }
            [JsonProperty(PropertyName = "accuracy")]
            public int Accuracy { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "phone_number")]
            public string PhoneNumber { get; set; }
            [JsonProperty(PropertyName = "address")]
            public string Address { get; set; }
            [JsonProperty(PropertyName = "types")]
            public string[] Types { get; set; }
            [JsonProperty(PropertyName = "website")]
            public string Website { get; set; }
            [JsonProperty(PropertyName = "language")]
            public string Language { get; set; }
        }

        public class Location
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Response
        {
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            [JsonProperty(PropertyName = "scope")]
            public string Scope { get; set; }
            [JsonProperty(PropertyName = "reference")]
            public string Reference { get; set; }
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
        }

    }

    public class PlaceDeleteHelper
    {/// <summary>
     /// Add a missing place to Google Maps 
     /// </summary>
     /// <param name="PlaceInfo">Information about the place you want to add</param>
     /// <returns>return status about the place you added</returns>
        public static async Task<Response> DeletePlace(Rootobject PlaceInfo)
        {
            try
            {
                var http = Initializer.httpclient;
                using (var r = await http.PostAsync(new Uri($"https://maps.googleapis.com/maps/api/place/delete/json?key={Initializer.GoogleMapAPIKey}", UriKind.RelativeOrAbsolute), new HttpStringContent(JsonConvert.SerializeObject(PlaceInfo))))
                {
                    return JsonConvert.DeserializeObject<Response>((await r.Content.ReadAsStringAsync()));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public class Rootobject
        {
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
        }

        public class Response
        {
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

    }

    public class PlaceAutoComplete
    {
        public static async Task<Rootobject> GetAutoCompleteResults(string input, int radius = 0, Geopoint location = null)
        {
            try
            {
                var http = Initializer.httpclient;
                var para = $"input={input}&language={Initializer.GoogleMapRequestsLanguage}&key={Initializer.GoogleMapAPIKey}";
                if (radius != 0) para += $"&radius={radius}"; if (location != null) para += $"&location={location.Position.Latitude},{location.Position.Longitude}";
                var r = await http.GetStringAsync(new Uri($"https://maps.googleapis.com/maps/api/place/autocomplete/json?{para}"));

                return JsonConvert.DeserializeObject<Rootobject>(r);
            }
            catch
            {
                return null;
            }
        }

        public class Rootobject
        {
            [JsonProperty(PropertyName = "predictions")]
            public Prediction[] Predictions { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

        public class Prediction
        {
            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "matched_substrings")]
            public Matched_Substrings[] MatchedSubstrings { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            [JsonProperty(PropertyName = "reference")]
            public string Reference { get; set; }
            [JsonProperty(PropertyName = "structured_formatting")]
            public Structured_Formatting StructuredFormatting { get; set; }
            [JsonProperty(PropertyName = "terms")]
            public Term[] Terms { get; set; }
            [JsonProperty(PropertyName = "types")]
            public string[] Types { get; set; }
        }

        public class Structured_Formatting
        {
            [JsonProperty(PropertyName = "main_text")]
            public string MainTtext { get; set; }
            [JsonProperty(PropertyName = "main_text_matched_substrings")]
            public Main_Text_Matched_Substrings[] MainTextMatchedSubstrings { get; set; }
            [JsonProperty(PropertyName = "secondary_text")]
            public string SecondaryText { get; set; }
        }

        public class Main_Text_Matched_Substrings
        {
            [JsonProperty(PropertyName = "length")]
            public int Length { get; set; }
            [JsonProperty(PropertyName = "offset")]
            public int Offset { get; set; }
        }

        public class Matched_Substrings
        {
            [JsonProperty(PropertyName = "length")]
            public int Length { get; set; }
            [JsonProperty(PropertyName = "offset")]
            public int Offset { get; set; }
        }

        public class Term
        {
            [JsonProperty(PropertyName = "offset")]
            public int offset { get; set; }
            [JsonProperty(PropertyName = "value")]
            public string Value { get; set; }
        }

    }
}
