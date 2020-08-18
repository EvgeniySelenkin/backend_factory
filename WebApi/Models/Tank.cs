using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi
{
    public class Tank
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }
        [ForeignKey("Unit")]
        [Column(Order = 5)]
        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}

