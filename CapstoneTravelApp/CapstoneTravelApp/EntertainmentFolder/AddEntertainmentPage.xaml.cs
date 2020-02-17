using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.EntertainmentFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddEntertainmentPage : ContentPage
	{
		public AddEntertainmentPage (DatabaseTables.Trips_Table _currentTrip)
		{
			InitializeComponent ();
		}
	}
}