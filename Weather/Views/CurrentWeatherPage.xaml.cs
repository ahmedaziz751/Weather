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

namespace Weather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWeatherPage : ContentPage
    {
        public CurrentWeatherPage()
        {
            InitializeComponent();
            getLocation();


            var metrics = DeviceDisplay.MainDisplayInfo;
            var width = metrics.Width;
            var height = metrics.Height;


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


        private async void getLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var loc = await Geolocation.GetLocationAsync(request);


                if (loc != null) { 
                    latitude = loc.Latitude;
                    longitude = loc.Longitude;
                    location = await GetCity(loc);


                    getWeatherInfo();
                }




            }
            catch (Exception ex)
            {

                await DisplayAlert("Weather Info 20", ex.Message, "OK");

            }
        
        }

        private async Task<String> GetCity(Location loc) {


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

                    currentWeatherStatueTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    currentWeatherIcon.Source = $"w{weatherInfo.weather[0].icon}";
                    locationName.Text = weatherInfo.name.ToUpper();
                    currentDegreeTxt.Text = $"{weatherInfo.main.temp.ToString("0")}°";
                    wetTxt.Text = $"{weatherInfo.main.humidity}%";
                    windTxt.Text = $"{weatherInfo.wind.speed} m/s";

                    var dt = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);

                    getForecast();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info 23", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info24", "No weather information found", "OK");
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

                        if (date > DateTime.Now && date.Hour == 12 && date.Minute == 0 && date.Second == 0) { 
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


                }
                catch (Exception ex)
                {
                Debug.WriteLine(ex.ToString());
                await DisplayAlert("Weather Info25", ex.ToString(), "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info26", "No forecast information found", "OK");
            }
        }

        private async void handleAddButton(object sender, EventArgs e)
        {
            await add.TranslateTo(Width / 2-26.25, Height / 2 - 26.25, 250, null);
            await add.RelScaleTo(3,250,null);
            await add.RelRotateTo(1710, 1500,Easing.CubicOut);
            await PopupNavigation.Instance.PushAsync(new AddLocation());
            await add.RelScaleTo(-3, 250, null);
            await add.TranslateTo(5, 5, 250, null);
            
            

        }
    }
}