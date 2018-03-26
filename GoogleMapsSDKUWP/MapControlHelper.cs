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
        public static void UseGoogleMaps(MapControl Map, MapMode MapMode = MapMode.Standard, bool ShowTraffic = false, bool AllowCaching = true, bool AllowOverstretch = false, bool IsFadingEnabled = true)
        {
            Map.TileSources.Clear();
            string mapuri = "";
            if (MapMode == MapMode.Standard && !ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=r&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            if (MapMode == MapMode.Standard && ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=r@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            if (MapMode == MapMode.Satellite && !ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=s&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            if (MapMode == MapMode.Satellite && ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=s@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            if (MapMode == MapMode.Hybrid && !ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=y&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            if (MapMode == MapMode.Hybrid && ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=y@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            if (MapMode == MapMode.RoadsOnly && !ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=h&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            if (MapMode == MapMode.RoadsOnly && ShowTraffic)
                mapuri = "http://mt1.google.com/vt/lyrs=h@221097413,traffic&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            Map.TileSources.Add(new MapTileSource(new HttpMapTileDataSource(mapuri) { AllowCaching = AllowCaching }) { AllowOverstretch = AllowOverstretch, IsFadingEnabled = IsFadingEnabled });
        }
    }
}
