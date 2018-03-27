using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Maps;

namespace GMapsUWP.Map
{
    public class MapControlHelper
    {
        public enum MapMode
        {
            Standard,
            RoadsOnly,
            Satellite,
            Hybrid
        }
        /// <summary>
        /// This function allow you to use google maps easily on map control
        /// </summary>
        /// <param name="Map">MapControl of your app</param>
        /// <param name="MapMode">Mode of map you want to show</param>
        /// <param name="ShowTraffic">Show traffic on map or no</param>
        /// <param name="AllowCaching">Allow cache tiles (true is better for sure)</param>
        /// <param name="AllowOverstretch">Zoom in on fetched images to get new ones for higher zoom level</param>
        /// <param name="IsFadingEnabled">Fade Animation for showig new tiles.</param>
        public static void UseGoogleMaps(MapControl Map, MapMode MapMode = MapMode.Standard, bool ShowTraffic = false, bool AllowCaching = true, bool AllowOverstretch = false, bool IsFadingEnabled = true)
        {
            if (Map == null) return;
            Map.Style = MapStyle.None;
            Map.TileSources.Clear();
            string mapuri = "";
            switch (MapMode)
            {
                case MapMode.Standard:
                    if (!ShowTraffic)
                        mapuri = "http://mt1.google.com/vt/lyrs=r&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    else
                        mapuri = "http://mt1.google.com/vt/lyrs=r@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    break;
                case MapMode.RoadsOnly:
                    if (!ShowTraffic)
                        mapuri = "http://mt1.google.com/vt/lyrs=h&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    else
                        mapuri = "http://mt1.google.com/vt/lyrs=h@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    break;
                case MapMode.Satellite:
                    if (!ShowTraffic)
                        mapuri = "http://mt1.google.com/vt/lyrs=s&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    else
                        mapuri = "http://mt1.google.com/vt/lyrs=s@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    break;
                case MapMode.Hybrid:
                    if (!ShowTraffic)
                        mapuri = "http://mt1.google.com/vt/lyrs=y&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    else
                        mapuri = "http://mt1.google.com/vt/lyrs=y@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
                    break;
                default:
                    break;
            }
            Map.TileSources.Add(new MapTileSource(new HttpMapTileDataSource(mapuri)
                        { AllowCaching = AllowCaching })
                            { AllowOverstretch = AllowOverstretch, IsFadingEnabled = IsFadingEnabled });
        }
    }
}
