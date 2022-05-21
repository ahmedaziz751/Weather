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
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            Application.Current.Properties["Name"] = location.Text;
            ChangeLocation.newLocation = location.Text;
            PopupPage page = this; 
            PopupNavigation.Instance.RemovePageAsync(page);
            Navigation.PushAsync(new CurrentWeatherPage());

        }
    }
}