using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("Unit")]
        [Column(Order = 3)]
        public int? FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual ICollection<Tank> Tanks { get; set; }
    }
}


