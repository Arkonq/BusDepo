using BusDepo.Domain;
using BusDepo.Repository;
using Dapper;
using DbUp;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace BusDepo
{
	class Program
	{
		private const string CONNECTION_STRING = "Server=A-305-04;Database=BusDepo;Trusted_Connection=true;";

		static string[] Status = { "Work", "Crash", "Repair" };

		static BusRepository busRepository = new BusRepository(CONNECTION_STRING);
		static MechanicRepository mechanicRepository = new MechanicRepository(CONNECTION_STRING);
		static ServiceRepository serviceRepository = new ServiceRepository(CONNECTION_STRING);

		static void StartRepairing()
		{
			Console.Write("Введите имя механика: ");
			string mechName = Console.ReadLine();
			if (mechanicRepository.GetByName(mechName) == null)
			{
				WrongData();
				return;
			}
			Guid MechanicId = mechanicRepository.GetByName(mechName).Id;
			Console.Write("Введите номер автобуса: ");
			string busNum = Console.ReadLine();
			if (busRepository.GetByNum(busNum) == null)
			{
				WrongData();
				return;
			}
			Guid BusId = busRepository.GetByNum(busNum).Id;

			var service = new Service
			{
				BusId = BusId,
				MechanicId = MechanicId
			};
			serviceRepository.SetRepair(service);
			busRepository.Update(BusId, Status[2]);
		}

		private static void WrongData()
		{
			Console.Clear();
			Console.WriteLine("Введены неверные данные. Возвращение в главное меню");
			Console.ReadLine();
			Console.Clear();
		}

		static void FinishRepairing()
		{
			Console.Write("Введите номер автобуса: ");
			string busNum = Console.ReadLine();
			if (busRepository.GetByNum(busNum) == null)
			{
				WrongData();
				return;
			}
			Guid BusId = busRepository.GetByNum(busNum).Id;

			serviceRepository.DelRepair(BusId);
			busRepository.Update(BusId, Status[0]);
		}

		static void ShowBusesInfo()
		{
			var buses = busRepository.GetAll();
			int num = 1;
			foreach (var bus in buses)
			{
				if (bus.Status == "Repair")
				{
					var mechId = serviceRepository.GetMechanicId(bus.Id);
					var mechName = mechanicRepository.GetById(mechId).Name;
					Console.WriteLine($"{num++}) Автобус номер {bus.Num} - Статус {bus.Status} - Механик {mechName}");
				}
				else
				{
					Console.WriteLine($"{num++}) Автобус номер {bus.Num} - Статус {bus.Status}");
				}
			}
			Console.ReadLine();
		}

		static void ShowMechanicsInfo()
		{
			Console.Clear();
			var mechanics = mechanicRepository.GetAll();
			int num = 1;
			foreach (var mechanic in mechanics)
			{
				if (serviceRepository.GetMechanic(mechanic) != null)
				{
					Console.WriteLine($"{num++}) Имя {mechanic.Name} - Работа - Есть");
				}
				else
				{
					Console.WriteLine($"{num++}) Имя {mechanic.Name} - Работа - Нет");
				}
			}
			Console.ReadLine();
		}

		static void CrashBus()
		{
			Console.Write("Введите номер автобуса: ");
			string busNum = Console.ReadLine();
			if (busRepository.GetByNum(busNum) == null)
			{
				WrongData();
				return;
			}
			Guid BusId = busRepository.GetByNum(busNum).Id;

			busRepository.Update(BusId, Status[1]);
		}


		static void Main(string[] args)
		{
			while (true)
			{
				Console.Clear();
				Console.WriteLine("\t1. Вывести список всех автобусов.");
				Console.WriteLine("\t2. Вывести список всех механиков.");
				Console.WriteLine("\t3. Записать автобус в список сломанных.");
				Console.WriteLine("\t4. Записать автобус на ремонт.");
				Console.WriteLine("\t5. Записать автобус как починенный.");
				Console.WriteLine("\t0. Завершение работы программы.");
				Console.Write("Введите действие: ");
				int answ = Int32.Parse(Console.ReadLine());
				Console.WriteLine();
				Console.Clear();
				switch (answ)
				{
					case 1:
						ShowBusesInfo();
						break;
					case 2:
						ShowMechanicsInfo();
						break;
					case 3:
						CrashBus();
						break;
					case 4:
						StartRepairing();
						break;
					case 5:
						FinishRepairing();
						break;
					default:
						Console.WriteLine("Завершение работы программы.");
						Console.ReadLine();
						return;
				}
			}
		}

		private static void DbUp()
		{
			EnsureDatabase.For.SqlDatabase(CONNECTION_STRING);  // Создание БД
			var upgrader =
					DeployChanges.To  // Накатывание всех скриптов
							.SqlDatabase(CONNECTION_STRING)
							.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()) // Получение всех файлов в сборке со словом Script и параметром Embedded
							.LogToConsole()
							.Build();
			upgrader.PerformUpgrade();
		}

		private static void InsertData()
		{
			using (var connection = new SqlConnection(CONNECTION_STRING))
			{
				var bus = new Bus
				{
					Num = "A-061",
					Status = Status[0]
				};
				connection.Execute("Insert into Buses values(@Id, @Num, @Status)", bus);
			}
		}
	}
}
