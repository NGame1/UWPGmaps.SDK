using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;

namespace GMapsUWP.Directions
{
    public class DirectionsHelper
    {
        /// <summary>
        /// Available modes for Direction
        /// </summary>
        public enum DirectionModes
        {
            driving, walking, bicycling, transit
        }
        /// <summary>
        /// Get Directions from a Origin to a Destination
        /// </summary>
        /// <param name="Origin">The Origin BasicGeoposition</param>
        /// <param name="Destination">The Destination BasicGeoposition</param>
        /// <param name="Mode">Mode for example Driving, walking or etc.</param>
        /// <param name="WayPoints">Points you want to go in your way</param>
        /// <exception cref="ArgumentOutOfRangeException">Waypoints are not available in transit mode.</exception>
        /// <returns></returns>
        public static async Task<Rootobject> GetDirections(BasicGeoposition Origin, BasicGeoposition Destination, DirectionModes Mode = DirectionModes.driving, List<BasicGeoposition> WayPoints = null)
        {
            try
            {
                if (Mode == DirectionModes.transit && WayPoints != null)
                {
                    throw new ArgumentOutOfRangeException("Waypoints are not available in transit mode.");
                }
                var m = Mode.ToString();
                var requestUrl = String.Format("https://maps.google.com/maps/api/directions/json?origin=" + Origin.Latitude + "," + Origin.Longitude + "&destination=" + Destination.Latitude + "," + Destination.Longitude + "&units=metric&mode=" + Mode + "&language=" + Initializer.GoogleMapRequestsLanguage);
                if (WayPoints != null && WayPoints.Count != 0)
                {
                    requestUrl += "&waypoints=";
                    for (int i = 0; i <= WayPoints.Count - 1; i++)
                    {
                        if (i < WayPoints.Count - 1)
                            requestUrl += $"{WayPoints[i].Latitude},{WayPoints[i].Longitude}|";
                        else
                            requestUrl += $"{WayPoints[i].Latitude},{WayPoints[i].Longitude}";
                    }
                }
                requestUrl += $"&key={AppCore.GoogleMapAPIKey}";
                var http = Initializer.httpclient;
                var s = await http.GetStringAsync(new Uri(requestUrl, UriKind.RelativeOrAbsolute));
                return JsonConvert.DeserializeObject<Rootobject>(s);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Create a MapPolyLine from RootObject and select FirstOrDefault route to show on map control
        /// </summary>
        /// <param name="FuncResp">Convert first or default route to mappolyline to show on map</param>
        /// <returns></returns>
        public static MapPolyline GetDirectionAsRoute(Rootobject FuncResp, Color ResultColor)
        {
            var loclist = new List<BasicGeoposition>();
            //var points = DecodePolylinePoints(FuncResp.Routes.FirstOrDefault().OverviewPolyline.Points);
            var points = DecodePolylinePoints(FuncResp.Routes.FirstOrDefault().Legs.FirstOrDefault().Steps.ToList());
            foreach (var item in points)
            {
                loclist.Add(item);
            }
            //foreach (var leg in FuncResp.routes.FirstOrDefault().legs)
            //{
            //    foreach (var step in leg.steps)
            //    {
            //        loclist.Add(
            //            new BasicGeoposition()
            //            { Latitude = step.start_location.lat, Longitude = step.start_location.lng });
            //        loclist.Add(
            //            new BasicGeoposition()
            //            { Latitude = step.end_location.lat, Longitude = step.end_location.lng });
            //    }
            //}
            MapPolyline line = new MapPolyline()
            {
                StrokeThickness = 5,
                StrokeDashed = true,
                StrokeColor = ResultColor,
                Path = new Geopath(loclist)
            };
            return line;
        }
        /// <summary>
        /// Create a MapPolyLine from Route to show on map control
        /// </summary>
        /// <param name="Route">Selected route for converting to mappolyline</param>
        /// <returns></returns>
        public static MapPolyline GetDirectionAsRoute(Route Route, Color ResultColor)
        {
            var loclist = new List<BasicGeoposition>();
            //var points = DecodePolylinePoints(Route.OverviewPolyline.Points);
            var points = DecodePolylinePoints(Route.Legs.FirstOrDefault().Steps.ToList());
            foreach (var item in points)
            {
                loclist.Add(item);
            }
            //foreach (var leg in Route.legs)
            //{
            //    foreach (var step in leg.steps)
            //    {
            //        loclist.Add(
            //            new BasicGeoposition()
            //            { Latitude = step.start_location.lat, Longitude = step.start_location.lng });
            //        loclist.Add(
            //            new BasicGeoposition()
            //            { Latitude = step.end_location.lat, Longitude = step.end_location.lng });
            //    }
            //}
            MapPolyline line = new MapPolyline()
            {
                StrokeThickness = 5,
                StrokeDashed = true,
                StrokeColor = ResultColor,
                Path = new Geopath(loclist)
            };
            return line;
        }

        public static string GetDistance(Route Route)
        {
            var Distance = 0;
            foreach (var item in Route.Legs)
            {
                Distance += item.Distance.Value;
            }
            return $"{Distance} meters";
        }

        public static string GetTotalEstimatedTime(Route Route)
        {
            var EstimatedTime = 0;
            foreach (var item in Route.Legs)
            {
                EstimatedTime += item.Duration.Value;
            }
            return $"{Convert.ToInt32((EstimatedTime / 60))} minutes";
        }

        public static List<BasicGeoposition> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<BasicGeoposition> poly = new List<BasicGeoposition>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    BasicGeoposition p = new BasicGeoposition();
                    p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                    p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch
            {
                // logo it
            }
            return poly;
        }
        public static List<BasicGeoposition> DecodePolylinePoints(List<Step> encodedPoints)
        {
            if (encodedPoints == null) return null;
            List<BasicGeoposition> poly = new List<BasicGeoposition>();
            foreach (var encodedPoint in encodedPoints)
            {
                if (encodedPoint != null)
                {
                    char[] polylinechars = encodedPoint.Polyline.Points.ToCharArray();
                    int index = 0;

                    int currentLat = 0;
                    int currentLng = 0;
                    int next5bits;
                    int sum;
                    int shifter;

                    try
                    {
                        while (index < polylinechars.Length)
                        {
                            // calculate next latitude
                            sum = 0;
                            shifter = 0;
                            do
                            {
                                next5bits = (int)polylinechars[index++] - 63;
                                sum |= (next5bits & 31) << shifter;
                                shifter += 5;
                            } while (next5bits >= 32 && index < polylinechars.Length);

                            if (index >= polylinechars.Length)
                                break;

                            currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                            //calculate next longitude
                            sum = 0;
                            shifter = 0;
                            do
                            {
                                next5bits = (int)polylinechars[index++] - 63;
                                sum |= (next5bits & 31) << shifter;
                                shifter += 5;
                            } while (next5bits >= 32 && index < polylinechars.Length);

                            if (index >= polylinechars.Length && next5bits >= 32)
                                break;

                            currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                            BasicGeoposition p = new BasicGeoposition();
                            p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                            p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                            poly.Add(p);
                        }
                    }
                    catch
                    {
                        // logo it
                    }
                }
            }
            return poly;
        }

        public class Rootobject
        {
            [JsonProperty(PropertyName = "geocoded_waypoints")]
            public Geocoded_Waypoints[] GeocodedWaypoints { get; set; }
            [JsonProperty(PropertyName = "routes")]
            public Route[] Routes { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

        public class Geocoded_Waypoints
        {
            [JsonProperty(PropertyName = "geocoder_status")]
            public string Geocoder_Status { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            [JsonProperty(PropertyName = "types")]
            public string[] Types { get; set; }
        }

        public class Route
        {
            [JsonProperty(PropertyName = "bounds")]
            public Bounds Bounds { get; set; }
            [JsonProperty(PropertyName = "copyrights")]
            public string Copyrights { get; set; }
            [JsonProperty(PropertyName = "legs")]
            public Leg[] Legs { get; set; }
            [JsonProperty(PropertyName = "overview_polyline")]
            public Overview_Polyline OverviewPolyline { get; set; }
            [JsonProperty(PropertyName = "summary")]
            public string Summary { get; set; }
            [JsonProperty(PropertyName = "warnings")]
            public string[] Warnings { get; set; }
            [JsonProperty(PropertyName = "waypoint_order")]
            public object[] WaypointOrder { get; set; }
        }

        public class Bounds
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

        public class Overview_Polyline
        {
            [JsonProperty(PropertyName = "points")]
            public string Points { get; set; }
        }

        public class Leg
        {
            [JsonProperty(PropertyName = "distance")]
            public Distance Distance { get; set; }
            [JsonProperty(PropertyName = "duration")]
            public Duration Duration { get; set; }
            [JsonProperty(PropertyName = "end_address")]
            public string EndAddress { get; set; }
            [JsonProperty(PropertyName = "end_location")]
            public End_Location EndLocation { get; set; }
            [JsonProperty(PropertyName = "start_address")]
            public string StartAddress { get; set; }
            [JsonProperty(PropertyName = "start_location")]
            public Start_Location StartLocation { get; set; }
            [JsonProperty(PropertyName = "steps")]
            public Step[] Steps { get; set; }
            [JsonProperty(PropertyName = "traffic_speed_entry")]
            public object[] TrafficSpeedEntry { get; set; }
            [JsonProperty(PropertyName = "via_waypoint")]
            public object[] ViaWaypoint { get; set; }
        }

        public class Distance
        {
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "value")]
            public int Value { get; set; }
        }

        public class Duration
        {
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "value")]
            public int Value { get; set; }
        }

        public class End_Location
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Start_Location
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Step
        {
            [JsonProperty(PropertyName = "distance")]
            public Distance1 Distance { get; set; }
            [JsonProperty(PropertyName = "duration")]
            public Duration1 DSuration { get; set; }
            [JsonProperty(PropertyName = "end_location")]
            public End_Location1 EndLocation { get; set; }
            [JsonProperty(PropertyName = "html_instructions")]
            public string HtmlInstructions { get; set; }
            [JsonProperty(PropertyName = "polyline")]
            public Polyline Polyline { get; set; }
            [JsonProperty(PropertyName = "start_location")]
            public Start_Location1 StartLocation { get; set; }
            [JsonProperty(PropertyName = "travel_mode")]
            public string TravelMode { get; set; }
            [JsonProperty(PropertyName = "maneuver")]
            public string Maneuver { get; set; }
        }

        public class Distance1
        {
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "value")]
            public int Value { get; set; }
        }

        public class Duration1
        {
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "value")]
            public int Value { get; set; }
        }

        public class End_Location1
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

        public class Polyline
        {
            [JsonProperty(PropertyName = "points")]
            public string Points { get; set; }
        }

        public class Start_Location1
        {
            [JsonProperty(PropertyName = "lat")]
            public float Latitude { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public float Longitude { get; set; }
        }

    }
}
