using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Homework_07_WPF_Organizer
{
	/// <summary>
	/// Interaction logic for NewNote.xaml
	/// </summary>
	public partial class NewNote : Window
	{
		public SimpleDateTime selectedDT = new SimpleDateTime(DateTime.Now);
		
		//public string noteLocation { get; set; }
		//public string noteTitle { get; set; }
		//public string noteText { get; set; }
		public NewNote(SimpleDateTime currDate)
		{
			InitializeComponent();
			selectedDT.Year  = currDate.Year;
			selectedDT.Month = currDate.Month;
			selectedDT.Day   = currDate.Day;
			DateTime dt = new DateTime(currDate.Year, currDate.Month, currDate.Day);
			currentDate.Text = $"{dt.ToString("dddd, d MMMM yyyy г.")}";
			  enterTime.Text = $"{selectedDT.Hour:00}:{selectedDT.Min:00}";
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

	}
}
