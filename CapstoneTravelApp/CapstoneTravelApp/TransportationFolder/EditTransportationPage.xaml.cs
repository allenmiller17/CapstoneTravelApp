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
using CapstoneTravelApp.TransportationFolder;
using Xamarin.Essentials;

namespace CapstoneTravelApp.TransportationFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTransportationPage : ContentPage
	{
		private SQLiteConnection conn;
		private Transportation_Table _rental;
		UserHelper userData;


		public EditTransportationPage (Transportation_Table _currentRental)
		{
			InitializeComponent ();
			_rental = _currentRental;
			Title = _rental.RentalName;

			conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

		protected override void OnAppearing()
		{
			conn.CreateTable<Transportation_Table>();
			rentalNameEntry.Text = _rental.RentalName;
			rentalConfEntry.Text = _rental.ConfNumber;
			rentalPhoneEntry.Text = _rental.RentalPhone;
			pickupDatePicker.Date = _rental.PickUpDate;
			pickupTimePicker.Time = _rental.PickUpDate.TimeOfDay;
			pickupLocEntry.Text = _rental.PickUpLocation;
			dropOffDatePicker.Date = _rental.ReturnDate;
			dropOffTimePicker.Time = _rental.ReturnDate.TimeOfDay;
			dropOffLocEntry.Text = _rental.ReturnLocation;
			notificationSwitch.IsToggled = _rental.RentalNotifications == 1 ? true : false;


			base.OnAppearing();
		}

		private async void saveButton_Clicked(object sender, EventArgs e)
		{
			string cInDate = pickupDatePicker.Date.ToString("MM/dd/yyyy");
			string cInTime = DateTime.Today.Add(pickupTimePicker.Time).ToString(pickupTimePicker.Format);
			DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

			string cOutDate = dropOffDatePicker.Date.ToString("MM/dd/yyyy");
			string cOutTime = DateTime.Today.Add(dropOffTimePicker.Time).ToString(dropOffTimePicker.Format);
			DateTime cOutFull = Convert.ToDateTime(cOutDate + " " + cOutTime);

			_rental.RentalName = rentalNameEntry.Text;
			_rental.ConfNumber = rentalConfEntry.Text;
			_rental.RentalPhone = rentalPhoneEntry.Text;
			_rental.PickUpDate = cInDateFull;
			_rental.PickUpLocation = pickupLocEntry.Text;
			_rental.ReturnDate = cOutFull;
			_rental.ReturnLocation = dropOffLocEntry.Text;
			_rental.RentalNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

			if (_rental.PickUpDate <= _rental.ReturnDate)
			{
				if (UserHelper.PhoneCheck(rentalPhoneEntry.Text))
				{
					conn.Update(_rental);
					await DisplayAlert("Notice", "Transportation record updated", "Ok");
					await Navigation.PopModalAsync();
				}
				else
				{
					await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Warning", "Pickup must be earlier than Drop off", "Ok");
			}
		}
	}
}