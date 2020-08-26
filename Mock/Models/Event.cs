using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mock.Models
{
    public class Event
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public double StorageValue { get; set; }
        public string Name { get; set; }
        public int UnitId { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longituted { get; set; }
        public List<string> Tags { get; set; }
        public List<Operator> ResponsibleOperators { get; set; }
    }
}
