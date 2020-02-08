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
	public partial class TripNotesPage : ContentPage
	{
		public TripNotesPage (Trips_Table CurrentTrip)
		{
			InitializeComponent ();
		}
	}
}