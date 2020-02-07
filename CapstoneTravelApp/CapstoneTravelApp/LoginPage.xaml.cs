using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CapstoneTravelApp.DatabaseTables;
using SQLite;
using CapstoneTravelApp.HelperFolders;
using System.Collections.ObjectModel;

namespace CapstoneTravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        UserHelper userData;
        private SQLiteConnection conn;
        User_Table _currentUser;

        public LoginPage(User_Table user)
        {
            InitializeComponent();

            userData = new UserHelper();
            Title = "Login";
            NavigationPage.SetHasBackButton(this, true);
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            _currentUser = user;
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (_currentUser.UserName == userNameEntry.Text)
            {
                if (userNameEntry.Text != null && passwordEntry.Text != null)
                {
                    var validAdmin = userData.AdminValidate(userNameEntry.Text, passwordEntry.Text);
                    var validUser = userData.LoginValidate(userNameEntry.Text, passwordEntry.Text);

                    if ((string)adminPicker.SelectedItem == "Yes")
                    {
                        if (validAdmin)
                        {

                        }
                        else
                        {
                            await DisplayAlert("Warning", "Incorrect Username or Password", "Ok");
                        }
                    }
                    else
                    {
                        if (validUser)
                        {

                            await Navigation.PushAsync(new TripsPage(_currentUser));
                        }
                        else
                        {
                            await DisplayAlert("Warning", "All Incorrect Username or Password", "Ok");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Warning", "All Fields Are Required", "Ok");
                } 
            }
            else
            {
                await DisplayAlert("Warning", "Selected Username does not match input Username", "Ok");
            }
        }

        private void LoginButton_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}