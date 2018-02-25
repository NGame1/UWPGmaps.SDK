# UWPGmaps.SDK

Unofficial Google Map SDK for Universal Windows Platform (UWP) 

# What is supported 
Finding routes and directions

Geocoding and reverse-Geocoding

Photo helper (Convert Photo references to URI)

Place Helper (Add / Delete / TextSearch / Nearby Search / Place Auto Complete)

Tile API Helper (Premium APIKey needed)

# Related Links

[SDK NuGet package](https://www.nuget.org/packages/GMapsUWPSDK)

# How to use
This class library and also NuGet package (V1.5.0 and higher) are really easy to use . 

Let's find a place address using it's Latitude and Longitude :

`var Address = await GMapsUWP.GeoCoding.GeocodeHelper.GetAddress(GEOPOINT_OF_A_LOCATION);`

OK Now you have Address of the mentioned place.

Let's get Directions between two points and show it on map : 

`var Directions = await GMapsUWP.Directions.DirectionsHelper.GetDirections(origin_BasicGeoposition, Destination_BasicGeoposition);
var DirectionsPolyline = GMapsUWP.Directions.DirectionsHelper.GetDirectionAsRoute(Directions, Color_Of_Result_PolyLine);
Map.MapElements.Add(DirectionsPolyline);`

In this code Map is an instance of MapControl. 

## Be sure using Google Maps API can't be easier :) 
