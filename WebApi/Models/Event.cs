using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi.Models
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

        public string SerializeTags()
        {
            return JsonSerializer.Serialize(Tags);
        }

        public string SerializeOperators()
        {
            return JsonSerializer.Serialize(ResponsibleOperators);
        }

    }
}
