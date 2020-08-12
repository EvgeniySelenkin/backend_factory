using Dapper;
using NPOI.SS.Formula.Functions;
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
        private readonly IOService ioService;
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
    }
}