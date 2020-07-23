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
		public NewNote(ref Note newNote)
		{
			InitializeComponent();
			// Преобразовываем текущую дату (которая на экране) в строку
			DateTime dt = new DateTime(newNote.Date.Year, newNote.Date.Month, newNote.Date.Day);
			// И выводим в окно для ввода записи
			currentDate.Text = $"{dt:dddd, d MMMM yyyy г.}";

			this.DataContext = newNote;
			// Выводим в поле для времени текущее время
			//enterTime.Text = $"{newNote.Time.Hour:00}:{newNote.Time.Minute:00}";
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

	}
}
