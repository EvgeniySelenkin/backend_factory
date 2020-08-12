using Dapper;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lesson1
{
    public class DataServiceSql : IDataService
    {
        private readonly string connectionString = @"Data Source=LAPTOP-8TVASHA4;Initial Catalog=Factories;Integrated Security=True";
        private readonly IOService ioService = new IOService();
        public async Task<IList<Unit>> GetUnits()
        {
            var units = new List<Unit>();
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch(Exception ex)
            {
                ioService.Exeption(ex);
                return units;
            }

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Unit;";
            await using var reader = await command.ExecuteReaderAsync();
            while(reader.Read())
            {
                units.Add(new Unit 
                { Id = (int)reader.GetFieldValue<object>(0), 
                    Name = reader.GetFieldValue<object>(1).ToString(), 
                    FactoryId = (int)reader.GetFieldValue<object>(2)
                });
            }
            return units;
        }
        
        public async Task<Unit> FindUnitByName(string unitName)
        {
            var unit = new Unit();
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch(Exception ex)
            {
                ioService.Exeption(ex);
                return unit;
            }
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT TOP 1 * FROM Unit WHERE Unit.Name=\'{unitName}\';";
            await using var reader = await command.ExecuteReaderAsync();
            if(reader.Read())
            {
                unit = new Unit
                {
                    Id = (int)reader.GetFieldValue<object>(0),
                    Name = reader.GetFieldValue<object>(1).ToString(),
                    FactoryId = (int)reader.GetFieldValue<object>(2)
                };
            }
            return unit;
        }

        public async Task<Factory> FindFactoryByUnit(Unit unit)
        {
            var factory = new Factory();
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch(Exception ex)
            {
                ioService.Exeption(ex);
                return factory;
            }
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT TOP 1 * FROM Factory Where Factory.Id={unit.FactoryId};";
            await using var reader = await command.ExecuteReaderAsync();
            if(reader.Read())
            {
                factory = new Factory
                {
                    Id = (int)reader.GetFieldValue<object>(0),
                    Name = reader.GetFieldValue<object>(1).ToString(),
                    Description = reader.GetFieldValue<object>(2).ToString()
                };
            }
            return factory;
        }

        public async Task<Unit> FindUnitByTank(Tank tank)
        {
            var unit = new Unit();
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch(Exception ex)
            {
                ioService.Exeption(ex);
                return unit;
            }
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT TOP 1 * FROM Unit WHERE Unit.Id={tank.UnitId};";
            await using var reader = await command.ExecuteReaderAsync();
            if(reader.Read())
            {
                unit = new Unit
                {
                    Id = (int)reader.GetFieldValue<object>(0),
                    Name = reader.GetFieldValue<object>(1).ToString(),
                    FactoryId = (int)reader.GetFieldValue<object>(2)
                };
            }
            return unit;
        }

        public async Task CreateUnit()
        {
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                ioService.Exeption(ex);
            }
            ioService.Output("Введите название установки");
            var Name = ioService.Input();
            ioService.Output("Введите номер завода к которому относится кстановка");
            var FactoryId = int.Parse(ioService.Input());
            await using var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Unit (Name, FactoryId) VALUES (\'{Name}\', {FactoryId});";
            int number = command.ExecuteNonQuery();
            ioService.Output($"Добавлено объектов: {number}");
        }

        public void ReadUnit()
        {
            ioService.Output("Введите название установки");
            var Name = ioService.Input();
            var unit = FindUnitByName(Name).Result;
            ioService.Output($"Установка с названием {Name} Стоит на заводе с индексом {unit.FactoryId}");
        }

        public async Task UpdateUnit()
        {
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                ioService.Exeption(ex);
            }
            ioService.Output("Введите название установки");
            var Name = ioService.Input();
            var unit = FindUnitByName(Name).Result;
            ioService.Output($"Установка с названием {Name} Стоит на заводе с индексом {unit.FactoryId}");
            ioService.Output("Введите новые данные для записи");
            ioService.Output("Введите новое имя установки");
            var newName = ioService.Input();
            ioService.Output("Введите новый индекс завода");
            var newFactoryId = ioService.Input();
            await using var command = connection.CreateCommand();
            command.CommandText = $"UPDATE Unit SET Name=\'{newName}\', FactoryId={newFactoryId} WHERE Unit.Name=\'{Name}\' ;";
            int number = command.ExecuteNonQuery();
            ioService.Output($"Обновлено объектов: {number}");
        }

        public async Task DeleteUnit()
        {
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                ioService.Exeption(ex);
            }
            ioService.Output("Введите название установки");
            var Name = ioService.Input();
            await using var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Unit WHERE Unit.Name=\'{Name}\';";
            int number = command.ExecuteNonQuery();
            ioService.Output($"Удалено объектов: {number}");
        }
    }
}