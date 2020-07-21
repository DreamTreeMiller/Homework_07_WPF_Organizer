using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07_WPF_Organizer
{
	public class KeyStruct : IComparable
	{
		public short Year { get; set; }
		public sbyte Month { get; set; }
		public sbyte Day { get; set; }

		public sbyte Hour { get; set; }
		public sbyte Min { get; set; }
		public uint ID { get; private set; }

		public int CompareTo(object o)
		{
			KeyStruct key = o as KeyStruct;
			int result = Year.CompareTo(key.Year);
			if (result != 0) return result;
			result = Month.CompareTo(key.Month);
			if (result != 0) return result;
			result = Day.CompareTo(key.Day);
			if (result != 0) return result;
			result = Hour.CompareTo(key.Hour);
			if (result != 0) return result;
			result = Min.CompareTo(key.Min);
			if (result != 0) return result;
			result = ID.CompareTo(key.ID);
			return result;
		}

		public KeyStruct(DateTime dt, uint id)
		{
			this.Year  = (short)dt.Year;
			this.Month = (sbyte)dt.Month;
			this.Day   = (sbyte)dt.Day;
			this.Hour  = (sbyte)dt.Hour;
			this.Min   = (sbyte)dt.Minute;
			this.ID    = id;
		}

		public KeyStruct(short yyyy, sbyte mm, sbyte dd, sbyte hh, sbyte min, uint id)
		{
			this.Year	= yyyy;
			this.Month	= mm;
			this.Day	= dd;
			this.Hour	= hh;
			this.Min	= min;
			this.ID		= id;
		}

		public KeyStruct(SimpleDateTime dt, uint id)
		{
			this.Year  = (short)dt.Year;
			this.Month = (sbyte)dt.Month;
			this.Day   = (sbyte)dt.Day;
			this.Hour  = (sbyte)dt.Hour;
			this.Min   = (sbyte)dt.Min;
			this.ID    = id;
		}
		public override string ToString()
		{
			return $"{this.Day,2:00}-{this.Month,2:00}-{this.Year,4} " +
				   $"{this.Hour,2:00}:{this.Min,2:00}";
		}

		public string ToStringWithID()
		{
			return $"{this.ID,5} " +
				   $"{this.Day,2:00}-{this.Month,2:00}-{this.Year,4} " +
				   $"{this.Hour,2:00}:{this.Min,2:00}";
		}
	}

	public struct SimpleDateTime
	{
		public short Year  { get; set; }
		public sbyte Month { get; set; }
		public sbyte Day   { get; set; }

		public sbyte Hour  { get; set; }
		public sbyte Min   { get; set; }

		public SimpleDateTime(DateTime dt)
		{
			this.Year  = (short)dt.Year;
			this.Month = (sbyte)dt.Month;
			this.Day   = (sbyte)dt.Day;
			this.Hour  = (sbyte)dt.Hour;
			this.Min   = (sbyte)dt.Minute;
		}

		public SimpleDateTime(short yyyy, sbyte mm, sbyte dd, sbyte hh, sbyte min)
		{
			this.Year  = yyyy;
			this.Month = mm;
			this.Day   = dd;
			this.Hour  = hh;
			this.Min   = min;
		}

		public override string ToString()
		{
			return $"{this.Day,2:00}-{this.Month,2:00}-{this.Year,4} " +
				   $"{this.Hour,2:00}:{this.Min,2:00}";
		}
	}
	
	/// <summary>
	/// Класс базовая заметка. Содержит только заголовок и текст
	/// </summary>
	class BaseNote
	{
		public string Title { get; set; }           // Заголовок заметки
													// Для события - название события
													// Для дела    - само дело
		public string Text { get; set; }            // Текст заметки
													// Для события - необязательный комментарий
													// Для дела    - необязательный комментарий
		
		/// <summary>
		/// Создаёт базовую заметку с текущими системными датой и временем.
		/// </summary>
		/// <param name="title">Заголовок заметки</param>
		/// <param name="text">Текст заметки</param>
		public BaseNote(string title, string text)
		{
			this.Title		= title;
			this.Text		= text;
		}

	}

	public enum TypeOfNote : byte
	{
		Note  = 0,      // просто текстовая заметка
		Event = 1,      // мероприятие привязанное ко времени (можно и на весь день)
		ToDo  = 2       // дело, которое надо сделать
	}
	class Note			// Запись
	{
		public KeyStruct DisplayDT { get; set; }            // Содержит уникальный ID заметки
															// Генерируется на самом высоком уровне
															// OrganizerClass
															// Отображаемые дата и время записи в ежедневнике
		public SimpleDateTime CreationDT { get; set; }		// Дата и время создания записи в ежедневнике
		public SimpleDateTime ChangeDT   { get; set; }		// Дата и время последнего изменения записи
		public SimpleDateTime DeleteDT   { get; set; }		// Дата и время удаления записи

		public Stack<BaseNote> noteStack { get; set; }	// Текст заметки.
														// Организован как стек, чтобы можно было откатываться
														// назад
		public TypeOfNote TypeOfNote { get; set; }		// тип заметки
														// Event		- мероприятие в опр время
														// Note			- просто заметка
														// To Do Action	- дело

		public bool WholeDay { get; set; }				// Event на весь день true - yes, false - no
		public bool Done	 { get; set; }				// To Do сделано или нет true - yes, false - no
		public bool Star	 { get; set; }				// To Do приоритетное? true - да, false - нет
		
		/// <summary>
		/// Конструктор создаёт новую запись ЗАМЕТКА в заданных дате и времени
		/// </summary>
		/// <param name="displayDT">Дата и время отображения записи</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст заметки</param>
		public Note(KeyStruct keyDT, string title, string text)
		{
			// Помним, что конструктор класса вызывается при первом создании экземпляра класса через new
			// Поэтому переменным память не выделена. Переменные не инициализированы
			CreateNote(keyDT, title, text);
			this.TypeOfNote = TypeOfNote.Note;					  // тип записи 
		}

		/// <summary>
		/// Конструктор создаёт новую запись СОБЫТИЕ в заданных дате и времени
		/// </summary>
		/// <param name="displayDT">Дата и время отображения записи</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст заметки</param>
		/// <param name="wholeDay">true - whole day, false - в конкретное время</param>
		public Note(KeyStruct keyDT, string title, string text, bool wholeDay)
		{
			// Помним, что конструктор класса вызывается при первом создании экземпляра класса через new
			// Поэтому переменным память не выделена. Переменные не инициализированы
			CreateNote(keyDT, title, text);
			this.TypeOfNote = TypeOfNote.Event;				// тип записи - событие
			this.WholeDay   = wholeDay;

			// генерируется время 
			// Т.к. DisplayDT - это структура-свойство класса Note,
			// т.е. доступ к ней через { get; set; }, 
			// то полям этой структуры нельзя присвоить значения напрямую!
			// Поэтому создаём вспомогательную переменную tempDT

			KeyStruct tempDT = keyDT;  // отображаемые дата и время заметки 
			if (wholeDay)									
			{								// Если событие на весь день
				tempDT.Hour = 9;			// То отображаем его с 9:00 утра
				tempDT.Min  = 0;								
			}
			this.DisplayDT = tempDT;
		}

		/// <summary>
		/// Конструктор создаёт новую запись ДЕЛО для заданных даты и времени
		/// </summary>
		/// <param name="displayDT">Дата и время отображения записи</param>
		/// <param name="title">Заголовок записи</param>
		/// <param name="text">Текст заметки</param>
		/// <param name="done">Флаг - сделано/не сделано</param>
		/// <param name="star">Флаг - приоритетное/не приоритетное</param>
		public Note(KeyStruct keyDT, string title, string text, bool done, bool star)
		{
			// Помним, что конструктор класса вызывается при первом создании экземпляра класса через new
			// Поэтому переменным память не выделена. Переменные не инициализированы
			CreateNote(keyDT, title, text);
			this.TypeOfNote = TypeOfNote.ToDo;      // тип записи - дело
			this.Done = done;                       // флаг - сделано/не сделано
			this.Star = star;						// флаг - приоритетное/не приоритетное
		}

		/// <summary>
		/// Создаёт стек заметок, помещает туда общие для всех записей эл-ты: ID, заголовок и текст,
		/// дату отображения, дату создания. Устанавливает в минимальное значение даты изменения и удаления.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="dt"></param>
		/// <param name="title"></param>
		/// <param name="text"></param>
		private void CreateNote(KeyStruct keyDT, string title, string text)
		{
			// Помним, что конструктор класса вызывается при первом создании экземпляра класса через new
			// Поэтому переменным память не выделена. Переменные не инициализированы
			this.noteStack   = new Stack<BaseNote>();        // создаём стек заметок
			BaseNote newNote = new BaseNote(title, text);    // создаём заметку. 
			this.noteStack.Push(newNote);                    // помещаем заметку в стек

			// генерируется время 
			this.DisplayDT  = keyDT;	  // передаём уникальный ID и отображаемые дату и время заметки 
			this.CreationDT = new SimpleDateTime(DateTime.Now);       // Текущее значение даты и времени
			this.ChangeDT   = new SimpleDateTime(DateTime.MinValue);  // Значение по умлочанию 01.01.0001 00:00:00
			this.DeleteDT   = new SimpleDateTime(DateTime.MinValue);  // Значение по умлочанию 01.01.0001 00:00:00
		}

		/// <summary>
		/// Изменяет текущую заметку, т.е. помещает на верх стека новые заголовок и текст
		/// </summary>
		/// <param name="newTitle"></param>
		/// <param name="newText"></param>
		public void ChangeNote(string newTitle, string newText)
		{
			BaseNote newNote = new BaseNote(newTitle, newText);
			// заметка хранит до 10 последних изменений, включая текущее видимое значение
			if (this.noteStack.Count == 10)
			{
				// удаляем самый глубокий элемент
				// К сожалению, нет метода, удаляющего самый первый элемент стека,
				// хотя реализовать его проще простого - просто отсекаешь последний эл-т массива,
				// через который реализован стек.
				
				BaseNote[] tempArr = noteStack.ToArray();		// Поэтому просто копируем стек в массив, 
				noteStack.Clear();								// очищаем стек
				for (int i = tempArr.Length - 2; i >= 0; i--)   // а потом запихиваем назад в стек
					noteStack.Push(tempArr[i]);					// все эл-ты, кроме первого (посл. в массиве) 
			}

			this.noteStack.Push(newNote);

			SimpleDateTime newChangeDT = new SimpleDateTime(DateTime.Now);
			this.ChangeDT = newChangeDT;
		}

		public void ChangeNoteTilte(string newTitle) 
		{
			ChangeNote(newTitle, noteStack.Peek().Text);
		}
		public void ChnageNoteText (string newText) 
		{
			ChangeNote(noteStack.Peek().Title, newText);
		}
		public void ChangeNoteType (TypeOfNote newType) 
		{
			this.TypeOfNote = newType;
		}
		public void ChangeNoteFlag (bool wDayOrDone, bool star) 
		{
			this.WholeDay = wDayOrDone;
			this.Done	  = wDayOrDone;
			this.Star	  = star;
		}

	}

	class DayClass
	{
		public SortedList<KeyStruct, Note> Day { get; set; }

		public DayClass()
		{
			Day = new SortedList<KeyStruct, Note>();
		}

		/// <summary>
		/// Добавляет запись в ежедневник
		/// </summary>
		/// <param name="noteDT">Дата и время записи</param>
		/// <param name="note">Запись</param>
		public void AddNote(KeyStruct noteDT, Note note)
		{
			this.Day.Add(noteDT, note);
		}

		public void RemoveNote(KeyStruct noteDT)
		{
			this.Day.Remove(noteDT);
		}
	}

	/// <summary>
	/// Класс структуры года - 
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

		/// <summary>
		/// Конструктор для класса ежедневник. Создаёт экземпляр для текущего года.
		/// Инициализирует все счётчики.
		/// </summary>
		public OrganizerClass()             // Когда только создаём ежедневник
		{
			IDcounter = 0;                  // Обнуляем счётчик уникальных номеров
			Count	  = 0;                  // Обнуляем счётчик записей в ежедневнике

			Organizer = new SortedList<short, YearClass>(); // Создаём пустой органайзер
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
		public void AddNote(SimpleDateTime displayDT, string title, string text, 
							TypeOfNote typeOfNote, 
							bool wDayOrDone,		// флаг весь день - событие, сделано - дело
							bool star)				// флаг приоритета для дела
		{
			short currYear = displayDT.Year;
			sbyte currMon  = displayDT.Month;
			sbyte currDay  = displayDT.Day;
			DayClass dayToAdd;
			KeyStruct keyDT;
			Note newNote;

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
				keyDT = new KeyStruct(displayDT, ++IDcounter);
				switch (typeOfNote)
				{
					case TypeOfNote.Note:
						newNote = new Note(keyDT, title, text);
						dayToAdd.AddNote(keyDT, newNote);
						return;
					case TypeOfNote.Event:
						newNote = new Note(keyDT, title, text, wDayOrDone);
						dayToAdd.AddNote(keyDT, newNote);
						return;
					case TypeOfNote.ToDo:
						newNote = new Note(keyDT, title, text, wDayOrDone, star);
						dayToAdd.AddNote(keyDT, newNote);
						return;
					default:
						Console.WriteLine("Unknown type of a record!!!");
						break;
				}
			}

			// Если пришли сюда, значит в этом дне уже есть запись(и)
			dayToAdd = Organizer[currYear][currMon][currDay];

			// Существует ли уже запись с такими же временем, типом, заголовком, текстом?
			//
			// Если существует и полностью всё совпадает - то значит, мы просто ничего не добавляем
			// Если совпадает дата, но время отличается - добавляем
			// Если совпадает дата и время, но тип отличается - добавляем
			// Если совпадает дата и время, и тип, но заголовок и/или текст заметки оличается - добавляем
			// Если совпадает дата и время, и тип, и заголовок с текстом, - не добавляем.

			foreach(var e in dayToAdd.Day)
				if (e.Value.DisplayDT.Hour == displayDT.Hour)	// Запись с таким временем 
				if (e.Value.DisplayDT.Min  == displayDT.Min)    // уже существует?
				if (e.Value.TypeOfNote     == typeOfNote)		// Тип совпадает?
				if (e.Value.noteStack.Peek().Title == title)	// Заголовок и текст записи
				if (e.Value.noteStack.Peek().Text  == text)		// совпадает?
					return;			// ничего не добавляем!!! Такая заметка уже есть!!!
	
			// Специально сделал несколько  if-ов, а не составное выражение выр1 & выр2 & выр3 
			// Потому что в логическом выражении вычисляются обе части выражения, 
			// а сравнение текста - это оч. долго

			Count++;
			keyDT = new KeyStruct(displayDT, ++IDcounter);
			switch (typeOfNote)
			{
				case TypeOfNote.Note:
					newNote = new Note(keyDT, title, text);
					dayToAdd.AddNote(keyDT, newNote);
					return;
				case TypeOfNote.Event:
					newNote = new Note(keyDT, title, text, wDayOrDone);
					dayToAdd.AddNote(keyDT, newNote);
					return;
				case TypeOfNote.ToDo:
					newNote = new Note(keyDT, title, text, wDayOrDone, star);
					dayToAdd.AddNote(keyDT, newNote);
					return;
			}

		}


		public void EditNoteContent(KeyStruct keyDT, string newTitle, string newText)
		{
			BaseNote noteToEdit =
				Organizer[keyDT.Year][keyDT.Month][keyDT.Day].Day[keyDT].noteStack.Peek();
			noteToEdit.Title = newTitle;
			noteToEdit.Text  = newText;
			Organizer[keyDT.Year][keyDT.Month][keyDT.Day].Day[keyDT].noteStack.Push(noteToEdit);

			SimpleDateTime changeDT = new SimpleDateTime(DateTime.Now);
			Organizer[keyDT.Year][keyDT.Month][keyDT.Day].Day[keyDT].ChangeDT = changeDT;
		}

		/// <summary>
		/// Перемещает запись из указанной даты и с указанным ID в папку помойка
		/// </summary>
		/// <param name="id">Уникальный идентификатор записи</param>
		/// <param name="noteDT">Дата и время записи</param>
		public void RemoveNote(KeyStruct noteKeyDT)
		{
			Note noteToRemove =
				Organizer[noteKeyDT.Year][noteKeyDT.Month][noteKeyDT.Day].Day[noteKeyDT];
			Organizer[noteKeyDT.Year][noteKeyDT.Month][noteKeyDT.Day].Day.Remove(noteKeyDT);
			Count--;
		}

		#endregion Note Operations: Add, Change, Remove


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

	/// <summary>
	/// Класс помойки ... в помой
	/// Класс папки ... 
	/// </summary>
	class Trash
	{
		public string Name { get; set; }		// название папки
		
		// 
		//public List<Note> Content { get; set; } // содержание папки - заметки
		public SortedList<DateTime, Note> Content { get; set; } // содержание папки - заметки
	}

	/// <summary>
	/// Класс помойки ... в помой
	/// Класс папки ... 
	/// </summary>
	class Filter
	{
		public string Name { get; set; }        // название папки

		// 
		//public List<Note> Content { get; set; } // содержание папки - заметки
		public SortedList<DateTime, Object> Content { get; set; } // содержание папки - заметки
	}


}
