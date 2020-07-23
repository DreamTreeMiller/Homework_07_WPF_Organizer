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
using System.IO;
using System.Xml.Serialization;


namespace Homework_07_WPF_Organizer
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Собственно ежедневник
		OrganizerClass myOrganizer = new OrganizerClass();

		DateTime tmpDT;

		// Видимая на экране дата ежедневника
		public SimpleDate currDate;
		
		public MainWindow()
		{
			InitializeComponent();

			// Специально выделяем одну переменную, в которую записываем текущую дату
			// Делаем на случай, если программа будет работать в 00:00,
			// чтобы текущая дата (currDate) и дата на экране были одинаковыми 
			tmpDT = DateTime.Now;
			currDate = new SimpleDate(tmpDT);
			MyCalendar.DisplayDate = tmpDT;
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
			//myOrganizer.GetDayList(currDate).Sort();
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
			if (startYear <= 0) startYear = (short)DateTime.Now.Year;
			if (endYear <=0 )     endYear = (short)DateTime.Now.Year;
			if (startYear > endYear)
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
			SimpleDate rD;
			SimpleTime rT;

			for (int i = 0; i < numberOfNotes; i++)
			{
				rYear  = (short)r.Next(startYear, endYear+1);
				rMonth = (sbyte)r.Next(1, 13);
				if (DateTime.IsLeapYear(rYear) & rMonth == 2) 
					 rDay = (sbyte)r.Next(1, 30); // високосный февраль
				else rDay = (sbyte)r.Next(1, YearClass.monthsLength[rMonth] + 1);
				rHour = (sbyte)r.Next(1, 24);
				rMin  = (sbyte)r.Next(1, 60);
				rSec  = (sbyte)r.Next(1, 60);
				rLocation = $"Location_{r.Next(1,1000):000}";
				rTitle = $"Title_{rYear}{rMonth}{rDay}_{rHour}:{rMin}:{rSec}";
				rText = $"This is the day {rDay} of month {rMonth} of the year {rYear}." +
						$" Time now is {rHour} hours {rMin} minutes {rSec} seconds.";

				rD = new SimpleDate(rYear, rMonth, rDay);
				rT = new SimpleTime(rHour, rMin);
				myOrganizer.AddNote(rD, rT, rLocation, rTitle, rText);
			}
			tmpDT    = DateTime.Now;
			currDate = new SimpleDate(tmpDT);

			// Выводим дату на экран
			currentDate.Text = $"{tmpDT.ToString("dddd, d MMMM yyyy г.")}";
			MyCalendar.DisplayDate = tmpDT;
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
			// Напоминание. currDate - видимая на экране дата ежедневника
			SimpleTime   currTime = new SimpleTime(DateTime.Now);
			Note		  newNote = new Note(currDate, currTime, "", "", "");
			NewNote newNoteWindow = new NewNote(ref newNote);
			newNoteWindow.ShowDialog();
			if (newNoteWindow.DialogResult != true) return;
			if (!String.IsNullOrEmpty(newNoteWindow.enterTime.Text))
			{
				DateTime tmpDT;
				if (DateTime.TryParseExact(newNoteWindow.enterTime.Text, 
										   "HH:mm",
										   CultureInfo.InvariantCulture,
										   DateTimeStyles.NoCurrentDateDefault,
										   out tmpDT))
				{
					currTime.Update(tmpDT);
				}
			}

			myOrganizer.AddNote(newNote);
			//myOrganizer.AddNote(currDate,
			//					currTime,
			//					newNoteWindow.enterLocation.Text,
			//					newNoteWindow.enterTitle.Text,
			//					newNoteWindow.enterText.Text);
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
			SearchMenu searchMenuWin = new SearchMenu();
			bool? result = searchMenuWin.ShowDialog();

			if (result != true) return;

			// Проверим, не ялвяется ли дата начала больше даты конца
			DateTime? sD = searchMenuWin.startDatePicker.SelectedDate;
			DateTime? eD = searchMenuWin.endDatePicker.SelectedDate;
			DateTime? tmpD;

			if (sD != null && eD != null && sD > eD)
			{
				tmpD = sD; sD = eD; eD = tmpD;
			}

			List<Note> workList = 
			myOrganizer.Search(sD, eD,
							   searchMenuWin.enterTime.Text,
							   searchMenuWin.enterLocation.Text,
							   searchMenuWin.enterTitle.Text,
							   searchMenuWin.enterText.Text);
			WorkingList workListWin = new WorkingList(workList);
			workListWin.ShowDialog();

		}

		private void Click_UploadFromXLM(object sender, RoutedEventArgs e)
		{
			UploadFileMenu uploadFileMenuWin = new UploadFileMenu();
			bool? haveFileName = uploadFileMenuWin.ShowDialog();

			// Была ли нажата клавиша Ок?
			if (haveFileName != true) return;		// Если нет, то выходим

			// Даже если был нажат Ок, был ли выбран файл?
			if (uploadFileMenuWin.result != true) return;	// Если нет, то выходим
			
			// Полное имя файла для загрузки записей
			string filepathname = uploadFileMenuWin.filepathname;
			
			// Дата, с которой грузим записи. Если null - то с начала
			DateTime? startDate = uploadFileMenuWin.startDatePicker.SelectedDate;
			
			// Дата, до которой грузим записи. Если null - то до конца
			DateTime?   endDate = uploadFileMenuWin.endDatePicker.SelectedDate;

			// Очищаем ежедневник и вставляем новые данные, или добавляем к существующим
			bool   replaceNotes = uploadFileMenuWin.replaceNotes;

			List<Note> tmp = new List<Note>();
			// Создаем сериализатор на основе указанного типа 
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Note>));

			// Создаем поток для чтения данных
			Stream fStream = new FileStream(filepathname, FileMode.Open, FileAccess.Read);

			// Запускаем процесс десериализации
			tmp = xmlSerializer.Deserialize(fStream) as List<Note>;

			// Закрываем поток
			fStream.Close();

			myOrganizer.UploadXML(tmp, startDate, endDate, replaceNotes);

			tmpDT = DateTime.Now;
			currDate = new SimpleDate(tmpDT);

			// Выводим дату на экран
			currentDate.Text = $"{tmpDT.ToString("dddd, d MMMM yyyy г.")}";
			MyCalendar.DisplayDate = tmpDT;

			dayToShowList.ItemsSource = myOrganizer.GetDayList(currDate);

			dayToShowList.Items.Refresh();

		}

		/// <summary>
		/// Сохраняет ежедневник в XML файл. Предлагает выбрать путь и имя файла
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_SaveToXLM(object sender, RoutedEventArgs e)
		{

			List<Note> tmp = myOrganizer.CollectAllNotesToList();
			// Configure save file dialog box
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.FileName = "Organizer"; // Default file name
			dlg.DefaultExt = ".xml"; // Default file extension
			dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

			// Show save file dialog box
			bool? result = dlg.ShowDialog();

			// Process save file dialog box results
			if (result != true) return;

			// Save document
			string Path = dlg.FileName;

			// Создаем сериализатор на основе указанного типа 
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Note>));

			// Создаем поток для сохранения данных
			Stream fStream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);

			// Запускаем процесс сериализации
			xmlSerializer.Serialize(fStream, tmp);

			// Закрываем поток
			fStream.Close();

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

		private void dayToShowList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			dayToShowList.Items.Refresh();
		}
	}

}
