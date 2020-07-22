// Version 3.0. Delivered to SkillBox
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

		public SimpleDateTime() { }

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

		public int ComparebyDate(object o)
		{
			SimpleDateTime key = o as SimpleDateTime;
			int result = Year.CompareTo(key.Year);
			if (result != 0) return result;
			result = Month.CompareTo(key.Month);
			if (result != 0) return result;
			result = Day.CompareTo(key.Day);
			return result;
		}

		public int ComparebyTime(object o)
		{
			SimpleDateTime key = o as SimpleDateTime;
			int result = Hour.CompareTo(key.Hour);
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
		public SimpleDateTime NoteDateTime { get; set; }      // Отображаемые дата и время записи в ежедневнике

		public string NoteDate
		{
			get { return $"{NoteDateTime.Year:0000}.{NoteDateTime.Month:00}.{NoteDateTime.Day:00}"; }
		}

		public string NoteTime
		{
			get { return $"{NoteDateTime.Hour:00}:{NoteDateTime.Min:00}"; }
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
					NoteDateTime.Hour = (sbyte)tmpDT.Hour;
					NoteDateTime.Min  = (sbyte)tmpDT.Minute;
				}
			}
		}

		private string location;                            // Место
		public string Location
		{
			get { return this.location; }
			set
			{
				this.location = value;                      // При изменении места
			}
		}

		private string title;                               // Заголовок заметки
		public string Title
		{
			get { return this.title; }
			set { this.title = value;}
		}

		private string text;                                // Текст заметки
		public string Text
		{
			get { return this.text; }
			set	{ this.text = value;}
		}

		public Note() { }
		/// <summary>
		/// Конструктор создаёт новую запись в заданных дате и времени
		/// </summary>
		/// <param name="noteID">Уникальный номер записи</param>
		/// <param name="noteDT">Дата и время отображения записи</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст заметки</param>
		public Note(SimpleDateTime noteDT, string location, string title, string text)
		{
			// Помним, что конструктор класса вызывается при первом создании экземпляра класса через new
			// Поэтому переменным память не выделена. Переменные не инициализированы
			this.NoteDateTime  = new SimpleDateTime(noteDT);             // Отображаемые дата и время заметки 
			this.Location   = location;			// записываем место
			this.Title		= title;            // записываем заголовок
			this.Text		= text;             // и текст заметки
		}

		/// <summary>
		/// Компаратор класса по умолчанию
		/// </summary>
		public int CompareTo(object o)
		{
			Note key = o as Note;
			return NoteDateTime.CompareTo(key.NoteDateTime);
		}
		/// <summary>
		/// Компаратор типа IComarer<Note> по умолчанию для класса Note  
		/// </summary>
		public int Compare(Note x, Note y)
		{
			return x.CompareTo(y);
		}
		// // //////////////////////////////////////////////

		public int CompareToDate(object o)
		{
			Note key = o as Note;
			return NoteDateTime.ComparebyDate(key.NoteDateTime);
		}
		public int CompareByDate(Note x, Note y)
		{
			return x.CompareToDate(y);
		}
		// //////////////////////////////////////////////

		public int CompareToTime(object o)
		{
			Note key = o as Note;
			return NoteDateTime.ComparebyTime(key.NoteDateTime);
		}

		public int CompareByTime(Note x, Note y)
		{
			return x.CompareToTime(y);
		}

		public int CompareByLocation(Note x, Note y)
		{
			return x.Location.CompareTo(y.Location);
		}

		public int CompareByTitle(Note x, Note y)
		{
			return x.Title.CompareTo(y.Title);
		}

		public int CompareByText(Note x, Note y)
		{
			return x.Text.CompareTo(y.Text);
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
		public void AddNote(SimpleDateTime noteDT, string location, string title, string text)
		{
			Note newNote = new Note(noteDT, location, title, text);
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
				if (e.NoteDateTime == noteDT)
				{
					// Если я правильно понимаю, то е - это ссылка на объект. И при удалении из списка
					// объект в памяти остаётся, просто из списка удаляется ссылка.
					// но сама ссылка продолжает указывать на тот же объект
					
					this.Day.Remove(e);					// Удаляем объект из списка
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
		public uint Count { get; private set; }        // Количество записей в ежедневнике

		// Список лет в ежедневнике
		public SortedList<short, YearClass> Organizer { get; set; }

		// Корзина
		public List<Note> RecycleBin { get; set; }

		// Рабочий список. Для сохранения в файл, чтения из файла, поиска
		private List<Note> WorkingList { get; set; }

		/// <summary>
		/// Конструктор для класса ежедневник. Создаёт экземпляр для текущего года.
		/// Инициализирует все счётчики.
		/// </summary>
		public OrganizerClass()             // Когда только создаём ежедневник
		{
			Count = 0;                  // Обнуляем счётчик записей в ежедневнике

			Organizer = new SortedList<short, YearClass>(); // Создаём пустой органайзер
			RecycleBin = new List<Note>();                 // Создаём пустую корзину
			WorkingList = new List<Note>();                 // Создаём пустой рабочий список				
		}

		#region Note Operations: Add, Change, Remove

		/// <summary>
		/// Добавляет в ежедневник запись в конкретную дату и время, с заданными типом, заголовком и текстом.
		/// При добавлении генерирует уникальный ID записи.
		/// </summary>
		/// <param name="noteDateTime">Дата и время, в которое показана запись</param>
		/// <param name="location">Место мероприятия</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст записи</param>
		public void AddNote(SimpleDateTime noteDateTime, string location, string title, string text)
		{
			short currYear = noteDateTime.Year;
			sbyte currMon = noteDateTime.Month;
			sbyte currDay = noteDateTime.Day;

			// Проверка, выделена ли память для этой даты
			// Инициализирован ли год?
			if (!Organizer.ContainsKey(currYear))
			{
				YearClass yearToAdd = new YearClass(currYear);
				Organizer.Add(currYear, yearToAdd);                 // создаём новый год
			}

			// Инициализирован ли месяц для этой даты?
			if (Organizer[currYear][currMon] == null)
				Organizer[currYear].CreateMonth(currMon);           // создаём новый месяц

			// Инициализирован ли день?
			if (Organizer[currYear][currMon][currDay] == null)
				Organizer[currYear].CreateDay(currMon, currDay);    // создаём новый день

			// Добавляем запись
			Organizer[currYear][currMon][currDay].AddNote(noteDateTime, location, title, text);
			Count++;
		}

		private void AddNote(Note note)
		{
			AddNote(note.NoteDateTime, note.Location, note.Title, note.Text);
		}

		/// <summary>
		/// Перемещает запись из ежедневника в корзину
		/// </summary>
		/// <param name="noteToRemove">Запись для удаления</param>
		public void RemoveNote(Note noteToRemove)
		{
			//if (noteToRemove == null) return;
			short yyyy = noteToRemove.NoteDateTime.Year;
			sbyte mm = noteToRemove.NoteDateTime.Month;
			sbyte dd = noteToRemove.NoteDateTime.Day;
			Organizer[yyyy][mm][dd].Day.Remove(noteToRemove);
			Count--;
			RecycleBin.Add(noteToRemove);
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
			sbyte currMon = dt.Month;
			sbyte currDay = dt.Day;

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
			short yyyy = deletedNote.NoteDateTime.Year;
			sbyte mm = deletedNote.NoteDateTime.Month;
			sbyte dd = deletedNote.NoteDateTime.Day;
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

		/// <summary>
		/// Ищет все записи в диапазоне дат, в которых поля содержат подстроки
		/// Если какой-либо параметр равен null, то проверка этого параметра не осуществляется
		/// </summary>
		/// <param name="startDt">Поиск записей с этой даты</param>
		/// <param name="endDt">До этой даты</param>
		/// <param name="time">С таким временем</param>
		/// <param name="location">Содержащим такую подстроку в поле Место</param>
		/// <param name="title">Содержащим такую подстроку в поле Заголовок</param>
		/// <param name="text">Содержащим такую подстроку в поле Текст</param>
		/// <returns></returns>
		public List<Note> Search(DateTime? startDt, DateTime? endDt,
								 string time, string location, string title, string text)
		{
			List<Note> WorkingList = new List<Note>();
			// Ежедневник пуст
			if (this.Count == 0) return null;
			
			SimpleDateTime startDate = new SimpleDateTime(DateTime.MinValue);
			SimpleDateTime   endDate = new SimpleDateTime(DateTime.MaxValue);


			if (startDt == null)
			{
				// Если начальная дата не задана, тогда она равна 1 января первого года в ежедневнике
				startDate.Year  = Organizer.First().Value.YearValue;
				startDate.Month = 1;
				startDate.Day   = 1;
			}
			else
			{
				startDate = new SimpleDateTime((DateTime)startDt);
			}

			if (endDt == null)
			{
				// Если конечная дата не задана, тогда она равна 31 декабря последнего года в ежедневнике
				endDate.Year = Organizer.Last().Value.YearValue;
				endDate.Month = 12;
				endDate.Day = 31;
			}
			else
			{
				endDate = new SimpleDateTime((DateTime)endDt);
			}

			// Чисто для сокращения записи введём переменные
			short sY = startDate.Year;
			sbyte sM = startDate.Month, sMindex;
			sbyte sD = startDate.Day, sDindex;
			short eY = endDate.Year;
			sbyte eM = endDate.Month, eMindex;
			sbyte eD = endDate.Day, eDindex;

			foreach (var year in Organizer)                     // Берём по очереди каждый год.
			{
				if (sY <= year.Value.YearValue &&				// Если год в диапазоне дат
					year.Value.YearValue <= eY)
				{
					if (sY == year.Value.YearValue)				// Если текущий год, год начальной даты	
						sMindex = sM;							// устанавливаем, с какого месяца искать
					else sMindex = 1;							// иначе ищем с 1-ого месяца

					if (eY == year.Value.YearValue)				// Если текущий год, год конечной даты
						eMindex = eM;							// устанавливаем, до какого месяца искать
					else eMindex = 12;							// иначе ищем до декабря

					for (sbyte i = sMindex; i <= eMindex; i++)	// В году каждый месяц в диапазоне дат
					{
						if (year.Value[i] != null)				// Если месяц активирован,
						{
							if (sY == year.Value.YearValue &&	// год - начальной даты		
								sM == i)						// и месяц начальной даты
								sDindex = sD;					// устанавливаем день, с какого искать
							else sDindex = 1;					// иначе ищем с 1-ого дня месяца

							if (eY == year.Value.YearValue &&	// год - конечной даты
								eM == i)						// и месяц конечной даты
								eDindex = eD;					// устанавливаем день, до какого искать
							else								// иначе ищем до посл. дня месяца
								eDindex = (sbyte)(year.Value[i].Length-1);

							for (sbyte j = sDindex; j <= eDindex; j++)      // берём очередной день в месяце.
							{
								if (year.Value[i][j] != null)			// Если день активирован,
								{
									List<Note> day = year.Value[i][j].Day;
									if (day.Count != 0)						// и не пуст

										foreach (var note in day)			// то берём каждую запись дня
										{
										bool     isTime = String.IsNullOrEmpty(time) ?
																			true : 
																			(note.NoteTime == time);
										bool isLocation = String.IsNullOrEmpty(location) ? 
																			true : 
																			note.Location.Contains(location);
										bool	isTitle = String.IsNullOrEmpty(title) ?
																			true :
																			note.Title.Contains(title);
										bool	 isText = String.IsNullOrEmpty(text) ? 
																			true :
																			note.Text.Contains(text);
											if (isTime && isLocation && isTitle && isText)
											WorkingList.Add(note);      // и записываем в рабочий список
										}
								}

							}	// foreach по каждому дню месяца
						}	// проверка месяц активирован?
					} // for по месяцам года
				} // if проверка год в диапазоне
			} // foreach перебор лет

			//foreach (var note in source)
			//	if (note.NoteDateTime.CompareTo(startDate) >= 0 &&
			//		note.NoteDateTime.CompareTo(endDate) <= 0)
			//		AddNote(note);

			return WorkingList;
		}

		#region File Operations

		/// <summary>
		/// Собирает все записи ежедневника в один список
		/// </summary>
		/// <returns></returns>
		public List<Note> CollectAllNotesToList()
		{
			WorkingList.Clear();                                // Очищаем рабочий список

			foreach (var year in Organizer)                     // Берём по очереди каждый год.
				for (sbyte i = 1; i <= 12; i++)                 // В году каждый месяц.
					if (year.Value[i] != null)                  // Если месяц активирован,
						foreach (var day in year.Value[i])      // то берём каждый день в месяце.
							if (day != null)                    // Если день активирован,
								foreach (var note in day.Day)   // то берём каждую запись дня
									WorkingList.Add(note);      // и записываем в рабочий список
			return WorkingList;
		}

		/// <summary>
		/// Десериализация заметок из XML файла
		/// </summary>
		/// <param name="source">Список заметок</param>
		/// <param name="startDate">Дата, с какой добавлять. null - с начала</param>
		/// <param name="endDate">Дата, до какой добавлять. null - до конца</param>
		/// <param name="replaceNotes">Заменяем содержимое ежедневника или добавляем данные</param>
		public void UploadXML(List<Note> source, 
							   DateTime? startDt, 
							   DateTime? endDt,
							   bool replaceNotes)
		{
			SimpleDateTime startDate = new SimpleDateTime(DateTime.MinValue);
			SimpleDateTime   endDate = new SimpleDateTime(DateTime.MaxValue);
			// Если true - очищаем ежедневник
			if (replaceNotes) Organizer.Clear();
			if (startDt != null) startDate = new SimpleDateTime((DateTime)startDt);
			if (endDt   != null)   endDate = new SimpleDateTime((DateTime)endDt);

			if (startDt == null && endDt == null)   // просто добавляем все данные
			{
				foreach (var note in source) AddNote(note);
				return;
			}
			if (startDt == null)					// добавляем все с начала
			{
				foreach (var note in source)
					if (note.NoteDateTime.CompareTo(endDate) <= 0) AddNote(note);
				return;
			}

			if (endDt == null)                      // добавляем все до конца
			{
				foreach (var note in source)
					if (note.NoteDateTime.CompareTo(startDate) >= 0) AddNote(note);
				return;
			}

			foreach (var note in source)
				if (note.NoteDateTime.CompareTo(startDate) >= 0 &&
					note.NoteDateTime.CompareTo(endDate) <= 0)
					AddNote(note);
		}

		#endregion File Operations
	}

}
