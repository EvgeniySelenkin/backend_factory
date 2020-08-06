using System.Collections.Generic;
namespace Lesson1
{    public class OurFactory
    {
        public IEnumerable<Factory> Factories { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<Tank> Tanks { get; set; }
    }
}
