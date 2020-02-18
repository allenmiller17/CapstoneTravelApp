using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CapstoneTravelApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            Current.Resources = new ResourceDictionary();
            Current.Resources.Add("UlycesColor", Color.FromRgb(121, 248, 81));
            var navigationStyle = new Style(typeof(NavigationPage));
            var barTextColorSetter = new Setter { Property = NavigationPage.BarTextColorProperty, Value = Color.White };
            var barBackgroundColorSetter = new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = "#F25F5C" };

            navigationStyle.Setters.Add(barTextColorSetter);
            navigationStyle.Setters.Add(barBackgroundColorSetter);

            Current.Resources.Add(navigationStyle);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }
}
