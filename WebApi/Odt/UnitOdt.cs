using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Odt
{
    public class UnitOdt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual FactoryForUnitOdt Factory { get; set; }
        public virtual ICollection<TankListOdt> Tanks { get; set; }
    }
}
