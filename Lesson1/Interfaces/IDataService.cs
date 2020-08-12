using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson1
{
    interface IDataService
    {
        IList<Unit> GetUnits();
        Unit FindUnitByTank(Tank tank);
        Factory FindFactoryByUnit(Unit unit);
        Unit FindUnitByName(string unitName);

    }
}
