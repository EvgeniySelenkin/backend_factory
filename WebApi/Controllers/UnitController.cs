using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly UnitRepository repo;
        public UnitController(UnitRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/units
        [HttpGet("api/units")]
        public async Task<IEnumerable<Unit>> GetUnits()
        {
            var units = await repo.GetAll();
            return units;
        }

        // GET api/units/5
        [HttpGet("api/units/{id}")]
        public async Task<Unit> GetUnitById(int id)
        {
            return await repo.GetId(id);
        }

        [HttpPost("api/units")]
        public async Task PostUnit(Unit unit)
        {
            await repo.Post(unit);
        }

        [HttpDelete("api/units/{id}")]
        public async Task DeleteUnit(int id)
        {
            await repo.Delete(id);
        }

        [HttpPut("api/units")]
        public async Task UpdateUnit(Unit unit)
        {
            await repo.Update(unit);
        }
    }
}
