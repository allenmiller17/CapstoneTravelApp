using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.EntertainmentFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditEntertainmentPage : ContentPage
	{
		private SQLiteConnection conn;
		private Entertainment_Table currentActivity;

		public EditEntertainmentPage(Entertainment_Table _currentActivity)
		{
			InitializeComponent();

			currentActivity = _currentActivity;

			Title = currentActivity.EntertainName;

			conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

		protected override void OnAppearing()
		{
			entertainNameEntry.Text = currentActivity.EntertainName;
			entertainLocEntry.Text = currentActivity.EnterainAddress;
			entertainPhoneEntry.Text = currentActivity.EntertainPhone;
			startDatePicker.Date = currentActivity.EntertaninStart;
			startTimePicker.Time = currentActivity.EntertaninStart.TimeOfDay;
			endDatePicker.Date = currentActivity.EntertainEnd;
			endTimePicker.Time = currentActivity.EntertainEnd.TimeOfDay;
			notificationSwitch.IsToggled = currentActivity.EntertainNotifications == 1 ? true : false;


			base.OnAppearing();
		}

		private async void SaveButton_Clicked(object sender, EventArgs e)
		{
			string cInDate = startDatePicker.Date.ToString("MM/dd/yyyy");
			string cInTime = DateTime.Today.Add(startTimePicker.Time).ToString(startTimePicker.Format);
			DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

			string cOutDate = endDatePicker.Date.ToString("MM/dd/yyyy");
			string cOutTime = DateTime.Today.Add(endTimePicker.Time).ToString(endTimePicker.Format);
			DateTime cOutFull = Convert.ToDateTime(cOutDate + " " + cOutTime);

			currentActivity.EntertainName = entertainNameEntry.Text;
			currentActivity.EnterainAddress = entertainLocEntry.Text;
			currentActivity.EntertainPhone = entertainPhoneEntry.Text;
			currentActivity.EntertaninStart = cInDateFull;
			currentActivity.EntertainEnd = cOutFull;
			currentActivity.EntertainNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

			if (currentActivity.EntertaninStart <= currentActivity.EntertainEnd)
			{
				if (UserHelper.PhoneCheck(entertainPhoneEntry.Text))
				{
					conn.Update(currentActivity);
					await DisplayAlert("Notice", "Entertainment record updated", "Ok");
					await Navigation.PopModalAsync();
				}
				else
				{
					await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Warning", "Check-in must be earlier than check-out", "Ok");
			}
		}
	}
}