using System;
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
	/// Interaction logic for GenerateNotesMenu.xaml
	/// </summary>
	public partial class GenerateNotesMenu : Window
	{
		public class NotesRange
		{
			public short StartYear { get; set; }
			public short EndYear { get; set; }
			public uint NumberOfNotes { get; set; }
		}
		NotesRange notesRange;
		public GenerateNotesMenu()
		{
			InitializeComponent();
			notesRange = new NotesRange();
			this.DataContext = notesRange;
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}
