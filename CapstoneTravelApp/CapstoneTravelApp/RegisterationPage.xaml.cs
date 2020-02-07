using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CapstoneTravelApp.HelperFolders;
using CapstoneTravelApp.DatabaseTables;
using System.Diagnostics;

namespace CapstoneTravelApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterationPage : ContentPage
	{
        User_Table users = new User_Table();
        UserHelper helper = new UserHelper();

		public RegisterationPage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasBackButton(this, false);
            emailEntry.ReturnCommand = new Command(() => userNameEntry.Focus());
            userNameEntry.ReturnCommand = new Command(() => passwordEntry.Focus());
            passwordEntry.ReturnCommand = new Command(() => confirmPasswordEntry.Focus());
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(userNameEntry.Text)) || (string.IsNullOrWhiteSpace(passwordEntry.Text)) ||
                (string.IsNullOrWhiteSpace(firstNameEntry.Text)) || (string.IsNullOrWhiteSpace(lastNameEntry.Text))||
                (string.IsNullOrWhiteSpace(emailEntry.Text)))
            {
                await DisplayAlert("Warning","All fields are required", "Ok");
            }
            else if (!string.Equals(passwordEntry.Text, confirmPasswordEntry.Text))
            {
                await DisplayAlert("Warning", "Passwords don't match", "Ok");
                passwordEntry.Text = string.Empty;
                confirmPasswordEntry.Text = string.Empty;
            }
            else
            {
                users.UserEmail = emailEntry.Text;
                users.UserName = userNameEntry.Text;
                users.FirstName = firstNameEntry.Text;
                users.LastName = lastNameEntry.Text;
                users.Password = passwordEntry.Text;

                //try
                //{
                //    var returnValue = helper.AddUser(users);
                //    if (returnValue == "User Added")
                //    {
                //        await DisplayAlert("Success!", "New user created", "Ok");
                //        await Navigation.PushAsync(new LoginPage());
                //    }
                //    else
                //    {
                //        await DisplayAlert("Failure", "User not created", "OK");

                //        warningLabel.IsVisible = false;

                //        emailEntry.Text = string.Empty;

                //        userNameEntry.Text = string.Empty;

                //        passwordEntry.Text = string.Empty;

                //        confirmPasswordEntry.Text = string.Empty;
                //    }
                //}
                //catch (Exception ex)
                //{

                //    Debug.WriteLine(ex);
                //}
            }
        }

        //private async void LoginButton_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new LoginPage());
        //}
    }
}