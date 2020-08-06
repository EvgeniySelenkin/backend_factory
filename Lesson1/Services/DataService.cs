using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Linq;

namespace Lesson1
{
    public class DataService
    {
        private string path;
        public DataService()
        {
            var dirInfo = new DirectoryInfo(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).Parent?.Parent?.Parent;
            var pathFile = Path.Combine(dirInfo.FullName, "Таблица_резервуаров.xlsx");
            path = pathFile;
        }

        /// <summary>
        /// Получение фабрики из таблицы excel
        /// </summary>
        /// <returns>Массив фабрик</returns>
        public IEnumerable<Factory> GetFactoriesFromExcel()
        {
            var factories = new List<Factory>();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XSSFWorkbook workbook = new XSSFWorkbook(fs);
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);

            for (int i = 1; i < 3; i++)
            {
                ICell factoryIdCell = sheet.GetRow(i).GetCell(0);
                factoryIdCell.SetCellType(CellType.Numeric);
                ICell factoryNameCell = sheet.GetRow(i).GetCell(1);
                factoryNameCell.SetCellType(CellType.String);
                ICell factoryDescriptionCell = sheet.GetRow(i).GetCell(2);
                factoryDescriptionCell.SetCellType(CellType.String);
                var factory = new Factory((int)factoryIdCell.NumericCellValue, factoryNameCell.StringCellValue, factoryDescriptionCell.StringCellValue);
                factories.Add(factory);
            }

            return factories;
        }

        /// <summary>
        /// Получение установок из таблицы excel
        /// </summary>
        /// <returns>Массив установок</returns>
        public IEnumerable<Unit> GetUnitsFromExcel()
        {
            var units = new List<Unit>();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XSSFWorkbook workbook = new XSSFWorkbook(fs);
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(1);

            for (int i = 1; i < 3; i++)
            {
                ICell unitIdCell = sheet.GetRow(i).GetCell(0);
                unitIdCell.SetCellType(CellType.Numeric);
                ICell unitNameCell = sheet.GetRow(i).GetCell(1);
                unitNameCell.SetCellType(CellType.String);
                ICell unitFactoryIdCell = sheet.GetRow(i).GetCell(2);
                unitFactoryIdCell.SetCellType(CellType.Numeric);
                var unit = new Unit((int)unitIdCell.NumericCellValue, unitNameCell.StringCellValue, (int)unitFactoryIdCell.NumericCellValue);
                units.Add(unit);
            }

            return units;
        }

        /// <summary>
        /// Получение резервуаров из таблицы excel
        /// </summary>
        /// <returns>Массив резервуаров</returns>
        public IEnumerable<Tank> GetTanksFromExcel()
        {
            var tanks = new List<Tank>();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XSSFWorkbook workbook = new XSSFWorkbook(fs);
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(2);

            for (int i = 1; i < 3; i++)
            {
                ICell tankIdCell = sheet.GetRow(i).GetCell(0);
                tankIdCell.SetCellType(CellType.Numeric);
                ICell tankNameCell = sheet.GetRow(i).GetCell(1);
                tankNameCell.SetCellType(CellType.String);
                ICell tankVolumeCell = sheet.GetRow(i).GetCell(2);
                tankVolumeCell.SetCellType(CellType.Numeric);
                ICell tankMaxVolumeCell = sheet.GetRow(i).GetCell(3);
                tankMaxVolumeCell.SetCellType(CellType.Numeric);
                ICell tankUnitIdCell = sheet.GetRow(i).GetCell(4);
                tankUnitIdCell.SetCellType(CellType.Numeric);
                var tank = new Tank((int)tankIdCell.NumericCellValue, tankNameCell.StringCellValue, (int)tankVolumeCell.NumericCellValue, (int)tankMaxVolumeCell.NumericCellValue, (int)tankUnitIdCell.NumericCellValue);
                tanks.Add(tank);
            }

            return tanks;
        }

        /// <summary>
        /// Получить все заводы
        /// </summary>
        public IEnumerable<Factory> GetFactories()
        {
            var factory1 = new Factory
            {
                Id = 1,
                Name = "МНПЗ",
                Description = "Московский нефтеперерабатывающий завод"
            };

            var factory2 = new Factory
            {
                Id = 2,
                Name = "ОНПЗ",
                Description = "Омский нефтеперерабатывающий завод"
            };

            var factories = new[] { factory1, factory2 };
            return factories;
        }

        /// <summary>
        /// Получить все установки
        /// </summary>
        public IEnumerable<Unit> GetUnits()
        {
            var unit1 = new Unit
            {
                Id = 1,
                Name = "ГФУ-1",
                FactoryId = 1
            };

            var unit2 = new Unit
            {
                Id = 2,
                Name = "ГФУ-2",
                FactoryId = 1
            };

            var unit3 = new Unit
            {
                Id = 3,
                Name = "АВТ-6",
                FactoryId = 2
            };

            var units = new[] { unit1, unit2, unit3 };
            return units;
        }

        /// <summary>
        /// Получить все резервуары
        /// </summary>
        public IEnumerable<Tank> GetTanks()
        {
            var tank1 = new Tank
            {
                Id = 1,
                Name = "Резервуар 1",
                Volume = 1500,
                MaxVolume = 2000,
                UnitId = 1
            };

            var tank2 = new Tank
            {
                Id = 2,
                Name = "Резервуар 2",
                Volume = 2500,
                MaxVolume = 3000,
                UnitId = 1
            };

            var tank3 = new Tank
            {
                Id = 3,
                Name = "Дополнительный резервуар 24",
                Volume = 3000,
                MaxVolume = 3000,
                UnitId = 2
            };

            var tank4 = new Tank
            {
                Id = 4,
                Name = "Резервуар 35",
                Volume = 3000,
                MaxVolume = 3000,
                UnitId = 2
            };

            var tank5 = new Tank
            {
                Id = 5,
                Name = "Резервуар 47",
                Volume = 4000,
                MaxVolume = 5000,
                UnitId = 2
            };

            var tank6 = new Tank
            {
                Id = 6,
                Name = "Резервуар 256",
                Volume = 500,
                MaxVolume = 500,
                UnitId = 3
            };

            var tanks = new[] { tank1, tank2, tank3, tank4, tank5, tank6 };
            return tanks;
        }


        /// <summary>
        /// Получить установку по резервуару
        /// </summary>
        /// <param name="tank">Резервуар</param>
        /// <param name="units">Список установок</param>
        /// <returns>Установка</returns>
        public Unit FindUnitByTank(Tank tank, IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                if (unit.Id == tank.UnitId)
                {
                    return unit;
                }
            }
            throw new Exception("Установка по резервуару не найдена!");
        }

        /// <summary>
        /// Получить завод по установке
        /// </summary>
        /// <param name="unit">Установка</param>
        /// <param name="factories">Список заводов</param>
        /// <returns>Завод</returns>
        public Factory FindFactoryByUnit(Unit unit, IEnumerable<Factory> factories)
        {
            foreach (var factory in factories)
            {
                if (factory.Id == unit.FactoryId)
                {
                    return factory;
                }
            }
            throw new Exception("Завод по установке не найден!");
        }

        /// <summary>
        /// Суммарный объем резервуаров
        /// </summary>
        /// <param name="tanks">Список резервуаров</param>
        /// <returns>Объем</returns>
        public int GetTotalVolume(IEnumerable<Tank> tanks)
        {
            int totalVolume = 0;
            foreach(var tank in tanks)
            {
                totalVolume += tank.Volume;
            }
            return totalVolume;
        }

        /// <summary>
        /// Поиск установки по имени.
        /// </summary>
        /// <param name="unitName">Имя установки</param>
        /// <param name="units">Установки</param>
        /// <returns>Установка</returns>
        public Unit FindUnitByName(string unitName, IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                if (unit.Name == unitName)
                {
                    return unit;
                }
            }
            throw new Exception($"Установка с названием {unitName} не найдена!");
        }

        /// <summary>
        /// Поиск фабрики по имени.
        /// </summary>
        /// <param name="factoryName">Имя завода</param>
        /// <param name="factories">Заводы</param>
        /// <returns>Завод</returns>
        public Factory FindFactoryByName(string factoryName, IEnumerable<Factory> factories)
        {
            foreach (var factory in factories)
            {
                if (factory.Name == factoryName)
                {
                    return factory;
                }
            }
            throw new Exception($"Завод с названием {factoryName} не найден!");
        }

        /// <summary>
        /// Поиск резервуара по имени.
        /// </summary>
        /// <param name="tankName">Имя завода</param>
        /// <param name="tanks">Заводы</param>
        /// <returns>Завод</returns>
        public Tank FindTankByName(string tankName, IEnumerable<Tank> tanks)
        {
            foreach (var tank in tanks)
            {
                if (tank.Name == tankName)
                {
                    return tank;
                }
            }
            throw new Exception($"Резервуар с названием {tankName} не найдена!");
        }

        public Tank FindTankByNameLINQQuery(string tankName, IEnumerable<Tank> tanks)
        {
            var ioService = new IOService();
            IEnumerable<Tank> findQuery =
                from tank in tanks
                where tank.Name == tankName
                select tank;
            foreach(var tank in findQuery)
            {
                return tank;
            }
            throw new Exception("Резервуар не найден.");
        }

        public Tank FindTankByNameLINQMethod(string tankName, IEnumerable<Tank> tanks)
        {
            var ioService = new IOService();
            IEnumerable<Tank> findQuery = tanks.Where(c => c.Name == tankName);
            foreach (var tank in findQuery)
            {
                return tank;
            }
            throw new Exception("Резервуар не найден.");
        }
    }

}
