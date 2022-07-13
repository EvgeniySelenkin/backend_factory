using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Models;

namespace WpfApp.Data
{
    public class Repository
    {
        public async Task<IReadOnlyCollection<Unit>> GetUnits()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            var unit1 = new Unit
            {
                Id = 1,
                Name = "ГФУ-1"
            };

            var unit2 = new Unit
            {
                Id = 2,
                Name = "ГФУ-2"
            };

            var unit3 = new Unit
            {
                Id = 3,
                Name = "АВТ-6"
            };

            var units = new[] {unit1, unit2, unit3};
            return units;
        }
    }
}