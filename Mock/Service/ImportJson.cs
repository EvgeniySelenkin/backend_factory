using Mock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Mock.Service
{
    public class ImportJson
    {
        private List<Event> ListEvents { get; set; }
        public ImportJson()
        {
            var pathFile = Path.Combine(@"D:\IT\C#\Backend\SelenkinEE\Mock", "events.json");
            var js = new JsonSer();
            ListEvents = js.Deserialize(pathFile);
        }

        public List<Event> GetEvent()
        {
            return ListEvents;
        }
    }
}
