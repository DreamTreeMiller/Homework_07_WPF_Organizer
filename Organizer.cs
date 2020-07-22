using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07_WPF_Organizer
{
	public class SimpleDateTime : IComparable
	{
		public short Year  { get; set; }
		public sbyte Month { get; set; }
		public sbyte Day   { get; set; }

		public sbyte Hour  { get; set; }
		public sbyte Min   { get; set; }

		public int CompareTo(object o)
		{
			SimpleDateTime key = o as SimpleDateTime;
			int result = Year.CompareTo(key.Year);
			if (result != 0) return result;
			result = Month.CompareTo(key.Month);
			if (result != 0) return result;
			result = Day.CompareTo(key.Day);
			if (result != 0) return result;
			result = Hour.CompareTo(key.Hour);
			if (result != 0) return result;
			result = Min.CompareTo(key.Min);
			return result;
		}
		public SimpleDateTime(DateTime dt)
		{
			this.Year  = (short)dt.Year;
			this.Month = (sbyte)dt.Month;
			this.Day   = (sbyte)dt.Day;
			this.Hour  = (sbyte)dt.Hour;
			this.Min   = (sbyte)dt.Minute;
		}

		public SimpleDateTime(SimpleDateTime dt)
		{
			this.Year  = dt.Year;
			this.Month = dt.Month;
			this.Day   = dt.Day;
			this.Hour  = dt.Hour;
			this.Min   = dt.Min;
		}


		public SimpleDateTime(short yyyy, sbyte mm, sbyte dd, sbyte hh, sbyte min)
		{
			this.Year  = yyyy;
			this.Month = mm;
			this.Day   = dd;
			this.Hour  = hh;
			this.Min   = min;
		}

		public void Update(DateTime dt)
        {
			this.Year  = (short)dt.Year;
			this.Month = (sbyte)dt.Month;
			this.Day   = (sbyte)dt.Day;
			this.Hour  = (sbyte)dt.Hour;
			this.Min   = (sbyte)dt.Minute;
		}

		public override string ToString()
		{
			return $"{this.Day,2:00}-{this.Month,2:00}-{this.Year,4} " +
				   $"{this.Hour,2:00}:{this.Min,2:00}";
		}
	}

	public class Note : IComparable, IComparer<Note>   // Запись
	{
		public uint noteID { get; private set; }            // Уникальный ID заметки
															// Генерируется на самом высоком уровне
															// OrganizerClass
		public SimpleDateTime DisplayDT { get; set; }      // Отображаемые дата и время записи в ежедневнике

		public string DisplayDate
		{
			get { return $"{DisplayDT.Year:0000}.{DisplayDT.Month:00}.{DisplayDT.Day:00}"; }
			//set
			//{
			//	DateTime tmpDT;
			//	if (DateTime.TryParseExact(value,
			//							   "HH:mm",
			//							   CultureInfo.InvariantCulture,
			//							   DateTimeStyles.NoCurrentDateDefault,
			//							   out tmpDT)
			//		)
			//	{
			//		DisplayDT.Hour = (sbyte)tmpDT.Hour;
			//		DisplayDT.Min = (sbyte)tmpDT.Minute;
			//	}
			//}
		}

		public string DisplayTime
		{
			get { return $"{DisplayDT.Hour:00}:{DisplayDT.Min:00}"; }
			set 
			{ 
				DateTime tmpDT;
				if (DateTime.TryParseExact(value, 
										   "HH:mm", 
										   CultureInfo.InvariantCulture, 
										   DateTimeStyles.NoCurrentDateDefault, 
										   out tmpDT)
					)
				{
					DisplayDT.Hour = (sbyte)tmpDT.Hour;
					DisplayDT.Min  = (sbyte)tmpDT.Minute;
				}
			}
		}
		public SimpleDateTime CreationDT { get; set; }      // Дата и время создания записи в ежедневнике
		public string CreationDate
		{
			get { return $"{CreationDT.Year:0000}.{CreationDT.Month:00}.{CreationDT.Day:00}"; }
			//set
			//{
			//	DateTime tmpDT;
			//	if (DateTime.TryParseExact(value,
			//							   "HH:mm",
			//							   CultureInfo.InvariantCulture,
			//							   DateTimeStyles.NoCurrentDateDefault,
			//							   out tmpDT)
			//		)
			//	{
			//		CreationDT.Hour = (sbyte)tmpDT.Hour;
			//		CreationDT.Min = (sbyte)tmpDT.Minute;
			//	}
			//}
		}

		public string CreationTime
		{
			get { return $"{CreationDT.Hour:00}:{CreationDT.Min:00}"; }
			//set
			//{
			//	DateTime tmpDT;
			//	if (DateTime.TryParseExact(value,
			//							   "HH:mm",
			//							   CultureInfo.InvariantCulture,
			//							   DateTimeStyles.NoCurrentDateDefault,
			//							   out tmpDT)
			//		)
			//	{
			//		CreationDT.Hour = (sbyte)tmpDT.Hour;
			//		CreationDT.Min = (sbyte)tmpDT.Minute;
			//	}
			//}
		}
		public SimpleDateTime ChangeDT { get; set; }        // Дата и время последнего изменения записи
		public SimpleDateTime DeleteDT { get; set; }        // Дата и время удаления записи

		public string DeleteDate
		{
			get { return $"{DeleteDT.Year:0000}.{DeleteDT.Month:00}.{DeleteDT.Day:00}"; }
			//set
			//{
			//	DateTime tmpDT;
			//	if (DateTime.TryParseExact(value,
			//							   "HH:mm",
			//							   CultureInfo.InvariantCulture,
			//							   DateTimeStyles.NoCurrentDateDefault,
			//							   out tmpDT)
			//		)
			//	{
			//		DeleteDT.Hour = (sbyte)tmpDT.Hour;
			//		DeleteDT.Min = (sbyte)tmpDT.Minute;
			//	}
			//}
		}

		public string DeleteTime
		{
			get { return $"{DeleteDT.Hour:00}:{DeleteDT.Min:00}"; }
			//set
			//{
			//	DateTime tmpDT;
			//	if (DateTime.TryParseExact(value,
			//							   "HH:mm",
			//							   CultureInfo.InvariantCulture,
			//							   DateTimeStyles.NoCurrentDateDefault,
			//							   out tmpDT)
			//		)
			//	{
			//		DeleteDT.Hour = (sbyte)tmpDT.Hour;
			//		DeleteDT.Min = (sbyte)tmpDT.Minute;
			//	}
			//}
		}

		private string location;                            // Место
		public string Location
		{
			get { return this.location; }
			set
			{
				this.location = value;                      // При изменении места
				this.ChangeDT.Update(DateTime.Now);         // устанавливаем дату и время изменений
			}
		}

		private string title;                               // Заголовок заметки
		public string Title
		{
			get { return this.title; }
			set
			{
				this.title = value;                         // При изменении заголовка
				this.ChangeDT.Update(DateTime.Now);         // устанавливаем дату и время изменений
			}
		}

		private string text;                                // Текст заметки
		public string Text
		{
			get { return this.text; }
			set
			{
				this.text = value;                          // При изменении текста
				this.ChangeDT.Update(DateTime.Now);         // устанавливаем дату и время изменений
			}
		}

		/// <summary>
		/// Конструктор создаёт новую запись в заданных дате и времени
		/// </summary>
		/// <param name="noteID">Уникальный номер записи</param>
		/// <param name="noteDT">Дата и время отображения записи</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст заметки</param>
		public Note(uint noteID, SimpleDateTime noteDT, string location, string title, string text)
		{
			// Помним, что конструктор класса вызывается при первом создании экземпляра класса через new
			// Поэтому переменным память не выделена. Переменные не инициализированы
			this.noteID		= noteID;            // передаём уникальный ID
			this.DisplayDT  = new SimpleDateTime(noteDT);             // Отображаемые дата и время заметки 
			this.CreationDT = new SimpleDateTime(DateTime.Now);       // Текущее значение даты и времени
			this.ChangeDT	= new SimpleDateTime(DateTime.MinValue);  // Значение по умлочанию 01.01.0001 00:00:00
			this.DeleteDT	= new SimpleDateTime(DateTime.MinValue);  // Значение по умлочанию 01.01.0001 00:00:00
			this.Location   = location;			// записываем место
			this.Title		= title;            // записываем заголовок
			this.Text		= text;             // и текст заметки
		}

		public int CompareTo(object o)
		{
			Note key = o as Note;
			int result = DisplayDT.CompareTo(key.DisplayDT);
			if (result != 0) return result;
			result = noteID.CompareTo(key.noteID);
			return result;
		}

		public int Compare(Note x, Note y)
		{
			return x.CompareTo(y);
		}

	}

	class DayClass
	{
		public List<Note> Day { get; set; }

		public DayClass()
		{
			Day = new List<Note>();
		}

		/// <summary>
		/// Добавляет запись в ежедневник
		/// </summary>
		/// <param name="noteDT">Дата и время записи</param>
		/// <param name="note">Запись</param>
		public void AddNote(uint noteID, SimpleDateTime noteDT, string location, string title, string text)
		{
			Note newNote = new Note(noteID, noteDT, location, title, text);
			this.Day.Add(newNote);                  // Добавляем запись в список
			this.Day.Sort(newNote.Compare);         // сортируем по времени и ИД
		}

		///// <summary>
		///// Удаляет запись из списка записей дня.
		///// (пока не реализовано) и помещает эту запись в корзину
		///// </summary>
		///// <param name="noteToRemove">Запись для удаления</param>
		//public void RemoveNote(Note noteToRemove)			// Может быть сделать bool. True - removed
		//{
		//	this.Day.Remove(noteToRemove);
		//}

		/// <summary>
		/// Удаляет запись с уникальным ID, датой и временем из списка записей дня.
		/// (пока не реализовано) и помещает эту запись в корзину
		/// </summary>
		/// <param name="noteID"></param>
		/// <param name="noteDT"></param>
		/// <returns>Возвращает удалённую запись, в которой поле DeletedDT установлено 
		/// в текущую дату. Возвращает null, если такой записи не найдено.</returns>
		public Note RemoveNote(uint noteID, SimpleDateTime noteDT)
        {
			foreach (var e in this.Day)
				if (e.noteID == noteID & e.DisplayDT == noteDT)
				{
					// Если я правильно понимаю, то е - это ссылка на объект. И при удалении из списка
					// объект в памяти остаётся, просто из списка удаляется ссылка.
					// но сама ссылка продолжает указывать на тот же объект
					
					this.Day.Remove(e);					// Удаляем объект из списка
					e.DeleteDT.Update(DateTime.Now);	// Помещаем текущие дату и время в ДВ удаления
					return e;							// Возвращаем эту запись
				}
			return null;				// Запись с таким ИД и временем не найдена
        }
	}

	/// <summary>
	/// Класс структуры года
	/// </summary>
	class YearClass
	{
		public short YearValue { get; private set; }     // год 1981, ... 2020 и т.д.
		private bool Leap { get; set; }				// високосный или нет
		public static byte[] monthsLength = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

		public DayClass[][] year;

		public DayClass[] this[sbyte mm]
		{
			get { return year[mm]; }
		}

		/// <summary>
		/// Конструктор для создания года. 
		/// </summary>
		/// <param name="year">Год - ГГГГ</param>
		public YearClass(short year)
		{
			this.YearValue = year;
			this.Leap = DateTime.IsLeapYear(year);		// Если год високосный
			this.year = new DayClass[13][];				// Создаём массив из 12 месяцев, считаем с 1
		}

		/// <summary>
		/// Создаёт месяц: создаёт, но не инициализирует массив дней для месяца.
		/// Дни месяца считаем с 1
		/// </summary>
		/// <param name="mm">Номер месяца от 1 до 12</param>
		public void CreateMonth(sbyte mm)
		{
			int monthLen = (this.Leap & mm == 2) ? 29 : monthsLength[mm];
			year[mm] = new DayClass[monthLen + 1];	// Дни месяца будем считать с 1
		}

		public void CreateDay(sbyte mm, sbyte dd)
		{
			year[mm][dd] = new DayClass();
		}
	}

	/// <summary>
	/// Класс ежедневник - самый высокий уровень. 
	/// Создание, удаление,редакция заметок инициируется на этом уровне для:
	/// - генерации уникальных ID для каждой заметки;
	/// - подсчёта количества заметок во всём ежедневнике
	/// </summary>
	// В общем-то, какой бы ни была структура самого ежедневника, такой сложной, как здесь,
	// Или это просто список заметок без иерархии, где заметки отличаются датой, временем и ключом,
	// Всё равно, надо было бы управлять заметками на самом высоком уровне, для выполнение задач,
	// описанных выше - генерация уникального ID, подсчёт кол-ва заметок
	// Такая сложная структура - прежде всего для быстрого отображения по дням, особенно при прокрутке,
	// при случайном выборе дня, для сохранения иерархии и загрузки из иерархии
	class OrganizerClass
	{
		public uint IDcounter { get; private set; }		// Уникальный номер для следующей заметки
		public uint Count	  { get; private set; }        // Количество записей в ежедневнике

		// Список лет в ежедневнике
		public SortedList<short,YearClass> Organizer { get; set; }

		// Корзина
		public List<Note> RecycleBin { get; set; }

		/// <summary>
		/// Конструктор для класса ежедневник. Создаёт экземпляр для текущего года.
		/// Инициализирует все счётчики.
		/// </summary>
		public OrganizerClass()             // Когда только создаём ежедневник
		{
			IDcounter = 0;                  // Обнуляем счётчик уникальных номеров
			Count	  = 0;                  // Обнуляем счётчик записей в ежедневнике

			Organizer = new SortedList<short, YearClass>(); // Создаём пустой органайзер
			RecycleBin = new List<Note>();					// Создаём пустую корзину
		}

		#region Note Operations: Add, Change, Remove

		/// <summary>
		/// Добавляет в ежедневник запись в конкретную дату и время, с заданными типом, заголовком и текстом.
		/// При добавлении генерирует уникальный ID записи.
		/// </summary>
		/// <param name="displayDT">Дата и время, в которое показана запись</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст записи</param>
		/// <param name="typeOfNote">Тип записи - заметка, событие или дело</param>
		public bool AddNote(SimpleDateTime displayDT, string location, string title, string text)
		{
			short currYear = displayDT.Year;
			sbyte currMon  = displayDT.Month;
			sbyte currDay  = displayDT.Day;
			DayClass dayToAdd;

			// Проверка, выделена ли память для этой даты
			// Инициализирован ли год?
			if (!Organizer.ContainsKey(currYear))
			{
				YearClass yearToAdd = new YearClass(currYear);
				Organizer.Add(currYear, yearToAdd);
			}

			// Инициализирован ли месяц для этой даты?
			if (Organizer[currYear][currMon] == null)
				Organizer[currYear].CreateMonth(currMon);

			// Инициализирован ли день?
			if (Organizer[currYear][currMon][currDay] == null)
			{
				Organizer[currYear].CreateDay(currMon, currDay);	// создаём новый день
				dayToAdd = Organizer[currYear][currMon][currDay];   // присваиваем ради сокращения записи

				Count++;
				IDcounter++;
				dayToAdd.AddNote(IDcounter, displayDT, location, title, text);
				return true;
			}

			// Если пришли сюда, значит в дне есть список записей. Возможно он пуст.
			dayToAdd = Organizer[currYear][currMon][currDay];

			// Существует ли уже запись с такими же временем, типом, заголовком, текстом?
			//
			// Если существует и полностью всё совпадает - то значит, мы просто ничего не добавляем
			// Если совпадает дата, но время отличается - добавляем
			// Если совпадает дата и время, но заголовок и/или текст заметки оличается - добавляем
			// Если совпадает дата и время, и заголовок с текстом, - не добавляем.

			// Если в дне список записей пуст, то цикл просто не выполнится ни разу
			foreach(var e in dayToAdd.Day)
				if (e.DisplayDT.Hour == displayDT.Hour)	// Запись с таким временем 
				if (e.DisplayDT.Min  == displayDT.Min)  // уже существует?
				if (e.Location == location)				// Место совпадает
				if (e.Title == title)					// Заголовок и текст записи
				if (e.Text  == text)					// совпадает?
					return false;			// ничего не добавляем!!! Такая заметка уже есть!!!
	
			// Специально сделал несколько  if-ов, а не составное выражение выр1 & выр2 & выр3 
			// Потому что в логическом выражении вычисляются обе части выражения, 
			// а сравнение текста - это оч. долго

			Count++;
			IDcounter++;
			dayToAdd.AddNote(IDcounter, displayDT, location, title, text);
			return true;
		}

		/// <summary>
		/// Перемещает запись из указанной даты и с указанным ID в корзину
		/// </summary>
		/// <param name="id">Уникальный идентификатор записи</param>
		/// <param name="noteDT">Дата и время записи</param>
		public void RemoveNote(uint noteID, SimpleDateTime noteDT)
		{
			Note noteToRemove =
				Organizer[noteDT.Year][noteDT.Month][noteDT.Day].RemoveNote(noteID, noteDT);
			if (noteToRemove != null)
			{
				RecycleBin.Add(noteToRemove);
				Count--;
			}
		}

		/// <summary>
		/// Перемещает запись из ежедневника в корзину
		/// </summary>
		/// <param name="noteToRemove">Запись для удаления</param>
		public void RemoveNote(Note noteToRemove)
		{
			//if (noteToRemove == null) return;
			short yyyy = noteToRemove.DisplayDT.Year;
			sbyte mm   = noteToRemove.DisplayDT.Month;
			sbyte dd   = noteToRemove.DisplayDT.Day;
			Organizer[yyyy][mm][dd].Day.Remove(noteToRemove);
			Count--;
			noteToRemove.DeleteDT.Update(DateTime.Now);
			RecycleBin.Add(noteToRemove);
		}

		/// <summary>
		/// Возвращает указатель на список записей указанного дня
		/// </summary>
		/// <param name="dt">указанный день</param>
		/// <returns>Указатель на список записей указнного дня, либо null - если день не был активирован
		/// либо в дне нет записей</returns>
		public List<Note> GetDayList(DateTime dt)
		{
			SimpleDateTime currDT = new SimpleDateTime(dt);
			if (IsDateActivated(currDT))
				return this.Organizer[currDT.Year][currDT.Month][currDT.Day].Day;
			return null;
		}

		/// <summary>
		/// Возвращает указатель на список записей указанного дня
		/// </summary>
		/// <param name="dt">указанный день</param>
		/// <returns>Указатель на список записей указнного дня, либо null - если день не был активирован
		/// либо в дне нет записей</returns>
		public List<Note> GetDayList(SimpleDateTime dt)
		{
			if (IsDateActivated(dt))
				return this.Organizer[dt.Year][dt.Month][dt.Day].Day;
			return null;
		}

		/// <summary>
		/// Проверяет, инициализирована ли память для дня
		/// </summary>
		/// <param name="dt">день для проверки</param>
		/// <returns>True - да, день инициализирован, хотя список записей в дне может быть пуст.
		/// False - день не инициализирован</returns>
		private bool IsDateActivated(SimpleDateTime dt)
		{
			short currYear = dt.Year;
			sbyte currMon  = dt.Month;
			sbyte currDay  = dt.Day;

			// Проверка, выделена ли память для этой даты
			// Инициализирован ли год?
			if (!Organizer.ContainsKey(currYear)) return false;

			// Инициализирован ли месяц для этой даты?
			if (Organizer[currYear][currMon] == null) return false;

			// Инициализирован ли день?
			if (Organizer[currYear][currMon][currDay] == null) return false;

			return true;
		}

		/// <summary>
		/// Перемещает выбранную запись из корзины обратно в ежедневник
		/// </summary>
		/// <param name="deletedNote">Выбранная запись</param>
		public void RestoreNoteFromBin(Note deletedNote)
		{
			short yyyy = deletedNote.DisplayDT.Year;
			sbyte mm = deletedNote.DisplayDT.Month;
			sbyte dd = deletedNote.DisplayDT.Day;
			Organizer[yyyy][mm][dd].Day.Add(deletedNote);
			Organizer[yyyy][mm][dd].Day.Sort();
			RecycleBin.Remove(deletedNote);
			this.Count++;
		}

		/// <summary>
		/// Навсегда удаляет запись из корзины
		/// </summary>
		/// <param name="deletedNote">Выбранная запись</param>
		public void DeleteForeverFromBin(Note deletedNote)
		{
			RecycleBin.Remove(deletedNote);
		}

		/// <summary>
		/// Очищает корзину от записей
		/// </summary>
		public void EmptyBin()
		{
			RecycleBin.Clear();
		}
		#endregion Note Operations: Add, Change, Remove, RestoreFromBin


		#region File Operations

		/// <summary>
		/// Метод загрузки ежедневника из файла. Тип файла определяется по расширению. Текущий ежедневник стирается
		/// </summary>
		/// <param name="path">Путь к файлу</param>
		public void Upload(string path)
		{ }

		/// <summary>
		/// Метод загрузки ежедневника из файла CSV. Текущий ежедневник стирается
		/// </summary>
		/// <param name="path">Путь к файлу</param>
		private void UploadCSV(string path)
		{ }

		/// <summary>
		/// Метод загрузки ежедневника из файла XML. Текущий ежедневник стирается
		/// </summary>
		/// <param name="path">Путь к файлу</param>
		private void UploadXML(string path)
		{ }

		public void Save(string path)
		{ }

		public enum FileType { CSV = 0, XML = 1}

		/// <summary>
		/// Сохраняет ежедневник в файл в формате CSV или XML
		/// Метод сохранения не сохраняет предыдущие редакции каждой записи, 
		/// т.е. берёт из стека записей текущие значения title, text и дат
		/// </summary>
		/// <param name="path">Путь файла для сохранения</param>
		/// <param name="fileType">Тип файла для сохранения CSV = 0, XML = 1</param>
		public void SaveAs(string path, FileType fileType)
		{ 
		}

		/// <summary>
		/// Загружает записи ежедневника из файла, тип которого определяется по расширению (CSC или XML)
		/// и вставляет их в существующий ежедневник. Запись из файла, полностью идентичная по дате, времени
		/// и содержанию записи в текущем ежедневнике, игнорируется.
		/// </summary>
		/// <param name="path">Путь файла записей</param>
		public void UploadAndMerge (string path)
		{

		}

		/// <summary>
		/// Загружает блок записей ежедневника из файла, тип которого определяется по расширению (CSC или XML)
		/// и вставляет их в существующий ежедневник. Границы блока определяются начальной и конечной датами.
		/// Запись из файла, полностью идентичная по дате, времени и содержанию записи в текущем ежедневнике,
		/// игнорируется.
		/// </summary>
		/// <param name="path">Путь файла записей</param>
		/// <param name="start">Дата, с которой надо загружать</param>
		/// <param name="finish">Дата, до которой включительно надо загружать записи</param>
		public void UploadAndMerge(string path, DateTime start, DateTime finish)
		{

		}

		#endregion File Operations
	}

}
