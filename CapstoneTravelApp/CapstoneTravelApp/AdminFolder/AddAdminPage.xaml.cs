using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using CapstoneTravelApp.TripsFolder;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CapstoneTravelApp.AdminFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAdminPage : ContentPage
    {
        UserHelper helper = new UserHelper();
        private SQLiteConnection conn;

        public AddAdminPage()
        {
            InitializeComponent();
            emailEntry.ReturnCommand = new Command(() => userNameEntry.Focus());
            userNameEntry.ReturnCommand = new Command(() => passwordEntry.Focus());
            passwordEntry.ReturnCommand = new Command(() => confirmPasswordEntry.Focus());

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(userNameEntry.Text)) || (string.IsNullOrWhiteSpace(passwordEntry.Text)) ||
    (string.IsNullOrWhiteSpace(emailEntry.Text)))
            {
                await DisplayAlert("Warning", "All fields are required", "Ok");
            }
            else if (!string.Equals(passwordEntry.Text, confirmPasswordEntry.Text))
            {
                await DisplayAlert("Warning", "Passwords don't match", "Ok");
                passwordEntry.Text = string.Empty;
                confirmPasswordEntry.Text = string.Empty;
            }
            else
            {
                var admin = new Admin_Table();
                conn.CreateTable<Admin_Table>();
                admin.AdminEmail = emailEntry.Text;
                admin.AdminUserName = userNameEntry.Text;
                admin.AdminPassword = passwordEntry.Text;

                var returnValue = helper.AddAdmin(userNameEntry.Text);
                if (returnValue)
                {
                    conn.Insert(admin);
                    await DisplayAlert("Success!", "New admin created", "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Failure", "That Admin Username already exists", "OK");

                    warningLabel.IsVisible = false;

                    emailEntry.Text = string.Empty;

                    userNameEntry.Text = string.Empty;

                    passwordEntry.Text = string.Empty;

                    confirmPasswordEntry.Text = string.Empty;

                }
            }
        }
    }
}