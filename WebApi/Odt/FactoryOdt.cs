using System.Collections.Generic;

namespace WebApi
{
    public class FactoryOdt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UnitListOdt> Units { get; set; }
    }
}
