using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Odt;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly UnitRepository repo;
        private readonly IMapper mapper;
        public UnitController(UnitRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        // GET: api/units
        [HttpGet("api/units")]
        public IEnumerable<UnitOdt> GetUnits()
        {
            var units = repo.GetAll().Result;
            var ods = new List<UnitOdt>();
            foreach(var unit in units)
            {
                //var config = new MapperConfiguration(cfg => cfg.CreateMap<Unit, UnitOdt>().ForMember("Factory", opt => opt.Ignore()).ForMember("Tanks", opt => opt.Ignore()));
                //var mapper = new Mapper(config);
                var odt = mapper.Map<UnitOdt>(unit);
                //var configFactory = new MapperConfiguration(cfg => cfg.CreateMap<Factory, FactoryForUnitOdt>());
                //mapper = new Mapper(configFactory);
                odt.Factory = mapper.Map<FactoryForUnitOdt>(unit.Factory);
                //var configTanks = new MapperConfiguration(cfg => cfg.CreateMap<Tank, TankListOdt>());
                //mapper = new Mapper(configTanks);
                odt.Tanks = mapper.Map<ICollection<TankListOdt>>(unit.Tanks);
                ods.Add(odt);
            }
            return ods;
        }

        // GET api/units/5
        [HttpGet("api/units/{id}")]
        public UnitOdt GetUnitById(int id)
        {
            var unit = repo.GetId(id).Result;
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<Unit, UnitOdt>().ForMember("Factory", opt => opt.Ignore()).ForMember("Tanks", opt => opt.Ignore()));
            //var mapper = new Mapper(config);
            var odt = mapper.Map<UnitOdt>(unit);
            //var configFactory = new MapperConfiguration(cfg => cfg.CreateMap<Factory, FactoryForUnitOdt>());
            //mapper = new Mapper(configFactory);
            odt.Factory = mapper.Map<FactoryForUnitOdt>(unit.Factory);
            //var configTanks = new MapperConfiguration(cfg => cfg.CreateMap<Tank, TankListOdt>());
            //mapper = new Mapper(configTanks);
            odt.Tanks = mapper.Map<ICollection<TankListOdt>>(unit.Tanks);
            return odt;
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
