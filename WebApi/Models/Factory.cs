using System.Collections.Generic;
namespace WebApi
{
    public class Factory
    {
        Factory()
        {
            this.Units = new HashSet<Unit>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}


