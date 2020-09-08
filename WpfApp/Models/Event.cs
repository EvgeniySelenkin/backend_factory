using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp.Models
{
    public class Event
    {
        public class EventOdt
        {
            public int Id { get; set; }
            public int EventId { get; set; }
            public bool IsActive { get; set; }
            public double StorageValue { get; set; }
            public string Name { get; set; }
            public int UnitId { get; set; }
            public string Description { get; set; }
            public double Latitude { get; set; }
            public double Longituted { get; set; }
            public string Tags { get; set; }
            public string ResponsibleOperators { get; set; }
        }
    }
}
