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
	/// Interaction logic for RecycleBin.xaml
	/// </summary>
	public partial class RecycleBin : Window
	{
		// Сюда будет передаваться ссылка на ежедневник
		OrganizerClass workOrg;
		public RecycleBin(object organizer)
		{
			InitializeComponent();
			this.workOrg = organizer as OrganizerClass;
			rbListView.ItemsSource = workOrg.RecycleBin;
		}

		/// <summary>
		/// Перемещает выбранную запись из корзины в ежедневник
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_RestoreNote(object sender, RoutedEventArgs e)
		{
			workOrg.RestoreNoteFromBin((Note)rbListView.SelectedItem);
			rbListView.Items.Refresh();
		}

		/// <summary>
		/// Навсегда удаляет из корзины выбранную запись 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteForever_Click(object sender, RoutedEventArgs e)
		{
			workOrg.DeleteForeverFromBin((Note)rbListView.SelectedItem);
			rbListView.Items.Refresh();
		}

		/// <summary>
		/// Очищает корзину от всех записей
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EmptyBin_Click(object sender, RoutedEventArgs e)
		{
			workOrg.EmptyBin();
			rbListView.Items.Refresh();
		}

		/// <summary>
		/// Закрывает окно корзины
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_ExitButton(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
