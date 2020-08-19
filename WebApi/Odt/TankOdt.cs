using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Odt
{
    public class TankOdt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }
        public virtual UnitListOdt Unit { get; set; }
    }
}
