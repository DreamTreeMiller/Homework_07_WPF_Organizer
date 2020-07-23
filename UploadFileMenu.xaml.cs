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
	/// Interaction logic for UploadFileMenu.xaml
	/// </summary>
	public partial class UploadFileMenu : Window
	{
		public bool?  result;
		public string filepathname;
		public bool   replaceNotes = true;

		public UploadFileMenu()
		{
			InitializeComponent();
		}

		private void Click_UploadFromXLM(object sender, RoutedEventArgs e)
		{
			// Configure open file dialog box
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.FileName = "Organizer"; // Default file name
			dlg.DefaultExt = ".xml"; // Default file extension
			dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

			// Show open file dialog box
			result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result != true) return;

			// Open document
			filepathname = dlg.FileName;
			filename.Text = dlg.SafeFileName; 
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void replaceWithNewFile_Checked(object sender, RoutedEventArgs e)
		{
			replaceNotes = true;
		}

		private void addNotes_Checked(object sender, RoutedEventArgs e)
		{
			replaceNotes = false;
		}
	}
}
