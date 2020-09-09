using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Odt;
using System.Configuration;
using System.Data;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly UnitRepository repo;
        private readonly IMapper mapper;
        private IConfiguration configuration;
        public UnitController(UnitRepository repo, IMapper mapper, IConfiguration configuration)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.configuration = configuration;
        }
        [Authorize]
        // GET: api/units
        [HttpGet("api/units")]
        public async Task<IEnumerable<UnitOdt>> GetUnits()
        {
            var units = await repo.GetAll();
            var ods = new List<UnitOdt>();
            foreach(var unit in units)
            {
                var odt = mapper.Map<UnitOdt>(unit);
                //odt.Factory = mapper.Map<FactoryForUnitOdt>(unit.Factory);
                //odt.Tanks = mapper.Map<ICollection<TankListOdt>>(unit.Tanks);
                ods.Add(odt);
            }
            return ods;
        }
        [Authorize]
        // GET api/units/5
        [HttpGet("api/units/{id}")]
        public async Task<UnitOdt> GetUnitById(int id)
        {
            var unit = await repo.GetId(id);
            var odt = mapper.Map<UnitOdt>(unit);
            odt.Factory = mapper.Map<FactoryForUnitOdt>(unit.Factory);
            odt.Tanks = mapper.Map<ICollection<TankListOdt>>(unit.Tanks);
            return odt;
        }
        [Authorize(Roles = "admin")]
        [HttpPost("api/units")]
        public void PostUnit(Unit unit)
        {
            repo.Post(unit);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("api/units/{id}")]
        public async Task DeleteUnit(int id)
        {
            var unit = await GetUnitById(id);
            if (unit == null)
                NotFound();
            if (unit.Tanks.Count != 0)
                BadRequest("Установка содержит резервуары, не может быть удалена");
            await repo.Delete(id);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("api/units")]
        public async Task UpdateUnit(Unit unit)
        {
            var Unit = await GetUnitById(unit.Id);
            if (Unit == null)
                NotFound();
            await repo.Update(unit);
        }

        [HttpGet("api/unit/events/{unitId}")]
        public async Task<IList<EventOdt>> GetEvents(int unitId)
        {
            var events = await repo.UnitEvents(unitId);
            if (events == null)
                NotFound();
            return events;
        }
    }
}
