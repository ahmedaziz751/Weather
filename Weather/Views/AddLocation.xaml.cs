using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Weather.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AddLocation : PopupPage
    {
        public AddLocation()
        {
            InitializeComponent();

            int rnd = LocationSuggestions.RandomInt();
            if (rnd < LocationSuggestions.locationSuggestion.Length / 2)
            {
                sug1.Text = LocationSuggestions.locationSuggestion[rnd];
                sug2.Text = LocationSuggestions.locationSuggestion[rnd + 1];
                sug3.Text = LocationSuggestions.locationSuggestion[rnd + 2];
                sug4.Text = LocationSuggestions.locationSuggestion[rnd + 3];
                sug5.Text = LocationSuggestions.locationSuggestion[rnd + 4];
                sug6.Text = LocationSuggestions.locationSuggestion[rnd + 5];
            }

            else if (rnd > LocationSuggestions.locationSuggestion.Length / 2)
            {
                sug1.Text = LocationSuggestions.locationSuggestion[rnd];
                sug2.Text = LocationSuggestions.locationSuggestion[rnd - 1];
                sug3.Text = LocationSuggestions.locationSuggestion[rnd - 2];
                sug4.Text = LocationSuggestions.locationSuggestion[rnd - 3];
                sug5.Text = LocationSuggestions.locationSuggestion[rnd - 4];
                sug6.Text = LocationSuggestions.locationSuggestion[rnd - 5];
            }
        }


        private void Button_click(object sender, EventArgs e)
        {

            Application.Current.Properties["Name"] = location.Text;
            ChangeLocation.newLocation = location.Text;
            PopupPage page = this;
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());

        }

        private void sug1_btnClick(object sender, EventArgs e)
        {

            //Application.Current.Properties["Name"] = "Batman";
            ChangeLocation.newLocation = sug1.Text;
            PopupPage page = this;
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());
        }

        private void sug2_btnClick(object sender, EventArgs e)
        {
            ChangeLocation.newLocation = sug2.Text;
            PopupPage page = this;
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());
        }

        private void sug3_btnClick(object sender, EventArgs e)
        {
            ChangeLocation.newLocation = sug3.Text;
            PopupPage page = this;
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());
        }

        private void sug4_btnClick(object sender, EventArgs e)
        {

            ChangeLocation.newLocation = sug4.Text;
            PopupPage page = this;
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());
        }

        private void sug5_btnClick(object sender, EventArgs e)
        {
            ChangeLocation.newLocation = sug5.Text;
            PopupPage page = this;
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());
        }

        private void sug6_btnClick(object sender, EventArgs e)
        {
            ChangeLocation.newLocation = sug6.Text;
            PopupPage page = this;
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());
        }

    }
}