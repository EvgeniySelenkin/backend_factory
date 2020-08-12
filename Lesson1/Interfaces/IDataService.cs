using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    interface IDataService
    {
        Task<IList<Unit>> GetUnits();
        Task<Unit> FindUnitByTank(Tank tank);
        Task<Factory> FindFactoryByUnit(Unit unit);
        Task<Unit> FindUnitByName(string unitName);

    }
}
