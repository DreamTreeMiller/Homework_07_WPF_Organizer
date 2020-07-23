// Version 4.0 simplified
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07_WPF_Organizer
{
	public class SimpleDate 
	{
		public short Year { get; set; }
		public sbyte Month { get; set; }
		public sbyte Day { get; set; }

		public SimpleDate() { }
		public int CompareTo(object o)
		{
			SimpleDate key = o as SimpleDate;
			int result = Year.CompareTo(key.Year);
			if (result != 0) return result;
			result = Month.CompareTo(key.Month);
			if (result != 0) return result;
			return Day.CompareTo(key.Day);
		}

		public SimpleDate(DateTime dt)
		{
			this.Year  = (short)dt.Year;
			this.Month = (sbyte)dt.Month;
			this.Day   = (sbyte)dt.Day;
		}

		public SimpleDate(short yyyy, sbyte mm, sbyte dd)
		{
			this.Year  = yyyy;
			this.Month = mm;
			this.Day   = dd;
		}

		public void Update(DateTime dt)
		{
			this.Year  = (short)dt.Year;
			this.Month = (sbyte)dt.Month;
			this.Day   = (sbyte)dt.Day;
		}

		public override string ToString()
		{
			return $"{this.Day,2:00}-{this.Month,2:00}-{this.Year,4}";
		}
	}

	public class SimpleTime 
	{
		#region Свойства, по сути поля
		public sbyte Hour { get; set; }
		public sbyte Minute { get; set; }
		#endregion

		#region Конструкторы
		public SimpleTime() { }

		public SimpleTime(DateTime dt)
		{
			this.Hour   = (sbyte)dt.Hour;
			this.Minute = (sbyte)dt.Minute;
		}

		public SimpleTime(sbyte hh, sbyte min)
		{
			this.Hour   = hh;
			this.Minute = min;
		}
		#endregion

		public int CompareTo(object o)
		{
			SimpleTime key = o as SimpleTime;
			int result = Hour.CompareTo(key.Hour);
			if (result != 0) return result;
			return Minute.CompareTo(key.Minute);
		}

		public override bool Equals(object obj)
		{
			SimpleTime key = obj as SimpleTime;
			return Hour.Equals(key.Hour) && Minute.Equals(key.Minute);
		}
		public static bool operator ==(SimpleTime x, SimpleTime y) => x.Equals(y);
		public static bool operator !=(SimpleTime x, SimpleTime y) => !x.Equals(y);

		public void Update(DateTime dt)
		{
			this.Hour   = (sbyte)dt.Hour;
			this.Minute = (sbyte)dt.Minute;
		}

		public override string ToString()
		{
			return $"{this.Hour,2:00}:{this.Minute,2:00}";
		}
	}

	public class Note   // Запись
	{
		public SimpleDate Date { get; set; }
		public int CompareByDate(Note x, Note y)
		{
			return x.Date.CompareTo(y.Date);
		}
		public string stringDate
		{
			get { return this.Date.ToString(); }
		}

		public SimpleTime Time { get; set; }
		public int CompareByTime(Note x, Note y)
		{
			return x.Time.CompareTo(y.Time);
		}
		public string stringTime
		{
			get { return this.Time.ToString(); }
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
					Time.Hour   = (sbyte)tmpDT.Hour;
					Time.Minute = (sbyte)tmpDT.Minute;
				}
				else
				{
					Time.Hour   = (sbyte)DateTime.Now.Hour;
					Time.Minute = (sbyte)DateTime.Now.Minute;
				}
			}
		}

		public string Location { get; set; }				// Место
		public int CompareByLocation(Note x, Note y)		// Компаратор для поля место
		{
			return x.Location.CompareTo(y.Location);
		}

		public string Title { get; set; }					// Заголовок заметки
		public int CompareByTitle(Note x, Note y)
		{
			return x.Title.CompareTo(y.Title);
		}		// Компаратор для поля заголовок

		public string Text { get; set; }					// Текст заметки
		public int CompareByText(Note x, Note y)
		{
			return x.Text.CompareTo(y.Text);
		}			// Компаратор для поля текст

		public Note() {	}

		/// <summary>
		/// Конструктор создаёт новую запись в заданных дате и времени
		/// </summary>
		/// <param name="noteID">Уникальный номер записи</param>
		/// <param name="noteDT">Дата и время отображения записи</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст заметки</param>
		public Note(SimpleDate date, SimpleTime time, string location, string title, string text)
		{
			// Помним, что конструктор класса вызывается при первом создании экземпляра класса через new
			// Поэтому переменным память не выделена. Переменные не инициализированы
			this.Date	  = date;             // Отображаемые дата и время заметки 
			this.Time	  = time;             // Отображаемые дата и время заметки 
			this.Location = location;		  // записываем место
			this.Title	  = title;            // записываем заголовок
			this.Text	  = text;             // и текст заметки
		}

		public int Compare(Note x, Note y)
		{
			int result = CompareByDate(x, y);
			if (result != 0) return result;
			return CompareByTime(x, y);
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
		public void AddNote(SimpleDate date, SimpleTime time, string location, string title, string text)
		{
			Note newNote = new Note(date, time, location, title, text);
			this.Day.Add(newNote);                  // Добавляем запись в список
			this.Day.Sort(newNote.Compare);         // сортируем по времени и ИД
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

		/// <summary>
		/// Конструктор для класса ежедневник. Создаёт экземпляр для текущего года.
		/// Инициализирует все счётчики.
		/// </summary>
		public OrganizerClass()             // Когда только создаём ежедневник
		{
			Count = 0;                  // Обнуляем счётчик записей в ежедневнике

			Organizer = new SortedList<short, YearClass>(); // Создаём пустой органайзер
			RecycleBin = new List<Note>();                 // Создаём пустую корзину
		}

		#region Note Operations: Add, Change, Remove

		/// <summary>
		/// Добавляет в ежедневник запись в конкретную дату и время, с заданными типом, заголовком и текстом.
		/// При добавлении генерирует уникальный ID записи.
		/// </summary>
		/// <param name="date">Дата, в которое показана запись</param>
		/// <param name="time">Дата, в которое показана запись</param>
		/// <param name="location">Место мероприятия</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст записи</param>
		public void AddNote(SimpleDate date, SimpleTime time, string location, string title, string text)
		{
			short currYear = date.Year;
			sbyte currMon  = date.Month;
			sbyte currDay  = date.Day;

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
			Organizer[currYear][currMon][currDay].AddNote(date, time, location, title, text);
			Count++;
		}

		public void AddNote(Note note)
		{
			AddNote(note.Date, note.Time, note.Location, note.Title, note.Text);
		}

		/// <summary>
		/// Перемещает запись из ежедневника в корзину
		/// </summary>
		/// <param name="noteToRemove">Запись для удаления</param>
		public void RemoveNote(Note noteToRemove)
		{
			//if (noteToRemove == null) return;
			short yyyy = noteToRemove.Date.Year;
			sbyte   mm = noteToRemove.Date.Month;
			sbyte   dd = noteToRemove.Date.Day;
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
		public List<Note> GetDayList(SimpleDate dt)
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
		private bool IsDateActivated(SimpleDate dt)
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
			short yyyy = deletedNote.Date.Year;
			sbyte mm   = deletedNote.Date.Month;
			sbyte dd   = deletedNote.Date.Day;
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
			
			SimpleDate startDate = new SimpleDate(DateTime.MinValue);
			SimpleDate   endDate = new SimpleDate(DateTime.MaxValue);
			SimpleTime  notetime = new SimpleTime(0, 0);
			if(!String.IsNullOrEmpty(time))
			{
				DateTime tmpDT;
				if (DateTime.TryParseExact(time, "HH:mm",
										  CultureInfo.InvariantCulture,
										  DateTimeStyles.NoCurrentDateDefault,
										  out tmpDT))
				{
					notetime.Update(tmpDT);
				}
				else time = "";	
			}

			if (startDt == null)
			{
				// Если начальная дата не задана, тогда она равна 1 января первого года в ежедневнике
				startDate.Year  = Organizer.First().Value.YearValue;
				startDate.Month = 1;
				startDate.Day   = 1;
			}
			else
			{
				startDate = new SimpleDate((DateTime)startDt);
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
				endDate = new SimpleDate((DateTime)endDt);
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
							if (sY == year.Value.YearValue &&	// Если год, в котором ищем, это год начальной даты,		
								sM == i)						// и месяц, в кот. ищем, это месяц начальной даты
								sDindex = sD;					// устанавливаем день, с какого искать
							else sDindex = 1;					// иначе ищем с 1-ого дня месяца

							if (eY == year.Value.YearValue &&	// Если год, в котором ищем, это год конечной даты,
								eM == i)						// и месяц, в котором ищем, это месяц конечной даты
								eDindex = eD;					// устанавливаем день, до какого искать
							else								// иначе ищем до посл. дня месяца
								eDindex = (sbyte)(year.Value[i].Length-1);

							for (sbyte j = sDindex; j <= eDindex; j++)		// берём очередной день в месяце.
							{
								if (year.Value[i][j] != null)				// Если день активирован,
								{
									List<Note> day = year.Value[i][j].Day;	// это лишь для сокращения записи
									if (day.Count != 0)						// и не пуст

										foreach (var note in day)			// то берём каждую запись дня
										{
										bool     isTime = String.IsNullOrEmpty(time) ?
																			true : 
																			(note.Time == notetime);
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
			List<Note> workingList = new List<Note>();                               

			foreach (var year in Organizer)                     // Берём по очереди каждый год.
				for (sbyte i = 1; i <= 12; i++)                 // В году каждый месяц.
					if (year.Value[i] != null)                  // Если месяц активирован,
						foreach (var day in year.Value[i])      // то берём каждый день в месяце.
							if (day != null)                    // Если день активирован,
								foreach (var note in day.Day)   // то берём каждую запись дня
									workingList.Add(note);      // и записываем в рабочий список
			return workingList;
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
			SimpleDate startDate = new SimpleDate(DateTime.MinValue);
			SimpleDate   endDate = new SimpleDate(DateTime.MaxValue);
			// Если true - очищаем ежедневник
			if (replaceNotes) Organizer.Clear();
			if (startDt != null) startDate = new SimpleDate((DateTime)startDt);
			if (endDt   != null)   endDate = new SimpleDate((DateTime)endDt);

			if (startDt == null && endDt == null)   // просто добавляем все данные
			{
				foreach (var note in source) AddNote(note);
				return;
			}
			if (startDt == null)					// добавляем все с начала
			{
				foreach (var note in source)
					if (note.Date.CompareTo(endDate) <= 0) AddNote(note);
				return;
			}

			if (endDt == null)                      // добавляем все до конца
			{
				foreach (var note in source)
					if (note.Date.CompareTo(startDate) >= 0) AddNote(note);
				return;
			}

			foreach (var note in source)
				if (note.Date.CompareTo(startDate) >= 0 &&
					note.Date.CompareTo(endDate) <= 0)
					AddNote(note);
		}

		#endregion File Operations
	}

}
