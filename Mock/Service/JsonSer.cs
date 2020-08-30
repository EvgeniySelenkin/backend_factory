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
            if(!File.Exists(pathFile))
            {
                throw new Exception("Файл не найден");
            }
            var jsonString = File.ReadAllText(pathFile);
            return JsonSerializer.Deserialize<List<Event>>(jsonString);
        }
    }
}
