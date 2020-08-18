
namespace WebApi
{
    public class Tank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }
        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}

