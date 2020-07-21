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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Homework_07_WPF_Organizer
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Собственно ежедневник
		OrganizerClass myOrganizer = new OrganizerClass();

		// Видимая на экране дата ежедневника
		public SimpleDateTime currDate;
		
		public MainWindow()
		{
			InitializeComponent();

			// Специально выделяем одну переменную, в которую записываем текущую дату
			// Делаем на случай, если программа будет работать в 00:00,
			// чтобы текущая дата (currDate) и дата на экране были одинаковыми 
			DateTime tmpDT = DateTime.Now;
			currDate = new SimpleDateTime(tmpDT);

			// Выводим дату на экран
			currentDate.Text = $"{tmpDT.ToString("dddd, d MMMM yyyy г.")}";
		}

		/// <summary>
		/// Выбирает текущую дату в ежедневнике
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CalendarSelectedDate(object sender, SelectionChangedEventArgs e)
		{
			DateTime? dt = MyCalendar.SelectedDate;
			if (dt == null) return;
			currDate.Update((DateTime)dt);
			currentDate.Text = $"{((DateTime)dt).ToString("dddd, d MMMM yyyy г.")}";
			dayToShowList.ItemsSource = myOrganizer.GetDayList(currDate);
		}

		/// <summary>
		/// Генерирует случайные записи в ежедневнике
		/// (пока не реализовано) Задаём диапазон лет и количество записей для генерации
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_GenerateNotes(object sender, RoutedEventArgs e)
		{
			GenerateNotesMenu getYearsWindow = new GenerateNotesMenu();
			getYearsWindow.ShowDialog();

			if (getYearsWindow.DialogResult == null ||
				getYearsWindow.DialogResult == false) 
				return;

			short startYear = Int16.Parse(getYearsWindow.enterStartYear.Text);
			short endYear   = Int16.Parse(getYearsWindow.enterEndYear.Text);
			uint numberOfNotes = UInt32.Parse(getYearsWindow.enterNumberOfNotes.Text);
			
			if(startYear > endYear)
			{
				short tmp = startYear;
				startYear = endYear;
				endYear   = tmp;
			}

			Random r = new Random();
			short rYear;             // 5 лет
			sbyte rMonth;            // 1 - 12
			sbyte rDay;              // 1 - 31
			sbyte rHour;
			sbyte rMin;
			sbyte rSec;
			string rLocation;
			string rTitle;
			string rText;
			SimpleDateTime rDT;

			for (int i = 0; i < numberOfNotes; i++)
			{
				rYear  = (short)r.Next(startYear, endYear);
				rMonth = (sbyte)r.Next(1, 13);
				if (DateTime.IsLeapYear(rYear) & rMonth == 2) 
					 rDay = (sbyte)r.Next(1, 30); // високосный февраль
				else rDay = (sbyte)r.Next(1, YearClass.monthsLength[rMonth] + 1);
				rHour = (sbyte)r.Next(1, 24);
				rMin  = (sbyte)r.Next(1, 60);
				rSec  = (sbyte)r.Next(1, 60);
				rLocation = $"Location_{rMin}";
				rTitle = $"Title_{rYear}{rMonth}{rDay}_{rHour}:{rMin}:{rSec}";
				rText = $"This is the day {rDay} of month {rMonth} of the year {rYear}." +
						$" Time now is {rHour} hours {rMin} minutes {rSec} seconds.";

				rDT = new SimpleDateTime(rYear, rMonth, rDay, rHour, rMin);
				myOrganizer.AddNote(rDT, rLocation, rTitle, rText);
			}
			currDate = new SimpleDateTime(DateTime.Now);
			dayToShowList.ItemsSource = myOrganizer.GetDayList(currDate);
			
			dayToShowList.Items.Refresh();

		}

		/// <summary>
		/// Открывает диалоговое окно для ввода новой записи в текущую, 
		/// открытую для показа дату ежедневника.
		/// По умолчанию время записи - текущее
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_NewNote(object sender, RoutedEventArgs e)
		{
			NewNote newNoteWindow = new NewNote(currDate);
			newNoteWindow.ShowDialog();
			if(newNoteWindow.DialogResult == true)
			{
				if (!myOrganizer.AddNote(newNoteWindow.selectedDT,
										 newNoteWindow.enterLocation.Text,
										 newNoteWindow.enterTitle.Text,
										 newNoteWindow.enterText.Text))
				{
					MessageBox.Show("Идентичная запись уже существует!");
				}
			}
			dayToShowList.ItemsSource = myOrganizer.GetDayList(currDate);
			dayToShowList.Items.Refresh();
		}

		/// <summary>
		/// Перемещает выбранную запись из ежедневника в корзину. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_DeleteNote(object sender, RoutedEventArgs e)
		{
			if (dayToShowList.SelectedItem == null) return;
			myOrganizer.RemoveNote((Note)dayToShowList.SelectedItem);
			dayToShowList.Items.Refresh();
		}

		private void Click_SearchNote(object sender, RoutedEventArgs e)
		{

		}

		private void Click_UploadFromXLM(object sender, RoutedEventArgs e)
		{

		}

		private void Click_SaveToXLM(object sender, RoutedEventArgs e)
		{

		}

		/// <summary>
		/// Открывает окно корзины
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_RecycleBin(object sender, RoutedEventArgs e)
		{
			RecycleBin recycleBin = new RecycleBin(myOrganizer);
			recycleBin.ShowDialog();
			dayToShowList.Items.Refresh();
		}

		/// <summary>
		/// Выход из приложения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_ExitButton(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}

	}

}
