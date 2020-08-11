using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson1
{
    interface IDataService
    {
        IEnumerable<Factory> GetFactories();
        IEnumerable<Unit> GetUnits();
        IEnumerable<Tank> GetTanks();
        Unit FindUnitByTank(Tank tank, IEnumerable<Unit> units);
        Factory FindFactoryByUnit(Unit unit, IEnumerable<Factory> factories);
        int GetTotalVolume(IEnumerable<Tank> tanks);
        Unit FindUnitByName(string unitName, IEnumerable<Unit> units);
        Factory FindFactoryByName(string factoryName, IEnumerable<Factory> factories);
        Tank FindTankByName(string tankName, IEnumerable<Tank> tanks);

    }
}
