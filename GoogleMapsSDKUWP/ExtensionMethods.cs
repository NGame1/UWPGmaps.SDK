using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Devices.Geolocation;

namespace GMapsUWP
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// This method can be used for convert HTML Instructions of a direction to simple string
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns>string with no HTML sign</returns>
        public static string NoHTMLString(this string HTML)
        {
            return Regex.Replace(HTML, @"<[^>]+>|&nbsp;", "").Trim();
        }
        public enum DistanceUnit
        {
            Kilometers,
            NauticalMiles,
            Miles
        }

        /// <summary>
        /// This method help you in getting Direct Distance between two geopoints.
        /// </summary>
        /// <param name="Origin">First point</param>
        /// <param name="Destination">Second point</param>
        /// <param name="unit">Value will be returned in this distance unit</param>
        /// <returns>Direct Distance between 2 points</returns>
        public static double DistanceTo(this Geopoint Origin, Geopoint Destination, DistanceUnit unit = DistanceUnit.Kilometers)
        {
            var lat1 = Origin.Position.Latitude;
            var lon1 = Origin.Position.Longitude;
            var lat2 = Destination.Position.Latitude;
            var lon2 = Destination.Position.Longitude;
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            switch (unit)
            {
                case DistanceUnit.Kilometers: //Kilometers -> default
                    return dist * 1.609344;
                case DistanceUnit.NauticalMiles: //Nautical Miles 
                    return dist * 0.8684;
                case DistanceUnit.Miles: //Miles
                    return dist;
            }
            return dist;
        }
    }


}