using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NPOI.OpenXmlFormats;

namespace Lesson1
{
    class Program
    {
        static void Main(string[] args)
        {

            // Инициализация сервиса для работы с данными.
            var dataService = new DataService();
            var dataServiceSql = new DataServiceSql();
            //инициализация сервиса для ввода/вывода
            var ioService = new IOService();

            var factoriesFromExcel = dataService.GetFactoriesFromExcel().ToList();

            var factories = dataService.GetFactories().ToList();
            var units = dataServiceSql.GetUnits().Result;
            var tanks = dataService.GetTanks().ToList();
            
            ioService.Output($"Количество резервуаров {tanks.Count()}");
            foreach (var tank in tanks)
            {
                try
                {
                    var unit = dataServiceSql.FindUnitByTank(tank).Result;
                    var factory = dataServiceSql.FindFactoryByUnit(unit).Result;
                    ioService.Output($"{tank.Name} принадлежит установке {unit.Name} и заводу {factory.Name}");
                }
                catch(Exception e)
                {
                    ioService.Exeption(e);
                }
            }

            //проверка валидации резервуаров
            bool tankIsValid = ValidateTank(tanks[0]);
            ioService.Output($"Результат валидации {tanks[0].Name}: {tankIsValid}");

            var totalVolume = dataService.GetTotalVolume(tanks);
            ioService.Output($"Общий объем резервуаров: {totalVolume}");

            var flag = true;
            while (flag)
            {
                ioService.ProgramMenu();

                // Считываем нажатие кнопки на клавиатуре.
                var key = Console.ReadKey();
                ioService.SkipLine();
                switch(key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        FindFactoryByName(dataService, factories, ioService);
                        break;
                    case ConsoleKey.D2: 
                    case ConsoleKey.NumPad2:
                        FindUnitByName(dataServiceSql, ioService);//Переделан на SQL
                        break;
                    case ConsoleKey.D3: 
                    case ConsoleKey.NumPad3:
                        FindTankByName(dataService, tanks, ioService);
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        Task.Run(async () => await dataServiceSql.CreateUnit()).Wait();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                         dataServiceSql.ReadUnit();
                        break;
                    case ConsoleKey.D6: 
                    case ConsoleKey.NumPad6:
                        Task.Run(async () => await dataServiceSql.UpdateUnit()).Wait();
                        break;
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        Task.Run(async () => await dataServiceSql.DeleteUnit()).Wait();
                        break;
                    case ConsoleKey.D8: 
                    case ConsoleKey.NumPad8:
                        FindTankByNameLinqMethodForMenu(ioService, dataService, tanks);
                        break;
                    default:
                        flag = false;
                        break;
                }
            }
        }

        private static void FindTankByNameLinqQueryForMenu(IOService ioService, DataService dataService, IEnumerable<Tank> tanks)
        {
            try
            {
                ioService.Output("Введите название резервуара");
                var tankName = ioService.Input();
                var tank = dataService.FindTankByNameLINQQuery(tankName, tanks);
                tank.GetInformation();
                ioService.SkipLine();
            }
            catch (Exception e)
            {
                ioService.Exeption(e);
            }
        }

        private static void FindTankByNameLinqMethodForMenu(IOService ioService, DataService dataService, IEnumerable<Tank> tanks)
        {
            try
            {
                ioService.Output("Введите название резервуара");
                var tankName = ioService.Input();
                var tank = dataService.FindTankByNameLINQMethod(tankName, tanks);
                tank.GetInformation();
                ioService.SkipLine();
            }
            catch (Exception e)
            {
                ioService.Exeption(e);
            }
        }
        public static bool SerializerFactoryToJSON(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks, IOService ioService)
        {
            var factory = new OurFactory
            {
                Factories = factories,
                Units = units,
                Tanks = tanks
            };
            var json = JsonSerializer.Serialize(factory,
                new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                });
            var fileName = "ourFactory.json";
            File.WriteAllText(fileName, json);
            if(System.IO.File.Exists(fileName))
            {
                ioService.Output($"Файл {fileName} создан рядом с lesson1.exe");
                return false;
            }
            throw new Exception("При выгрузке json произошла ошибка.");
        }

        private static bool SerializeToJsonForMenu(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks, IOService ioService)
        {
            try
            {
                var flag = SerializerFactoryToJSON(factories, units, tanks, ioService);
                return flag;
            }
            catch (Exception e)
            {
                ioService.Exeption(e);
                return true;
            }
        }
        private static void FindFactoryByName(DataService dataService, IEnumerable<Factory> factories, IOService ioService)
        {
            ioService.Notify += DisplayMessage;
            var factoryName = ioService.InputForFindSth("завода", ioService);
            try
            {
                var factory = dataService.FindFactoryByName(factoryName, factories);
                if (factory != null)
                {
                    ioService.Output($"Завод {factory.Name} найден с индексом {factory.Id}");
                }
                ioService.SkipLine();
            }
            catch(Exception e)
            {
                ioService.Exeption(e);
            } 
        }
        //Переделан на SQL
        private static void FindUnitByName(DataServiceSql dataServiceSql, IOService ioService)
        {
            ioService.Notify += DisplayMessage;
            var unitName = ioService.InputForFindSth("установки", ioService);
            try
            {
                var unit = dataServiceSql.FindUnitByName(unitName).Result;
                if (unit != null)
                {
                    ioService.Output($"Установка {unit.Name} найдена с индексом {unit.Id}");
                }
                ioService.SkipLine();
            }
            catch(Exception e)
            {
                ioService.Exeption(e);
            }
        }

        private static void FindTankByName(DataService dataService, IEnumerable<Tank> tanks, IOService ioService)
        {
            ioService.Notify += DisplayMessage;
            var tankName = ioService.InputForFindSth("резервуара", ioService);
            try
            {
                var tank = dataService.FindTankByName(tankName, tanks);
                if (tank != null)
                {
                    ioService.Output($"Резервуар {tank.Name} найдена с индексом {tank.Id}");
                }
                ioService.SkipLine();
            }
            catch(Exception e)
            {
                ioService.Exeption(e);
            }
        }

        private static void DisplayMessage(string message, IOService ioService)
        {
            ioService.Output(message);
        }
        /// <summary>
        /// add/change/delete
        /// добавление/изменение/удаление
        /// </summary>
        private static void ACDjson(IOService ioService)
        {
            bool flag = true;
            while (flag)
            {
                ioService.JsonMenu();
                var fileName = "Test.json";
                // Считываем нажатие кнопки на клавиатуре.
                var key = Console.ReadKey();
                ioService.SkipLine();
                if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        var jsonElements = File.ReadAllText(fileName);
                        File.Delete(fileName);
                        var elements = JsonSerializer.Deserialize<List<string> >(jsonElements);
                        ioService.Output("Введите элемент на добавление.");
                        var elem = ioService.Input();
                        elements.Add(elem);
                        var json = JsonSerializer.Serialize<List<string> >(elements,
                                                        new JsonSerializerOptions
                                                        {
                                                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                                                        });
                        File.WriteAllText(fileName, json);
                        ioService.Output("Элемент добавлен");
                    }
                    else
                    {
                        ioService.Output("Введите элемент на добавление.");
                        var elem = ioService.Input();
                        var elements = new List<string> { elem }; 
                        var json = JsonSerializer.Serialize<List<string> >(elements,
                                                        new JsonSerializerOptions
                                                        {
                                                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                                                        });
                        File.WriteAllText(fileName, json);
                        ioService.Output("Элемент добавлен");
                    }
                    
                }
                else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
                {
                    ioService.Output("В файле содержится: ");
                    var jsonElements = File.ReadAllText(fileName);
                    File.Delete(fileName);
                    var elements = JsonSerializer.Deserialize<List<string>>(jsonElements);
                    foreach(var elem in elements)
                    {
                        ioService.Output(elem);
                    }
                    ioService.Output("Какой элемент вы хотите изменить?");
                    var indexElem = int.Parse(ioService.Input()) - 1;
                    ioService.Output("Введите новый элемент");
                    var newElem = ioService.Input();
                    elements[indexElem] = newElem;
                    var json = JsonSerializer.Serialize<List<string>>(elements,
                                                    new JsonSerializerOptions
                                                    {
                                                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                                                    });
                    File.AppendAllText(fileName, json);
                    ioService.Output("Элемент изменен");
                }
                else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3)
                {
                    ioService.Output("В файле содержится: ");
                    var jsonElements = File.ReadAllText(fileName);
                    File.Delete(fileName);
                    var elements = JsonSerializer.Deserialize<List<string>>(jsonElements);
                    foreach (var elem in elements)
                    {
                        ioService.Output(elem);
                    }
                    ioService.Output("Какой элемент вы хотите удалить? (нумерация с 1)");
                    var indexElem = int.Parse(ioService.Input()) - 1;
                    elements.RemoveAt(indexElem);
                    var json = JsonSerializer.Serialize<List<string>>(elements,
                                                    new JsonSerializerOptions
                                                    {
                                                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                                                    });
                    File.AppendAllText(fileName, json);
                    ioService.Output("Элемент удалён");
                }
                else
                {
                    flag = false;
                }
            }
        }

        /// <summary>
        /// Асинхронное считывание из json
        /// </summary>
        static async Task ReadJsonAsync(IOService ioService)
        {
            ioService.Output("Считывание началось");
            var dirInfo = new DirectoryInfo(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).Parent?.Parent?.Parent;
            var path = Path.Combine(dirInfo.FullName, "Async.json");
            var result = await Task.Run(()=> File.ReadAllTextAsync(path));
            ioService.Output("Файл считался. Содержимое: ");
            ioService.Output(result);
        }

        public static bool ValidateTank(Tank tank)
        {
            Type t = typeof(Tank);
            var fi = t.GetProperty("Volume");
            var attrs = fi.GetCustomAttributes(typeof(AllowedRangeAttribute),false);
            foreach (AllowedRangeAttribute attr in attrs)
            {
                return tank.Volume >= attr.minValue && tank.Volume <= attr.maxValue && tank.Volume <= tank.MaxVolume;
            }
            return true;
        }
    }

}
