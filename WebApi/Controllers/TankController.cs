using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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
    public class TankController : ControllerBase
    {
        private readonly TankRepository repo;
        public TankController(TankRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/tanks
        [HttpGet("api/tanks")]
        public IEnumerable<TankOdt> GetTanks()
        {
            var tanks = repo.GetAll().Result;
            var ods = new List<TankOdt>();
            foreach(Tank tank in tanks)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Tank, TankOdt>().ForMember("Unit", opt => opt.Ignore()));
                var mapper = new Mapper(config);
                var odt = mapper.Map<TankOdt>(tank);
                var configUnit = new MapperConfiguration(cfg => cfg.CreateMap<Unit, UnitListOdt>());
                var mapperUnit = new Mapper(configUnit);
                odt.Unit = mapperUnit.Map<UnitListOdt>(tank.Unit);
                ods.Add(odt);
            }
            return ods;
        }

        // GET api/tanks/5
        [HttpGet("api/tanks/{id}")]
        public  TankOdt GetTankById(int id)
        {
            var tank = repo.GetId(id).Result;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Tank, TankOdt>().ForMember("Unit", opt => opt.Ignore()));
            var mapper = new Mapper(config);
            var odt = mapper.Map<TankOdt>(tank);
            var configUnit = new MapperConfiguration(cfg => cfg.CreateMap<Unit, UnitListOdt>());
            var mapperUnit = new Mapper(configUnit);
            odt.Unit = mapperUnit.Map<UnitListOdt>(tank.Unit);
            return odt;
        }

        [HttpPost("api/tanks")]
        public async Task PosTank(Tank tank)
        {
            await repo.Post(tank);
        }

        [HttpDelete("api/tanks/{id}")]
        public async Task DeleteTank(int id)
        {
            await repo.Delete(id);
        }

        [HttpPut("api/tanks")]
        public async Task UpdateFactory(Tank tank)
        {
            await repo.Update(tank);
        }


    }
}
