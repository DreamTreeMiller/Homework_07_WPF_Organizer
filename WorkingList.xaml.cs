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
	/// Interaction logic for WorkingList.xaml
	/// </summary>
	public partial class WorkingList : Window
	{
		List<Note> listToShow;
		Note tmpNote = new Note();
		public WorkingList(List<Note> listToShow)
		{
			InitializeComponent();
			this.listToShow = listToShow;
			workingListView.ItemsSource = this.listToShow;
		}

		private void sortbydate_Click(object sender, RoutedEventArgs e)
		{

			listToShow.Sort(tmpNote.CompareByDate);
			workingListView.ItemsSource = listToShow;
			workingListView.Items.Refresh();
		}

		private void sortbytime_Click(object sender, RoutedEventArgs e)
		{
			listToShow.Sort(tmpNote.CompareByTime);
			workingListView.ItemsSource = listToShow;
			workingListView.Items.Refresh();
		}

		private void sortbylocation_Click(object sender, RoutedEventArgs e)
		{
			listToShow.Sort(tmpNote.CompareByLocation);
			workingListView.ItemsSource = listToShow;
			workingListView.Items.Refresh();
		}

		private void sortbytitle_Click(object sender, RoutedEventArgs e)
		{
			listToShow.Sort(tmpNote.CompareByTitle);
			workingListView.ItemsSource = listToShow;
			workingListView.Items.Refresh();
		}

		private void sortbytext_Click(object sender, RoutedEventArgs e)
		{
			listToShow.Sort(tmpNote.CompareByText);
			workingListView.ItemsSource = listToShow;
			workingListView.Items.Refresh();
		}
	}
}
