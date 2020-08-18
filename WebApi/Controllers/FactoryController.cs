using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace WebApi
{
    //[Route("api/[controller]")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        private readonly FactoryRepository repo;
        public FactoryController(FactoryRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/factories
        [HttpGet("api/factories")]
        public async Task<IEnumerable<Factory>> GetFactories()
        {
            var factories = await repo.GetAll();
            return factories;
        }

        // GET api/factories/5
        [HttpGet("api/factories/{id}")]
        public FactoryOdt GetFactoryById(int id)
        {
            var factory = repo.GetId(id).Result;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Factory, FactoryOdt>().ForMember("Units", opt => opt.Ignore()));
            var mapper = new Mapper(config);
            var odt = mapper.Map<Factory, FactoryOdt>(factory);
            var configUnit = new MapperConfiguration(cfg => cfg.CreateMap<Unit, UnitListOdt>());
            var mapperUnit = new Mapper(configUnit);
            odt.Units = mapperUnit.Map<ICollection<Unit>, ICollection<UnitListOdt>>(factory.Units);
            return odt;
        }

        [HttpPost("api/factories")]
        public async Task PostFactory(Factory factory)
        {
            await repo.Post(factory);
        }

        [HttpDelete("api/factories/{id}")]
        public async Task DeleteFactory(int id)
        {
            await repo.Delete(id);
        }

        [HttpPut("api/factories")]
        public async Task UpdateFactory(Factory factory)
        {
            await repo.Update(factory);
        }
    }
}
