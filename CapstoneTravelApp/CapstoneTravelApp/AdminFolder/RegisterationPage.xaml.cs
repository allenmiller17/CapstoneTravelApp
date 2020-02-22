using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace CapstoneTravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterationPage : ContentPage
    {
        UserHelper helper = new UserHelper();
        private SQLiteConnection conn;

        public RegisterationPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            emailEntry.ReturnCommand = new Command(() => userNameEntry.Focus());
            userNameEntry.ReturnCommand = new Command(() => passwordEntry.Focus());
            passwordEntry.ReturnCommand = new Command(() => confirmPasswordEntry.Focus());

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(userNameEntry.Text)) || (string.IsNullOrWhiteSpace(passwordEntry.Text)) ||
                (string.IsNullOrWhiteSpace(firstNameEntry.Text)) || (string.IsNullOrWhiteSpace(lastNameEntry.Text)) ||
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
                conn.CreateTable<User_Table>();
                var users = new User_Table();
                users.UserEmail = emailEntry.Text;
                users.UserName = userNameEntry.Text;
                users.FirstName = firstNameEntry.Text;
                users.LastName = lastNameEntry.Text;
                users.Password = passwordEntry.Text;

                var returnValue = helper.AddUser(userNameEntry.Text);
                if (returnValue)
                {
                    conn.Insert(users);
                    await DisplayAlert("Success!", "New user created", "Ok");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Failure", "That Username already exists.", "OK");

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