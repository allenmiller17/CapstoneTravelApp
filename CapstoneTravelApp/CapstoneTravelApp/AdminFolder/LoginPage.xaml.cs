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
        private ObservableCollection<User_Table> UserList;

        public LoginPage()
        {
            InitializeComponent();

            userData = new UserHelper();
            Title = "Login";
            NavigationPage.SetHasBackButton(this, true);
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            conn.CreateTable<User_Table>();

            string _userName = userNameEntry.Text;

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

                        await Navigation.PushAsync(new TripsPage(_userName));
                    }
                    else
                    {
                        await DisplayAlert("Warning", "Incorrect Username or Password", "Ok");
                    }
                }
            }
            else
            {
                await DisplayAlert("Warning", "All Fields Are Required", "Ok");
            }
        }

        private void LoginButton_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}