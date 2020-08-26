using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock.Models;
using Mock.Service;

namespace Mock.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        public List<Event> ListEvents { get; set; }
        public MockController()
        {
            var dirInfo = new DirectoryInfo(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).Parent?.Parent?.Parent;
            var pathFile = Path.Combine(dirInfo.FullName, "events.json");
            var Js = new JsonSer();
            ListEvents = Js.Deserialize(pathFile);
        }

        [HttpGet("api/events/keys")]
        public IList<int> GetEventsId(int unitId, int take, int skip)
        {
            var ListEvent = ListEvents.Where(x => x.UnitId == unitId).Skip(skip).Take(take);
            var ListId = new List<int>();
            foreach(var elem in ListEvent)
            {
                ListId.Add(elem.Id);
            }
            
            return ListId;
        }

        [HttpPost("api/events")]
        public IList<Event> GetEvents(List<int> ListId)
        {
            var result = new List<Event>();
            foreach (var elem in ListId)
            {
                result.Add(ListEvents.FirstOrDefault(x => x.Id == elem));
            }
            return result;
        }
    }
}
