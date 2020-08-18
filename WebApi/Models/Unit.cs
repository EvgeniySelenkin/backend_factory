using System.Collections.Generic;
namespace WebApi
{
    public class Unit
    {
        public Unit()
        {
            this.Tanks = new HashSet<Tank>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual ICollection<Tank> Tanks { get; set; }
    }
}


