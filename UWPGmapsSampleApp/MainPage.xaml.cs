using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPGmapsSampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Geopoint origin = null;
        Geopoint Destination = null;
        public enum SampleMode
        {
            GeoCodeAddress,
            GeoCodeInfo,
            Directions,
            ReverseGeocoding,
            OfflineMapDL
        }
        GMapsUWP.OfflineMapsDownloader.OfflineMapDownloader OfflineDL;
        SampleMode CurrentSampleMode { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            GMapsUWP.Map.MapControlHelper.UseGoogleMaps(Map);
            //Map.TileSources.Clear();
            //string mapuri = "http://mt1.google.com/vt/lyrs=r&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            //Map.TileSources.Add(new MapTileSource(new HttpMapTileDataSource(mapuri)));
            GMapsUWP.Initializer.Initialize("Your_API_KEY_HERE", "en-US");
            OfflineDL = GMapsUWP.OfflineMapsDownloader.OfflineMapDownloader.GetInstance();
            OfflineDL.DownloadCompleted += OfflineDL_DownloadCompleted;
        }

        private async void OfflineDL_DownloadCompleted(object sender, bool e)
        {
            var f = OfflineDL.GetMapDownloadFolder();
            await Launcher.LaunchFolderAsync(f);
        }

        private async void MapControl_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            switch (CurrentSampleMode)
            {
                case SampleMode.GeoCodeAddress:
                    var GeoCodeResult = await GMapsUWP.GeoCoding.GeocodeHelper.GetAddress(args.Location);
                    await new MessageDialog(GeoCodeResult).ShowAsync();
                    break;
                case SampleMode.GeoCodeInfo:
                    var GeoCodeInfoResult1 = await GMapsUWP.Place.PlaceSearchHelper.NearbySearch(args.Location.Position, 10);
                    if (GeoCodeInfoResult1.Results.Any())
                    {
                        var GeoCodeInfoResult2 = await GMapsUWP.GeoCoding.GeocodeHelper.GetInfo(GeoCodeInfoResult1.Results.FirstOrDefault().PlaceId);
                        if (GeoCodeInfoResult2.Results.Any())
                        {
                            var GeoCodeInfoResult3 = GeoCodeInfoResult2.Results.FirstOrDefault();
                            await new MessageDialog($"{GeoCodeInfoResult3.FormattedAddress}\n{GeoCodeInfoResult3.Geometry.LocationType}\n{GeoCodeInfoResult3.types.FirstOrDefault()}").ShowAsync();
                        }
                    }
                    break;
                case SampleMode.Directions:
                    if (origin == null)
                    {
                        origin = args.Location;
                        return;
                    }
                    else if (Destination == null)
                    {
                        Destination = args.Location;
                        var DirectionsResult1 = await GMapsUWP.Directions.DirectionsHelper.GetDirections(origin.Position, Destination.Position);
                        var DirectionsResult1Polyline = GMapsUWP.Directions.DirectionsHelper.GetDirectionAsRoute(DirectionsResult1, Colors.SkyBlue);
                        Map.MapElements.Add(DirectionsResult1Polyline);
                        return;
                    }
                    break;
                case SampleMode.ReverseGeocoding:
                    var ReverseGeocoding1 = await GMapsUWP.GeoCoding.GeocodeHelper.GetAddress(args.Location);
                    var ReverseGeocoding2 = await GMapsUWP.GeoCoding.ReverseGeoCode.GetLocation(ReverseGeocoding1);
                    await new MessageDialog($"Latitude : {ReverseGeocoding2.Position.Latitude}\nLongitude : {ReverseGeocoding2.Position.Longitude}").ShowAsync();
                    break;
                case SampleMode.OfflineMapDL:
                    if (origin == null)
                    {
                        origin = args.Location;
                        return;
                    }
                    else if (Destination == null)
                    {
                        Destination = args.Location;
                        //Here I used Max Zoom Level to 2 to download complete faster. I prefer to use default value (17) or at least 15.
                        OfflineDL.DownloadMap(origin.Position.Latitude, origin.Position.Longitude, Destination.Position.Latitude, Destination.Position.Longitude, 2);
                        return;
                    }
                    break;
                default:
                    break;
            }
        }

        private async void GeoCodeAddress_Click(object sender, RoutedEventArgs e)
        {
            CurrentSampleMode = SampleMode.GeoCodeAddress;
            await new MessageDialog("Now click somewhere on map on a place, Road or anything else").ShowAsync();

        }

        private async void GeoCodeInfo_Click(object sender, RoutedEventArgs e)
        {
            CurrentSampleMode = SampleMode.GeoCodeInfo;
            await new MessageDialog("Now click somewhere on map on a place").ShowAsync();
        }

        private async void GetDirections_Click(object sender, RoutedEventArgs e)
        {
            CurrentSampleMode = SampleMode.Directions;
            origin = null;
            Destination = null;
            await new MessageDialog("Now click 2 places on map").ShowAsync();
        }

        private async void ReverseGeocoding_Click(object sender, RoutedEventArgs e)
        {
            CurrentSampleMode = SampleMode.ReverseGeocoding;
            await new MessageDialog("Now click somewhere on map on a place, Road or anything else").ShowAsync();
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtbox = TextSearch;
            if (txtbox.Text.Length > 3)
            {
                var res = await GMapsUWP.Place.PlaceSearchHelper.TextSearch(txtbox.Text);
                if (res.Results.Any())
                {
                    var myres = res.Results.FirstOrDefault();
                    await new MessageDialog($"{myres.Icon}\n{myres.Name}\n{myres.PlaceId}\n{myres.Types.FirstOrDefault()}").ShowAsync();
                }
            }
        }

        private async void OfflineMapDL_Click(object sender, RoutedEventArgs e)
        {
            CurrentSampleMode = SampleMode.OfflineMapDL;
            origin = null;
            Destination = null;
            await new MessageDialog("Now click 2 places on map. First at top left, Second at bottom right.").ShowAsync();
        }
    }
}
