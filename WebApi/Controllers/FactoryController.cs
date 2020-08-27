using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace WebApi
{
    //[Route("api/[controller]")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        private readonly FactoryRepository repo;
        private readonly IMapper mapper;
        public FactoryController(FactoryRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        // GET: api/factories
        [Authorize]
        [HttpGet("api/factories")]
        public async Task<IEnumerable<FactoryOdt>> GetFactories()
        {
            var factories = await repo.GetAll();
            var ods = new List<FactoryOdt>();
            foreach(Factory factory in factories)
            {
                var odt = mapper.Map<Factory, FactoryOdt>(factory);
                //odt.Units = mapper.Map<ICollection<Unit>, ICollection<UnitListOdt>>(factory.Units);
                ods.Add(odt);
            }
            return ods;
        }

        // GET api/factories/5
        [Authorize]
        [HttpGet("api/factories/{id}")]
        public async Task<FactoryOdt> GetFactoryById(int id)
        {
            var factory = await repo.GetId(id);
            var odt = mapper.Map<Factory, FactoryOdt>(factory);
            odt.Units = mapper.Map<ICollection<Unit>, ICollection<UnitListOdt>>(factory.Units);
            return odt;
        }
        [Authorize(Roles = "admin")]
        [HttpPost("api/factories")]
        public void PostFactory(Factory factory)
        {
            repo.Post(factory);
        }
        [Authorize(Roles ="admin")]
        [HttpDelete("api/factories/{id}")]
        public async Task DeleteFactory(int id)
        {
            var factory = await GetFactoryById(id);
            if (factory == null)
                NotFound();
            if (factory.Units.Count != 0)
                BadRequest("Завод содержит установки, не может быть удален");
            await repo.Delete(id);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("api/factories")]
        public async Task UpdateFactory(Factory factory)
        {
            var Factory = await GetFactoryById(factory.Id);
            if (Factory == null)
                NotFound();
            await repo.Update(factory);
        }
    }
}
