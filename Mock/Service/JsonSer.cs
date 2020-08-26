using Mock.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mock.Service
{
    public class JsonSer
    {
        public List<Event> Deserialize(string pathFile)
        {
            var jsonString = File.ReadAllText(pathFile);
            return JsonSerializer.Deserialize<List<Event>>(jsonString);
        }
    }
}
