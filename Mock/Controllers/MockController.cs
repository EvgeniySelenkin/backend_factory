using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
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
        private ImportJson import { get; set; }
        public MockController(ImportJson import)
        {
            this.import = import;
        }

        [HttpGet("api/events/keys")]
        public IList<int> GetEventsId(int unitId, int take, int skip)
        {
            var events = import.GetEvent().Where(x => x.UnitId == unitId).Skip(skip).Take(take).ToList();
            var ids = events.Select(u => u.Id).ToList();
            return ids;
        }

        [HttpPost("api/events")]
        public string GetEvents(List<int> ids)
        {
            var result = import.GetEvent().Where(u => ids.Exists(y => y == u.Id));
            return JsonSerializer.Serialize(result);
        }
    }
}
