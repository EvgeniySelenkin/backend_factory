using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace SelenkinEE
{
    class Program
    {
        static void Main(string[] args)
        {
            var tanks = GetTanks();
            var units = GetUnits();
            var factories = GetFactories();
            Console.WriteLine($"Количество резервуаров {tanks.Length}");

            for (int i = 0; i < tanks.Length; i++)
            {
                tanks[i].GetInformation();
                Console.WriteLine($"{tanks[i].Name} принадлежит установке {units[tanks[i].UnitId - 1].Name} и заводу {factories[units[tanks[i].UnitId - 1].FactoryId - 1].Name}");
                Console.WriteLine();
            }

            var totalVolume = GetTotalVolume(tanks);
            Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
            Console.WriteLine();
            Console.WriteLine("В какой коллекции вы хотите произвести поиск? (Заводы, Установки, Резервуары)");
            string choise = Console.ReadLine().ToLower();
            while(choise!= "заводы" || choise!= "установки" || choise!= "резервуары")
            {
                Console.WriteLine("Похоже вы ошиблись при вводе, введите снова");
                choise = Console.ReadLine().ToLower();
            }
            if (choise == "заводы")
            {
                Console.WriteLine("Введите название завода");
                var Name = Console.ReadLine();
                Console.WriteLine("Информация по данному заводу");
                FindFactory(factories, Name).GetInformation();
            }
            else if (choise == "установки")
            {
                Console.WriteLine("Введите название установки");
                var Name = Console.ReadLine();
                Console.WriteLine("Информация по данной установке");
                Console.WriteLine("Установка " + FindUnit(units, Name).Name + " находится на заводе " + factories[FindUnit(units, Name).FactoryId - 1].Name);
            }
            else if (choise == "резервуары")
            {
                Console.WriteLine("Введите название резервуара");
                var Name = Console.ReadLine();
                Console.WriteLine("Информация по данному резервуару");
                FindTank(tanks, Name).GetInformation();
            }

            Console.WriteLine("Выгружаем объекты в json файл");
            SerializerToJSON(factories, units, tanks);

        }

        //возврат массива резервуаров
        public static Tank[] GetTanks()
        {
            var tanks = new Tank[6];
            FileStream fs = new FileStream("Таблица_резервуаров.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XSSFWorkbook workbook = new XSSFWorkbook(fs);
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(2);

            for(int i = 1;i<7;i++)
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
                tanks[i-1] = new Tank((int)tankIdCell.NumericCellValue, tankNameCell.StringCellValue, (int)tankVolumeCell.NumericCellValue, (int)tankMaxVolumeCell.NumericCellValue, (int)tankUnitIdCell.NumericCellValue);
            }
            /*
            tanks[0] = new Tank(1, "Резервуар 1", 1500, 2000, 1);
            tanks[1] = new Tank(2, "Резервуар 2", 2500, 3000, 1);
            tanks[2] = new Tank(3, "Дополнительный резервуар 24", 3000, 3000, 2);
            tanks[3] = new Tank(4, "Резервуар 35", 3000, 3000, 2);
            tanks[4] = new Tank(5, "Резервуар 47", 4000, 5000, 2);
            tanks[5] = new Tank(6, "Резервуар 256", 500, 500, 3);*/
            return tanks;
        }

        //возврат массива установок
        public static Unit[] GetUnits()
        {
            FileStream fs = new FileStream("Таблица_резервуаров.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XSSFWorkbook workbook = new XSSFWorkbook(fs);
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(1);
            var units = new Unit[3];
            for (int i = 1; i < 4; i++)
            {
                ICell unitIdCell = sheet.GetRow(i).GetCell(0);
                unitIdCell.SetCellType(CellType.Numeric);
                ICell unitNameCell = sheet.GetRow(i).GetCell(1);
                unitNameCell.SetCellType(CellType.String);
                ICell unitFactoryIdCell = sheet.GetRow(i).GetCell(2);
                unitFactoryIdCell.SetCellType(CellType.Numeric);
                units[i - 1] = new Unit((int)unitIdCell.NumericCellValue, unitNameCell.StringCellValue, (int)unitFactoryIdCell.NumericCellValue);
            }

            /*units[0] = new Unit(1, "ГФУ-1", 1);
            units[1] = new Unit(2, "ГФУ-2", 1);
            units[2] = new Unit(3, "АВТ-6", 2);*/

            return units;
        }

        //возврат массива заводов
        public static Factory[] GetFactories()
        {
            var factories = new Factory[2];
            FileStream fs = new FileStream("Таблица_резервуаров.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                factories[i - 1] = new Factory((int)factoryIdCell.NumericCellValue, factoryNameCell.StringCellValue, factoryDescriptionCell.StringCellValue);
            }
            /*factories[0] = new Factory(1, "МНПЗ", "Московский нефтеперерабатывающий завод");
            factories[1] = new Factory(2, "ОНПЗ", "Омский нефтеперерабатывающий завод");*/

            return factories;
        }

        // суммарный объем резервуаров в массиве
        public static int GetTotalVolume(Tank[] tanks)
        {
            int totalVolume = 0;
            for (int i = 0; i < tanks.Length; i++)
                totalVolume += tanks[i].Volume;
            return totalVolume;
        }

        public static Factory FindFactory(Factory[] factories, string name)
        {
            var factory = new Factory();
            for (int i = 0; i < factories.Length; i++)
                if (factories[i].Name == name)
                    return factories[i];
        }

        public static Unit FindUnit(Unit[] units, string name)
        {
            var unit = new Unit();
            for (int i = 0; i < units.Length; i++)
                if (units[i].Name == name)
                    return units[i];
        }

        public static Tank FindTank(Tank[] tanks, string name)
        {
            var tank = new Tank();
            for (int i = 0; i < tanks.Length; i++)
                if (tanks[i].Name == name)
                    return tanks[i];
        }

        public static void SerializerToJSON(Factory[] factories, Unit[] units, Tank[] tanks)
        {
            
            var factory = JsonConvert.SerializeObject(factories);
            var unit = JsonConvert.SerializeObject(units);
            var tank = JsonConvert.SerializeObject(tanks);
            string result = "Factories: " + factory + " Units: " + unit + " Tanks: " + tank;
            File.WriteAllText("object.json", JsonConvert.SerializeObject(result));
            Console.WriteLine("Данные загружены");
        }
    }

}
