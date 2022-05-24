using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Helper;
using Weather.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using Weather.Views;
using System.IO;

namespace Weather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWeatherPage : ContentPage
    {

        public CurrentWeatherPage()
        {
            InitializeComponent();
            getLocation();
            LocationSuggestions.RandomInt();
            background.Source = $"{date()}.png";
        }



        private string date()
        {
            string cases = null;
            if (int.Parse(DateTime.Now.Hour.ToString()) >= 5 && int.Parse(DateTime.Now.Hour.ToString()) <= 13)
            {
                cases = "morning";
            }

            else if (int.Parse(DateTime.Now.Hour.ToString()) > 13 && int.Parse(DateTime.Now.Hour.ToString()) <= 21)
            {
                cases = "cloudy";
            }

            else
                cases = "night";
            return cases;

        }


        private String location { get; set; } = "Istanbul";

        public double latitude { get; set; }
        public double longitude { get; set; }

        int locId { get; set; }


        private async void getLocation()
        {
            try
            {

                if (ChangeLocation.newLocation != null)
                {
                    location = ChangeLocation.newLocation;

                    getWeatherInfo();
                }
                else
                {
                    var connection = Connectivity.NetworkAccess;

                    if (connection == NetworkAccess.Internet)
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Best);
                        var loc = await Geolocation.GetLocationAsync(request);

                        if (loc != null)
                        {
                            latitude = loc.Latitude;
                            longitude = loc.Longitude;
                            location = await GetCity(loc);
                            getWeatherInfo();
                        }
                    }
                    else
                    {

                        await DisplayAlert("Exception 000035", "No Internet Conntection!", "K");

                    }
                }

            }
            catch (FeatureNotEnabledException ex)
            {

                await DisplayAlert("Exception 000020", ex.Message, "K");
                location = "istanbul";
                getWeatherInfo();

            }

        }

        private async Task<String> GetCity(Location loc)
        {


            var city = await Geocoding.GetPlacemarksAsync(loc);
            var currentCity = city?.FirstOrDefault();

            if (currentCity != null)
            {
                return $"{currentCity.AdminArea},{currentCity.CountryName}";
            }

            return null;


        }

        private async void getWeatherInfo()
        {

            String key = "989ba5eaf6e2bc9ff91c51fa8102f5e0";
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&units=metric&appid={key}";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {

                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);

                    currentWeatherStatueTxt.Text = weatherInfo.weather[0].description;
                    currentWeatherIcon.Source = $"w{weatherInfo.weather[0].icon}";
                    locationName.Text = weatherInfo.name.ToUpper() + "," + weatherInfo.sys.country.ToUpper();
                    currentDegreeTxt.Text = $"{weatherInfo.main.temp.ToString("0")}°";
                    wetTxt.Text = $"{weatherInfo.main.humidity}%";
                    windTxt.Text = $"{weatherInfo.wind.speed} m/s";

                    var dt = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);
                    locId = weatherInfo.id;

                    getForecast();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Exception 000023", ex.Message, "K");
                }
            }
            else
            {
                await DisplayAlert("Exception 000024", "Error! no infromation", "K");
                location = "istanbul";
                getWeatherInfo();
            }
        }

        private async void getForecast()
        {

            String key = "989ba5eaf6e2bc9ff91c51fa8102f5e0";
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={location}&units=metric&appid={key}";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    List<aList> allList = new List<aList>();

                    foreach (var list in forcastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 12 && date.Minute == 0 && date.Second == 0)
                        {
                            allList.Add(list);
                        }


                    }

                    day1Txt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd");
                    day1icon.Source = $"w{allList[0].weather[0].icon}";
                    day1DegTxt.Text = $"{allList[0].main.temp.ToString("0")}°";

                    day2Txt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd");
                    day2icon.Source = $"w{allList[1].weather[0].icon}";
                    day2DegTxt.Text = $"{allList[1].main.temp.ToString("0")}°";

                    day3Txt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd");
                    day3icon.Source = $"w{allList[2].weather[0].icon}";
                    day3DegTxt.Text = $"{allList[2].main.temp.ToString("0")}°";

                    day4Txt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd");
                    day4icon.Source = $"w{allList[3].weather[0].icon}";
                    day4DegTxt.Text = $"{allList[3].main.temp.ToString("0")}°";

                    loading.IsVisible = false;
                    loadBack.IsVisible = false;


                }
                catch (Exception ex)
                {
                    await DisplayAlert("Exeption 000025", ex.ToString(), "K");
                }
            }
            else
            {
                await DisplayAlert("Exception 000026", "Error! no infromation", "K");
            }
        }

        private async void handleAddButton(object sender, EventArgs e)
        {
            add.IsEnabled = false;
            await add.TranslateTo(Width / 2 - 26.25, Height / 2 - 26.25, 250, null);
            await add.RelScaleTo(3, 250, null);
            await add.RelRotateTo(1710, 1500, Easing.CubicOut);
            await PopupNavigation.Instance.PushAsync(new AddLocation());
            await add.RelScaleTo(-3, 250, null);
            await add.TranslateTo(5, 5, 250, null);
            add.IsEnabled = true;
        }


        private async void redirect(object sender, EventArgs e)
        {
            await openWeb();
        }

        private async Task openWeb()
        {

            try
            {
                await Browser.OpenAsync("https://openweathermap.org/city/" + locId, BrowserLaunchMode.SystemPreferred);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Exeption 000042", ex.ToString(), "K");
            }
        }


    }
}